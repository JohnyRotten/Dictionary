using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Dictionary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDataProvider _dataProvider;

        public MainWindow()
        {
            InitializeComponent();

            _dataProvider = new DataProviderProxy(new DbDataProvider());

            _dataProvider.UpdateListEvent += ShowWordList;

            LoadButton.Click += ClickLoadButtonHandler;
            SearchBox.TextChanged += SearchBoxInputHandler;

            ShowWordList();
        }

        private void ShowWordList()
        {
            var items = SearchBox.Text == "" ? _dataProvider.Items : _dataProvider.Search(SearchBox.Text);
            WordList.Children.Clear();
            foreach (var item in items)
            {
                WordList.Children.Add(new TextBlock { Text = item });
            }
        }

        private void LoadFileData(string fileName)
        {
            var reader = new StreamReader(new FileStream(fileName, FileMode.Open));

            var words = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null) words.Add(line.Trim());
            }

            if (words.Count != 0) _dataProvider.AddItems(words.ToArray());
        }

        private void SearchBoxInputHandler(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ShowWordList();
        }

        private async void ClickLoadButtonHandler(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Text(*.txt)|*.txt";
            openDialog.CheckFileExists = true;
            openDialog.Multiselect = false;
            if (openDialog.ShowDialog() == false) return;
            LoadFileData(openDialog.FileName);
        }
    }
}
