using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidChatApplication.ViewModels.Chat;
using AndroidChatApplication.Views.Chat;
using Com.Twilio.Chat;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;
using Message = Com.Twilio.Chat.Message;

namespace AndroidChatApplication.Droid.Listeners
{
    public class ChannelListener : Java.Lang.Object, IChannelListener
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnMemberAdded(Member member)
        {
            
        }

        public void OnMemberDeleted(Member member)
        {
            throw new NotImplementedException();
        }

        public void OnMemberUpdated(Member member, Member.UpdateReason reason)
        {
            throw new NotImplementedException();
        }

        public void OnMessageAdded(Message message)
        {
            App.IsMessageAdded = false;

            if (MainActivity.GeneralChannel == message.Channel)
            {
                MessagingCenter.Send(this, "NewMessage", message);
                foreach (var channelMessage in MainActivity.ChannelMessages)
                {
                    foreach (var messages in channelMessage)
                    {
                        if (messages.Channel == message.Channel)
                        {
                            channelMessage.Add(message);
                            return;
                        }
                    }
                }
            }
            else
            {
                foreach (var channelMessage in MainActivity.ChannelMessages)
                {
                    foreach (var messages in channelMessage)
                    {
                        if (messages.Channel == message.Channel)
                        {
                            channelMessage.Add(message);
                            return;
                        }
                    }
                }
            }
        }

        public void OnMessageDeleted(Message message)
        {
            throw new NotImplementedException();
        }

        public void OnMessageUpdated(Message message, Message.UpdateReason reason)
        {
            throw new NotImplementedException();
        }

        public void OnSynchronizationChanged(Channel channel)
        {
            if (channel.SynchronizationStatus.Equals(Channel.ChannelSynchronizationStatus.All))
            {
                //channel.SynchronizationChanged -= OnSynchronizationChanged;
            }
        }

        public void OnTypingEnded(Channel channel, Member member)
        {
            
        }

        public void OnTypingStarted(Channel channel, Member member)
        {
            if (channel == MainActivity.GeneralChannel)
            {
                App.TypingMessage = member.Identity + " is typing...";
                Application.Current.MainPage = new NavigationPage(new MessagePage());
                Task.Delay(10000);
            }

        }
    }
}