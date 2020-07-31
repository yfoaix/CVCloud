using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CQUCloudWeb.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Algorithm",
                columns: table => new
                {
                    AlgorithmId = table.Column<string>(nullable: false),
                    AlgorithmName = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: false),
                    UserNameID = table.Column<string>(maxLength: 450, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastCallTime = table.Column<DateTime>(nullable: false),
                    MonthCallCount = table.Column<long>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    DisplayType = table.Column<int>(nullable: false),
                    EnvironmentType = table.Column<int>(nullable: false),
                    CodeFile = table.Column<byte[]>(nullable: true),
                    ContainerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Algorithm", x => x.AlgorithmId);
                });

            migrationBuilder.CreateTable(
                name: "App",
                columns: table => new
                {
                    AppId = table.Column<string>(nullable: false),
                    AppName = table.Column<string>(maxLength: 20, nullable: false),
                    UserNameID = table.Column<string>(maxLength: 450, nullable: true),
                    AlgorithmRefId = table.Column<string>(nullable: true),
                    APIKey = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastCallTime = table.Column<DateTime>(nullable: false),
                    MonthCallCount = table.Column<long>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    AIFunction = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App", x => x.AppId);
                    table.ForeignKey(
                        name: "FK_App_Algorithm_AlgorithmRefId",
                        column: x => x.AlgorithmRefId,
                        principalTable: "Algorithm",
                        principalColumn: "AlgorithmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_AlgorithmRefId",
                table: "App",
                column: "AlgorithmRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App");

            migrationBuilder.DropTable(
                name: "Algorithm");
        }
    }
}
