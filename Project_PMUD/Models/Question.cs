using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_PMUD.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? Questiontype { get; set; }

        public int? Difficulty { get; set; }

        public int Topicid { get; set; }

        [ForeignKey("Topicid")]
        public virtual Topic? Topic { get; set; }

        public DateTime Createdat { get; set; } = DateTime.Now;

        public virtual ICollection<Option>? Options { get; set; }
    }
}