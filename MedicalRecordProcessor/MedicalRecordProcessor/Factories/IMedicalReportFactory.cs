using MedicalRecordProcessor.Models.Dto;

namespace MedicalRecordProcessor.Factories
{
    public interface IMedicalReportFactory
    {
        Task<FileParseResult> PrepareMedicalReport(FileUpload file);
    }
}
