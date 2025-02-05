using System.ComponentModel.DataAnnotations.Schema;

namespace portfolio.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string message { get; set; }
        public string? photo { get; set; }
        [NotMapped]
        public IFormFile? img { get; set; }
    }
}
