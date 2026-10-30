namespace Project_PMUD.Models
{
    public class StudentResult
    {
        public int Id { get; set; }
        public string Studentid { get; set; }
        public int Examid { get; set; }
        public double Score { get; set; }
        public int Totalcorrect { get; set; }
        public int Timespentseconds { get; set; }
        public DateTime Submittedat { get; set; } = DateTime.Now;
    }
}
