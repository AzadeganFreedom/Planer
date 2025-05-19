using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Planer.Class;

namespace Planer.Models
{
    public class List
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
