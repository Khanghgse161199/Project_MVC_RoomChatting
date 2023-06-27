using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Entities;

public partial class RoomChattingContext : DbContext
{
    public RoomChattingContext()
    {
    }

    public RoomChattingContext(DbContextOptions<RoomChattingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImgSet> ImgSets { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<RoomUserMapping> RoomUserMappings { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=RoomChatting;User Id=test;Password=Test;TrustServerCertificate=True;Trusted_Connection=true;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Email).HasMaxLength(400);
            entity.Property(e => e.Password).HasMaxLength(400);
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.ToTable("ChatRoom");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Admin).HasMaxLength(400);
            entity.Property(e => e.Creator).HasMaxLength(400);
            entity.Property(e => e.LastMessage).HasMaxLength(700);
            entity.Property(e => e.LastSenderMessage).HasMaxLength(200);
            entity.Property(e => e.LastedUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(400);
            entity.Property(e => e.Serect).HasMaxLength(100);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("image");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.ImgSetId).HasMaxLength(400);
            entity.Property(e => e.ImgUrl).HasMaxLength(800);

            entity.HasOne(d => d.ImgSet).WithMany(p => p.Images)
                .HasForeignKey(d => d.ImgSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_image_ImgSet");
        });

        modelBuilder.Entity<ImgSet>(entity =>
        {
            entity.ToTable("ImgSet");

            entity.Property(e => e.Id).HasMaxLength(400);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.ChatRoomId).HasMaxLength(400);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImgSetId).HasMaxLength(400);
            entity.Property(e => e.Message1)
                .HasMaxLength(700)
                .HasColumnName("Message");
            entity.Property(e => e.Sender).HasMaxLength(400);

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_ChatRoom");

            entity.HasOne(d => d.ImgSet).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ImgSetId)
                .HasConstraintName("FK_Messages_ImgSet");
        });

        modelBuilder.Entity<RoomUserMapping>(entity =>
        {
            entity.ToTable("RoomUserMapping");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.ChatRoomId).HasMaxLength(400);

            entity.HasOne(d => d.Acc).WithMany(p => p.RoomUserMappings)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomUserMapping_Account");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.RoomUserMappings)
                .HasForeignKey(d => d.ChatRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomUserMapping_ChatRoom");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.ToTable("Token");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.AccessToken).HasMaxLength(400);
            entity.Property(e => e.DateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Acc).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Account");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.Fullname).HasMaxLength(400);
            entity.Property(e => e.ImgSetId).HasMaxLength(400);

            entity.HasOne(d => d.Acc).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Account1");

            entity.HasOne(d => d.ImgSet).WithMany(p => p.Users)
                .HasForeignKey(d => d.ImgSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_ImgSet");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
