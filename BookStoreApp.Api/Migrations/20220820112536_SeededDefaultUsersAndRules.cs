using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.Api.Migrations
{
    public partial class SeededDefaultUsersAndRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b87f8d7-bb91-4f78-a15f-8012d0b63cd4", "22caa177-4992-49cb-a41a-2dd46dec1501", "Admin", "ADMINISTRATOR" },
                    { "72ca8c68-e23e-4f08-8b38-f544e2fd5e5d", "0a6c5c2d-ee49-4a2c-8294-0ec64ccc7b72", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0271cb0c-eb30-4a54-a973-62c227713452", 0, "4b68e58b-5463-4b6e-9043-cd49b4ee8da5", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", null, "AQAAAAEAACcQAAAAEIJQMh5eVa2eTLYBqnsnw8r22a4kxi43pApiacyzCmmlHnYsuc+rj4zkJvn0Kax+iA==", null, false, "8bdeff1d-9689-4ad4-8442-3bfeabde9137", false, "user@bookstore.com" },
                    { "6b671dfd-9d7c-4f20-9d6a-e6a3334c68b8", 0, "44560eed-87d6-476b-866f-2c9f21f01ddb", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", null, "AQAAAAEAACcQAAAAEBO5bmnH6a+CN+tpQQMxt/K11wllWIwLFJILe3+TQz3j0HqelzpvMaUYwjOjkKurLA==", null, false, "0583eb28-935e-4235-880d-2636934edb25", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "72ca8c68-e23e-4f08-8b38-f544e2fd5e5d", "0271cb0c-eb30-4a54-a973-62c227713452" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0b87f8d7-bb91-4f78-a15f-8012d0b63cd4", "6b671dfd-9d7c-4f20-9d6a-e6a3334c68b8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "72ca8c68-e23e-4f08-8b38-f544e2fd5e5d", "0271cb0c-eb30-4a54-a973-62c227713452" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0b87f8d7-bb91-4f78-a15f-8012d0b63cd4", "6b671dfd-9d7c-4f20-9d6a-e6a3334c68b8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b87f8d7-bb91-4f78-a15f-8012d0b63cd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72ca8c68-e23e-4f08-8b38-f544e2fd5e5d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0271cb0c-eb30-4a54-a973-62c227713452");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b671dfd-9d7c-4f20-9d6a-e6a3334c68b8");
        }
    }
}
