﻿// <auto-generated />
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Z011.ConsoleApp.Migrations
{
    public partial class Start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Stocks");

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockPrices",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    Frequency = table.Column<byte>(type: "INTEGER", nullable: false),
                    Period = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Open = table.Column<double>(type: "REAL", nullable: false),
                    Low = table.Column<double>(type: "REAL", nullable: false),
                    High = table.Column<double>(type: "REAL", nullable: false),
                    Close = table.Column<double>(type: "REAL", nullable: false),
                    AdjClose = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPrices_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "Stocks",
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockPrices_StockId_Frequency_Period",
                schema: "Stocks",
                table: "StockPrices",
                columns: new[] { "StockId", "Frequency", "Period" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Symbol",
                schema: "Stocks",
                table: "Stocks",
                column: "Symbol",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPrices",
                schema: "Stocks");

            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "Stocks");
        }
    }
}
