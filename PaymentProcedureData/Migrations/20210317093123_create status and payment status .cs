using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentProcedureData.Migrations
{
    public partial class createstatusandpaymentstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PaymentProcesses",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaymentProcessId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentStatuses_PaymentProcesses_PaymentProcessId",
                        column: x => x.PaymentProcessId,
                        principalTable: "PaymentProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_PaymentProcessId",
                table: "PaymentStatuses",
                column: "PaymentProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_StatusId",
                table: "PaymentStatuses",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PaymentProcesses",
                newName: "ID");
        }
    }
}
