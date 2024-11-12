﻿// <auto-generated />
using System;
using AMMS.Hanet.Datas.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    [DbContext(typeof(DeviceAutoPushDbContext))]
    partial class DeviceAutoPushDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Hanet")
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AMMS.Hanet.Datas.Entities.app_config", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientId")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientScret")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<long?>("Expire")
                        .HasColumnType("bigint");

                    b.Property<string>("GrantType")
                        .HasColumnType("longtext");

                    b.Property<string>("PlaceId")
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<string>("TokenType")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("hanet_app_config", "Hanet");
                });

            modelBuilder.Entity("AMMS.Hanet.Datas.Entities.hanet_commandlog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("change_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("command_ation")
                        .HasColumnType("longtext");

                    b.Property<string>("command_type")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("commit_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("content")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("create_time")
                        .HasColumnType("datetime(6)");

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

                    b.ToTable("hanet_commandlog", "Hanet");
                });

            modelBuilder.Entity("AMMS.Hanet.Datas.Entities.hanet_terminal", b =>
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

                    b.Property<DateTime?>("last_activity")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("last_checkconnection")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool?>("online_status")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("port")
                        .HasColumnType("int");

                    b.Property<DateTime?>("push_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("sn")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("time_offline")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("time_online")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("upload_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("user_count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("hanet_terminal", "Hanet");
                });

            modelBuilder.Entity("AMMS.Hanet.Datas.Entities.hanet_transaction", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("created_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("deviceID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double?>("time")
                        .HasColumnType("double");

                    b.Property<string>("transaction_type")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("hanet_transaction", "Hanet");
                });
#pragma warning restore 612, 618
        }
    }
}
