using System;
using System.Collections.Generic;
using IquraStudyBE.Entities;
using Microsoft.EntityFrameworkCore;

namespace IquraStudyBE.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupPerson> GroupPeople { get; set; }

    public virtual DbSet<GroupTask> GroupTasks { get; set; }

    public virtual DbSet<GroupTaskProblem> GroupTaskProblems { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<ProblemRelatedCategory> ProblemRelatedCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SimilarProblem> SimilarProblems { get; set; }

    public virtual DbSet<Submittion> Submittions { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=IquraStudyDB;Username= postgres;Password=root;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Grade_pkey");

            entity.ToTable("Grade");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_at");
            entity.Property(e => e.Grade1).HasColumnName("Grade");

            entity.HasOne(d => d.Problem).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("Grade_ProblemId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Grades)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Grade_UserId_fkey");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Group_pkey");

            entity.ToTable("Group");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedByUserId).HasColumnName("Created_by_userId");
            entity.Property(e => e.Name).HasColumnType("character varying");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("Group_Created_by_userId_fkey");
        });

        modelBuilder.Entity<GroupPerson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GroupPeople_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.GroupPeople)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("GroupPeople_GroupId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.GroupPeople)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("GroupPeople_UserId_fkey");
        });

        modelBuilder.Entity<GroupTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GroupTask_pkey");

            entity.ToTable("GroupTask");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasColumnType("character varying");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupTasks)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("GroupTask_GroupId_fkey");
        });

        modelBuilder.Entity<GroupTaskProblem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GroupTaskProblem_pkey");

            entity.ToTable("GroupTaskProblem");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.GroupTask).WithMany(p => p.GroupTaskProblems)
                .HasForeignKey(d => d.GroupTaskId)
                .HasConstraintName("GroupTaskProblem_GroupTaskId_fkey");

            entity.HasOne(d => d.Problem).WithMany(p => p.GroupTaskProblems)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("GroupTaskProblem_ProblemId_fkey");
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Problem_pkey");

            entity.ToTable("Problem");

            entity.HasIndex(e => e.Title, "Title_index");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_at");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Title).HasColumnType("character varying");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.User).WithMany(p => p.Problems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Problem_UserId_fkey");
        });

        modelBuilder.Entity<ProblemRelatedCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProblemRelatedCategory_pkey");

            entity.ToTable("ProblemRelatedCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.ProblemRelatedCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("ProblemRelatedCategory_CategoryId_fkey");

            entity.HasOne(d => d.Problem).WithMany(p => p.ProblemRelatedCategories)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("ProblemRelatedCategory_ProblemId_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasComment("Teacher or Student")
                .HasColumnType("character varying");
        });

        modelBuilder.Entity<SimilarProblem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SimilarProblems_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Problem).WithMany(p => p.SimilarProblemProblems)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("SimilarProblems_ProblemId_fkey");

            entity.HasOne(d => d.RelatedProblem).WithMany(p => p.SimilarProblemRelatedProblems)
                .HasForeignKey(d => d.RelatedProblemId)
                .HasConstraintName("SimilarProblems_RelatedProblemId_fkey");
        });

        modelBuilder.Entity<Submittion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Submittion_pkey");

            entity.ToTable("Submittion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.Result)
                .HasComment("Accepted, Wrong Answer")
                .HasColumnType("character varying");
            entity.Property(e => e.SubmissionDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Problem).WithMany(p => p.Submittions)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("Submittion_ProblemId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Submittions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Submittion_UserId_fkey");
        });

        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TestCase_pkey");

            entity.ToTable("TestCase");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpectedResult)
                .HasComment("file")
                .HasColumnType("character varying");
            entity.Property(e => e.Input)
                .HasComment("file")
                .HasColumnType("character varying");

            entity.HasOne(d => d.Problem).WithMany(p => p.TestCases)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("TestCase_ProblemId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "Email_index");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_at");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Image).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("User_RoleId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
