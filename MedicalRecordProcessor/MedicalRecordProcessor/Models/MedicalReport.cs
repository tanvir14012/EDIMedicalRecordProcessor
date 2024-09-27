namespace MedicalRecordProcessor.Models
{
    /// <summary>
    /// Represents a report file/document
    /// </summary>
    public partial record MedicalReport
    {
        public MedicalReport()
        {
            ReportDetails = new HashSet<ReportDetails>();   
        }
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int FileSize { get; set; }
        public string UploadStatus { get; set; }
        public DateTime UploadedAt { get; set; }
        public virtual ICollection<ReportDetails> ReportDetails { get; set; }

        //Ignored
        public int Total { get; set; }
    }
}
