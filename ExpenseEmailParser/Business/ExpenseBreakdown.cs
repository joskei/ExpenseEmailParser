using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseEmailParser.Business
{
    public class ExpenseBreakdown
    {
        public string XmlExtracted { get; set; }
        public string GST { get; set; }
        public decimal BeforeTotal { get; set; }
        public string ErrorMessage { get; set; }
    }
}
