﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransMonAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TransactionMonitoringEntities : DbContext
    {
        public TransactionMonitoringEntities()
            : base("name=TransactionMonitoringEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Bank> Banks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<View_Users> View_Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MyAnalytic> MyAnalytics { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<WeeklyRating> WeeklyRatings { get; set; }
    }
}
