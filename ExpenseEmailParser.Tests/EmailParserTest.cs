using ExpenseEmailParser.Controllers;
using NUnit.Framework;
using System;

namespace ExpenseEmailParser.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //TODO:
        [Test]
        public void OnlyOneXml_Test()
        {
            var controller = new EmailParserController();
            var input = string.Empty;

            Assert.Throws<NotImplementedException>(() => controller.ParseEmail(input));
        }
    }
}