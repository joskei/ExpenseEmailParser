using ExpenseEmailParser.Business;
using ExpenseEmailParser.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpenseEmailParser.Tests
{
    public class Tests
    {
        EmailParserController controller;

        [SetUp]
        public void Setup()
        {
            controller = new EmailParserController();
        }

        #region Business-Tests       
        [Test]
        public void XMLOnlyValidValueDev001_Test()
        {            

            var input = $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>";

            var expected = new List<ExpenseBreakdown>()
            {
                new ExpenseBreakdown()
                {
                    XmlExtracted = $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>",
                    GST = "10.00%",
                    BeforeTotal = 10,
                    ErrorMessage = string.Empty
                }
            };


            var actual = controller.ParseEmail(input);

            Assert.AreEqual(expected[0].XmlExtracted, actual[0].XmlExtracted);
            Assert.AreEqual(expected[0].GST, actual[0].GST);
            Assert.AreEqual(expected[0].BeforeTotal, actual[0].BeforeTotal);
            Assert.AreEqual(actual.Count, 1);
        }

        [Test]
        public void XMLOnlyValidValueDev002_Test()
        {

            var input = $"<expense>" +
                        "<cost_centre>DEV002</cost_centre>" +
                        "<total>11.20</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>";

            var expected = new List<ExpenseBreakdown>()
            {
                new ExpenseBreakdown()
                {
                    XmlExtracted = $"<expense>" +
                        "<cost_centre>DEV002</cost_centre>" +
                        "<total>11.20</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>",
                    GST = "12.00%",
                    BeforeTotal = 10,
                    ErrorMessage = string.Empty
                }
            };


            var actual = controller.ParseEmail(input);

            Assert.AreEqual(expected[0].XmlExtracted, actual[0].XmlExtracted);
            Assert.AreEqual(expected[0].GST, actual[0].GST);
            Assert.AreEqual(expected[0].BeforeTotal, actual[0].BeforeTotal);
            Assert.AreEqual(actual.Count, 1);
        }

        [Test]
        public void XMLOnlyValidValueDev003_Test()
        {

            var input = $"<expense>" +
                        "<cost_centre>DEV003</cost_centre>" +
                        "<total>11.50</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>";

            var expected = new List<ExpenseBreakdown>()
            {
                new ExpenseBreakdown()
                {
                    XmlExtracted = $"<expense>" +
                        "<cost_centre>DEV003</cost_centre>" +
                        "<total>11.50</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>",
                    GST = "15.00%",
                    BeforeTotal = 10,
                    ErrorMessage = string.Empty
                }
            };


            var actual = controller.ParseEmail(input);

            Assert.AreEqual(expected[0].XmlExtracted, actual[0].XmlExtracted);
            Assert.AreEqual(expected[0].GST, actual[0].GST);
            Assert.AreEqual(expected[0].BeforeTotal, actual[0].BeforeTotal);
            Assert.AreEqual(actual.Count, 1);
        }

        [Test]
        public void TextXMLWithStringAtStart_Test()
        {            

            var input = $"Dear <SupervisorName>," +
                        "<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>";

            var expected = new List<ExpenseBreakdown>()
            {
                new ExpenseBreakdown()
                {
                    XmlExtracted =  $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>",
                    GST = "10.00%",
                    BeforeTotal = 10
                }
            };

            var actual = controller.ParseEmail(input);

            Assert.AreEqual(expected[0].XmlExtracted, actual[0].XmlExtracted);
            Assert.AreEqual(expected[0].GST, actual[0].GST);
            Assert.AreEqual(expected[0].BeforeTotal, actual[0].BeforeTotal);
            Assert.AreEqual(actual.Count, 1);
        }

        [Test]
        public void TextXMLWithStringAtEnd_Test()
        {            

            var input = $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>" +
                        "Please also.. ";

            var expected = new List<ExpenseBreakdown>()
            {
                new ExpenseBreakdown()
                {
                    XmlExtracted =  $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<total>11.00</total>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>",
                    GST = "10.00%",
                    BeforeTotal = 10
                }
            };

            var actual = controller.ParseEmail(input);

            Assert.AreEqual(expected[0].XmlExtracted, actual[0].XmlExtracted);
            Assert.AreEqual(expected[0].GST, actual[0].GST);
            Assert.AreEqual(expected[0].BeforeTotal, actual[0].BeforeTotal);
            Assert.AreEqual(actual.Count, 1);
        }

        #endregion

        #region Validator-Tests
        [Test]
        public void MissingTotalElement_Test()
        {
            var input = $"<expense>" +
                        "<cost_centre>DEV001</cost_centre>" +
                        "<payment_method>personal card</payment_method>" +
                        "</expense>";

            Assert.Throws<ArgumentException>(() => controller.ParseEmail(input));
        }

        [Test]
        public void MissingClosingElement_Test()
        {           
            var input = $"<expense>" +
                       "<cost_centre>DEV001</cost_centre>" +
                       "<total>11.00</total>" +
                       "<payment_method>personal card</payment_method>";

            Assert.Throws<ArgumentException>(() => controller.ParseEmail(input));
        }

        [Test]
        public void MalformedXmlInvalidTag_Test()
        {            

            var input = $"<expense></test>" +
                       "<cost_centre>DEV001</cost_centre>" +
                       "<total>11.00</total>" +
                       "<payment_method>personal card</payment_method>" +
                       "</expense>";

            Assert.Throws<ArgumentException>(() => controller.ParseEmail(input));
        }

        #endregion
    }
}