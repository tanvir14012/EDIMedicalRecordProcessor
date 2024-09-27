using MedicalRecordProcessor.Models;

namespace MedicalRecordProcessor.Services
{
    public interface IReportDetailsService
    {
        Task<(int, IList<ReportDetails>)> GetReportDetails(int reportId, string searchText, int pageIndex, int pageSize);
        Task AddMedicalReport(IList<ReportDetails> model);
    }
}
