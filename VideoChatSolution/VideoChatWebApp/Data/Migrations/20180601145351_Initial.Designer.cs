﻿//// <auto-generated />
//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using VideoChatWebApp.Data;

//namespace VideoChatWebApp.Data.Migrations
//{
//    [DbContext(typeof(ApplicationDbContext))]
//    [Migration("20180601145351_Initial")]
//    partial class Initial
//    {
//        protected override void BuildTargetModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128)
//                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

//            modelBuilder.Entity("VideoChatWebApp.Data.TestMakerFreeWebApp.Data.ApplicationUser", b =>
//                {
//                    b.Property<string>("Id")
//                        .ValueGeneratedOnAdd();

//                    b.Property<DateTime>("CreatedDate");

//                    b.Property<string>("DisplayName");

//                    b.Property<string>("Email")
//                        .IsRequired();

//                    b.Property<int>("Flags");

//                    b.Property<DateTime>("LastModifiedDate");

//                    b.Property<string>("Notes");

//                    b.Property<int>("Type");

//                    b.Property<string>("UserName")
//                        .IsRequired()
//                        .HasMaxLength(128);

//                    b.HasKey("Id");

//                    b.ToTable("Users");
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
