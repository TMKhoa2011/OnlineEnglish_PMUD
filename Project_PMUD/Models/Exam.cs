namespace Project_PMUD.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Durationminutes { get; set; } = 60; // Giới hạn 60 phút
        public int Totalquestions { get; set; }
        public DateTime Createdat { get; set; } = DateTime.Now;
        public string Createdbyuserid { get; set; }
    }
}
