using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PMUD.Models;
using Project_PMUD.Models.Data;

namespace Project_PMUD.Controllers
{
    public class OptionController : Controller
    {
        private readonly Appdbcontext _context;

        public OptionController(Appdbcontext context)
        {
            _context = context;
        }

        // GET: Option
        public async Task<IActionResult> Index(string searchString, int? questionId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["Questionid"] = new SelectList(_context.Questions, "Id", "Content", questionId);

            var options = _context.Options.Include(o => o.Question).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                options = options.Where(o => o.Optiontext.Contains(searchString));
            }

            if (questionId.HasValue && questionId.Value > 0)
            {
                options = options.Where(o => o.Questionid == questionId.Value);
            }

            return View(await options.ToListAsync());
        }

        // GET: Option/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var option = await _context.Options
                .Include(o => o.Question)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (option == null) return NotFound();

            return View(option);
        }

        // GET: Option/Create
        public IActionResult Create()
        {
            ViewData["Questionid"] = new SelectList(_context.Questions, "Id", "Content");
            return View();
        }

        // POST: Option/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Optiontext,Iscorrect,Questionid")] Option option)
        {
            if (ModelState.IsValid)
            {
                _context.Add(option);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Questionid"] = new SelectList(_context.Questions, "Id", "Content", option.Questionid);
            return View(option);
        }

        // GET: Option/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var option = await _context.Options.FindAsync(id);
            if (option == null) return NotFound();

            ViewData["Questionid"] = new SelectList(_context.Questions, "Id", "Content", option.Questionid);
            return View(option);
        }

        // POST: Option/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Optiontext,Iscorrect,Questionid")] Option option)
        {
            if (id != option.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(option);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OptionExists(option.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Questionid"] = new SelectList(_context.Questions, "Id", "Content", option.Questionid);
            return View(option);
        }

        // GET: Option/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var option = await _context.Options
                .Include(o => o.Question)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (option == null) return NotFound();

            return View(option);
        }

        // POST: Option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var option = await _context.Options.FindAsync(id);
            if (option != null)
            {
                _context.Options.Remove(option);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OptionExists(int? id)
        {
            return _context.Options.Any(e => e.Id == id);
        }
    }
}