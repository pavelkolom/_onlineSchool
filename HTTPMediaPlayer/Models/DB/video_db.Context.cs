﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTTPMediaPlayer.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class videodbEntities : DbContext
    {
        public videodbEntities()
            : base("name=videodbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<vd_UserCourses> vd_UserCourses { get; set; }
        public virtual DbSet<vd_Files> vd_Files { get; set; }
        public virtual DbSet<vd_UserLessons> vd_UserLessons { get; set; }
        public virtual DbSet<vd_CourseLessons> vd_CourseLessons { get; set; }
        public virtual DbSet<vd_Lessons> vd_Lessons { get; set; }
        public virtual DbSet<vd_Users> vd_Users { get; set; }
        public virtual DbSet<vd_Orders> vd_Orders { get; set; }
        public virtual DbSet<vd_Countries> vd_Countries { get; set; }
        public virtual DbSet<vd_Log> vd_Log { get; set; }
        public virtual DbSet<vd_ActionLog> vd_ActionLog { get; set; }
        public virtual DbSet<vd_ActionType> vd_ActionType { get; set; }
        public virtual DbSet<vd_Courses> vd_Courses { get; set; }
    }
}
