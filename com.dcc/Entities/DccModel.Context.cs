﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace com.dcc.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DCCEntities : DbContext
    {
        public DCCEntities()
            : base("name=DCCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Believer> Believers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UsersAccount> UsersAccounts { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Due> Dues { get; set; }
        public virtual DbSet<Month> Months { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
    }
}