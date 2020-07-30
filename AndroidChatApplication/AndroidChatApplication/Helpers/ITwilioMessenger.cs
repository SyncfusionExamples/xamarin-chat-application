using AndroidChatApplication.Models.Chat;
using Syncfusion.XForms.Chat;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AndroidChatApplication.Helpers
{
    public interface ITwilioMessenger
    {
        void SendMessage(string text, ObservableCollection<object> ChatMessageInfo, Author CurrentUser);

        bool GetChatMessages();

        bool GetAllPublicChannels();
    }
}
