using MedicalRecordProcessor.Models;

namespace MedicalRecordProcessor.Services
{
    public interface IMedicalReportService
    {
        Task<(int, IList<MedicalReport>)> GetMedicalReports(int pageIndex, int pageSize);
        Task<MedicalReport> AddMedicalReport(MedicalReport model);
    }
}
