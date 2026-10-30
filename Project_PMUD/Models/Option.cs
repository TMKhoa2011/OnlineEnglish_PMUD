namespace Project_PMUD.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Optiontext { get; set; }
        public bool Iscorrect { get; set; }
        public int Questionid { get; set; }
    }
}
