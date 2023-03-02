using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitMgr.Infrastructure.Migrations;

/// <inheritdoc />
public partial class updateUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Code",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "AspNetUsers",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedDate",
            table: "AspNetUsers",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "AspNetUsers",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "AspNetUsers",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedDate",
            table: "AspNetUsers",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Code",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "CreatedDate",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "UpdatedDate",
            table: "AspNetUsers");
    }
}
