using AndroidChatApplication.Helpers;
using AndroidChatApplication.Views.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidChatApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            if(App.IsButtonVisible)
            {
                Connect_Button.IsVisible = true;
                Connect_label.Text = "Connected";
            }
        }
    }
}