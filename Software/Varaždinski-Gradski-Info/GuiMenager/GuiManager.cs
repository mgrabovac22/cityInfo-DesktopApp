using System;
using System.Windows;
using System.Windows.Controls;
using Varaždinski_Gradski_Info;
using static BusinessLogicLayer.UsersService;

namespace PresentationLayer
{
    internal static class GuiManager
    {
        public static Window MainWindow { get; private set; }

        private static UserControl currentContent;
        private static UserControl previousContent;

        public static void OpenContent(UserControl userControl)
        {
            if (MainWindow is AdminMainWindow)
            {
                (MainWindow as AdminMainWindow).contentPanel.Content = userControl;
            }
            else if (MainWindow is UsersMainWindow)
            {
                (MainWindow as UsersMainWindow).contentPanel.Content = userControl;
            }
            else if (MainWindow is EmployeeMainWindow)
            {
                (MainWindow as EmployeeMainWindow).contentPanel.Content = userControl;
            }
        }

        public static void CloseContent()
        {
            OpenContent(previousContent);
        }

        internal static void SetWindow(Window window)
        {
            MainWindow = window;
        }
    }
}
