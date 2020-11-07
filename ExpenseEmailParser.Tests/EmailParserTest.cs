using ExpenseEmailParser.Business;
using ExpenseEmailParser.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpenseEmailParser.Tests
{
    public class Tests
    {      
                
        [Test]
        public void XMLOnlyValidValueDev001_Test()
        {
            var controller = new EmailParserController();

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
    }
}