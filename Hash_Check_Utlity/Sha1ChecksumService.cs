using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace Hash_Check_Utlity
{
    internal class Sha1ChecksumService
    {
        public Task<string> GetSHAOneChecksumAsync(string file)
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
                        var shaOne = SHA1.Create();
                        byte[] checksum = shaOne.ComputeHash(bufferedStream);
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
