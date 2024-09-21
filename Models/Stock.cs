using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeLens.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int TickerSymbol { get; set; }
        public string CompanyName { get; set; }
    }
}