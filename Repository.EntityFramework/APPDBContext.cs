using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Models.Models.MedicalModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EntityFramework
{
    public class APPDBContext:DbContext
    {
        public APPDBContext( DbContextOptions<APPDBContext>options):base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<BenefitType> BenefitTypes { get; set; }

        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<BenefitWorkflow> BenefitWorkflows { get; set; }

        public DbSet<BenefitRequest> BenefitRequests { get; set; }
        public DbSet<RequestWorkflow> RequestWorkflows { get; set; }

        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<EmployeeDependent> EmployeeDependents { get; set; }

        public DbSet<RequestStatus> RequestStatus { get; set; }
        //public DbSet<EmployeeRequest> EmployeeRequests { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<GroupEmployee> GroupEmployee { get; set; }
        public DbSet<Privilege> Privileges { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<RequestDocument> RequestDocuments { get; set; }

        public DbSet<MobileVersion> MobileVersions { get; set; }

        public DbSet<BenefitMail> BenefitMails { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        public DbSet<Violation> Violations { get; set; }

        public DbSet<MedicalCategory> MedicalCategories { get; set; }

        public DbSet<MedicalSubCategory> MedicalSubCategories { get; set; }

        public DbSet<MedicalDetails> MedicalDetails { get; set; }
        public DbSet<Relative> Relatives { get; set; }

        public DbSet<MedicalRequestType> MedicalRequestTypes { get; set; }

        public DbSet<MedicalRequest> MedicalRequests { get; set; }

        public DbSet<MedicalRequestLog> MedicalRequestLogs { get; set; }

        public DbSet<MedicalAttachment> MedicalAttachments { get; set; }

        public DbSet<MedicalResponse> MedicalResponses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BenefitWorkflow>().HasKey(w => new { w.BenefitId, w.RoleId });
            modelBuilder.Entity<Employee>().HasIndex(E => E.Id).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(E => E.SapNumber).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(E => E.PhoneNumber).IsUnique();
            modelBuilder.Entity<GroupEmployee>().HasKey(GE => new { GE.EmployeeId, GE.GroupId });
            modelBuilder.Entity<EmployeeDependent>().HasKey(d => new { d.Id, d.EmployeeNumber });
            modelBuilder.Entity<EmployeeDependent>().HasIndex(D => D.Id).IsUnique();
            modelBuilder.Entity<EmployeeDependent>().HasIndex(D => D.PhoneNumber).IsUnique();
            //modelBuilder.Entity<RequestWorkflow>().HasKey(RW => new { RW.BenefitRequestId, RW.EmployeeId });
            modelBuilder.Entity<UserNotification>().HasKey(UN => new { UN.NotificationId, UN.EmployeeId });

        }
    }
}
