using System.ComponentModel.DataAnnotations.Schema;

namespace portfolio.Models
{
    public class Project
    {
        public int id {  get; set; }
        public string title { get; set; }
        public string? url { get; set; }
        public string? photo { get; set; }
        [NotMapped]
        public IFormFile? imgs { get; set; }
    }
}
