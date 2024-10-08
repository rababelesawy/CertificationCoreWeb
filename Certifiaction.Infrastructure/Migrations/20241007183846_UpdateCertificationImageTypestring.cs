using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Certifiaction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCertificationImageTypestring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificationImageFileName",
                table: "Courses",
                newName: "CertificationImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificationImage",
                table: "Courses",
                newName: "CertificationImageFileName");
        }
    }
}
