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
            entity.ToTable("send_emails");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AttachFile)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("attach_file");
            entity.Property(e => e.BccEmails).HasColumnName("bcc_emails");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.CcEmails).HasColumnName("cc_emails");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.EmailSenderId).HasColumnName("email_sender_id");
            entity.Property(e => e.NumberOfResend)
                .HasDefaultValueSql("((0))")
                .HasColumnName("number_of_resend");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.Sent)
                .HasDefaultValueSql("((0))")
                .HasColumnName("sent");
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .HasColumnName("subject");
            entity.Property(e => e.TimeSent)
                .HasColumnType("datetime")
                .HasColumnName("time_sent");
            entity.Property(e => e.ToEmails).HasColumnName("to_emails");
        });

        modelBuilder.Entity<SendEmailLog>(entity =>
        {
            entity.ToTable("send_email_logs");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.SendEmailId).HasColumnName("send_email_id");
            entity.Property(e => e.TimeSent)
                .HasColumnType("datetime")
                .HasColumnName("time_sent");
            entity.Property(e => e.MessageLog).HasColumnName("message_log");
            entity.Property(e => e.Sent)
                .HasDefaultValueSql("((0))")
                .HasColumnName("sent");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

