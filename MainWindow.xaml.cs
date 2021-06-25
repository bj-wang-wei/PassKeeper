using AdonisUI.Controls;
using Microsoft.Win32;
using PassKeeper.FileOperator;
using PassKeeper.Model;
using PassKeeper.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace PassKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        private DbConfig dbConfig = new();
        private List<ItemData> itemDatas = new();
        //private bool isChanged = false;
        private string dbOpenFileName;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ModifyTitle(string name)
        {
            if (name.Length != 0)
                this.Title = "Double U 密码管家 - " + name;
            else
                this.Title = "Double U 密码管家";
        }
        private void ModifyStatus(string msg)
        {
            lbFileName.Content = msg;
        }
        private void CloseDbFile()
        {
            if (itemDatas != null)
                itemDatas.Clear();
            dbConfig = null;
            ModifyTitle("");
            ModifyStatus("");
            RefreshItemDatas();
        }
        private void SaveDbFile(bool isCloseDbFile)
        {
            if (this.btSave.IsEnabled)
            {
                if (isCloseDbFile)
                {
                    if (AdonisUI.Controls.MessageBox.Show(CustomMessage.GetQuestionBoxModel("库文件尚未保存，是否保存？", "提示信息")) == AdonisUI.Controls.MessageBoxResult.No)
                        return;
                }
                DbFile.Save(dbConfig, itemDatas, dbOpenFileName);
                EnableSaveButton(false);
            }
        }
        private void EnableSaveButton(bool isChanged)
        {
            if (isChanged)
            {
                this.btSave.IsEnabled = true;
                this.imSave.Opacity = 1;
            }
            else
            {
                this.btSave.IsEnabled = false;
                this.imSave.Opacity = 0.3;
            }
        }
        private void EnableButton(bool isEnable, double opacity)
        {
            this.btPass.IsEnabled = isEnable;
            this.imPass.Opacity = opacity;
            this.btNew.IsEnabled = isEnable;
            this.imNew.Opacity = opacity;
            this.btModify.IsEnabled = isEnable;
            this.imModify.Opacity = opacity;
            this.btDelete.IsEnabled = isEnable;
            this.imDelete.Opacity = opacity;
        }
        private void RefreshItemDatas()
        {
            int currentIndex = 0;
            if (dgItemData.Items.Count > 0)
                currentIndex = dgItemData.SelectedIndex;
            if (itemDatas != null)
            {
                dgItemData.DataContext = itemDatas;
                dgItemData.Items.Refresh();
                if (dgItemData.Items.Count > 0)
                    dgItemData.SelectedIndex = currentIndex;
            }
        }
        private bool LoadDbFromFile(string dbFileName)
        {
            dbConfig = DbFile.ReadDbCofnig(dbFileName);
            if (dbConfig != null)
            {
                itemDatas = DbFile.ReadItemDatas(dbConfig, dbFileName);
                ModifyTitle(dbConfig.Name);
                ModifyStatus(dbFileName);
                dbOpenFileName = dbFileName;
                return true;
            }
            else
                return false;
        }
        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            SaveDbFile(true);
            this.Close();
        }
        private void BtNewDb_Click(object sender, RoutedEventArgs e)
        {
            SaveDbFile(true);
            CloseDbFile();
            NewDbWindow newDb = new();
            newDb.ShowDialog();
            if (newDb.dbFileName != null)
            {
                if (LoadDbFromFile(newDb.dbFileName))
                {
                    EnableButton(true, 1);
                    RefreshItemDatas();
                }
                else
                    AdonisUI.Controls.MessageBox.Show(CustomMessage.GetErrorBoxModel("文件建立失败！", "提示信息"));
            }
        }
        private void BtOpenDb_Click(object sender, RoutedEventArgs e)
        {
            SaveDbFile(true);
            string exName = DbFile.fileExtension.ToUpper().Substring(DbFile.fileExtension.Length - 4, 4);
            var openFileDialog = new OpenFileDialog
            {
                Filter = string.Format("密码库文件 {0}(*.{1})|*.{2}", exName, exName, exName),
                Multiselect = false,
                CheckFileExists = false
            };
            if (openFileDialog.ShowDialog() == true)
            {
                CloseDbFile();
                if (LoadDbFromFile(openFileDialog.FileName))
                {
                    PasswordWindow password = new()
                    {
                        dbConfig = dbConfig
                    };
                    password.ShowDialog();
                    if (password.isSuccess)
                    {
                        EnableButton(true, 1);
                        RefreshItemDatas();
                    }
                    else
                    {
                        CloseDbFile();
                    }
                }
                else
                {
                    AdonisUI.Controls.MessageBox.Show(CustomMessage.GetErrorBoxModel("文件打开失败！", "提示信息"));
                    EnableButton(false, 0.3);
                }
            }
        }
        private void BtPass_Click(object sender, RoutedEventArgs e)
        {
            SetDbConfigWindow setDbConfig = new();
            setDbConfig.dbConfig = DbFile.ReadDbCofnig(dbOpenFileName);
            setDbConfig.ShowDialog();
            if (setDbConfig.isChanged)
            {
                EnableSaveButton(true);
                dbConfig = setDbConfig.dbConfig;
                ModifyTitle(dbConfig.Name);
            }
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            SaveDbFile(false);
        }
        private void BtNew_Click(object sender, RoutedEventArgs e)
        {
            ItemDataWindow itemDataWindow = new();
            itemDataWindow.ShowDialog();
            if (itemDataWindow.isChanged)
            {
                if (itemDatas == null)
                    itemDatas = new List<ItemData>();
                itemDatas.Add(itemDataWindow.itemData);
                EnableSaveButton(true);
                RefreshItemDatas();
            }
        }
        private void BtModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
            {
                return;
            }
            ItemDataWindow itemDataWindow = new();
            itemDataWindow.itemData = itemDatas[dgItemData.SelectedIndex];
            itemDataWindow.Title = "编辑密码保存项";
            itemDataWindow.ShowDialog();
            if (itemDataWindow.isChanged)
            {
                itemDatas[dgItemData.SelectedIndex].Title = itemDataWindow.itemData.Title;
                itemDatas[dgItemData.SelectedIndex].UserName = itemDataWindow.itemData.UserName;
                itemDatas[dgItemData.SelectedIndex].Password = itemDataWindow.itemData.Password;
                itemDatas[dgItemData.SelectedIndex].URL = itemDataWindow.itemData.URL;
                itemDatas[dgItemData.SelectedIndex].Notes = itemDataWindow.itemData.Notes;
                EnableSaveButton(true);
                RefreshItemDatas();
            }
        }
        private void DgItemData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtModify_Click(sender, null);
        }
        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
                return;
            if (AdonisUI.Controls.MessageBox.Show(CustomMessage.GetQuestionBoxModel("确定要删除该数据吗？", "提示信息")) == AdonisUI.Controls.MessageBoxResult.No)
                return;
            itemDatas.RemoveAt(dgItemData.SelectedIndex);
            EnableSaveButton(true);
            RefreshItemDatas();
        }
        private void DgItemData_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
            {
                return;
            }
            ContextMenu cm = this.FindResource("contexMenu") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
        private void MiCopyUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
            {
                return;
            }
            Clipboard.SetText(itemDatas[dgItemData.SelectedIndex].UserName);
        }
        private void MiCopyPassword_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
            {
                return;
            }
            Clipboard.SetText(itemDatas[dgItemData.SelectedIndex].Password);
        }
        private void MiCopyUrl_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemData.SelectedIndex < 0)
            {
                return;
            }
            Clipboard.SetText(itemDatas[dgItemData.SelectedIndex].URL);
        }
    }
}
