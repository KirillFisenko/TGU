using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Task3
{    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.Filter = "(*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    StreamReader reader = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read), System.Text.Encoding.UTF8);
                    textBox.Text = reader.ReadToEnd();
                    reader.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            try
            {
                saveFileDialog1.Filter = "(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.UTF8))
                    {
                        sw.Write(textBox.Text);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}