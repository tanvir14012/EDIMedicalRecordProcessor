using System.ComponentModel.DataAnnotations;

namespace MedicalRecordProcessor.Models.Utility
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] extensions;

        public AllowedExtensions(string[] extensions)
        {
            this.extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (value == null)
                {
                    return ValidationResult.Success;
                }
                var picture = value as IFormFile;
                var extension = Path.GetExtension(picture.FileName).Replace(".", "").ToLower();
                if (picture.FileName.Length > 800)
                {
                    return new ValidationResult("The file name is too big");
                }

                if (this.extensions.Contains(extension))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("The file extension is not allowed");
            }
            catch (Exception ex)
            {
                return new ValidationResult("Something is wrong with the file");
            }

        }
    }
}
