using AndroidChatApplication.ViewModels.Chat;
using AndroidChatApplication.Views.Chat;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidChatApplication.Behaviors.Chat
{
    public class MessagePageBehavior : Behavior<MessagePage>
    {

        private MessagesViewModel viewModel;

        private SfChat sfChat;
        public MessagePageBehavior()
        {
            viewModel = new MessagesViewModel();
        }

        protected override void OnAttachedTo(MessagePage bindable)
        {
            this.sfChat = bindable.FindByName<SfChat>("sfChat");
            //this.viewModel = bindable.FindByName<MessagesViewModel>("viewModel");
            this.viewModel.Messages.CollectionChanged += Messages_CollectionChanged;

            base.OnAttachedTo(bindable);
        }

        private async void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var chatItem in e.NewItems)
                {
                    TextMessage textMessage = chatItem as TextMessage;
                    if (textMessage != null && textMessage.Author == this.viewModel.CurrentUser)
                    {
                        (sender as SfChat).ShowOutgoingMessageAvatar = false;
                    }
                    else
                    {
                        await Task.Delay(50);
                        this.sfChat.ScrollToMessage(chatItem);
                    }
                }
            }

        }

        protected override void OnDetachingFrom(MessagePage bindable)
        {
            this.viewModel.Messages.CollectionChanged -= Messages_CollectionChanged;
            this.sfChat = null;
            this.viewModel = null;
            base.OnDetachingFrom(bindable);
        }
    }
}
