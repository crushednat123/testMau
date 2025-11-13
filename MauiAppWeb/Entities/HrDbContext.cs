using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MauiAppWeb.Entities;

public partial class HrDbContext : DbContext
{
    public HrDbContext()
    {
    }

    public HrDbContext(DbContextOptions<HrDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacation> Vacations { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-E1C84SB;Database=HR_DB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movement>(entity =>
        {
            entity.Property(e => e.DateOfAcceptance).HasColumnName("Date_of_acceptance");
            entity.Property(e => e.DateOfDismissal).HasColumnName("Date_of_dismissal");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Movements)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Movements_Users");

            entity.HasOne(d => d.IdWorkerNavigation).WithMany(p => p.Movements)
                .HasForeignKey(d => d.IdWorker)
                .HasConstraintName("FK_Movements_Workers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Named).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);
        });

        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.Property(e => e.BusinessTripEndDate).HasColumnName("Business_trip_end_date");
            entity.Property(e => e.DateOfBusinessTrip).HasColumnName("Date_of_business_trip");
            entity.Property(e => e.SickLeaveEndDate).HasColumnName("Sick_leave_end_date");
            entity.Property(e => e.SickLeaveStartDate).HasColumnName("Sick_leave_start_date");
            entity.Property(e => e.VacationDate).HasColumnName("Vacation_date");
            entity.Property(e => e.VacationEndDate).HasColumnName("Vacation_end_date");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Vacations)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Vacations_Users");

            entity.HasOne(d => d.IdWorkesNavigation).WithMany(p => p.Vacations)
                .HasForeignKey(d => d.IdWorkes)
                .HasConstraintName("FK_Vacations_Workers");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Post).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("money");
            entity.Property(e => e.SurName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
