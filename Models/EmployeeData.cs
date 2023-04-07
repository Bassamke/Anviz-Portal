using System.ComponentModel.DataAnnotations;

namespace AnvizWeb.Models
{
    public class EmployeeData
    {
        [Key]
        public int Id { get; set; }
        public string? XRefCode { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? DisplayName { get; set; }
        public string? PrimaryLocation { get; set; }
        public string? Location { get; set; }
        public bool Uploaded { get; set; }
        public string? FaceTemplate { get; set; }
        public DateTime? LastModified { get; set; }

        public bool Active { get; set; }
        public string? OldLocation { get; set; }

        public bool? DeletedFromOldLocation { get; set; }

    }
}
