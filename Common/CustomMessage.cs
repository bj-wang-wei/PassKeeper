using AdonisUI.Controls;

namespace PassKeeper.Common
{
    public static class CustomMessage
    {
        public static MessageBoxModel GetInformationBoxModel(string text, string caption)
        {
            return new MessageBoxModel
            {
                Text = text,
                Caption = caption,
                Icon = MessageBoxImage.Information,
                Buttons = new[]
                {
                        MessageBoxButtons.Ok("确定")
                }
            };
        }
        public static MessageBoxModel GetQuestionBoxModel(string text, string caption)
        {
            return new MessageBoxModel
            {
                Text = text,
                Caption = caption,
                Icon = MessageBoxImage.Question,
                Buttons = new[]
                {
                        MessageBoxButtons.Yes("是"),
                        MessageBoxButtons.No("否")
                }
            };
        }
        public static MessageBoxModel GetErrorBoxModel(string text, string caption)
        {
            return new MessageBoxModel
            {
                Text = text,
                Caption = caption,
                Icon = MessageBoxImage.Error,
                Buttons = new[]
                {
                        MessageBoxButtons.Ok("确定")
                }
            };
        }






    }
}
