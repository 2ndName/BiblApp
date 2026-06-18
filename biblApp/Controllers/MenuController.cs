using Microsoft.AspNetCore.Mvc;
using biblApp.Data;
using Microsoft.EntityFrameworkCore;

namespace biblApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly LibraryContext _context;

        public MenuController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public async Task<IActionResult> ViewBooks(int authorId, string sortOrder)
        {
            ViewData["TitleSort"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PagesSort"] = sortOrder == "Pages" ? "pages_desc" : "Pages";
            ViewData["AuthorSort"] = sortOrder == "Author" ? "author_desc" : "Author";

            ViewBag.Authors = await _context.Authors.ToListAsync();

            var books = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            // Сортировка
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;

                case "Pages":
                    books = books.OrderBy(b => b.Pages);
                    break;

                case "pages_desc":
                    books = books.OrderByDescending(b => b.Pages);
                    break;

                case "Author":
                    books = books.OrderBy(b => b.Author.FullName);
                    break;

                case "author_desc":
                    books = books.OrderByDescending(b => b.Author.FullName);
                    break;

                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            return View(await books.ToListAsync());
        }
    }
}