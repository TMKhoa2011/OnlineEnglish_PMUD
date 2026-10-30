using Microsoft.EntityFrameworkCore;
using Project_PMUD.Models;

namespace Project_PMUD.Models.Data
{
    public class Appdbcontext : DbContext
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<StudentResult> StudentResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Khóa chính gồm 2 cột cho bảng trung gian ExamQuestions
            modelBuilder.Entity<ExamQuestion>().HasKey(eq => new { eq.Examid, eq.Questionid });
        }
    }
}