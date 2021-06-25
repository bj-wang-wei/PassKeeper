using Google.Authenticator;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PassKeeper.Security
{
    public static class GoogleAuthenticator
    {
        //public  const string key = "";
        private const string user = "Double U PassKeeper";
        //private const string title = "";
        public static SetupCode GetSetupCode(string key, string title)
        {
            string base32_key = Base32Encoding.ToString(Encoding.ASCII.GetBytes(key));
            TwoFactorAuthenticator tfa = new();
            return tfa.GenerateSetupCode(user, title, base32_key, false, 3);
        }
        public static bool CodeVerify(string code, string key)
        {
            string base32_key = Base32Encoding.ToString(Encoding.ASCII.GetBytes(key));
            TwoFactorAuthenticator tfa = new();
            return tfa.ValidateTwoFactorPIN(base32_key, code);
        }
        public static ImageSource GetQrCode(string key, string title)
        {
            SetupCode setupCode = GetSetupCode(key, title);
            byte[] streamBase = Convert.FromBase64String(setupCode.QrCodeSetupImageUrl[(setupCode.QrCodeSetupImageUrl.IndexOf(",") + 1)..]);
            BitmapImage bi = new();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(streamBase);
            bi.EndInit();
            return bi;
        }
    }
}
