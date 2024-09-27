using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalRecordProcessor.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    UploadStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    MedicalReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDetails_MedicalReportId",
                        column: x => x.MedicalReportId,
                        principalTable: "MedicalReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportDetailsErrors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ReportDetailsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDetailsErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDetailsError_ReportDetailsId",
                        column: x => x.ReportDetailsId,
                        principalTable: "ReportDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetails_ContractId_RecordId",
                table: "ReportDetails",
                columns: new[] { "ContractId", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetails_MedicalReportId",
                table: "ReportDetails",
                column: "MedicalReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetailsErrors_ReportDetailsId",
                table: "ReportDetailsErrors",
                column: "ReportDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportDetailsErrors");

            migrationBuilder.DropTable(
                name: "ReportDetails");

            migrationBuilder.DropTable(
                name: "MedicalReports");
        }
    }
}
