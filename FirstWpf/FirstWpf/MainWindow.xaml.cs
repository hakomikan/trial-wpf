using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;

namespace FirstWpf
{
    class Data
    {
        public string TextMessage { get; set; } = "aaaaaaaaaaaaaa";
    }

    static class Utility
    {
        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public static void Retry(Action action, int retryCount, TimeSpan interval)
        {
            var exceptions = new List<Exception>();

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                    exceptions.Add(e);
                    Thread.Sleep(interval);
                }
            }

            throw new AggregateException(exceptions);
        }
    }

    class TestCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine($"test: {parameter}");
        }
    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private FileInfo DynamicContentXmlPath = new FileInfo("C:/home/prj/trial-wpf/FirstWpf/FirstWpf/DynamicContent.xaml");
        private FileSystemWatcher UpdateWatcher;

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = new Data();

            //AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;
            AppearanceManager.Current.AccentColor = Colors.Magenta;

            var targetContainer = (this.Content as Grid).FindName("DynamicContent") as Grid;
            var firstButton = targetContainer.FindName("FirstButton") as Button;
            firstButton.Command = new TestCommand();

            UpdateWatcher = new FileSystemWatcher()
            {
                Filter = "*",
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite,
                Path = DynamicContentXmlPath.Directory.FullName
            };
            UpdateWatcher.Changed += (f, e) =>
            {
                var path = e.FullPath;
                if (path == DynamicContentXmlPath.FullName)
                {
                    Console.WriteLine($"Update: {e.FullPath}");
                    Utility.Retry(() =>
                    {
                        using (var stream = DynamicContentXmlPath.OpenRead())
                        {
                            Console.WriteLine($"Update: {e.FullPath}");

                            var dispatcher = Application.Current.Dispatcher;

                            dispatcher.Invoke(() =>
                            {
                                var dynamicContent = XamlReader.Load(DynamicContentXmlPath.OpenRead()) as UIElement;
                                targetContainer.Children.Clear();
                                targetContainer.Children.Add(dynamicContent);
                            });
                        }
                    }, 20, TimeSpan.FromMilliseconds(100));
                }
            };
            UpdateWatcher.EnableRaisingEvents = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
