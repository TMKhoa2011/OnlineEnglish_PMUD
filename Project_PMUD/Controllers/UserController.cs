using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_PMUD.Models;
using Project_PMUD.Models.Data;
using BCrypt.Net;

namespace Project_PMUD.Controllers
{
    public class UserController : Controller
    {
        private readonly Appdbcontext _context;

        public UserController(Appdbcontext context)
        {
            _context = context;
        }

        // GET: USERS (Thêm chức năng Tìm kiếm & Phân trang)
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            ViewData["CurrentFilter"] = searchString;

            var users = from u in _context.Users
                        select u;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Fullname.Contains(searchString) || u.Email.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var paginatedUsers = await users
                .OrderByDescending(u => u.Createdat)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(await users.CountAsync() / (double)pageSize);

            return View(paginatedUsers);
        }

        // GET: USERS/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: USERS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: USERS/Create (Tự động Mã hóa Mật khẩu & Gán ngày tạo)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Email,Passwordhash,Role,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                // 1. Mã hóa mật khẩu trước khi lưu vào DB
                if (!string.IsNullOrEmpty(user.Passwordhash))
                {
                    user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);
                }

                // 2. Gán ngày tạo mặc định
                user.Createdat = DateTime.Now;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: USERS/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: USERS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Fullname,Email,Passwordhash,Role,Phone,Createdat")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser != null)
                    {
                        // Kiểm tra nếu người dùng đổi mật khẩu mới thì mã hóa, nếu không thì giữ nguyên mật khẩu cũ
                        if (!string.IsNullOrEmpty(user.Passwordhash) && user.Passwordhash != existingUser.Passwordhash)
                        {
                            user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);
                        }
                        else
                        {
                            user.Passwordhash = existingUser.Passwordhash;
                        }
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: USERS/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: USERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string? id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}