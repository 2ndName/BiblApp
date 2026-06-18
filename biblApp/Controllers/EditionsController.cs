
using biblApp.Data;
using biblApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class EditionsController : Controller
{
    private readonly LibraryContext _context;

    public EditionsController(LibraryContext context)
    {
        _context = context;
    }

    // GET: EDITIONS
    public async Task<IActionResult> Index()    
    {
        var editions = await _context.Editions
         .Include(e => e.Authors)
         .ToListAsync();

        return View(editions);
    }

    // GET: EDITIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var edition = await _context.Editions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (edition == null)
        {
            return NotFound();
        }

        return View(edition);
    }

    // GET: EDITIONS/Create
    public IActionResult Create()
    {
        ViewData["Authors"] = new MultiSelectList(_context.Authors, "Id", "FullName");
        return View();
    }

    // POST: EDITIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Edition edition)
    {
        if (ModelState.IsValid)
        {
            if (edition.SelectedAuthorIds != null)
            {
                edition.Authors = await _context.Authors
                    .Where(a => edition.SelectedAuthorIds.Contains(a.Id))
                    .ToListAsync();
            }

            _context.Add(edition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Authors"] = new MultiSelectList(_context.Authors, "Id", "FullName", edition.SelectedAuthorIds);
        return View(edition);
    }

    // GET: EDITIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var edition = await _context.Editions
            .Include(e => e.Authors)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (edition == null) return NotFound();

        edition.SelectedAuthorIds = edition.Authors.Select(a => a.Id).ToList();

        ViewData["Authors"] = new MultiSelectList(
            _context.Authors,
            "Id",
            "FullName",
            edition.SelectedAuthorIds
        );

        return View(edition);
    }

    // POST: EDITIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Edition edition)
    {
        if (id != edition.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var existingEdition = await _context.Editions
                .Include(e => e.Authors)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEdition == null) return NotFound();

            existingEdition.Title = edition.Title;
            existingEdition.Description = edition.Description;

            existingEdition.Authors.Clear();

            if (edition.SelectedAuthorIds != null)
            {
                var authors = await _context.Authors
                    .Where(a => edition.SelectedAuthorIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var author in authors)
                    existingEdition.Authors.Add(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Authors"] = new MultiSelectList(_context.Authors, "Id", "FullName", edition.SelectedAuthorIds);
        return View(edition);
    }

    // GET: EDITIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var edition = await _context.Editions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (edition == null)
        {
            return NotFound();
        }

        return View(edition);
    }

    // POST: EDITIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var edition = await _context.Editions.FindAsync(id);
        if (edition != null)
        {
            _context.Editions.Remove(edition);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
