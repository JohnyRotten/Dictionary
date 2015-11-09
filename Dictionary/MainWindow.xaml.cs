using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

            _dataProvider = new DataProviderProxy(new DbDataProvider(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));

            _dataProvider.UpdateListEvent += ShowWordList;

            LoadButton.Click += ClickLoadButtonHandler;
            SaveButton.Click += ClickSaveButtomHandler;
            SearchBox.TextChanged += SearchBoxInputHandler;

            ShowWordList();
        }

        protected string[] GetWords => SearchBox.Text == "" ? _dataProvider.Items : _dataProvider.Search(SearchBox.Text);

        private void ShowWordList()
        {
            var items = GetWords;
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
            reader.Close();

            if (words.Count != 0) _dataProvider.AddItems(words.ToArray());
        }

        private void SaveFileData(string fileName)
        {
            var writter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate));
            foreach (var word in GetWords)
            {
                writter.WriteLine(word);
            }
            writter.Close();

        }

        private void SearchBoxInputHandler(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ShowWordList();
        }

        private void ClickLoadButtonHandler(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Text(*.txt)|*.txt",
                CheckFileExists = true,
                Multiselect = false
            };
            if (openDialog.ShowDialog() == false) return;
            LoadFileData(openDialog.FileName);
        }

        private void ClickSaveButtomHandler(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Text(*.txt)|*.txt",
                Multiselect = false,
                CheckFileExists = false
            };
            if (openDialog.ShowDialog() == false) return;
            SaveFileData(openDialog.FileName);
        }
    }
}
