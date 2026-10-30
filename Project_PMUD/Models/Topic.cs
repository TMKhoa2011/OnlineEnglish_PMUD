namespace Project_PMUD.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Createdat { get; set; } = DateTime.Now;
    }
}
