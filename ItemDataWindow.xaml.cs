using AdonisUI.Controls;
using PassKeeper.Model;
using PassKeeper.Security;
using PassKeeper.Common;
using System.Windows;

namespace PassKeeper
{
    /// <summary>
    /// ItemData.xaml 的交互逻辑
    /// </summary>
    public partial class ItemDataWindow : AdonisWindow
    {
        public ItemData itemData = null;
        public bool isChanged = false;
        public ItemDataWindow()
        {
            InitializeComponent();
        }
        private void AdonisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (itemData != null)
            {
                tbTitle.Text = itemData.Title;
                tbUserName.Text = itemData.UserName;
                tbPassword.Text = itemData.Password;
                tbURL.Text = itemData.URL;
                tbNotes.Text = itemData.Notes;
            }
            tbTitle.Focus();
        }

        private void BtGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.Text = PasswordGenerator.GeneratePassword(8);
        }
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbTitle.Text.Length == 0
                || tbPassword.Text.Length == 0
                || tbUserName.Text.Length == 0)
            {
                AdonisUI.Controls.MessageBox.Show(CustomMessage.GetInformationBoxModel("请完整填写信息。", "提示信息"));
                return;
            }
            itemData = new ItemData();
            itemData.Title = tbTitle.Text;
            itemData.UserName = tbUserName.Text;
            itemData.Password = tbPassword.Text;
            itemData.URL = tbURL.Text;
            itemData.Notes = tbNotes.Text;
            isChanged = true;
            this.Close();
        }
    }
}
