using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TooManyRules.Models;

namespace TooManyRules.Models.Migrations
{
    [DbContext(typeof(TooManyRulesContext))]
    [Migration("20170312020353_CreateDatabase")]
    partial class CreateDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TooManyRules.Models.Policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Definition")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Name")
                        .HasName("Unique_PolicyName");

                    b.ToTable("Polies");
                });

            modelBuilder.Entity("TooManyRules.Models.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Definition")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PolicyId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name")
                        .HasName("Unique_RuleName");

                    b.HasIndex("PolicyId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("TooManyRules.Models.Rule", b =>
                {
                    b.HasOne("TooManyRules.Models.Policy", "Policy")
                        .WithMany("Rules")
                        .HasForeignKey("PolicyId")
                        .HasConstraintName("FK_Rule_Policy")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
