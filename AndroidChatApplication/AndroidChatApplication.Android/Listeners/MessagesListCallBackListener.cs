using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AndroidChatApplication.Models.Chat;
using AndroidChatApplication.Views;
using Com.Twilio.Chat;
using Xamarin.Forms;

namespace AndroidChatApplication.Droid.Listeners
{
    public class MessagesListCallBackListener<T> : CallbackListener<IList<Message>>
    {
        public MessagesListCallBackListener()
        {
            if (MainActivity.IsChannelMessageInitialized == false)
            {
                MainActivity.ChannelMessages = new ObservableCollection<IList<Message>>();
                MainActivity.IsChannelMessageInitialized = true;
            }
        }

        public override void OnSuccess(IList<Message> result)
        {
            if (result.Count > 0)
            {
                MainActivity.ChannelMessages.Add(result);
            }
            foreach (var message in result)
            {
                var messageBody = message.MessageBody;
            }
            MainActivity.ChannelsCount++;
            if (MainActivity.GeneralChannelDescriptor != null && MainActivity.GeneralChannelDescriptor.Count == MainActivity.ChannelsCount)
            {
                App.IsButtonVisible = true;
                Application.Current.MainPage = new NavigationPage(new StartPage());
            }
        }
    }
}