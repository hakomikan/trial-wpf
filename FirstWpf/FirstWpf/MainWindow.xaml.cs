using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new Data();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("aaaaaaaaaaaaaaa");

        //    <Window
        //xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
        //xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        //xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        //xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        //xmlns:local=""clr-namespace:FirstWpf""
        //mc:Ignorable=""d""
        //Title=""MainWindow"" Height=""356.353"" Width=""525"">

            var xaml = @"

    <Grid 
        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""

        Margin=""7,0,0,0"" RenderTransformOrigin=""0.397,0.502"">
        <Grid.RowDefinitions>
            <RowDefinition Height=""40""/>
            <RowDefinition/>
            <RowDefinition/>
        </Grid.RowDefinitions>
        <Button Content=""Button"" HorizontalAlignment=""Left"" Margin=""10,10,0,0"" VerticalAlignment=""Top"" Width=""75""/>
        <TextBox Text=""asdfghjk"" Grid.Row=""1"" HorizontalAlignment=""Left"" Height=""23"" Margin=""9.5,10,0,0"" TextWrapping=""Wrap"" VerticalAlignment=""Top"" Width=""120"" IsReadOnlyCaretVisible=""True""/>
        <TextBox Grid.Row=""2"" HorizontalAlignment=""Left"" Height=""22"" Margin=""10,10.5,0,0"" TextWrapping=""Wrap"" Text=""hhhh"" VerticalAlignment=""Top"" Width=""120"" RenderTransformOrigin=""1.934,10.165""/>
    </Grid>
";

            var a = XamlReader.Load(Utility.GenerateStreamFromString(xaml));

            this.Content = a;
        }
    }
}
