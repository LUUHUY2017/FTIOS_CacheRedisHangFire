using AMMS.Notification.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.Notification.Datas;

public partial class NotificationDbContext : DbContext
{
    // This constructor is used of runit testing
    public NotificationDbContext()
    {
    }
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Notification> Notification { get; set; }
    public virtual DbSet<SendEmail> SendEmails { get; set; }
    public virtual DbSet<SendEmailLog> SendEmailLogs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SendEmail>(entity =>
        {
            entity.ToTable("SendEmail");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.EmailSenderId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.ToEmails)
                .IsRequired()
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.CcEmails)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.BccEmails)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Subject)
                .IsRequired()
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Body)
                .IsRequired()
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Sent)
                .HasColumnType("tinyint(1)");

            entity.Property(e => e.TimeSent)
                .HasColumnType("datetime(6)");

            entity.Property(e => e.NumberOfResend)
                .HasColumnType("int");

            entity.Property(e => e.AttachFile)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.LastModifiedDate)
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.Property(e => e.Actived)
                .HasColumnType("tinyint(1)");

            entity.Property(e => e.Reason)
                .HasMaxLength(1000)
                .HasColumnType("varchar(1000)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Logs)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.OrganizationId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.ReferenceId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");
        });


        modelBuilder.Entity<SendEmailLog>(entity =>
        {
            entity.ToTable("SendEmailLogs");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.SendEmailId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.TimeSent)
                .HasColumnType("datetime(6)");

            entity.Property(e => e.MessageLog)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Sent)
                .HasColumnType("tinyint(1)");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.LastModifiedDate)
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.Property(e => e.Actived)
                .HasColumnType("tinyint(1)");

            entity.Property(e => e.Reason)
                .HasMaxLength(1000)
                .HasColumnType("varchar(1000)")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.Logs)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.OrganizationId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");

            entity.Property(e => e.ReferenceId)
                .HasColumnType("longtext")
                .HasCharSet("utf8mb4");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

