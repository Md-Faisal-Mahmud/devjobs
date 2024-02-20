﻿// <auto-generated />
using System;
using DevJobs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevJobs.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240208163350_DbLogClaim")]
    partial class DbLogClaim
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevJobs.Domain.Entities.DbLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogEvent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpanId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TraceId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DbLog");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.ExperienceMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Keyword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ExperienceMapping");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobAnalysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<Guid>("JobPostID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("JobTrack")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JobPostID");

                    b.ToTable("JobAnalysis");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalJobRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AgeMax")
                        .HasColumnType("int");

                    b.Property<int?>("AgeMin")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("EducationalRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExperienceMax")
                        .HasColumnType("int");

                    b.Property<int>("ExperienceMin")
                        .HasColumnType("int");

                    b.Property<string>("ExperienceRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("JobDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobNature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfVacancies")
                        .HasColumnType("int");

                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OtherBenefits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SalaryMax")
                        .HasColumnType("int");

                    b.Property<int?>("SalaryMin")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationID");

                    b.ToTable("JobPost");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobTechnology", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("JobAnalysisId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JobAnalysisId");

                    b.ToTable("JobTechnology");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BusinessType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.Technology", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TechnologyTrack")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Technology");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.TrackMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("JobTrack")
                        .HasColumnType("int");

                    b.Property<string>("Keyword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TrackMapping");
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"),
                            ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f51",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"),
                            ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f52",
                            Name = "Developer",
                            NormalizedName = "DEVELOPER"
                        });
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f47",
                            Email = "admin@devjobs.com",
                            EmailConfirmed = true,
                            FirstName = "Mr",
                            LastName = "Admin",
                            LockoutEnabled = true,
                            NormalizedEmail = "ADMIN@DEVJOBS.COM",
                            NormalizedUserName = "ADMIN@DEVJOBS.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAECGFKYvSkdPYW6Whbul5Q6tTOpj+qYDFOw2pxXrFTEtFyvu3UbWk1EGoCI8LMi8Rnw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "FCRKKDDXJGESVHVL4FJCQ5HY6LZP4OR5",
                            TwoFactorEnabled = false,
                            UserName = "admin@devjobs.com"
                        },
                        new
                        {
                            Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f48",
                            Email = "developer@devjobs.com",
                            EmailConfirmed = true,
                            FirstName = "Mr",
                            LastName = "Developer",
                            LockoutEnabled = true,
                            NormalizedEmail = "DEVELOPER@DEVJOBS.COM",
                            NormalizedUserName = "DEVELOPER@DEVJOBS.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAECGFKYvSkdPYW6Whbul5Q6tTOpj+qYDFOw2pxXrFTEtFyvu3UbWk1EGoCI8LMi8Rnw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "FCRKKDDXJGESVHVL4FJCQ5HY6LZP4OR5",
                            TwoFactorEnabled = false,
                            UserName = "developer@devjobs.com"
                        });
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 2,
                            ClaimType = "ViewJobList",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "ViewJobDetails",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 1,
                            ClaimType = "OrganizationClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "UserCreateClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "UserUpdateClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "UserDeleteClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "UserViewClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "UserViewClaim",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff")
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "ServiceStatusView",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "LogListViewRequirementPolicy",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "LogListViewRequirementPolicy",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            Id = 12,
                            ClaimType = "DeleteLogByIdRequirementPolicy",
                            ClaimValue = "true",
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe")
                        });
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                            RoleId = new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe")
                        },
                        new
                        {
                            UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                            RoleId = new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff")
                        });
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobAnalysis", b =>
                {
                    b.HasOne("DevJobs.Domain.Entities.JobPost", "Post")
                        .WithMany("Analysis")
                        .HasForeignKey("JobPostID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobPost", b =>
                {
                    b.HasOne("DevJobs.Domain.Entities.Organization", "Company")
                        .WithMany()
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobTechnology", b =>
                {
                    b.HasOne("DevJobs.Domain.Entities.JobAnalysis", null)
                        .WithMany("Technologies")
                        .HasForeignKey("JobAnalysisId");
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationRoleClaim", b =>
                {
                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserClaim", b =>
                {
                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserLogin", b =>
                {
                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserRole", b =>
                {
                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevJobs.Infrastructure.Features.Membership.ApplicationUserToken", b =>
                {
                    b.HasOne("DevJobs.Infrastructure.Features.Membership.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobAnalysis", b =>
                {
                    b.Navigation("Technologies");
                });

            modelBuilder.Entity("DevJobs.Domain.Entities.JobPost", b =>
                {
                    b.Navigation("Analysis");
                });
#pragma warning restore 612, 618
        }
    }
}
