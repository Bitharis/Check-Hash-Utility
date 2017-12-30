using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace Hash_Check_Utlity
{
    internal class MD5ChecksumService
    {
        public Task<string> GetMD5ChecksumAsync(string file)
        {
            return Task.Run(() => GetChecksumBuffered(file));
        }

        private string GetChecksumBuffered(string file)
        {
            try
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var bufferedStream = new BufferedStream(fileStream, 1024 * 32))
                    {
                        var md5 = MD5.Create();
                        byte[] checksum = md5.ComputeHash(bufferedStream);
                        return BitConverter.ToString(checksum).Replace("-", String.Empty);
                    }
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }

        }
    }
}
