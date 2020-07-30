using AndroidChatApplication.Droid.Helpers;
using Com.Twilio.Chat;
using System.Threading.Tasks;

namespace AndroidChatApplication.Droid.Listeners
{
    public class ChatClientCallBackListener : CallbackListener<ChatClient>
    {
        private TwilioMessenger twilioMessenger;
        public ChatClientCallBackListener(TwilioMessenger twilioMessenger)
        {
            this.twilioMessenger = twilioMessenger;
            Task.Delay(1000);
        }
        public override void OnSuccess(ChatClient result)
        {
            //To create channel from chatClient.
            //twilioMessenger.CreateChatChannel(result);
            MainActivity.GeneralChatClient = result;
            App.MyIdentity = MainActivity.GeneralChatClient.MyIdentity;
            
            //Get Public Channels List.
            result.Channels.GetPublicChannelsList(new ChannelDescriptorCallBackListener<Paginator<ChannelDescriptor>>(twilioMessenger));
        }

        void ChannelSynchronizationChanged(object sender, SynchronizationChangedEventArgs args)
        {
            if (args.Channel.SynchronizationStatus.Equals(Channel.ChannelSynchronizationStatus.All))
            {
                args.Channel.SynchronizationChanged -= ChannelSynchronizationChanged;
            }
        }

        public override void OnError(ErrorInfo errorInfo)
        {
        }
    }
}