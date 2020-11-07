using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ExpenseEmailParser.Business
{
    internal class Helper
    {
        private const string expenseStartXml = "<expense>";
        private const string expenseEndXml = "</expense>";

        private const string dev001 = "dev001";
        private const string dev002 = "dev002";
        private const string dev003 = "dev003";
        private const string unknown = "unknown";

        internal static string GetXmlString(string emailMessage)
        {
            var startXmlExpense = emailMessage.IndexOf(expenseStartXml);
            var endXmlExpense = emailMessage.IndexOf(expenseEndXml);

            var xmlEmail = new XmlDocument();

            var lengthOfXml = endXmlExpense - startXmlExpense + expenseEndXml.Length;

            return emailMessage.Substring(startXmlExpense, lengthOfXml);

        }

        internal static decimal GetGSTAmount(string costCenter)
        {
            var center = costCenter.ToLower();

            //hard-coding for now with the assumption that there's 3 cost center only!            
            switch (center)
            {
                case dev001:
                    return 0.10M;
                case dev002:
                    return 0.12M;
                case dev003:
                    return 0.15M;
                case unknown: //assuming unknown to be 0.00% GST. 
                    return 0M;
                default:
                    throw new ArgumentException("Invalid Cost Center!");
            }
        }
    }
}
