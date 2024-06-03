using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        // public string Category { get; set; }

    // add Category
        // [Required]
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }

        public bool IsDelete { get; set; }
    }
}