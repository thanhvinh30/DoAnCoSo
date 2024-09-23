using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class EmptyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ordering = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    Levels = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CatId);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CusMess = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CusEmail = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameWithType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PathWithType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DesRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    ShipperId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipperName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_Shipper", x => x.ShipperId);
                });

            migrationBuilder.CreateTable(
                name: "TransactStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DesStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatId = table.Column<int>(type: "int", nullable: true),
                    ProName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProImage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitlnStock = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    BestSellers = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    HomeFlag = table.Column<bool>(type: "bit", nullable: false),
                    ShortDes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MeetaKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProId);
                    table.ForeignKey(
                        name: "FK_Products_Category",
                        column: x => x.CatId,
                        principalTable: "Category",
                        principalColumn: "CatId");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    CusPassword = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CusEmail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastLogin = table.Column<DateOnly>(type: "date", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Salt = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CusId);
                    table.ForeignKey(
                        name: "FK_Customer_Locations",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserNameAcc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    LastLogin = table.Column<DateOnly>(type: "date", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    PasswordAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Salt = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusId = table.Column<int>(type: "int", nullable: true),
                    OderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: true),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerPhone = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customer",
                        column: x => x.CusId,
                        principalTable: "Customer",
                        principalColumn: "CusId");
                    table.ForeignKey(
                        name: "FK_Orders_TransactStatus",
                        column: x => x.StatusId,
                        principalTable: "TransactStatus",
                        principalColumn: "StatusId");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProId = table.Column<int>(type: "int", nullable: false),
                    ProName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProImage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_Orders_CartId",
                        column: x => x.CartId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProId = table.Column<int>(type: "int", nullable: true),
                    ProName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProImage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusOrderDetail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LocationId",
                table: "Customer",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CusId",
                table: "Orders",
                column: "CusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CatId",
                table: "Products",
                column: "CatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "TransactStatus");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
