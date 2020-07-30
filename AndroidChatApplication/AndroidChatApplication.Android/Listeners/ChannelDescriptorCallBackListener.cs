using AndroidChatApplication.Droid.Helpers;
using Com.Twilio.Chat;
using Xamarin.Forms;

namespace AndroidChatApplication.Droid.Listeners
{
    public class ChannelDescriptorCallBackListener<T> : CallbackListener<Paginator<ChannelDescriptor>>
    {
        private TwilioMessenger twilioMessenger;

        public ChannelDescriptorCallBackListener(TwilioMessenger twilioMessenger)
        {
            this.twilioMessenger = twilioMessenger;
        }
        public override void OnSuccess(Paginator<ChannelDescriptor> result)
        {
            MainActivity.GeneralChannelDescriptor = result.Items;
            foreach (var channel in result.Items)
            {
                channel.GetChannel(new CreateChannelCallBackListener<Channel>(twilioMessenger));
            }
        }
    }
}