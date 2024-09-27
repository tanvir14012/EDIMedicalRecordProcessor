namespace MedicalRecordProcessor.Models
{
    /// <summary>
    /// Represents a record/row in a report file/document
    /// </summary>
    public partial record ReportDetails
    {
        public ReportDetails()
        {
            ReportDetailsErrors = new HashSet<ReportDetailsError>();
        }
        public long Id { get; set; }
        public string ContractId { get; set; }
        public string RecordId { get; set; }
        public string Status { get; set; }
        public int MedicalReportId { get; set; }
        public virtual MedicalReport MedicalReport { get; set; }
        public virtual ICollection<ReportDetailsError> ReportDetailsErrors { get; set; }
    }
}
