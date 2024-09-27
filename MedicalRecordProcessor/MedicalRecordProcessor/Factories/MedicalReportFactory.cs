using MedicalRecordProcessor.Models;
using MedicalRecordProcessor.Models.Dto;
using MedicalRecordProcessor.Services;

namespace MedicalRecordProcessor.Factories
{
    public class MedicalReportFactory : IMedicalReportFactory
    {
        private readonly IMedicalReportService medicalReportService;
        private readonly IReportDetailsService reportDetailsService;
        private readonly IWebHostEnvironment webHost;
        private const int CHUNK_SIZE = 50;

        public MedicalReportFactory(IMedicalReportService medicalReportService,
            IReportDetailsService reportDetailsService,
            IWebHostEnvironment webHost)
        {
            this.medicalReportService = medicalReportService;
            this.reportDetailsService = reportDetailsService;
            this.webHost = webHost;
        }
        public async Task<FileParseResult> PrepareMedicalReport(FileUpload model)
        {
            var result = new FileParseResult();
            var medicalRecord = new MedicalReport
            {
                FileSize = (int)model.File.Length,
                Name = model.File.FileName,
                UploadedAt = DateTime.UtcNow
            };

            try
            {
                string dir = Path.Join(webHost.WebRootPath, "MedicalReports");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var guid = Guid.NewGuid().ToString();
                var path = Path.Join(dir, $"{guid}_{model.File.FileName}");

                using Stream fileStream = new FileStream(path, FileMode.Create);
                await model.File.CopyToAsync(fileStream);

                medicalRecord.FilePath = path;
                medicalRecord.UploadStatus = "Succeeded";
                await medicalReportService.AddMedicalReport(medicalRecord);
                result.Success = true;

            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                medicalRecord.UploadStatus = "Failed";
                await medicalReportService.AddMedicalReport(medicalRecord);
            }

            if(result.Success)
            {
                try
                {
                    using (FileStream fs = File.Open(medicalRecord.FilePath, FileMode.Open, FileAccess.Read))
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        var reportRecords = new List<ReportDetails>();
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                var fields = line.Split('*');
                                if (fields.Length > 0 && fields[0] == "1")
                                {
                                    var record = new ReportDetails
                                    {
                                        MedicalReportId = medicalRecord.Id,
                                        ContractId = fields.Length > 1 ? fields[1] : null,
                                        RecordId = fields.Length > 2 ? fields[2] : null,
                                        Status = fields.Length > 3 ? fields[3] : null
                                    };
                                    if (fields.Length > 4 && !string.IsNullOrWhiteSpace(fields[4]))
                                    {
                                        record.ReportDetailsErrors.Add(new ReportDetailsError
                                        {
                                            ErrorCode = fields[4]
                                        });
                                    }
                                    reportRecords.Add(record);
                                }
                            }

                            if (reportRecords.Count >= CHUNK_SIZE)
                            {
                                await reportDetailsService.AddMedicalReport(reportRecords);
                                reportRecords.Clear();
                            }
                        }

                        if (reportRecords.Count > 0)
                        {
                            await reportDetailsService.AddMedicalReport(reportRecords);
                            reportRecords.Clear();
                        }
                    }

                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }

            result.ParseSuccess = true;
            return result;
        }
    }
}
