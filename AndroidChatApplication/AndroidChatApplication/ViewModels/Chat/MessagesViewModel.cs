using AndroidChatApplication.Helpers;
using AndroidChatApplication.Views.Chat;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AndroidChatApplication.ViewModels.Chat
{
    public class MessagesViewModel : BaseViewModel
    {
        private ObservableCollection<object> messages;

        /// <summary>
        /// current user of chat.
        /// </summary>
        private Author currentUser;

        private string profileName;

        ITwilioMessenger twilioMessenger;

        /// <summary>
        /// Indicates the typing indicator visibility. 
        /// </summary>
        private bool showTypingIndicator;

        /// <summary>
        /// Chat typing indicator.
        /// </summary>
        private ChatTypingIndicator typingIndicator;

        /// <summary>
        /// Gets or sets the command that is executed when the back button is clicked.
        /// </summary>
        public Command BackCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the send button is clicked.
        /// </summary>
        public Command SendMessageCommand { get; set; }

        public MessagesViewModel()
        {
            twilioMessenger = DependencyService.Get<ITwilioMessenger>();
            this.Messages = new ObservableCollection<object>();
            this.CurrentUser = new Author() { Name = App.MyIdentity, Avatar = "name.png" };
            this.GenerateMessages();

            if (!string.IsNullOrEmpty(App.TypingMessage))
            {
                this.TypingIndicator = new ChatTypingIndicator();
                this.TypingIndicator.Authors.Add(new Author() { 
                    Name = App.ChatMessages.Count > 0 ? 
                    App.ChatMessages[0].Identity : "", Avatar = "name.png" });
                this.TypingIndicator.AvatarViewType = AvatarViewType.Image;
                this.TypingIndicator.Text = App.TypingMessage;
                this.ShowTypingIndicator = true;
            }
            else
            {
                this.ShowTypingIndicator = false;
            }

            this.BackCommand = new Command(this.BackButtonClicked);
            this.SendMessageCommand = new Command(this.SendButtonClicked);
        }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public Author CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }

        /// <summary>
        /// Gets or sets the message conversation.
        /// </summary>
        public ObservableCollection<object> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
            }
        }
        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string ProfileName
        {
            get
            {
                return this.profileName;
            }

            set
            {
                this.profileName = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Chat typing indicator value.
        /// </summary>
        public ChatTypingIndicator TypingIndicator
        {
            get
            {
                return this.typingIndicator;
            }
            private set
            {
                this.typingIndicator = value;
                RaisePropertyChanged("TypingIndicator");
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the typing indicator is visible or not.
        /// </summary>
        public bool ShowTypingIndicator
        {
            get
            {
                return this.showTypingIndicator;
            }
            set
            {
                this.showTypingIndicator = value;
                RaisePropertyChanged("ShowTypingIndicator");
            }
        }

        /// <summary>
        /// Property changed handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        /// <param name="propName">changed property name<param>
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void GenerateMessages()
        {
            ProfileName = App.FriendlyName;
            if (App.ChatMessages != null && App.ChatMessages.Count > 0)
            {
                foreach (var message in App.ChatMessages)
                {
                    if (IsValidURL(message.Message))
                    {
                        this.Messages.Add(new HyperlinkMessage()
                        {
                            Author = message.IsReceived ? new Author() { Avatar = "name.png", Name = message.Identity } : CurrentUser,
                            Url = message.Message,
                            DateTime = message.Time,
                        });
                    }
                    else
                    {
                        this.Messages.Add(new TextMessage()
                        {
                            Author = message.IsReceived ? new Author() { Avatar = "name.png", Name = message.Identity } : CurrentUser,
                            Text = message.Message,
                            DateTime = message.Time,
                        });
                    }
                }
            }
            if(Messages.Count > 0)
            {
                var messageIndex = Messages.Count - 1;
                App.LastMessage = Messages[messageIndex];
            }
        }

        /// <summary>
        /// Invoked when the back button is clicked.
        /// </summary>
        /// <param name="obj">The object</param>
        private void BackButtonClicked(object obj)
        {
            Application.Current.MainPage = new NavigationPage(new RecentChatPage());
        }

        /// <summary>
        /// Invoked when the send button is clicked.
        /// </summary>
        /// <param name="obj">The object</param>
        private void SendButtonClicked(object selectedItem)
        {
            var message = selectedItem.GetType().GetProperty("Message").GetValue(selectedItem, null);
            var newMessage = message.GetType().GetProperty("Text").GetValue(message, null).ToString();
            twilioMessenger.SendMessage(newMessage, Messages, CurrentUser);
        }

        public void GetTypingIndication()
        {
            if(App.TypingMessage != null)
            {
                showTypingIndicator = true;
            }
            else
            {
                showTypingIndicator = false;
            }
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
