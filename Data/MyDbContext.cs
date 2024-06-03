using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Model;

namespace POS.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)   
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<SalesDetails> SalesDetails{ get; set; }
        public DbSet<Sale> Sales{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<User> Users{ get; set; }
    }
}