﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UPBank.Accounts.Data;

#nullable disable

namespace UPBank.Accounts.Migrations
{
    [DbContext(typeof(UPBankAccountsContext))]
    [Migration("20240624210704_CreateCreditCardTable")]
    partial class CreateCreditCardTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("UPBank.Models.Account", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AgencyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreditCardNumber")
                        .HasColumnType("bigint");

                    b.Property<double>("Overdraft")
                        .HasColumnType("float");

                    b.Property<int>("Profile")
                        .HasColumnType("int");

                    b.Property<bool>("Restriction")
                        .HasColumnType("bit");

                    b.Property<string>("SavingsAccount")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("UPBank.Models.AccountCustomer", b =>
                {
                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerCpf")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AccountNumber", "CustomerCpf");

                    b.ToTable("AccountCustomer");
                });

            modelBuilder.Entity("UPBank.Models.CreditCard", b =>
                {
                    b.Property<long>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Number"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExtractionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Holder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Limit")
                        .HasColumnType("float");

                    b.HasKey("Number");

                    b.ToTable("CreditCard");
                });
#pragma warning restore 612, 618
        }
    }
}
