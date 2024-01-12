﻿// <auto-generated />
using System;
using IquraStudyBE.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IquraStudyBE.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20240112222018_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IquraStudyBE.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("GradeScore")
                        .HasColumnType("integer");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.HasIndex("UserId");

                    b.ToTable("Grade");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("User")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("User");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupPerson");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("text");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupTask");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupTaskProblem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("GroupTaskId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupTaskId");

                    b.HasIndex("ProblemId");

                    b.ToTable("GroupTaskProblem");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Problem");
                });

            modelBuilder.Entity("IquraStudyBE.Models.ProblemRelatedCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProblemId");

                    b.ToTable("ProblemRelatedCategory");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Submittion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer");

                    b.Property<string>("Result")
                        .HasColumnType("text");

                    b.Property<int?>("ResultPercent")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("SubmissionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.HasIndex("UserId");

                    b.ToTable("Submittion");
                });

            modelBuilder.Entity("IquraStudyBE.Models.TestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ExpectedResult")
                        .HasColumnType("text");

                    b.Property<string>("Input")
                        .HasColumnType("text");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("TestCase");
                });

            modelBuilder.Entity("IquraStudyBE.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("IquraStudyBE.Models.Grade", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Problem", "Problem")
                        .WithMany("Grades")
                        .HasForeignKey("ProblemId");

                    b.HasOne("IquraStudyBE.Models.User", "User")
                        .WithMany("Grades")
                        .HasForeignKey("UserId");

                    b.Navigation("Problem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Group", b =>
                {
                    b.HasOne("IquraStudyBE.Models.User", "CreatedByUser")
                        .WithMany("Groups")
                        .HasForeignKey("User");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupPerson", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Group", "Group")
                        .WithMany("GroupPeople")
                        .HasForeignKey("GroupId");

                    b.HasOne("IquraStudyBE.Models.User", "User")
                        .WithMany("GroupPeople")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupTask", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Group", "Group")
                        .WithMany("GroupTasks")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupTaskProblem", b =>
                {
                    b.HasOne("IquraStudyBE.Models.GroupTask", "GroupTask")
                        .WithMany("GroupTaskProblems")
                        .HasForeignKey("GroupTaskId");

                    b.HasOne("IquraStudyBE.Models.Problem", "Problem")
                        .WithMany("GroupTaskProblems")
                        .HasForeignKey("ProblemId");

                    b.Navigation("GroupTask");

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Problem", b =>
                {
                    b.HasOne("IquraStudyBE.Models.User", "User")
                        .WithMany("Problems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("IquraStudyBE.Models.ProblemRelatedCategory", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Category", "Category")
                        .WithMany("ProblemRelatedCategories")
                        .HasForeignKey("CategoryId");

                    b.HasOne("IquraStudyBE.Models.Problem", "Problem")
                        .WithMany("ProblemRelatedCategories")
                        .HasForeignKey("ProblemId");

                    b.Navigation("Category");

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Submittion", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Problem", "Problem")
                        .WithMany("Submittions")
                        .HasForeignKey("ProblemId");

                    b.HasOne("IquraStudyBE.Models.User", "User")
                        .WithMany("Submittions")
                        .HasForeignKey("UserId");

                    b.Navigation("Problem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("IquraStudyBE.Models.TestCase", b =>
                {
                    b.HasOne("IquraStudyBE.Models.Problem", "Problem")
                        .WithMany("TestCases")
                        .HasForeignKey("ProblemId");

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("IquraStudyBE.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("IquraStudyBE.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IquraStudyBE.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("IquraStudyBE.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IquraStudyBE.Models.Category", b =>
                {
                    b.Navigation("ProblemRelatedCategories");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Group", b =>
                {
                    b.Navigation("GroupPeople");

                    b.Navigation("GroupTasks");
                });

            modelBuilder.Entity("IquraStudyBE.Models.GroupTask", b =>
                {
                    b.Navigation("GroupTaskProblems");
                });

            modelBuilder.Entity("IquraStudyBE.Models.Problem", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("GroupTaskProblems");

                    b.Navigation("ProblemRelatedCategories");

                    b.Navigation("Submittions");

                    b.Navigation("TestCases");
                });

            modelBuilder.Entity("IquraStudyBE.Models.User", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("GroupPeople");

                    b.Navigation("Groups");

                    b.Navigation("Problems");

                    b.Navigation("Submittions");
                });
#pragma warning restore 612, 618
        }
    }
}
