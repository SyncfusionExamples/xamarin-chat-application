using AndroidChatApplication.Models.Chat;
using AndroidChatApplication.Views;
using AndroidChatApplication.Views.Chat;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidChatApplication
{
    public partial class App : Application, INotifyPropertyChanged
    {
        public static ObservableCollection<ChatDetail> ChannelDetails { get; set; }
        public static ObservableCollection<ChatMessage> ChatMessages { get; set; }
        public static string TypingMessage { get; set; }
        public static object LastMessage { get; set; }
        public static bool IsMessageAdded { get; set; }
        public static bool IsButtonVisible { get; set; }
        public static string FriendlyName { get; set; }
        public static string MyIdentity { get; set; }
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        public App()
        {
            InitializeComponent();

            MainPage = new StartPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
