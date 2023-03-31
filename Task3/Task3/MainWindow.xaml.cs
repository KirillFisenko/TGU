using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстик (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                StreamReader reader = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read), System.Text.Encoding.UTF8);

                textBox.Text = reader.ReadToEnd();

                reader.Close();
                return;

            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Текстик (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.UTF8))
                {
                    sw.Write(textBox.Text);
                    sw.Close();
                }
            }
        }
    }
}