//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace VideoChatWebApp.Data.Migrations
//{
//    public partial class Initial : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Users",
//                columns: table => new
//                {
//                    Id = table.Column<string>(nullable: false),
//                    UserName = table.Column<string>(maxLength: 128, nullable: false),
//                    Email = table.Column<string>(nullable: false),
//                    DisplayName = table.Column<string>(nullable: true),
//                    Notes = table.Column<string>(nullable: true),
//                    Type = table.Column<int>(nullable: false),
//                    Flags = table.Column<int>(nullable: false),
//                    CreatedDate = table.Column<DateTime>(nullable: false),
//                    LastModifiedDate = table.Column<DateTime>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Users", x => x.Id);
//                });
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Users");
//        }
//    }
//}
