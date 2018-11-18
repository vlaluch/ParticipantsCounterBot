using ParticipantsCounter.Core.Entities;
using System.Linq;

namespace ParticipantsCounter.Core.Infrastructure
{
    public class MessageParser
    {
        private static readonly string[] _increaseCountSymbols = new[] { "+", };
        private static readonly string[] _decreaseCountSymbols = new[] { "-" };
        private static readonly string[] _notSureSymbols = new[] { "+-", "+/-", "+\\-", "±" };
        private static readonly string _startCountMessage = "+ на завтра";

        public static CommandType GetCommandTypeFromMessage(ChatMessage message)
        {
            if (IsNotSureMessage(message.Text))
            {
                return CommandType.NotSure;
            }
            if (IsIncreaseCountMessage(message.Text))
            {
                return CommandType.Add;
            }
            if (IsDecreaseCountMessage(message.Text))
            {
                return CommandType.Remove;
            }
            if (IsCountCommand(message.Text))
            {
                return CommandType.Count;
            }
            if (IsListCommand(message.Text))
            {
                return CommandType.List;
            }
            if (IsCleanCommand(message.Text))
            {
                return CommandType.Clean;
            }
            if (IsAutoAlertsCommand(message.Text))
            {
                return CommandType.AutoAlerts;
            }

            return CommandType.NotCommand;
        }

        public static ParticipantsGroup GetParticipantsGroupFromMessage(ChatMessage message)
        {
            var participantsGroup = new ParticipantsGroup();
            var chatName = message.ChatName;

            var countSymbols = _notSureSymbols.Concat(_increaseCountSymbols).Concat(_decreaseCountSymbols);
            var trimmedMessage = message.Text;

            foreach (var symbol in countSymbols)
            {
                trimmedMessage = trimmedMessage.Replace(symbol, "");
            }

            var messageParts = trimmedMessage.TrimStart().Split(' ');

            if (IsSimpleCountMessage(message.Text) || message.Text.Trim() == _startCountMessage)
            {
                participantsGroup.Name = message.AuthorName;
                participantsGroup.Count = 1;
            }
            else if (char.IsDigit(messageParts[0][0]))
            {
                participantsGroup.Count = int.Parse(messageParts[0]);

                if (messageParts.Length > 1)
                {
                    var participantName = messageParts[1];
                    for (var i = 2; i < messageParts.Length; i++)
                    {
                        participantName += " " + messageParts[i];
                    }
                    participantsGroup.Name = participantName;
                }
                else
                {
                    participantsGroup.Name = "вместе с " + message.AuthorName;
                }
            }
            else
            {
                participantsGroup.Name = string.Join(" ", messageParts);
                participantsGroup.Count = 1;
            }

            return participantsGroup;
        }

        private static bool IsNotSureMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _notSureSymbols);
        }

        private static bool IsSimpleCountMessage(string messageText)
        {
            return MessageEqualsAnyOfSymbols(messageText, _increaseCountSymbols) ||
                MessageEqualsAnyOfSymbols(messageText, _decreaseCountSymbols) ||
                MessageEqualsAnyOfSymbols(messageText, _notSureSymbols);
        }

        private static bool IsIncreaseCountMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _increaseCountSymbols);
        }

        private static bool IsDecreaseCountMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _decreaseCountSymbols);
        }

        private static bool MessageStartsWithAnyOfSymbols(string messageText, string[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (messageText.StartsWith(symbol))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MessageEqualsAnyOfSymbols(string messageText, string[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (messageText.ToLower().Trim() == symbol)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsCountCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/count" || messageText.Trim().ToLower() == "/c";
        }

        private static bool IsListCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/list" || messageText.Trim().ToLower() == "/l";
        }

        private static bool IsCleanCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/clean";
        }

        private static bool IsAutoAlertsCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/autoAlerts";
        }
    }
}
