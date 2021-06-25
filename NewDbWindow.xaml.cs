using AdonisUI.Controls;
using PassKeeper.FileOperator;
using PassKeeper.Model;
using PassKeeper.Security;
using PassKeeper.Common;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace PassKeeper
{
    /// <summary>
    /// NewDb.xaml 的交互逻辑
    /// </summary>
    public partial class NewDbWindow : AdonisWindow
    {
        public string dbFileName = null;
        public NewDbWindow()
        {
            InitializeComponent();
        }

        private void BtGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.Text = PasswordGenerator.GeneratePassword(8);
        }

        private void BtOpenFlord_Click(object sender, RoutedEventArgs e)
        {
            using FolderBrowserDialog dialog = new()
            {
                RootFolder = System.Environment.SpecialFolder.MyDocuments
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbFloder.Text = dialog.SelectedPath;
            }
        }
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length == 0 || tbFloder.Text.Length == 0 || tbPassword.Text.Length == 0)
            {
                AdonisUI.Controls.MessageBox.Show(CustomMessage.GetInformationBoxModel("请完整填写信息。", "提示信息"));
                return;
            }
            if (!Directory.Exists(tbFloder.Text))
            {
                AdonisUI.Controls.MessageBox.Show(CustomMessage.GetInformationBoxModel("位置文件夹不存在。", "提示信息"));
                return;
            }

            if (!tbFloder.Text.EndsWith("\\"))
                tbFloder.Text += "\\";

            DbConfig dbConfig = new()
            {
                Name = tbName.Text,
                Password = tbPassword.Text,
                IsUseGoogleAuthenticator = false,
                GoogleAuthenticatorKey = PasswordGenerator.GeneratePassword(8, true, false, true, false)
            };

            dbFileName = DbFile.Save(dbConfig, tbFloder.Text + dbConfig.Name);
            if (dbFileName != null)
            {
                //AdonisUI.Controls.MessageBox.Show(CustomMessage.GetInformationBoxModel("文件建立成功！", "提示信息"));
                this.Close();
            }
            else
                AdonisUI.Controls.MessageBox.Show(CustomMessage.GetErrorBoxModel("文件建立失败！", "提示信息"));
        }

        private void AdonisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbName.Focus();
        }
    }
}
