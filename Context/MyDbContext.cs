using System;
using System.Collections.Generic;
using IquraStudyBE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IquraStudyBE.Context;

public partial class MyDbContext : IdentityDbContext<User>
{
    public MyDbContext()
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
          //Database.EnsureDeleted();
         //Database.EnsureCreated();
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupPerson> GroupPeople { get; set; }

    public virtual DbSet<GroupTask> GroupTasks { get; set; }

    public virtual DbSet<GroupTaskProblem> GroupTaskProblems { get; set; }
    public virtual DbSet<GroupTaskQuiz> GroupTaskQuizzes { get; set; }
    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<ProblemRelatedCategory> ProblemRelatedCategories { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }
    
    public virtual DbSet<Quiz> Quizzes { get; set; }
    
    public virtual DbSet<QuizSubmittion> QuizSubmittions { get; set; }
    public virtual DbSet<ProblemSubmittion> ProblemSubmittions { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}
