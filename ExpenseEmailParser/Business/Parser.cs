using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ExpenseEmailParser.Business
{
    public class Parser
    {
        private const string expenseStartXml = "<expense>";
        private const string expenseEndXml = "</expense>";

        private const string elementCostCenter = "<cost_centre>";
        private const string elementTotal = "<total>";

        #region "Business"
        internal static ExpenseBreakdown ParseEmail(string emailMessage)
        {
            //Check that there's only 1 XML for <expense>..</expense>
            var validationResult = ValidateOneExpense(emailMessage);
            if (validationResult.Item2 == null)
            {
                //This means that the Xml tag is invalid and whole message can be rejected.
                throw new ArgumentException(validationResult.Item1);
            }

            if (validationResult.Item2 != null & validationResult.Item1 == "cost_center missing")
            {
                //this is the condition that cost_center is missing
                var xmlDoc = validationResult.Item2;

                XmlElement costCenterElement = xmlDoc.CreateElement("cost_centre");
                costCenterElement.InnerText = "UNKNOWN";
                xmlDoc.DocumentElement.AppendChild(costCenterElement);

                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    emailMessage = stringWriter.GetStringBuilder().ToString();
                }

            }

            var strippedXml = Helper.GetXmlString(emailMessage);

            //Get cost center and total in the xml
            var valueCostCenter = Regex.Match(strippedXml, String.Concat(elementCostCenter, "(.*)",
                                            elementCostCenter.Insert(1, "/"))).Groups[1].Value;

            var valueTotal = Regex.Match(strippedXml, String.Concat(elementTotal, "(.*)",
                                            elementTotal.Insert(1, "/"))).Groups[1].Value;
            
            var GSTPercentage = Helper.GetGSTAmount(valueCostCenter);

            //Compute for original amount before GST
            decimal total = Convert.ToDecimal(valueTotal);

            //make sure total precision has 2 decimal places only
            decimal calculatedTotal = (total / (GSTPercentage + 1M));
            decimal originalTotal = decimal.Round(calculatedTotal, 2, MidpointRounding.AwayFromZero);


            var result = new ExpenseBreakdown()
            {
                XmlExtracted = strippedXml,
                GST = (GSTPercentage * 100) + "%",
                BeforeTotal = originalTotal,
                ErrorMessage = string.Empty
            };

            return result;
        }
        #endregion

        #region Validator
        /// <summary>
        /// Function to parse email message for one expense XML
        /// </summary>
        /// <param name="emailMessage">String</param>
        /// <returns>string, XmlDocument => Error Message, Constructed XmlDocument</returns>
        internal static Tuple<string, XmlDocument> ValidateOneExpense(string emailMessage)
        {
            //get XML in email message
            var xmlEmail = new XmlDocument();

            try
            {
                //Parse the string to get XML                
                var startXmlExpense = emailMessage.IndexOf(expenseStartXml);
                var endXmlExpense = emailMessage.IndexOf(expenseEndXml);

                if (startXmlExpense == -1)
                {
                    return new Tuple<string, XmlDocument>("Missing opening <expense> tag", null);
                }

                if (endXmlExpense == -1)
                {
                    return new Tuple<string, XmlDocument>("Missing closing </expense> tag", null);
                }

                //calculate length of string to copy
                var lengthOfXml = endXmlExpense - startXmlExpense + expenseEndXml.Length;

                var xmlExpense = emailMessage.Substring(startXmlExpense, lengthOfXml);

                //Call the LoadXML to check for valid XML structure
                xmlEmail.LoadXml(xmlExpense);

                //Additional Validation
                if (xmlEmail.SelectSingleNode("//total") == null)
                {
                    return new Tuple<string, XmlDocument>("<total> is missing!", null);
                }

                if (xmlEmail.SelectSingleNode("//cost_centre") == null)
                {
                    //This is a special case that needs to be further processed
                    return new Tuple<string, XmlDocument>("cost_center missing", xmlEmail);
                }

                //if everything is good, return the new XML
                return new Tuple<string, XmlDocument>(string.Empty, xmlEmail);
            }
            catch (XmlException ex)
            {
                return new Tuple<string, XmlDocument>(ex.Message, null);
            }
        }

        #endregion
    }
}
