﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UPBank.Accounts.Data;

#nullable disable

namespace UPBank.Accounts.Migrations
{
    [DbContext(typeof(UPBankAccountsContext))]
    partial class UPBankAccountsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("CreditCardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Overdraft")
                        .HasColumnType("float");

                    b.Property<int>("Profile")
                        .HasColumnType("int");

                    b.Property<bool>("Restriction")
                        .HasColumnType("bit");

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
#pragma warning restore 612, 618
        }
    }
}
