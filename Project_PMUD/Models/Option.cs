using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_PMUD.Models
{
    public class Option
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nội dung đáp án không được để trống")]
        [Display(Name = "Nội dung đáp án")]
        public string Optiontext { get; set; } = string.Empty;

        [Display(Name = "Đáp án đúng")]
        public bool Iscorrect { get; set; } // true nếu đúng, false nếu sai

        public int Questionid { get; set; }

        [ForeignKey("Questionid")]
        public virtual Question? Question { get; set; }
    }
}