using System;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace AndroidChatApplication.Models.Chat
{
    /// <summary>
    /// Model for chat message 
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ChatMessage : INotifyPropertyChanged
    {
        #region Fields

        private string message;

        private DateTime time;

        private string imagePath;

        private string identity;

        #endregion

        #region Event

        /// <summary>
        /// The declaration of property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Gets or sets the message sent/received time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.OnPropertyChanged("Time");
            }
        }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                this.OnPropertyChanged("ImagePath");
            }
        }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public string Identity
        {
            get
            {
                return this.identity;
            }

            set
            {
                this.identity = value;
                this.OnPropertyChanged("Identity");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the message is received or sent.
        /// </summary>
        public bool IsReceived { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when property value is changed.
        /// </summary>
        /// <param name="property">property name</param>
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
