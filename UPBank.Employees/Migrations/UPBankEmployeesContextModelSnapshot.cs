﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UPBank.Employees.Data;

#nullable disable

namespace UPBank.Employees.Migrations
{
    [DbContext(typeof(UPBankEmployeesContext))]
    partial class UPBankEmployeesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("UPBank.Models.Person", b =>
                {
                    b.Property<string>("Cpf")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressZipcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Cpf");

                    b.ToTable("People", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Person");
                });

            modelBuilder.Entity("UPBank.Models.Employee", b =>
                {
                    b.HasBaseType("UPBank.Models.Person");

                    b.Property<string>("AgencyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Manager")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}
