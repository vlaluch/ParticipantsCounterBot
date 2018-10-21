using ParticipantsCounter.Core.Entities;
using Telegram.Bot.Types;

namespace ParticipantsCounter.App.Infrastructure
{
    public static class MessagesConverter
    {
        public static ChatMessage ToChatMessage(this Message message)
        {
            return new ChatMessage
            {
                Text = message.Text,
                AuthorName = GetUserName(message.From),
                ChatName = GetChatName(message.Chat)
            };
        }

        private static string GetUserName(User user)
        {
            var userNamePart = !string.IsNullOrEmpty(user.Username) ? $" ({user.Username})" : string.Empty;

            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}{userNamePart}";
            }
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                return $"{user.FirstName}{userNamePart}";
            }
            if (!string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.LastName}{userNamePart}";
            }

            return !string.IsNullOrEmpty(user.Username) ? user.Username : "Unknown user";
        }

        private static string GetChatName(Chat chat)
        {
            return !string.IsNullOrEmpty(chat.Title) ? chat.Title : $"{chat.FirstName} {chat.LastName}";
        }
    }
}