using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CampOrno.Data.COMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CO");

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Compounds",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compounds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Counselors",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Nickname = table.Column<string>(maxLength: 50, nullable: true),
                    SIN = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counselors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DietaryRestrictions",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryRestrictions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Campers",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(maxLength: 1, nullable: false),
                    eMail = table.Column<string>(maxLength: 255, nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    imageContent = table.Column<byte[]>(nullable: true),
                    imageMimeType = table.Column<string>(maxLength: 256, nullable: true),
                    imageFileName = table.Column<string>(maxLength: 100, nullable: true),
                    CounselorID = table.Column<int>(nullable: true),
                    CompoundID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Campers_Compounds_CompoundID",
                        column: x => x.CompoundID,
                        principalSchema: "CO",
                        principalTable: "Compounds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Campers_Counselors_CounselorID",
                        column: x => x.CounselorID,
                        principalSchema: "CO",
                        principalTable: "Counselors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CounselorCompounds",
                schema: "CO",
                columns: table => new
                {
                    CounselorID = table.Column<int>(nullable: false),
                    CompoundID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounselorCompounds", x => new { x.CompoundID, x.CounselorID });
                    table.ForeignKey(
                        name: "FK_CounselorCompounds_Compounds_CompoundID",
                        column: x => x.CompoundID,
                        principalSchema: "CO",
                        principalTable: "Compounds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CounselorCompounds_Counselors_CounselorID",
                        column: x => x.CounselorID,
                        principalSchema: "CO",
                        principalTable: "Counselors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CamperActivities",
                schema: "CO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fee = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    NumberOfSessions = table.Column<int>(nullable: false),
                    CamperID = table.Column<int>(nullable: false),
                    ActivityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamperActivities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CamperActivities_Activities_ActivityID",
                        column: x => x.ActivityID,
                        principalSchema: "CO",
                        principalTable: "Activities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CamperActivities_Campers_CamperID",
                        column: x => x.CamperID,
                        principalSchema: "CO",
                        principalTable: "Campers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CamperDiets",
                schema: "CO",
                columns: table => new
                {
                    CamperID = table.Column<int>(nullable: false),
                    DietaryRestrictionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamperDiets", x => new { x.CamperID, x.DietaryRestrictionID });
                    table.ForeignKey(
                        name: "FK_CamperDiets_Campers_CamperID",
                        column: x => x.CamperID,
                        principalSchema: "CO",
                        principalTable: "Campers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CamperDiets_DietaryRestrictions_DietaryRestrictionID",
                        column: x => x.DietaryRestrictionID,
                        principalSchema: "CO",
                        principalTable: "DietaryRestrictions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CamperActivities_ActivityID",
                schema: "CO",
                table: "CamperActivities",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_CamperActivities_CamperID_ActivityID",
                schema: "CO",
                table: "CamperActivities",
                columns: new[] { "CamperID", "ActivityID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamperDiets_DietaryRestrictionID",
                schema: "CO",
                table: "CamperDiets",
                column: "DietaryRestrictionID");

            migrationBuilder.CreateIndex(
                name: "IX_Campers_CompoundID",
                schema: "CO",
                table: "Campers",
                column: "CompoundID");

            migrationBuilder.CreateIndex(
                name: "IX_Campers_CounselorID",
                schema: "CO",
                table: "Campers",
                column: "CounselorID");

            migrationBuilder.CreateIndex(
                name: "IX_Campers_eMail",
                schema: "CO",
                table: "Campers",
                column: "eMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CounselorCompounds_CounselorID",
                schema: "CO",
                table: "CounselorCompounds",
                column: "CounselorID");

            migrationBuilder.CreateIndex(
                name: "IX_Counselors_SIN",
                schema: "CO",
                table: "Counselors",
                column: "SIN",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CamperActivities",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "CamperDiets",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "CounselorCompounds",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "Campers",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "DietaryRestrictions",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "Compounds",
                schema: "CO");

            migrationBuilder.DropTable(
                name: "Counselors",
                schema: "CO");
        }
    }
}
