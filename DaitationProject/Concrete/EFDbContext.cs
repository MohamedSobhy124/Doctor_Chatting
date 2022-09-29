using DaitationProject.Entity;
using DaitationProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaitationProject.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<OnlineUser> OnlineUsers { get; set; }
        public DbSet<FriendMapping> FriendMappings { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        //public DbSet<UserModel> userModels { get; set; }
        //public DbSet<UserNotificationModel> UserNotificationModel { get; set; }
       
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<AppointmentStatus> appointmentStatuses { get; set; }
        public DbSet<DoctorNotes> doctorNotes { get; set; }
        public virtual DbSet<Diagnosis> diagnoses { get; set; }
        public virtual DbSet<DiagnosisList> DiagnosisList { get; set; }
        public virtual DbSet<DiagnosisTypes>  DiagnosisTypes { get; set; }

        public virtual DbSet<ViewModelDiagnosis> ViewModelDiagnosis { get; set; }
        
        public DbSet<Roles> roles { get; set; }
        public DbSet<Payments> payments { get; set; }
    }
}
