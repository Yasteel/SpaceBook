﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spacebook;

#nullable disable

namespace Spacebook.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Spacebook.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkCommentId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int?>("CommentPost")
                        .HasColumnType("int")
                        .HasColumnName("fkCommentPost");

                    b.Property<int?>("OriginalPost")
                        .HasColumnType("int")
                        .HasColumnName("fkOriginalPost");

                    b.HasKey("CommentId");

                    b.HasIndex("CommentPost");

                    b.HasIndex("OriginalPost");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Spacebook.Models.Conversation", b =>
                {
                    b.Property<int>("ConversationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkConversationId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParticipantOne")
                        .HasColumnType("int")
                        .HasColumnName("fkParticipantOne");

                    b.Property<int?>("ParticipantTwo")
                        .HasColumnType("int")
                        .HasColumnName("fkParticipantTwo");

                    b.HasKey("ConversationId");

                    b.HasIndex("ParticipantOne");

                    b.HasIndex("ParticipantTwo");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("Spacebook.Models.Likes", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkLikeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<int?>("PostId")
                        .HasColumnType("int")
                        .HasColumnName("fkPostId");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int")
                        .HasColumnName("fkProfileId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("LikeId");

                    b.HasIndex("PostId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Spacebook.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkMessageId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ConversationId")
                        .HasColumnType("int")
                        .HasColumnName("fkConversationId");

                    b.Property<string>("MessageType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int")
                        .HasColumnName("fkSenderId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Spacebook.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkPostId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Media")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int")
                        .HasColumnName("fkProfileId");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Spacebook.Models.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Preferences")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int")
                        .HasColumnName("fkProfileId");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("Preference");
                });

            modelBuilder.Entity("Spacebook.Models.Profile", b =>
                {
                    b.Property<int?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("pkUserId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("UserId"));

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Spacebook.Models.Comment", b =>
                {
                    b.HasOne("Spacebook.Models.Post", "CommentEntity")
                        .WithMany()
                        .HasForeignKey("CommentPost");

                    b.HasOne("Spacebook.Models.Post", "OriginalEntity")
                        .WithMany()
                        .HasForeignKey("OriginalPost");

                    b.Navigation("CommentEntity");

                    b.Navigation("OriginalEntity");
                });

            modelBuilder.Entity("Spacebook.Models.Conversation", b =>
                {
                    b.HasOne("Spacebook.Models.Profile", "ParticipantOneEntity")
                        .WithMany()
                        .HasForeignKey("ParticipantOne");

                    b.HasOne("Spacebook.Models.Profile", "ParticipantTwoEntity")
                        .WithMany()
                        .HasForeignKey("ParticipantTwo");

                    b.Navigation("ParticipantOneEntity");

                    b.Navigation("ParticipantTwoEntity");
                });

            modelBuilder.Entity("Spacebook.Models.Likes", b =>
                {
                    b.HasOne("Spacebook.Models.Post", "PostEntity")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("Spacebook.Models.Profile", "ProfileEntity")
                        .WithMany()
                        .HasForeignKey("ProfileId");

                    b.Navigation("PostEntity");

                    b.Navigation("ProfileEntity");
                });

            modelBuilder.Entity("Spacebook.Models.Message", b =>
                {
                    b.HasOne("Spacebook.Models.Conversation", "ConversationEntity")
                        .WithMany()
                        .HasForeignKey("ConversationId");

                    b.HasOne("Spacebook.Models.Profile", "ProfileEntity")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("ConversationEntity");

                    b.Navigation("ProfileEntity");
                });

            modelBuilder.Entity("Spacebook.Models.Post", b =>
                {
                    b.HasOne("Spacebook.Models.Profile", "ProfileEntity")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfileEntity");
                });

            modelBuilder.Entity("Spacebook.Models.Preference", b =>
                {
                    b.HasOne("Spacebook.Models.Profile", "ProfileEntity")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfileEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
