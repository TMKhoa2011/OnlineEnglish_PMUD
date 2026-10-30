namespace Project_PMUD.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Passwordhash { get; set; }
        public string Role { get; set; } 
        public string Phone { get; set; }
        public DateTime Createdat { get; set; } = DateTime.Now;
    }
}
