using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParticipantsCounter.App;
using ParticipantsCounter.Core.Entities;
using System.Collections.Generic;

namespace ParticipantsCounter.Tests
{
    /*
      Варианты:
        Плюсы:
        +
        + на завтра
        +2
        +2 из вк
        + Вася

        Не знают:
        +/-
        +\-
        ±
        не знаю

        Минусы:
        -
        -2
        -2 из вк
        - Вася
     */

    [TestClass]
    public class MessagesProcessorTests
    {
        private MessagesProcessor _target;

        [TestInitialize]
        public void Init()
        {
            _target = new MessagesProcessor(new FictiveStorage<List<Event>>());
        }

        #region Count

        #region Add

        [TestMethod]
        public void SinglePlusIncrementsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void StarterPlusIncrementsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ на завтра");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusWithCountAddsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameAddsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+2");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithNameIncrementsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ Вася");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void DuplicatedSinglePlusIsIgnored()
        {
            var message = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusWithCountCanAddCountSeveralTimes()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("4", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameCanAddCountSeveralTimes()
        {
            var message = CreateTextMessageFromUser("Bob", "+2");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "+2");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("4", countResponse);
        }

        #endregion

        #region Remove

        [TestMethod]
        public void SingleMinusDecrementsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountRemovesCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "- 2 из вк");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameRemovesCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+2");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-2");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithNameDecrementsCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ Вася");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "- Вася");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void DuplicatedSingleMinusIsIgnored()
        {
            var message = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-");
            _target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser("Bob", "-");
            _target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountCanRemoveCountSeveralTimes()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 6 из вк");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "- 2 из вк");
            _target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser("Bob", "- 2 из вк");
            _target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameCanRemoveCountSeveralTimes()
        {
            var message = CreateTextMessageFromUser("Bob", "+6");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-2");
            _target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser("Bob", "-2");
            _target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountWithoutNameRemovesOnlyExistingCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+2");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-4");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountRemovesOnlyExistingCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "- 4 из вк");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        #endregion

        #region List

        #region Add


        [TestMethod]
        public void PlusWithCountUpdatesListWithCorrectWeightAndName()
        {
            var message = CreateTextMessageFromUser("Bob", "+ 2 из вк");
            _target.ProcessMessage(message);

            var listResponse = GetListCommandResult();
            Assert.AreEqual("1-2. из вк - 2\r\n", listResponse);
        }

        [TestMethod]
        public void PlusWithNameUpdatesListWithWholeName()
        {
            var message = CreateTextMessageFromUser("Bob", "+ Иван Иванов");
            _target.ProcessMessage(message);

            var listResponse = GetListCommandResult();
            Assert.AreEqual("1. Иван Иванов\r\n", listResponse);
        }

        #endregion

        #endregion

        #region Not sure

        [TestMethod]
        public void PlusAndMinusIncrementsNotSureCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+-");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndSlashAndMinusIncrementsNotSureCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+/-");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndBackslashAndMinusIncrementsNotSureCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+\\-");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusAsSingleSymbolIncrementsNotSureCount()
        {
            var message = CreateTextMessageFromUser("Bob", "±");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithCountAddsNotSuredWithCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+/- 2 из вк");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithNameIncrementsNotSureCount()
        {
            var message = CreateTextMessageFromUser("Bob", "+/- Вася");
            _target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndNotSureAreBothShown()
        {
            var message = CreateTextMessageFromUser("Bob", "+/-");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Jack", "+");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1 и 1±", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithTrueIncrementsCountOfSure()
        {
            var message = CreateTextMessageFromUser("Bob", "+/-");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "+");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithFalseDosntIncrementCountOfSure()
        {
            var message = CreateTextMessageFromUser("Bob", "+/-");
            _target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser("Bob", "-");
            _target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        #region Helpers

        private ChatMessage CreateTextMessageFromUser(string userName, string messageText)
        {
            var message = new ChatMessage
            {
                AuthorName = userName,
                ChatName = "My chat",
                Text = messageText
            };

            return message;
        }

        private string GetCountCommandResult()
        {
            var countCommand = new ChatMessage
            {
                AuthorName = "Bob",
                ChatName = "My chat",
                Text = "/count"
            };

            return _target.ProcessMessage(countCommand);
        }

        private string GetListCommandResult()
        {
            var listCommand = new ChatMessage
            {
                AuthorName = "Bob",
                ChatName = "My chat",
                Text = "/list"
            };

            return _target.ProcessMessage(listCommand);
        }

        #endregion
    }
}
