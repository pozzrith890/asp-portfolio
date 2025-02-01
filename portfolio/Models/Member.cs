using System.ComponentModel.DataAnnotations.Schema;

namespace portfolio.Models
{
    public class Member
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string description { get; set; }
        public string? cv { get; set; }
        [NotMapped]
        public IFormFile? cv_file { get; set; }
        public string? photo { get; set; }
        [NotMapped]
        public IFormFile? photo_path { get; set; }
    }
}
