//using System;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace VideoChatWebApp.Data.Migrations
//{
//    public partial class Identity : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AlterColumn<string>(
//                name: "UserName",
//                table: "Users",
//                maxLength: 256,
//                nullable: true,
//                oldClrType: typeof(string),
//                oldMaxLength: 128);

//            migrationBuilder.AlterColumn<string>(
//                name: "Email",
//                table: "Users",
//                maxLength: 256,
//                nullable: true,
//                oldClrType: typeof(string));

//            migrationBuilder.AddColumn<int>(
//                name: "AccessFailedCount",
//                table: "Users",
//                nullable: false,
//                defaultValue: 0);

//            migrationBuilder.AddColumn<string>(
//                name: "ConcurrencyStamp",
//                table: "Users",
//                nullable: true);

//            migrationBuilder.AddColumn<bool>(
//                name: "EmailConfirmed",
//                table: "Users",
//                nullable: false,
//                defaultValue: false);

//            migrationBuilder.AddColumn<bool>(
//                name: "LockoutEnabled",
//                table: "Users",
//                nullable: false,
//                defaultValue: false);

//            migrationBuilder.AddColumn<DateTimeOffset>(
//                name: "LockoutEnd",
//                table: "Users",
//                nullable: true);

//            migrationBuilder.AddColumn<string>(
//                name: "NormalizedEmail",
//                table: "Users",
//                maxLength: 256,
//                nullable: true);

//            migrationBuilder.AddColumn<string>(
//                name: "NormalizedUserName",
//                table: "Users",
//                maxLength: 256,
//                nullable: true);

//            migrationBuilder.AddColumn<string>(
//                name: "PasswordHash",
//                table: "Users",
//                nullable: true);

//            migrationBuilder.AddColumn<string>(
//                name: "PhoneNumber",
//                table: "Users",
//                nullable: true);

//            migrationBuilder.AddColumn<bool>(
//                name: "PhoneNumberConfirmed",
//                table: "Users",
//                nullable: false,
//                defaultValue: false);

//            migrationBuilder.AddColumn<string>(
//                name: "SecurityStamp",
//                table: "Users",
//                nullable: true);

//            migrationBuilder.AddColumn<bool>(
//                name: "TwoFactorEnabled",
//                table: "Users",
//                nullable: false,
//                defaultValue: false);

//            migrationBuilder.CreateTable(
//                name: "AspNetRoles",
//                columns: table => new
//                {
//                    Id = table.Column<string>(nullable: false),
//                    Name = table.Column<string>(maxLength: 256, nullable: true),
//                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
//                    ConcurrencyStamp = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    UserId = table.Column<string>(nullable: false),
//                    ClaimType = table.Column<string>(nullable: true),
//                    ClaimValue = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserClaims_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserLogins",
//                columns: table => new
//                {
//                    LoginProvider = table.Column<string>(nullable: false),
//                    ProviderKey = table.Column<string>(nullable: false),
//                    ProviderDisplayName = table.Column<string>(nullable: true),
//                    UserId = table.Column<string>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserLogins_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserTokens",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(nullable: false),
//                    LoginProvider = table.Column<string>(nullable: false),
//                    Name = table.Column<string>(nullable: false),
//                    Value = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserTokens_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetRoleClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    RoleId = table.Column<string>(nullable: false),
//                    ClaimType = table.Column<string>(nullable: true),
//                    ClaimValue = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserRoles",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(nullable: false),
//                    RoleId = table.Column<string>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "EmailIndex",
//                table: "Users",
//                column: "NormalizedEmail");

//            migrationBuilder.CreateIndex(
//                name: "UserNameIndex",
//                table: "Users",
//                column: "NormalizedUserName",
//                unique: true,
//                filter: "[NormalizedUserName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetRoleClaims_RoleId",
//                table: "AspNetRoleClaims",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "RoleNameIndex",
//                table: "AspNetRoles",
//                column: "NormalizedName",
//                unique: true,
//                filter: "[NormalizedName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserClaims_UserId",
//                table: "AspNetUserClaims",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserLogins_UserId",
//                table: "AspNetUserLogins",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserRoles_RoleId",
//                table: "AspNetUserRoles",
//                column: "RoleId");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "AspNetRoleClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserLogins");

//            migrationBuilder.DropTable(
//                name: "AspNetUserRoles");

//            migrationBuilder.DropTable(
//                name: "AspNetUserTokens");

//            migrationBuilder.DropTable(
//                name: "AspNetRoles");

//            migrationBuilder.DropIndex(
//                name: "EmailIndex",
//                table: "Users");

//            migrationBuilder.DropIndex(
//                name: "UserNameIndex",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "AccessFailedCount",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "ConcurrencyStamp",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "EmailConfirmed",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "LockoutEnabled",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "LockoutEnd",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "NormalizedEmail",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "NormalizedUserName",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "PasswordHash",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "PhoneNumber",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "PhoneNumberConfirmed",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "SecurityStamp",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "TwoFactorEnabled",
//                table: "Users");

//            migrationBuilder.AlterColumn<string>(
//                name: "UserName",
//                table: "Users",
//                maxLength: 128,
//                nullable: false,
//                oldClrType: typeof(string),
//                oldMaxLength: 256,
//                oldNullable: true);

//            migrationBuilder.AlterColumn<string>(
//                name: "Email",
//                table: "Users",
//                nullable: false,
//                oldClrType: typeof(string),
//                oldMaxLength: 256,
//                oldNullable: true);
//        }
//    }
//}
