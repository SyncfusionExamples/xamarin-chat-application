using AndroidChatApplication.Helpers;
using AndroidChatApplication.Views.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AndroidChatApplication.ViewModels
{
    public class StartPageViewModel : BaseViewModel
    {
        private Command connectCommand;

        /// <summary>
        /// Gets the command that is executed when click the button.
        /// </summary>
        public Command ConnectCommand
        {
            get { return this.connectCommand ?? (this.connectCommand = new Command(this.ConnectChannels)); }
        }

        public StartPageViewModel()
        {

        }

        /// <summary>
        /// Invoked when an click a button.
        /// </summary>
        private void ConnectChannels(object obj)
        {
            Application.Current.MainPage = new NavigationPage(new RecentChatPage());
        }
    }
}
