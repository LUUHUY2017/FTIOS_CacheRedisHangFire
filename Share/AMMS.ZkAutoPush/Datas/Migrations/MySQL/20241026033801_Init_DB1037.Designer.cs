﻿// <auto-generated />
using System;
using AMMS.ZkAutoPush.Datas.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AMMS.ZkAutoPush.Datas.Migrations.MySql
{
    [DbContext(typeof(DeviceAutoPushDbContext))]
    [Migration("20241026033801_Init_DB1037")]
    partial class Init_DB1037
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Zkteco")
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_biodata", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FingerData")
                        .HasColumnType("longtext");

                    b.Property<int?>("FingerIndex")
                        .HasColumnType("int");

                    b.Property<string>("PersonId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("zk_biodata", "Zkteco");
                });

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_biophoto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext");

                    b.Property<string>("Folder")
                        .HasColumnType("longtext");

                    b.Property<string>("PersonId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("zk_biophoto", "Zkteco");
                });

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_terminal", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("area_id")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("change_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("change_user")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("create_user")
                        .HasColumnType("longtext");

                    b.Property<int?>("face_count")
                        .HasColumnType("int");

                    b.Property<int?>("fv_count")
                        .HasColumnType("int");

                    b.Property<string>("ip_address")
                        .HasColumnType("longtext");

                    b.Property<bool?>("isconnect")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("last_activity")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("port")
                        .HasColumnType("int");

                    b.Property<DateTime?>("push_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("sn")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("upload_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("user_count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("zk_terminal", "Zkteco");
                });

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_terminalcommandlog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<double>("command_id")
                        .HasColumnType("double");

                    b.Property<DateTime?>("commit_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("content")
                        .HasColumnType("longtext");

                    b.Property<string>("parent_id")
                        .HasColumnType("longtext");

                    b.Property<string>("request_id")
                        .HasColumnType("longtext");

                    b.Property<string>("return_content")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("return_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("return_value")
                        .HasColumnType("int");

                    b.Property<bool?>("successed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("terminal_id")
                        .HasColumnType("longtext");

                    b.Property<string>("terminal_sn")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("transfer_time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("zk_terminalcommandlog", "Zkteco");
                });

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("longtext");

                    b.Property<string>("IpAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("PersonCode")
                        .HasColumnType("longtext");

                    b.Property<string>("PersonId")
                        .HasColumnType("longtext");

                    b.Property<string>("SerrialNumber")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("TimeEvent")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("VerifyType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("zk_transaction", "Zkteco");
                });

            modelBuilder.Entity("AMMS.ZkAutoPush.Datas.Entities.zk_user", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("area_id")
                        .HasColumnType("longtext");

                    b.Property<string>("card_no")
                        .HasColumnType("longtext");

                    b.Property<string>("first_name")
                        .HasColumnType("longtext");

                    b.Property<string>("full_name")
                        .HasColumnType("longtext");

                    b.Property<string>("last_name")
                        .HasColumnType("longtext");

                    b.Property<int?>("privilege")
                        .HasColumnType("int");

                    b.Property<string>("user_code")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("zk_user", "Zkteco");
                });
#pragma warning restore 612, 618
        }
    }
}
