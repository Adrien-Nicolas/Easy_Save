using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasySaveV2.Others
{
    class CryptoSoft
    {


        /// <summary>
        /// Method allowing to Launch Cryptosoft.exe, return time encrypt/decrypt in ms
        /// </summary>
        /// <param name="filePath">string</param>
        /// <returns>double</returns>
        public static double LaunchProcessCrypto(string key, string sourceFilePath, string targetFolderPath)
        {
            DateTime cryptDateTime = DateTime.Now;

            Process cryptoSoft = new Process();
            cryptoSoft.StartInfo = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "CryptoSoft/CryptoSoft.exe","\"" + key + "\" \"" + sourceFilePath + "\" \"" + targetFolderPath + "\"");
            
            cryptoSoft.StartInfo.UseShellExecute = false;
            cryptoSoft.StartInfo.CreateNoWindow = true;

            try
            {
                cryptoSoft.Start();
                cryptoSoft.WaitForExit();
                return DateTime.Now.Subtract(cryptDateTime).TotalMilliseconds;
            }
            catch
            {
                return -1;
            }
        }
    }
}
