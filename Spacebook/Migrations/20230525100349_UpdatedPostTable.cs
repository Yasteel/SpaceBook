using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spacebook.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    pkUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.pkUserId);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    pkConversationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fkParticipantOne = table.Column<int>(type: "int", nullable: true),
                    fkParticipantTwo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.pkConversationId);
                    table.ForeignKey(
                        name: "FK_Conversation_Profile_fkParticipantOne",
                        column: x => x.fkParticipantOne,
                        principalTable: "Profile",
                        principalColumn: "pkUserId");
                    table.ForeignKey(
                        name: "FK_Conversation_Profile_fkParticipantTwo",
                        column: x => x.fkParticipantTwo,
                        principalTable: "Profile",
                        principalColumn: "pkUserId");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    pkPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkProfileId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.pkPostId);
                    table.ForeignKey(
                        name: "FK_Post_Profile_fkProfileId",
                        column: x => x.fkProfileId,
                        principalTable: "Profile",
                        principalColumn: "pkUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkProfileId = table.Column<int>(type: "int", nullable: false),
                    Preferences = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preference_Profile_fkProfileId",
                        column: x => x.fkProfileId,
                        principalTable: "Profile",
                        principalColumn: "pkUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    pkMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkConversationId = table.Column<int>(type: "int", nullable: true),
                    fkSenderId = table.Column<int>(type: "int", nullable: true),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.pkMessageId);
                    table.ForeignKey(
                        name: "FK_Message_Conversation_fkConversationId",
                        column: x => x.fkConversationId,
                        principalTable: "Conversation",
                        principalColumn: "pkConversationId");
                    table.ForeignKey(
                        name: "FK_Message_Profile_fkSenderId",
                        column: x => x.fkSenderId,
                        principalTable: "Profile",
                        principalColumn: "pkUserId");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    pkCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkOriginalPost = table.Column<int>(type: "int", nullable: true),
                    fkCommentPost = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.pkCommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Post_fkCommentPost",
                        column: x => x.fkCommentPost,
                        principalTable: "Post",
                        principalColumn: "pkPostId");
                    table.ForeignKey(
                        name: "FK_Comment_Post_fkOriginalPost",
                        column: x => x.fkOriginalPost,
                        principalTable: "Post",
                        principalColumn: "pkPostId");
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    pkLikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkProfileId = table.Column<int>(type: "int", nullable: true),
                    fkPostId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.pkLikeId);
                    table.ForeignKey(
                        name: "FK_Likes_Post_fkPostId",
                        column: x => x.fkPostId,
                        principalTable: "Post",
                        principalColumn: "pkPostId");
                    table.ForeignKey(
                        name: "FK_Likes_Profile_fkProfileId",
                        column: x => x.fkProfileId,
                        principalTable: "Profile",
                        principalColumn: "pkUserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_fkCommentPost",
                table: "Comment",
                column: "fkCommentPost");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_fkOriginalPost",
                table: "Comment",
                column: "fkOriginalPost");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_fkParticipantOne",
                table: "Conversation",
                column: "fkParticipantOne");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_fkParticipantTwo",
                table: "Conversation",
                column: "fkParticipantTwo");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_fkPostId",
                table: "Likes",
                column: "fkPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_fkProfileId",
                table: "Likes",
                column: "fkProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_fkConversationId",
                table: "Message",
                column: "fkConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_fkSenderId",
                table: "Message",
                column: "fkSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_fkProfileId",
                table: "Post",
                column: "fkProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Preference_fkProfileId",
                table: "Preference",
                column: "fkProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
