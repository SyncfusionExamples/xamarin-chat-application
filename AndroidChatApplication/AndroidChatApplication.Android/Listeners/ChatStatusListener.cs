using System.Collections.Generic;
using Com.Twilio.Chat;

namespace AndroidChatApplication.Droid.Listeners
{
    public class ChatStatusListener : StatusListener
    {
        private Channel channel;
        public ChatStatusListener(Channel channel)
        {
            this.channel = channel;
        }
        public override void OnSuccess()
        {
            channel.Messages.GetLastMessages(50, 
                new MessagesListCallBackListener<List<Message>>());
        }
    }
}