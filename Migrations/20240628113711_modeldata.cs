using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebKimCuong.Migrations
{
    public partial class modeldata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chungNhans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chungNhans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DimInfoMsts",
                columns: table => new
                {
                    DimID = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    DimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DimSubType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DimCrt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DimPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DimImg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimInfoMsts", x => x.DimID);
                });

            migrationBuilder.CreateTable(
                name: "DimQltyMst",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diachi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimQltyMst", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DimQltySubMsts",
                columns: table => new
                {
                    DimSubType_ID = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    DimQlty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimQltySubMsts", x => x.DimSubType_ID);
                });

            migrationBuilder.CreateTable(
                name: "merchants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantWebLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantIpnUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantReturnUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Motois",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motois", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paymentDescriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesSortIndex = table.Column<int>(type: "int", nullable: false),
                    ParentIdid = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentDescriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentDescriptions_paymentDescriptions_ParentIdid",
                        column: x => x.ParentIdid,
                        principalTable: "paymentDescriptions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StoneQltyMsts",
                columns: table => new
                {
                    StoneQlty_ID = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    StoneQlty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoneQltyMsts", x => x.StoneQlty_ID);
                });

            migrationBuilder.CreateTable(
                name: "ThuonghieuMsts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuonghieuMsts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentRefId = table.Column<int>(type: "int", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpireDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PaymentLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantIdid = table.Column<int>(type: "int", nullable: true),
                    PaymentDestinationIdid = table.Column<int>(type: "int", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentLastMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_merchants_MerchantIdid",
                        column: x => x.MerchantIdid,
                        principalTable: "merchants",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_payments_paymentDescriptions_PaymentDestinationIdid",
                        column: x => x.PaymentDestinationIdid,
                        principalTable: "paymentDescriptions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserRegMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<bool>(type: "bit", nullable: false),
                    roleid = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRegMsts_roles_roleid",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "paymentNotifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentRefId = table.Column<int>(type: "int", nullable: true),
                    NotiDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIdid = table.Column<int>(type: "int", nullable: true),
                    MerchantId = table.Column<int>(type: "int", nullable: true),
                    NotiSatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiResDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentNotifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentNotifications_payments_PaymentIdid",
                        column: x => x.PaymentIdid,
                        principalTable: "payments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "paymentSignatures",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignAlgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SignOwn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIdid = table.Column<int>(type: "int", nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentSignatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentSignatures_payments_PaymentIdid",
                        column: x => x.PaymentIdid,
                        principalTable: "payments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "paymentTransactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranPayLoad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TranDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PaymentIdid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentTransactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentTransactions_payments_PaymentIdid",
                        column: x => x.PaymentIdid,
                        principalTable: "payments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CatMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    creatorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_CatMsts_UserRegMsts_accountid",
                        column: x => x.accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CertifyMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accountsid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertifyMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_CertifyMsts_UserRegMsts_Accountsid",
                        column: x => x.Accountsid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Inquirys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diachi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sodienthoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountid = table.Column<int>(type: "int", nullable: true),
                    vanchuyenid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquirys", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inquirys_DimQltyMst_vanchuyenid",
                        column: x => x.vanchuyenid,
                        principalTable: "DimQltyMst",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Inquirys_UserRegMsts_accountid",
                        column: x => x.accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "JewelTypeMst",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<float>(type: "real", nullable: true),
                    soluong = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false),
                    trangthai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accountid = table.Column<int>(type: "int", nullable: true),
                    vanchuyenid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JewelTypeMst", x => x.id);
                    table.ForeignKey(
                        name: "FK_JewelTypeMst_DimQltyMst_vanchuyenid",
                        column: x => x.vanchuyenid,
                        principalTable: "DimQltyMst",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_JewelTypeMst_UserRegMsts_Accountid",
                        column: x => x.Accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountid = table.Column<int>(type: "int", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    geneToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_tokens_UserRegMsts_accountid",
                        column: x => x.accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ProdMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<float>(type: "real", nullable: false),
                    click = table.Column<int>(type: "int", nullable: false),
                    Accountsid = table.Column<int>(type: "int", nullable: true),
                    Categorysid = table.Column<int>(type: "int", nullable: true),
                    Shopsid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProdMsts_CatMsts_Categorysid",
                        column: x => x.Categorysid,
                        principalTable: "CatMsts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ProdMsts_Inquirys_Shopsid",
                        column: x => x.Shopsid,
                        principalTable: "Inquirys",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ProdMsts_UserRegMsts_Accountsid",
                        column: x => x.Accountsid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "StoneMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shopid = table.Column<int>(type: "int", nullable: true),
                    Vanchuyenid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoneMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_StoneMsts_DimQltyMst_Vanchuyenid",
                        column: x => x.Vanchuyenid,
                        principalTable: "DimQltyMst",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_StoneMsts_Inquirys_shopid",
                        column: x => x.shopid,
                        principalTable: "Inquirys",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    images = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    accountsid = table.Column<int>(type: "int", nullable: true),
                    productsid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_ProdMsts_productsid",
                        column: x => x.productsid,
                        principalTable: "ProdMsts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comments_UserRegMsts_accountsid",
                        column: x => x.accountsid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "danhGias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sao = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountid = table.Column<int>(type: "int", nullable: true),
                    productid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danhGias", x => x.id);
                    table.ForeignKey(
                        name: "FK_danhGias_ProdMsts_productid",
                        column: x => x.productid,
                        principalTable: "ProdMsts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_danhGias_UserRegMsts_accountid",
                        column: x => x.accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DimMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    Ordersid = table.Column<int>(type: "int", nullable: true),
                    Productsid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_DimMsts_CertifyMsts_Ordersid",
                        column: x => x.Ordersid,
                        principalTable: "CertifyMsts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DimMsts_ProdMsts_Productsid",
                        column: x => x.Productsid,
                        principalTable: "ProdMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GoldKrtMst",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productid = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoldKrtMst", x => x.id);
                    table.ForeignKey(
                        name: "FK_GoldKrtMst_ProdMsts_productid",
                        column: x => x.productid,
                        principalTable: "ProdMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ItemMsts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Productid = table.Column<int>(type: "int", nullable: true),
                    Categoryid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMsts", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItemMsts_CatMsts_Categoryid",
                        column: x => x.Categoryid,
                        principalTable: "CatMsts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ItemMsts_ProdMsts_Productid",
                        column: x => x.Productid,
                        principalTable: "ProdMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "commentDescriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    images = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    commentDescriptionsid = table.Column<int>(type: "int", nullable: true),
                    commentid = table.Column<int>(type: "int", nullable: true),
                    accountid = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CretorEdit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commentDescriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_commentDescriptions_commentDescriptions_commentDescriptionsid",
                        column: x => x.commentDescriptionsid,
                        principalTable: "commentDescriptions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_commentDescriptions_comments_commentid",
                        column: x => x.commentid,
                        principalTable: "comments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_commentDescriptions_UserRegMsts_accountid",
                        column: x => x.accountid,
                        principalTable: "UserRegMsts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatMsts_accountid",
                table: "CatMsts",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_CertifyMsts_Accountsid",
                table: "CertifyMsts",
                column: "Accountsid");

            migrationBuilder.CreateIndex(
                name: "IX_commentDescriptions_accountid",
                table: "commentDescriptions",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_commentDescriptions_commentDescriptionsid",
                table: "commentDescriptions",
                column: "commentDescriptionsid");

            migrationBuilder.CreateIndex(
                name: "IX_commentDescriptions_commentid",
                table: "commentDescriptions",
                column: "commentid");

            migrationBuilder.CreateIndex(
                name: "IX_comments_accountsid",
                table: "comments",
                column: "accountsid");

            migrationBuilder.CreateIndex(
                name: "IX_comments_productsid",
                table: "comments",
                column: "productsid");

            migrationBuilder.CreateIndex(
                name: "IX_danhGias_accountid",
                table: "danhGias",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_danhGias_productid",
                table: "danhGias",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_DimMsts_Ordersid",
                table: "DimMsts",
                column: "Ordersid");

            migrationBuilder.CreateIndex(
                name: "IX_DimMsts_Productsid",
                table: "DimMsts",
                column: "Productsid");

            migrationBuilder.CreateIndex(
                name: "IX_GoldKrtMst_productid",
                table: "GoldKrtMst",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_Inquirys_accountid",
                table: "Inquirys",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_Inquirys_vanchuyenid",
                table: "Inquirys",
                column: "vanchuyenid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMsts_Categoryid",
                table: "ItemMsts",
                column: "Categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMsts_Productid",
                table: "ItemMsts",
                column: "Productid");

            migrationBuilder.CreateIndex(
                name: "IX_JewelTypeMst_Accountid",
                table: "JewelTypeMst",
                column: "Accountid");

            migrationBuilder.CreateIndex(
                name: "IX_JewelTypeMst_vanchuyenid",
                table: "JewelTypeMst",
                column: "vanchuyenid");

            migrationBuilder.CreateIndex(
                name: "IX_paymentDescriptions_ParentIdid",
                table: "paymentDescriptions",
                column: "ParentIdid");

            migrationBuilder.CreateIndex(
                name: "IX_paymentNotifications_PaymentIdid",
                table: "paymentNotifications",
                column: "PaymentIdid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_MerchantIdid",
                table: "payments",
                column: "MerchantIdid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_PaymentDestinationIdid",
                table: "payments",
                column: "PaymentDestinationIdid");

            migrationBuilder.CreateIndex(
                name: "IX_paymentSignatures_PaymentIdid",
                table: "paymentSignatures",
                column: "PaymentIdid");

            migrationBuilder.CreateIndex(
                name: "IX_paymentTransactions_PaymentIdid",
                table: "paymentTransactions",
                column: "PaymentIdid");

            migrationBuilder.CreateIndex(
                name: "IX_ProdMsts_Accountsid",
                table: "ProdMsts",
                column: "Accountsid");

            migrationBuilder.CreateIndex(
                name: "IX_ProdMsts_Categorysid",
                table: "ProdMsts",
                column: "Categorysid");

            migrationBuilder.CreateIndex(
                name: "IX_ProdMsts_Shopsid",
                table: "ProdMsts",
                column: "Shopsid");

            migrationBuilder.CreateIndex(
                name: "IX_StoneMsts_shopid",
                table: "StoneMsts",
                column: "shopid");

            migrationBuilder.CreateIndex(
                name: "IX_StoneMsts_Vanchuyenid",
                table: "StoneMsts",
                column: "Vanchuyenid");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_accountid",
                table: "tokens",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegMsts_roleid",
                table: "UserRegMsts",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chungNhans");

            migrationBuilder.DropTable(
                name: "commentDescriptions");

            migrationBuilder.DropTable(
                name: "danhGias");

            migrationBuilder.DropTable(
                name: "DimInfoMsts");

            migrationBuilder.DropTable(
                name: "DimMsts");

            migrationBuilder.DropTable(
                name: "DimQltySubMsts");

            migrationBuilder.DropTable(
                name: "GoldKrtMst");

            migrationBuilder.DropTable(
                name: "ItemMsts");

            migrationBuilder.DropTable(
                name: "JewelTypeMst");

            migrationBuilder.DropTable(
                name: "Motois");

            migrationBuilder.DropTable(
                name: "paymentNotifications");

            migrationBuilder.DropTable(
                name: "paymentSignatures");

            migrationBuilder.DropTable(
                name: "paymentTransactions");

            migrationBuilder.DropTable(
                name: "StoneMsts");

            migrationBuilder.DropTable(
                name: "StoneQltyMsts");

            migrationBuilder.DropTable(
                name: "ThuonghieuMsts");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "CertifyMsts");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "ProdMsts");

            migrationBuilder.DropTable(
                name: "merchants");

            migrationBuilder.DropTable(
                name: "paymentDescriptions");

            migrationBuilder.DropTable(
                name: "CatMsts");

            migrationBuilder.DropTable(
                name: "Inquirys");

            migrationBuilder.DropTable(
                name: "DimQltyMst");

            migrationBuilder.DropTable(
                name: "UserRegMsts");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
