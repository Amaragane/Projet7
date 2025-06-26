using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    public partial class initialise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BidList",
                columns: table => new
                {
                    BidListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BidType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BidQuantity = table.Column<double>(type: "float", nullable: true),
                    AskQuantity = table.Column<double>(type: "float", nullable: true),
                    Bid = table.Column<double>(type: "float", nullable: true),
                    Ask = table.Column<double>(type: "float", nullable: true),
                    Benchmark = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    BidListDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Commentary = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    BidSecurity = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    BidStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Trader = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Book = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreationName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    DealType = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    SourceListId = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Side = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidList", x => x.BidListId);
                });

            migrationBuilder.CreateTable(
                name: "CurvePoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurveId = table.Column<byte>(type: "tinyint", nullable: false),
                    AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<double>(type: "float", nullable: true),
                    CurvePointValue = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvePoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoodysRating = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    SandPRating = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    FitchRating = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    OrderNumber = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Json = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Template = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SqlStr = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    SqlPart = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trade",
                columns: table => new
                {
                    TradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BuyQuantity = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SellQuantity = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BuyPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SellPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TradeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TradeSecurity = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    TradeStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Trader = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Benchmark = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Book = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreationName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    DealType = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    SourceListId = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Side = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.TradeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", "ed71ae3a-af2f-4dc5-974c-345f61771fb9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2", "895a2c8f-2dc9-48bc-b322-ade24c984ec2", "User", "USER" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3", "a63e87d4-9fa4-4220-a7a1-7f97ef9ca8b8", "Manager", "MANAGER" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidList");

            migrationBuilder.DropTable(
                name: "CurvePoint");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "RuleName");

            migrationBuilder.DropTable(
                name: "Trade");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
