using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseEmailParser.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseEmailParser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailParserController : ControllerBase
    {
        
        [HttpPost]
        public List<ExpenseBreakdown> ParseEmail(string emailMessage)
        {
            if (emailMessage.ToLower().Contains("<expense>"))
            {
                return new List<ExpenseBreakdown>() { Parser.ParseEmail(emailMessage) };
            }
            else
            {
                throw new ArgumentException("No expense XML found!");
            }
        }
    }
}
