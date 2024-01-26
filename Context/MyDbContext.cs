﻿using System;
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
         // Database.EnsureDeleted();
         // Database.EnsureCreated();
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupPerson> GroupPeople { get; set; }

    public virtual DbSet<GroupTask> GroupTasks { get; set; }

    public virtual DbSet<GroupTaskProblem> GroupTaskProblems { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<ProblemRelatedCategory> ProblemRelatedCategories { get; set; }

    public virtual DbSet<Submittion> Submittions { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }
    
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=IquraStudyDB;Username= postgres;Password=root;Pooling=true;");
        base.OnConfiguring(optionsBuilder);
    }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
