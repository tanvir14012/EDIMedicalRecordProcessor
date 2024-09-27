namespace MedicalRecordProcessor.Models
{
    /// <summary>
    /// Represents an error in one record of a report document 
    /// </summary>
    public partial record ReportDetailsError
    {
        public long Id { get; set; }
        public string ErrorCode { get; set; }
        public long ReportDetailsId { get; set; }
        public virtual ReportDetails ReportDetails { get; set; }

    }
}
