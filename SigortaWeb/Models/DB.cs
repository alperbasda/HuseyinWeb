using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SigortaWeb.Models
{
    public class DB:DbContext
    {

        public DB()
            :base("NihaiVeri")
        {}

        public DbSet<BranchGroup> branchGroups { get; set; }

        public DbSet<Branch> Branchs { get; set; }

        public DbSet<Sayac> Sayacs { get; set; }

        public DbSet<CompanyType> CompanyTypes { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Calc> Calcs { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountCalcRelation> AccountCalcRelation { get; set; }

        public DbSet<Data> Datas { get; set; }

        public DbSet<Table> Tables { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data>().Property(e => e.amount)
                .HasColumnType("Decimal")
                .HasPrecision(18, 4);
            
        }

    }
}