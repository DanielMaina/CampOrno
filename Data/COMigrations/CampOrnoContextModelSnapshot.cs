﻿// <auto-generated />
using System;
using CampOrno.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CampOrno.Data.COMigrations
{
    [DbContext(typeof(CampOrnoContext))]
    partial class CampOrnoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("CO")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CampOrno.Models.Activity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("CampOrno.Models.Camper", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompoundID");

                    b.Property<int?>("CounselorID");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("DOB");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50);

                    b.Property<long>("Phone");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("eMail")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<byte[]>("imageContent");

                    b.Property<string>("imageFileName")
                        .HasMaxLength(100);

                    b.Property<string>("imageMimeType")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.HasIndex("CompoundID");

                    b.HasIndex("CounselorID");

                    b.HasIndex("eMail")
                        .IsUnique();

                    b.ToTable("Campers");
                });

            modelBuilder.Entity("CampOrno.Models.CamperActivity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActivityID");

                    b.Property<int>("CamperID");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("NumberOfSessions");

                    b.HasKey("ID");

                    b.HasIndex("ActivityID");

                    b.HasIndex("CamperID", "ActivityID")
                        .IsUnique();

                    b.ToTable("CamperActivities");
                });

            modelBuilder.Entity("CampOrno.Models.CamperDiet", b =>
                {
                    b.Property<int>("CamperID");

                    b.Property<int>("DietaryRestrictionID");

                    b.HasKey("CamperID", "DietaryRestrictionID");

                    b.HasIndex("DietaryRestrictionID");

                    b.ToTable("CamperDiets");
                });

            modelBuilder.Entity("CampOrno.Models.Compound", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("Compounds");
                });

            modelBuilder.Entity("CampOrno.Models.Counselor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50);

                    b.Property<string>("Nickname")
                        .HasMaxLength(50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SIN")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("SIN")
                        .IsUnique();

                    b.ToTable("Counselors");
                });

            modelBuilder.Entity("CampOrno.Models.CounselorCompound", b =>
                {
                    b.Property<int>("CompoundID");

                    b.Property<int>("CounselorID");

                    b.HasKey("CompoundID", "CounselorID");

                    b.HasIndex("CounselorID");

                    b.ToTable("CounselorCompounds");
                });

            modelBuilder.Entity("CampOrno.Models.DietaryRestriction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("DietaryRestrictions");
                });

            modelBuilder.Entity("CampOrno.Models.Camper", b =>
                {
                    b.HasOne("CampOrno.Models.Compound", "Compound")
                        .WithMany("Campers")
                        .HasForeignKey("CompoundID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CampOrno.Models.Counselor", "Counselor")
                        .WithMany("Campers")
                        .HasForeignKey("CounselorID");
                });

            modelBuilder.Entity("CampOrno.Models.CamperActivity", b =>
                {
                    b.HasOne("CampOrno.Models.Activity", "Activity")
                        .WithMany("CamperActivities")
                        .HasForeignKey("ActivityID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CampOrno.Models.Camper", "Camper")
                        .WithMany("CamperActivities")
                        .HasForeignKey("CamperID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CampOrno.Models.CamperDiet", b =>
                {
                    b.HasOne("CampOrno.Models.Camper", "Camper")
                        .WithMany("CamperDiets")
                        .HasForeignKey("CamperID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CampOrno.Models.DietaryRestriction", "DietaryRestriction")
                        .WithMany("CamperDiets")
                        .HasForeignKey("DietaryRestrictionID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CampOrno.Models.CounselorCompound", b =>
                {
                    b.HasOne("CampOrno.Models.Compound", "Compound")
                        .WithMany("CounselorCompounds")
                        .HasForeignKey("CompoundID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CampOrno.Models.Counselor", "Counselor")
                        .WithMany("CounselorCompounds")
                        .HasForeignKey("CounselorID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
