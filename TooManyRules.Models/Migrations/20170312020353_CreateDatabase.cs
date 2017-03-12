using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TooManyRules.Models.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Definition = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polies", x => x.Id);
                    table.UniqueConstraint("Unique_PolicyName", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Definition = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PolicyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.UniqueConstraint("Unique_RuleName", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Rule_Policy",
                        column: x => x.PolicyId,
                        principalTable: "Polies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PolicyId",
                table: "Rules",
                column: "PolicyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Polies");
        }
    }
}
