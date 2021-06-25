using AdonisUI.Controls;
using PassKeeper.Model;
using PassKeeper.Security;
using PassKeeper.Common;
using System.Windows;

namespace PassKeeper
{
    /// <summary>
    /// Password.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordWindow : AdonisWindow
    {
        public DbConfig dbConfig;
        public bool isSuccess = false;
        public PasswordWindow()
        {
            InitializeComponent();
        }
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            if (dbConfig.Password != tbPassword.Password)
            {
                AdonisUI.Controls.MessageBox.Show(CustomMessage.GetErrorBoxModel("库文件密码不正确！", "错误信息"));
                tbPassword.Focus();
                tbPassword.SelectAll();
                return;
            }
            if (dbConfig.IsUseGoogleAuthenticator)
            {
                if (!GoogleAuthenticator.CodeVerify(tbGoogleCode.Text, dbConfig.GoogleAuthenticatorKey))
                {
                    AdonisUI.Controls.MessageBox.Show(CustomMessage.GetErrorBoxModel("Google验证码错误！", "错误信息"));
                    tbGoogleCode.Focus();
                    tbGoogleCode.SelectAll();
                    return;
                }
            }
            this.isSuccess = true;
            this.Close();
        }

        private void AdonisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbPassword.Focus();
        }
    }
}
