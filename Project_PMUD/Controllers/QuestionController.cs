using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PMUD.Models;
using Project_PMUD.Models.Data;

namespace Project_PMUD.Controllers
{
    public class QuestionController : Controller
    {
        private readonly Appdbcontext _context;

        public QuestionController(Appdbcontext context)
        {
            _context = context;
        }

        // GET: Question (Hiển thị danh sách, load kèm tên Chủ đề & Tìm kiếm/Lọc)
        public async Task<IActionResult> Index(string searchString, int? topicId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", topicId);

            var questions = _context.Questions.Include(q => q.Topic).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(q => q.Content.Contains(searchString));
            }

            if (topicId.HasValue && topicId.Value > 0)
            {
                questions = questions.Where(q => q.Topicid == topicId.Value);
            }

            return View(await questions.OrderByDescending(q => q.Createdat).ToListAsync());
        }

        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Topic)
                .Include(q => q.Options)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            ViewData["Topicid"] = new SelectList(_context.Topics, "Id", "Name");
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Questiontype,Difficulty,Topicid")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.Createdat = DateTime.Now;
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Topicid"] = new SelectList(_context.Topics, "Id", "Name", question.Topicid);
            return View(question);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["Topicid"] = new SelectList(_context.Topics, "Id", "Name", question.Topicid);
            return View(question);
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Content,Questiontype,Difficulty,Topicid,Createdat")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Topicid"] = new SelectList(_context.Topics, "Id", "Name", question.Topicid);
            return View(question);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int? id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}