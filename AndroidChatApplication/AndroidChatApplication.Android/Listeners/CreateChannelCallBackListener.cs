using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidChatApplication.Droid.Helpers;
using Com.Twilio.Chat;

namespace AndroidChatApplication.Droid.Listeners
{
    public class CreateChannelCallBackListener<T> : CallbackListener<Channel>
    {
        private TwilioMessenger twilioMessenger;

        public CreateChannelCallBackListener(TwilioMessenger twilioMessenger)
        {
            this.twilioMessenger = twilioMessenger;
            MainActivity.PublicChannelsList = new ObservableCollection<Channel>();
        }

        public override void OnSuccess(Channel result)
        {
            if (result != null)
            {
                MainActivity.GeneralChannel = result;
                MainActivity.PublicChannelsList.Add(result);
                if (result.Members != null && result.Members.MembersList.Any(a => a.Identity == MainActivity.GeneralChatClient.MyIdentity))
                {
                    this.twilioMessenger.GetChannelMessagesList(result);
                }
                else
                {
                    this.twilioMessenger.JoinChannel(result);
                }
            }
        }
    }
}