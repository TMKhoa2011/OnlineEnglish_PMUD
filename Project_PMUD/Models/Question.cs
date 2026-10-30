namespace Project_PMUD.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Questiontype { get; set; } // Trắc nghiệm, Đúng/Sai, Điền khuyết, Tự luận
        public int Difficulty { get; set; } // 1: Dễ, 2: Trung bình, 3: Khó
        public int Topicid { get; set; }
        public DateTime Createdat { get; set; } = DateTime.Now;
    }
}
