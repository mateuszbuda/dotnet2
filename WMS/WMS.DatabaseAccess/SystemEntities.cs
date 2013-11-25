using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess
{
    public class SystemEntities : DbContext
    {
        /// <summary>
        /// Magazyny
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }
        
        /// <summary>
        /// Sektory
        /// </summary>
        public DbSet<Sector> Sectors { get; set; }

        /// <summary>
        /// Partie
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Przesunięcia
        /// </summary>
        public DbSet<Shift> Shifts { get; set; }

        /// <summary>
        /// Produkty
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Partnerzy
        /// </summary>
        public DbSet<Partner> Partners { get; set; }

        /// <summary>
        /// Szczegóły grup
        /// </summary>
        public DbSet<GroupDetails> GroupsDetails { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Shift>()
        //        .HasRequired(s => s.Recipient)
        //        .WithMany(w => w.Received)
        //        .HasForeignKey(s => s.RecipientId)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Shift>()
        //        .HasRequired(s => s.Sender)
        //        .WithMany(w => w.Sent)
        //        .HasForeignKey(s => s.SenderId)
        //        .WillCascadeOnDelete(false);
        //}
    }
}
