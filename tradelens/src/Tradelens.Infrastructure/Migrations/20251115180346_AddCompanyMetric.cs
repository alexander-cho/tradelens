using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tradelens.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyMetric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Stocks_Ticker",
                table: "Stocks",
                column: "Ticker");

            migrationBuilder.CreateTable(
                name: "CompanyMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ticker = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Interval = table.Column<string>(type: "text", nullable: false),
                    PeriodEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metric = table.Column<string>(type: "text", nullable: false),
                    ParentMetric = table.Column<string>(type: "text", nullable: false),
                    Section = table.Column<string>(type: "text", nullable: false),
                    SourcedFrom = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyMetrics_Stocks_Ticker",
                        column: x => x.Ticker,
                        principalTable: "Stocks",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMetrics_Metric",
                table: "CompanyMetrics",
                column: "Metric");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMetrics_Ticker_Period_Year_Metric",
                table: "CompanyMetrics",
                columns: new[] { "Ticker", "Period", "Year", "Metric" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMetrics_Ticker_Year",
                table: "CompanyMetrics",
                columns: new[] { "Ticker", "Year" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyMetrics");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Stocks_Ticker",
                table: "Stocks");
        }
    }
}
