using MedicalRecordProcessor.DataAccess;
using MedicalRecordProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace MedicalRecordProcessor.Services
{
    public class ReportDetailsService : IReportDetailsService
    {
        private readonly AppDbContext appDbContext;

        public ReportDetailsService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task AddMedicalReport(IList<ReportDetails> model)
        {
            var needUpdate = await appDbContext.ReportDetails.Where(rd => model.Select(m => m.RecordId).Contains(rd.RecordId) &&
                model.Select(m => m.ContractId).Contains(rd.ContractId)).ToListAsync();

            if (needUpdate.Count > 0)
            {
                foreach(var record in needUpdate)
                {
                    var duplicates = model.Where(r => r.ContractId == record.ContractId && r.RecordId == record.RecordId).ToList();
                    foreach(var duplicate in duplicates)
                    {
                        if (duplicate.ReportDetailsErrors.Any())
                        {
                            foreach (var err in duplicate.ReportDetailsErrors)
                            {
                                if (!record.ReportDetailsErrors.Any(e => e.ErrorCode == err.ErrorCode))
                                {
                                    err.ReportDetailsId = record.Id;
                                    record.ReportDetailsErrors.Add(err);
                                }
                            }

                        }

                        model.Remove(duplicate);
                    }
                }
            }
            await appDbContext.ReportDetails.AddRangeAsync(model);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<(int, IList<ReportDetails>)> GetReportDetails(int reportId, string searchText, int pageIndex, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = HttpUtility.UrlDecode(searchText);
            }
            var reportDetails = appDbContext.ReportDetails.AsNoTracking().Where(r => r.MedicalReportId == reportId);
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                reportDetails = reportDetails.Where(rd => rd.ContractId.ToLower().Contains(searchText) ||
                    rd.RecordId.ToLower().Contains(searchText)).AsQueryable();
            }
            var total = await reportDetails.CountAsync();
            reportDetails = reportDetails.Skip(pageIndex * pageSize).Take(pageSize);
            reportDetails = reportDetails.Include(rd => rd.ReportDetailsErrors);
            return (total, await reportDetails.ToListAsync());
        }
    }
}
