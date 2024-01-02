using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace COMP2001_Assessment2.Models
{
    public partial class COMP2001MAL_KChunleongContext : DbContext
    {
        public COMP2001MAL_KChunleongContext()
        {
        }

        public COMP2001MAL_KChunleongContext(DbContextOptions<COMP2001MAL_KChunleongContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Achievement> Achievements { get; set; } = null!;
        public virtual DbSet<Activity> Activities { get; set; } = null!;
        public virtual DbSet<ActivityTrail> ActivityTrails { get; set; } = null!;
        public virtual DbSet<FollowRelationship> FollowRelationships { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Trail> Trails { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001MAL_KChunleong;User Id=KChunleong;Password=TkpZ242*;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.Property(e => e.AchievementId).HasColumnName("AchievementID");

                entity.Property(e => e.AchievementDate).HasColumnType("date");

                entity.Property(e => e.AchievementName).HasMaxLength(100);

                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Achievements)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK__Achieveme__Profi__04E4BC85");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ActivityDate).HasColumnType("date");

                entity.Property(e => e.ActivityType).HasMaxLength(50);

                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK__Activity__Profil__0C85DE4D");
            });

            modelBuilder.Entity<ActivityTrail>(entity =>
            {
                entity.ToTable("ActivityTrail");

                entity.Property(e => e.ActivityTrailId)
                    .ValueGeneratedNever()
                    .HasColumnName("ActivityTrailID");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.TrailId).HasColumnName("TrailID");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivityTrails)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK__ActivityT__Activ__0D7A0286");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.ActivityTrails)
                    .HasForeignKey(d => d.TrailId)
                    .HasConstraintName("FK__ActivityT__Trail__123EB7A3");
            });

            modelBuilder.Entity<FollowRelationship>(entity =>
            {
                entity.HasKey(e => e.FollowId)
                    .HasName("PK__tmp_ms_x__2CE8108EA2DEA9DF");

                entity.ToTable("FollowRelationship");

                entity.Property(e => e.FollowId).HasColumnName("FollowID");

                entity.Property(e => e.FollowDate).HasColumnType("date");

                entity.Property(e => e.FollowedProfileId).HasColumnName("FollowedProfileID");

                entity.Property(e => e.FollowerProfileId).HasColumnName("FollowerProfileID");

                entity.HasOne(d => d.FollowedProfile)
                    .WithMany(p => p.FollowRelationshipFollowedProfiles)
                    .HasForeignKey(d => d.FollowedProfileId)
                    .HasConstraintName("FK__FollowRel__Follo__08B54D69");

                entity.HasOne(d => d.FollowerProfile)
                    .WithMany(p => p.FollowRelationshipFollowerProfiles)
                    .HasForeignKey(d => d.FollowerProfileId)
                    .HasConstraintName("FK__FollowRel__Follo__07C12930");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profile");

                entity.Property(e => e.ProfileId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProfileID");

                entity.Property(e => e.ImageUrl).HasMaxLength(200);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.ProfileBirthday).HasColumnType("date");

                entity.Property(e => e.ProfileName).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Profile__UserID__7A672E12");
            });

            modelBuilder.Entity<Trail>(entity =>
            {
                entity.ToTable("Trail");

                entity.Property(e => e.TrailId).HasColumnName("TrailID");

                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

                entity.Property(e => e.TrailName).HasMaxLength(100);

                entity.Property(e => e.TrailStartDate).HasColumnType("date");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK__Trail__ProfileID__114A936A");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.RegistrationDate).HasColumnType("date");

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
