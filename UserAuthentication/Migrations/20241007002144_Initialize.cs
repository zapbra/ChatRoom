using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_validation_status",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_description = table.Column<string>(type: "text", nullable: true),
                    isValidated = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_validation_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "external_providers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    provider_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ws_endpoint = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_external_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hashing_algorithms",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    algorithm_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hashing_algorithms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permissions_description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_user_user_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "granted_permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    permission_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_granted_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_granted_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_granted_permissions_user_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    confirmation_token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    token_generation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hash_algorithm_id = table.Column<long>(type: "bigint", nullable: true),
                    email_validation_status_id = table.Column<long>(type: "bigint", nullable: false),
                    password_recovery_token = table.Column<string>(type: "text", nullable: true),
                    recovery_token_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_login_chat_user_user_id",
                        column: x => x.user_id,
                        principalTable: "chat_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_login_email_validation_status_email_validation_status_~",
                        column: x => x.email_validation_status_id,
                        principalTable: "email_validation_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_login_hashing_algorithms_hash_algorithm_id",
                        column: x => x.hash_algorithm_id,
                        principalTable: "hashing_algorithms",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_login_data_external",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    external_provider_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login_data_external", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_login_data_external_chat_user_user_id",
                        column: x => x.user_id,
                        principalTable: "chat_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_login_data_external_external_providers_external_provid~",
                        column: x => x.external_provider_id,
                        principalTable: "external_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_state",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state_uuid = table.Column<string>(type: "text", nullable: false),
                    expiry_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Authenticated = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_state", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_state_chat_user_user_id",
                        column: x => x.user_id,
                        principalTable: "chat_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "hashing_algorithms",
                columns: new[] { "id", "algorithm_name" },
                values: new object[] { 1L, "SHA256" });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_name" },
                values: new object[,]
                {
                    { 1L, "admin" },
                    { 2L, "user" },
                    { 3L, "guest" },
                    { 4L, "viewer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_user_role_id",
                table: "chat_user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_granted_permissions_permission_id",
                table: "granted_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_granted_permissions_role_id",
                table: "granted_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_email_validation_status_id",
                table: "user_login",
                column: "email_validation_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_hash_algorithm_id",
                table: "user_login",
                column: "hash_algorithm_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_user_id",
                table: "user_login",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_login_data_external_external_provider_id",
                table: "user_login_data_external",
                column: "external_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_data_external_user_id",
                table: "user_login_data_external",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_name",
                table: "user_roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_state_user_id",
                table: "user_state",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "granted_permissions");

            migrationBuilder.DropTable(
                name: "user_login");

            migrationBuilder.DropTable(
                name: "user_login_data_external");

            migrationBuilder.DropTable(
                name: "user_state");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "email_validation_status");

            migrationBuilder.DropTable(
                name: "hashing_algorithms");

            migrationBuilder.DropTable(
                name: "external_providers");

            migrationBuilder.DropTable(
                name: "chat_user");

            migrationBuilder.DropTable(
                name: "user_roles");
        }
    }
}
