
using biblApp.Data;
using biblApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AuthorsController : Controller
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context)
    {
        _context = context;
    }

    // GET: AUTHORS
    public async Task<IActionResult> Index()
    {
        var authors = await _context.Authors
            .Include(a => a.Books)
            .Include(a => a.Editions)
            .ToListAsync();

        return View(authors);
    }

    // GET: AUTHORS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        return View(author);
    }

    // GET: AUTHORS/Create
    public IActionResult Create()
    {
        ViewData["Editions"] = new MultiSelectList(_context.Editions, "Id", "Title");
        return View();
    }

    // POST: AUTHORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Author author)
    {
        if (ModelState.IsValid)
        {
            if (author.SelectedEditionIds != null)
            {
                author.Editions = await _context.Editions
                    .Where(e => author.SelectedEditionIds.Contains(e.Id))
                    .ToListAsync();
            }

            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Editions"] = new MultiSelectList(_context.Editions, "Id", "Title", author.SelectedEditionIds);
        return View(author);
    }

    // GET: AUTHORS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Authors
            .Include(a => a.Editions)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null) return NotFound();

        author.SelectedEditionIds = author.Editions.Select(e => e.Id).ToList();

        ViewData["Editions"] = new MultiSelectList(
            _context.Editions,
            "Id",
            "Title",
            author.SelectedEditionIds
        );

        return View(author);
    }

    // POST: AUTHORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Author author)
    {
        if (id != author.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var existingAuthor = await _context.Authors
                .Include(a => a.Editions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAuthor == null) return NotFound();

            existingAuthor.FullName = author.FullName;
            existingAuthor.Biography = author.Biography;

            existingAuthor.Editions.Clear();

            if (author.SelectedEditionIds != null)
            {
                var editions = await _context.Editions
                    .Where(e => author.SelectedEditionIds.Contains(e.Id))
                    .ToListAsync();

                foreach (var edition in editions)
                    existingAuthor.Editions.Add(edition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Editions"] = new MultiSelectList(_context.Editions, "Id", "Title", author.SelectedEditionIds);
        return View(author);
    }

    // GET: AUTHORS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        return View(author);
    }

    // POST: AUTHORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AuthorExists(int? id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}
