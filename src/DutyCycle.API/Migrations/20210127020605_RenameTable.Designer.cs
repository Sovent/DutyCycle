﻿// <auto-generated />
using System;
using DutyCycle.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DutyCycle.API.Migrations
{
    [DbContext(typeof(DutyCycleDbContext))]
    [Migration("20210127020605_RenameTable")]
    partial class RenameTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("DutyCycle.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("CyclingCronExpression")
                        .HasColumnType("text");

                    b.Property<int>("DutiesCount")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("DutyCycle.GroupMember", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FollowedGroupMemberId")
                        .HasColumnType("uuid");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FollowedGroupMemberId")
                        .IsUnique();

                    b.HasIndex("GroupId");

                    b.ToTable("GroupMember");
                });

            modelBuilder.Entity("DutyCycle.Triggers.RotationChangedTrigger", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("TriggerType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("RotationChangedTrigger");

                    b.HasDiscriminator<string>("TriggerType").HasValue("RotationChangedTrigger");
                });

            modelBuilder.Entity("DutyCycle.Triggers.SendSlackMessageTrigger", b =>
                {
                    b.HasBaseType("DutyCycle.Triggers.RotationChangedTrigger");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<string>("MessageTextTemplate")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("send_slack_message");
                });

            modelBuilder.Entity("DutyCycle.GroupMember", b =>
                {
                    b.HasOne("DutyCycle.GroupMember", null)
                        .WithOne()
                        .HasForeignKey("DutyCycle.GroupMember", "FollowedGroupMemberId");

                    b.HasOne("DutyCycle.Group", null)
                        .WithMany("_groupMembers")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("DutyCycle.Triggers.RotationChangedTrigger", b =>
                {
                    b.HasOne("DutyCycle.Group", null)
                        .WithMany("_triggers")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("DutyCycle.Group", b =>
                {
                    b.Navigation("_groupMembers");

                    b.Navigation("_triggers");
                });
#pragma warning restore 612, 618
        }
    }
}
