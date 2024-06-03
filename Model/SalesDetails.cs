using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Model
{
    public class SalesDetails
    {
        public int SalesDetailsId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitePrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}