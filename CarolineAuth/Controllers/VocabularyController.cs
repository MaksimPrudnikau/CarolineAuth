using System.Security.Claims;
using CarolineAuth.Data;
using CarolineAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarolineAuth.Controllers;

[Authorize]
public class VocabularyController : Controller
{
    private readonly VocabularyContext context;
    
    public VocabularyController(VocabularyContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var isExist = TryGetVocabulary(out var vocabulary);
        if (!isExist)
        {
            return Problem("Entity set 'VocabularyContext.Vocabularies' is null.");
        }
        
        return View(vocabulary.Data);
    }
    
    // GET: Symbol/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Symbol/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,English,Japan")] Row row)
    {
        if (ModelState.IsValid)
        {
            var isExist = TryGetVocabulary(out var vocabulary);
            if (!isExist)
            {
                return Problem("Entity set 'VocabularyContext.Vocabularies' is null.");
            }

            row.Id = vocabulary.Data.Count + 1;
            vocabulary.Data.Add(row);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(row);
    }

    private bool TryGetVocabulary(out Vocabulary vocabulary)
    {
        vocabulary = null;
        try
        {
            vocabulary = GetVocabulary();
        }
        catch (NullReferenceException)
        {
            return false;
        }

        return true;
    }

    private Vocabulary GetVocabulary()
    {
        if (context.Vocabularies is null)
        {
            throw new NullReferenceException("Entity set 'VocabularyContext.Vocabularies' is null.");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var vocabulary = context.Vocabularies.FirstOrDefault(v => v.UserId == userId)!;
        return vocabulary;
    }
} 