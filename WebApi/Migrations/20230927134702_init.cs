using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ParagraphOne = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ParagraphTwo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Car_Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TitleKz = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TitleRu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DescriptionKz = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DescriptionRu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailOtp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailOtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article_Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Images_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Battery = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    BatteryKz = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    BatteryRu = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    Performance = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    PerformanceKz = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    PerformanceRu = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    Range = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    RangeKz = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    RangeRu = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    Efficiency = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    EfficiencyKz = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    EfficiencyRu = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    BatteryPower = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RealRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TopSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EfficiencyVal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    BestSeller = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Stars1 = table.Column<int>(type: "int", nullable: false),
                    Stars2 = table.Column<int>(type: "int", nullable: false),
                    Stars3 = table.Column<int>(type: "int", nullable: false),
                    Stars4 = table.Column<int>(type: "int", nullable: false),
                    Stars5 = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Car_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Car_Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.Role });
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car_Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Car_Images_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car_Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Car_Reviews_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_Images_ArticleId",
                table: "Article_Images",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarId",
                table: "Bookings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_Images_CarId",
                table: "Car_Images",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_Reviews_CarId",
                table: "Car_Reviews",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandId",
                table: "Cars",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedDate",
                table: "Users",
                column: "UpdatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article_Images");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Car_Images");

            migrationBuilder.DropTable(
                name: "Car_Reviews");

            migrationBuilder.DropTable(
                name: "ContactSubmissions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Car_Brands");
        }
    }
}
