using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndroidChatApplication.Helpers;
using AndroidChatApplication.Droid.Listeners;
using AndroidChatApplication.Models.Chat;
using Com.Twilio.Chat;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using AndroidChatApplication.ViewModels.Chat;
using AndroidChatApplication.Views.Chat;
using System.Linq;
using System.Globalization;
using Syncfusion.XForms.Chat;
using System.Runtime.CompilerServices;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidChatApplication.Droid.Helpers.TwilioMessenger))]
namespace AndroidChatApplication.Droid.Helpers
{
    public class TwilioMessenger : Java.Lang.Object, ITwilioMessenger
    {
        private ChatClient chatClient;

        public Com.Twilio.Chat.Message.Options MessageOptions;
        public Channel chatChannel { get; private set; }

        private List<Channel> channelLists;

        public TwilioMessenger()
        {
            App.ChatMessages = new ObservableCollection<ChatMessage>();
            App.ChannelDetails = new ObservableCollection<ChatDetail>();
            channelLists = new List<Channel>();
        }

        /// <summary>
        /// Setup the chat client
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetupAsync()
        {
            var task = new TaskCompletionSource<bool>();

            //Get token to access the Twilio Account
            var token = await TwilioHelper.GetTokenAsync();
            
            // Initialization of chat client
            ChatClient.Create(Android.App.Application.Context, 
                token, 
                (new ChatClient.ClientProperties.Builder()).CreateProperties(), 
                 new ChatClientCallBackListener(this));
            Task.Delay(1000);
            return await task.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Create the channel with chat client.
        /// </summary>
        /// <param name="chatClient"></param>
        public void CreateChatChannel(ChatClient result)
        {
            chatClient = result;
            MainActivity.GeneralChatClient = result;

            var friendlyName = "SyncfusionChannel";
            var channelType = Channel.ChannelType.Public;
            // Create channels with channel type and friendly name.
            chatClient.Channels.CreateChannel(friendlyName, channelType, 
                new CreateChannelCallBackListener<Channel>(this));
        }

        /// <summary>
        /// Join the Channel.
        /// </summary>
        /// <param name="result"></param>
        public void JoinChannel(Channel result)
        {
            chatChannel = result;
            MainActivity.GeneralChannel = result;
            chatChannel.Join(new ChatStatusListener(chatChannel));
        }

        /// <summary>
        /// Get the channel Messages.
        /// </summary>
        /// <param name="result"></param>
        public void GetChannelMessagesList(Channel result)
        {
            chatChannel = result;
            MainActivity.GeneralChannel = result;
            MainActivity.GeneralChannel.AddListener(MainActivity.channelListener);
            chatChannel.Messages.GetLastMessages(50, new MessagesListCallBackListener<List<Message>>());
        }

        public void SendMessage(string text, ObservableCollection<object> ChatMessageInfo, Author CurrentUser)
        {
            chatChannel = MainActivity.GeneralChannel;
            MessageOptions = Message.InvokeOptions().WithBody(text);
            chatChannel.Messages.SendMessage(MessageOptions, new MessageCallBackListener<Message>());

            MessagingCenter.Subscribe<ChannelListener, Message>(this, "NewMessage", (sender, args) =>
            {
                if (args != null && !App.IsMessageAdded)
                {
                    App.ChatMessages.Add(new ChatMessage
                    {
                        Message = args.MessageBody,
                        Time = Convert.ToDateTime(args.DateCreated),
                        IsReceived = args.Author != MainActivity.GeneralChatClient.MyIdentity,
                        Identity = args.Author
                    });
                    App.IsMessageAdded = true;
                    App.TypingMessage = null;
                    Application.Current.MainPage = new NavigationPage(new MessagePage());
                }
                if(args.Author != MainActivity.GeneralChatClient.MyIdentity)
                {
                    IsMemberTyping();
                }
                else
                {
                    App.TypingMessage = null;
                }
            });
        }

        public bool GetChatMessages()
        {
            var success = false;
            App.ChatMessages = new ObservableCollection<ChatMessage>();
            var channels = MainActivity.PublicChannelsList;
            var channelMessages = MainActivity.ChannelMessages;
            foreach(var channel in channels)
            {
                if(channel.FriendlyName == App.FriendlyName)
                {
                    MainActivity.GeneralChannel = channel;
                    if (channelMessages.Count > 0)
                    {
                        foreach (var channelMessage in channelMessages)
                        {
                            foreach (var message in channelMessage)
                            {
                                if (message.Channel.FriendlyName == App.FriendlyName)
                                {
                                    success = true;
                                    App.ChatMessages.Add(new ChatMessage
                                    {
                                        Message = message.MessageBody,
                                        Time = Convert.ToDateTime(message.DateCreated),
                                        IsReceived = message.Author != MainActivity.GeneralChatClient.MyIdentity,
                                        Identity = message.Author
                                    });
                                }
                                success = true;
                            }
                        }
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            App.TypingMessage = null;
            return success;
        }

        public bool GetAllPublicChannels()
        {
            var success = false;
            App.ChannelDetails = new ObservableCollection<ChatDetail>();
            channelLists = new List<Channel>();
            var channels = MainActivity.PublicChannelsList;
            var channelMessages = MainActivity.ChannelMessages;
            var selectedChannel = channelMessages.Select(a => a.Select(b => b.Channel).First()).ToList();
            if (channelMessages == null || channelMessages.Count == 0)
            {
                foreach(var channel in channels)
                {
                    channelLists.Add(channel);
                    App.ChannelDetails.Add(new ChatDetail
                    {
                        SenderName = channel.FriendlyName,
                        MessageType = "Text",
                        Message = "",
                        Time = "",
                        NotificationType = "New"
                    });
                    success = true;
                }
            }
            else
            {
                foreach (var channelMessage in channelMessages)
                {
                    foreach (var channel in channels)
                    {
                        var messageCount = channelMessage.Count;
                        var createdTime = Convert.ToDateTime(channelMessage[messageCount - 1].DateCreated);
                        var messageTime = DateTimeToStringConverter(createdTime);
                        if (channelMessage[0].Channel == channel)
                        {
                            channelLists.Add(channel);
                            App.ChannelDetails.Add(new ChatDetail
                            {
                                SenderName = channel.FriendlyName,
                                MessageType = "Text",
                                Message = channelMessage[messageCount - 1].MessageBody,
                                Time = messageTime,
                                NotificationType = "New"
                            });
                            success = true;
                        }
                        else if (!channelLists.Contains(channel) && !selectedChannel.Contains(channel))
                        {
                            channelLists.Add(channel);
                            App.ChannelDetails.Add(new ChatDetail
                            {
                                SenderName = channel.FriendlyName,
                                MessageType = "Text",
                                Message = "",
                                Time = "",
                                NotificationType = "Viewed"
                            });
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public string DateTimeToStringConverter(DateTime dateTime)
        {
            var currentTime = DateTime.Now;

            if (dateTime.Day == currentTime.Day)
            {
                return "Today";
            }

            return dateTime.Day == currentTime.AddDays(-1).Day ? "Yesterday" : dateTime.ToString("MMMM dd, yyyy", CultureInfo.CurrentCulture);
        }

        public void IsMemberTyping()
        {
            MainActivity.GeneralChannel.Typing();
        }

        public bool IsValidURL(string uriName)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
