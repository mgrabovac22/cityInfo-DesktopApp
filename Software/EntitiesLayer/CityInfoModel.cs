using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EntitiesLayer
{
    public partial class CityInfoModel : DbContext
    {
        public CityInfoModel()
            : base("name=CityInfoModel")
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<InstitutionType> InstitutionType { get; set; }
        public virtual DbSet<Like> Like { get; set; }
        public virtual DbSet<PeriodLocation> PeriodLocation { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Problem> Problem { get; set; }
        public virtual DbSet<ProblemCategory> ProblemCategory { get; set; }
        public virtual DbSet<Suggestion> Suggestion { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Institution>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Institution>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Institution>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Institution>()
                .Property(e => e.WorkingHours)
                .IsUnicode(false);

            modelBuilder.Entity<Institution>()
                .Property(e => e.InstitutionName)
                .IsUnicode(false);

            modelBuilder.Entity<Institution>()
                .HasMany(e => e.PeriodLocation)
                .WithRequired(e => e.Institution)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InstitutionType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<PeriodLocation>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<PeriodLocation>()
                .Property(e => e.Period)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostName)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostContent)
                .IsUnicode(false);

            modelBuilder.Entity<Problem>()
                .Property(e => e.ProblemName)
                .IsUnicode(false);

            modelBuilder.Entity<Problem>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Problem>()
                .Property(e => e.ProblemReply)
                .IsUnicode(false);

            modelBuilder.Entity<Problem>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ProblemCategory>()
                .Property(e => e.ProblemCategoryname)
                .IsUnicode(false);

            modelBuilder.Entity<ProblemCategory>()
                .HasMany(e => e.Problem)
                .WithRequired(e => e.ProblemCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Suggestion>()
                .HasMany(e => e.Like)
                .WithRequired(e => e.Suggestion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Post)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Problem)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.ID_Employee);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Problem1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ID_User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Suggestion)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.User)
                .WithRequired(e => e.UserType)
                .WillCascadeOnDelete(false);
        }
    }
}
