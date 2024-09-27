using MedicalRecordProcessor.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecordProcessor.DataAccess
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> ops): base(ops)
        {

        }

        public virtual DbSet<MedicalReport> MedicalReports { get; set; }
        public virtual DbSet<ReportDetails> ReportDetails { get; set; }
        public virtual DbSet<ReportDetailsError> ReportDetailsErrors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicalReport>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.UploadStatus).HasMaxLength(50);
                entity.Property(e => e.FilePath).HasMaxLength(800);
                entity.Property(e => e.UploadStatus).HasMaxLength(50);

                entity.HasMany(e => e.ReportDetails)
                      .WithOne(e => e.MedicalReport)
                      .HasForeignKey(e => e.MedicalReportId)
                      .HasConstraintName("FK_ReportDetails_MedicalReportId");
                entity.Ignore(e => e.Total);
            });

            modelBuilder.Entity<ReportDetails>(entity =>
            {
                entity.Property(e => e.ContractId).HasMaxLength(5);
                entity.Property(e => e.RecordId).HasMaxLength(15);
                entity.Property(e => e.Status).HasMaxLength(8);

                entity.HasMany(e => e.ReportDetailsErrors)
                     .WithOne(e => e.ReportDetails)
                     .HasForeignKey(e => e.ReportDetailsId)
                     .HasConstraintName("FK_ReportDetailsError_ReportDetailsId");

                entity.HasIndex(e => new { e.ContractId, e.RecordId });
            });

            modelBuilder.Entity<ReportDetailsError>(entity =>
            {
                entity.Property(e => e.ErrorCode).HasMaxLength(5);
            });
        }
    }
}
