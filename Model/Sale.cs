using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SalesDetails> SalesDetails { get; set; }
    }
}