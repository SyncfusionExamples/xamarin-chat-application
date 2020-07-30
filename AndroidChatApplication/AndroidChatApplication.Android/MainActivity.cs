using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Threading.Tasks;
using Com.Twilio.Chat;
using AndroidChatApplication.Helpers;
using AndroidChatApplication.Droid.Listeners;
using Xamarin.Forms;
using AndroidChatApplication.Droid.Helpers;
using Message = Com.Twilio.Chat.Message;
using AndroidChatApplication.Models.Chat;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AndroidChatApplication.Droid
{
    [Activity(Label = "AndroidChatApplication", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private TwilioMessenger twilioMessenger;
        public static bool IsChannelMessageInitialized;
        public static Channel GeneralChannel;
        public static ChatClient GeneralChatClient;
        public static IList<ChannelDescriptor> GeneralChannelDescriptor;
        public static ObservableCollection<Channel> PublicChannelsList;
        public static ObservableCollection<IList<Message>> ChannelMessages;
        public static IChannelListener channelListener;
        public static int ChannelsCount = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            twilioMessenger = new TwilioMessenger();

            twilioMessenger.SetupAsync();

            channelListener = new ChannelListener();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}