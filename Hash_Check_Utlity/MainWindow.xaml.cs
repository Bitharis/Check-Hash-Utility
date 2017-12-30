using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Hash_Check_Utlity
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
        
        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                fileTxt.Text = openFileDialog.FileName;  
            }
        }

        private async void VerifyBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
            if (string.IsNullOrEmpty(fileTxt.Text))
            {
                MessageBox.Show("Select a file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Cursor = Cursors.Wait;

            Sha256ChecksumService sha256ChecksumService = new Sha256ChecksumService();
            MD5ChecksumService md5ChecksumService = new MD5ChecksumService();
            Sha1ChecksumService sha1ChecksumService = new Sha1ChecksumService();
            
            Task<string> checksumMd5Task = md5ChecksumService.GetMD5ChecksumAsync(fileTxt.Text);
            Task<string> shaOneTask = sha1ChecksumService.GetSHAOneChecksumAsync(fileTxt.Text);
            Task<string> checksum256Task = sha256ChecksumService.GetSHA256ChecksumAsync(fileTxt.Text);


            var checksumMD5 = await checksumMd5Task;
            var shaOneChecksum = await shaOneTask;
            var checksum256 = await checksum256Task;

# region This can change with Binding
            //Bindng here
            md5Txt.Text = checksumMD5.ToLower();
            sha1Txt.Text = shaOneChecksum.ToLower();
            sha256Txt.Text = checksum256.ToLower();

            var result = CheckMatch(md5Txt.Text, devHashTxt.Text);
            md5Lbl.Content = result.message;
            md5Lbl.Foreground = result.color;

            result = CheckMatch(sha1Txt.Text, devHashTxt.Text);
            sha1Lbl.Content = result.message;
            sha1Lbl.Foreground = result.color;

            result = CheckMatch(sha256Txt.Text, devHashTxt.Text);
            sha256Lbl.Content = result.message;
            sha256Lbl.Foreground = result.color;
# endregion end of region

            this.Cursor = Cursors.Arrow;            
        }

        private (string message, Brush color) CheckMatch(string generatedHash, string devHash)
        {
            return generatedHash.Equals(devHash) ? ("Match" , Brushes.Green) : ("No Match",Brushes.Red);
        }

        private void ClearTextBoxes()
        {
            md5Txt.Text = String.Empty;
            sha1Txt.Text = String.Empty;
            sha256Txt.Text = String.Empty;

            md5Lbl.Content = string.Empty;
            sha1Lbl.Content = string.Empty;
            sha256Lbl.Content = string.Empty;
        }
    }
}
