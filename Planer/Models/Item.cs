using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Planer.Models;

namespace Planer.Class
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [ForeignKey("ListId")]
        public int ListId { get; set; }
        [JsonIgnore]
        public List? List { get; set; }
    }
}
