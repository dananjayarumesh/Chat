﻿using System.Windows.Controls;
using System.Windows.Input;
using ChatClient.ViewMediator;
using ChatClient.ViewModels.ChatWindowViewModel;
using ChatClient.ViewModels.MainWindowViewModel;

namespace ChatClient.Views
{
    /// <summary>
    /// Interaction logic for UserListWindow.xaml
    /// </summary>
    public partial class UserListWindow
    {
        public UserListWindow()
        {
            InitializeComponent();

            Mediator.Instance.Register(ViewName.ChatWindow, ShowChatWindow);
        }

        /// <summary>
        /// I can't find a good way of doing this directly with MVVM and bindings.
        /// Succumbed to creating the click event in the code-behind and then delegating the work off to the viewmodel.
        /// </summary>
        /// <param name="sender">The textblock clicked on</param>
        /// <param name="e">Mouse events for the selected textblock</param>
        private void OnNewUserSelection(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                var userClicked = (StackPanel) sender;
                var participantId = (int) userClicked.Tag;

                var viewmodel = DataContext as UserListViewModel;

                if (viewmodel != null)
                {
                    viewmodel.StartNewSingleUserConversation(participantId);
                }
            }
        }

        private static void ShowChatWindow(object viewModel)
        {
            var view = new ChatWindow((ChatWindowViewModel)viewModel);
            view.Show();
        }
    }
}