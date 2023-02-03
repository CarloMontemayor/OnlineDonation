using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineDonation.Data.Entity;
using OnlineDonation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDonation.Data
{
    public class ApplicationDbContext : IdentityDbContext<WebAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
