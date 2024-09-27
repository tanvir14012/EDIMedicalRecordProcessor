using MedicalRecordProcessor.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordProcessor.Models.Dto
{
    public partial record FileUpload
    {
        [Required]
        [AllowedExtensions(new string[] { "txt", "edi", "x12", "asc", "edifact"})]
        public IFormFile File { get; set; }
    }
}
