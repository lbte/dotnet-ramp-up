using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRFTLatam.OrdersData.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrdersTotalAttributeToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrdersTotal",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrdersTotal",
                table: "Clients");
        }
    }
}
