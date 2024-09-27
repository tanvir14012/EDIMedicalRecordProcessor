using MedicalRecordProcessor.DataAccess;
using MedicalRecordProcessor.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecordProcessor.Services
{
    public class MedicalReportService : IMedicalReportService
    {
        private readonly AppDbContext appDbContext;

        public MedicalReportService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<(int, IList<MedicalReport>)> GetMedicalReports(int pageIndex, int pageSize)
        {
            var reports = appDbContext.MedicalReports.AsNoTracking()
                .OrderByDescending(r => r.Id).AsQueryable();

            var total = await reports.CountAsync();

            reports = reports.Skip(pageIndex * pageSize)
                .Take(pageSize);

            var data = await reports.ToListAsync();
            foreach(var report in data)
            {
                report.Total = await appDbContext.ReportDetails.CountAsync(rd => rd.MedicalReportId == report.Id);
            }
            return (total, data);
        }

        public async Task<MedicalReport> AddMedicalReport(MedicalReport model)
        {
            await appDbContext.MedicalReports.AddAsync(model);
            await appDbContext.SaveChangesAsync();
            return model;
        }
    }
}
