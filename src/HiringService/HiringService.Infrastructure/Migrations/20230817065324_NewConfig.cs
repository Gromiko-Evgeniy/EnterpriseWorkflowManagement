using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiringService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_Candidates_CandidateId",
                table: "HiringStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_HiringStageNames_HiringStageNameId",
                table: "HiringStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_Workers_IntervierId",
                table: "HiringStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HiringStageNames",
                table: "HiringStageNames");

            migrationBuilder.RenameTable(
                name: "HiringStageNames",
                newName: "StageNames");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workers",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Workers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "HiringStages",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Candidates",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Candidates",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CV",
                table: "Candidates",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StageNames",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageNames",
                table: "StageNames",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_Candidates_CandidateId",
                table: "HiringStages",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_StageNames_HiringStageNameId",
                table: "HiringStages",
                column: "HiringStageNameId",
                principalTable: "StageNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_Workers_IntervierId",
                table: "HiringStages",
                column: "IntervierId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_Candidates_CandidateId",
                table: "HiringStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_StageNames_HiringStageNameId",
                table: "HiringStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringStages_Workers_IntervierId",
                table: "HiringStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StageNames",
                table: "StageNames");

            migrationBuilder.RenameTable(
                name: "StageNames",
                newName: "HiringStageNames");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Workers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "HiringStages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Candidates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Candidates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "CV",
                table: "Candidates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HiringStageNames",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HiringStageNames",
                table: "HiringStageNames",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_Candidates_CandidateId",
                table: "HiringStages",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_HiringStageNames_HiringStageNameId",
                table: "HiringStages",
                column: "HiringStageNameId",
                principalTable: "HiringStageNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringStages_Workers_IntervierId",
                table: "HiringStages",
                column: "IntervierId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
