using AdonisUI.Controls;
using PassKeeper.Model;
using PassKeeper.Security;
using System.Windows;

namespace PassKeeper
{
    /// <summary>
    /// SetDbConfig.xaml 的交互逻辑
    /// </summary>
    public partial class SetDbConfigWindow : AdonisWindow
    {
        public DbConfig dbConfig = null;
        public bool isChanged = false;
        public SetDbConfigWindow()
        {
            InitializeComponent();
        }
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AdonisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (dbConfig != null)
            {
                tbName.Text = dbConfig.Name;
                tbPassword.Text = dbConfig.Password;
                cbIsUseGoogleAuthenticator.IsChecked = dbConfig.IsUseGoogleAuthenticator;
                imgQrCode.Source = GoogleAuthenticator.GetQrCode(dbConfig.GoogleAuthenticatorKey, dbConfig.Name);
            }
        }
        private void BtGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.Text = PasswordGenerator.GeneratePassword(8);
        }
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            if (dbConfig.Name != tbName.Text || dbConfig.Password != tbPassword.Text || dbConfig.IsUseGoogleAuthenticator != cbIsUseGoogleAuthenticator.IsChecked)
            {
                dbConfig.Name = tbName.Text;
                dbConfig.Password = tbPassword.Text;
                dbConfig.IsUseGoogleAuthenticator = (bool)cbIsUseGoogleAuthenticator.IsChecked;
                isChanged = true;
            }
            this.Close();
        }
    }
}
