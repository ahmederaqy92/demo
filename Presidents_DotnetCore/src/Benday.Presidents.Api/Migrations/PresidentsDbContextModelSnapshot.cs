﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.Api.Migrations
{
    [DbContext(typeof(PresidentsDbContext))]
    partial class PresidentsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Feature");
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FeatureName");

                    b.Property<DateTime>("LogDate");

                    b.Property<string>("LogType");

                    b.Property<string>("Message");

                    b.Property<string>("ReferrerUrl");

                    b.Property<string>("RequestIpAddress");

                    b.Property<string>("RequestUrl");

                    b.Property<string>("UserAgent");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("LogEntry");
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.PersonFact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FactType");

                    b.Property<string>("FactValue");

                    b.Property<int>("PersonId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonFact");
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.Relationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FromPersonId");

                    b.Property<string>("RelationshipType")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("ToPersonId");

                    b.HasKey("Id");

                    b.HasIndex("FromPersonId");

                    b.HasIndex("ToPersonId");

                    b.ToTable("Relationship");
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.PersonFact", b =>
                {
                    b.HasOne("Benday.Presidents.Api.DataAccess.Person", "Person")
                        .WithMany("Facts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Benday.Presidents.Api.DataAccess.Relationship", b =>
                {
                    b.HasOne("Benday.Presidents.Api.DataAccess.Person", "FromPerson")
                        .WithMany("Relationships")
                        .HasForeignKey("FromPersonId");

                    b.HasOne("Benday.Presidents.Api.DataAccess.Person", "ToPerson")
                        .WithMany()
                        .HasForeignKey("ToPersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
