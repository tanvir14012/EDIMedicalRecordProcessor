namespace MedicalRecordProcessor.Models.Dto
{
    public partial record FileParseResult
    {
        /// <summary>
        /// Upload success status
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Parse success status
        /// </summary>
        public bool ParseSuccess { get; set; }
    }
}
