using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10330209PROG7311POE1.Migrations
{
   
    public partial class AddContractType : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Contracts");
        }
    }
}
