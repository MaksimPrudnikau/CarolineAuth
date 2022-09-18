using System.Security.Claims;
using Caroline.Extensions;
using CarolineAuth.Data;
using CarolineAuth.Extensions;
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
            return Problem("Entity set 'VocabularyContext.Vocabularies' is null.");
        
        return View(vocabulary);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(nameof(Row.English), nameof(Row.Japanese))] Row row)
    {
        if (!ModelState.IsValid) 
            return View(row);
        
        var isExist = TryGetVocabulary(out var vocabulary);
        
        if (!isExist)
            return Problem("Entity set 'VocabularyContext.Vocabularies' is null.");

        var index = vocabulary.Count == 0 ? 1 : vocabulary.Count + 1;
        row.Id = index;
        vocabulary.Add(row);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult Details(int? id)
    {
        var isExist = TryGetVocabulary(out var vocabulary);
        if (!isExist)
            return Problem("Entity set 'VocabularyContext.Vocabularies' is null.");


        if (id == null || !vocabulary.ContainsIndex(id))
            return NotFound();

        var row = vocabulary.First(row => row.Id == id);
        return View(row);
    }
    
    public IActionResult Edit(int? id)
    {
        var rowExist = TryGetRow(id, out var row);
        return rowExist
            ? View(row)
            : NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(nameof(Row.English), nameof(Row.Japanese))] Row row)
    {
        if (!ModelState.IsValid)
            return View(row);
        
        var vocabularyExist = TryGetVocabulary(out var vocabulary);
        var rowExist = vocabulary.ContainsIndex(id);
            
        if (!vocabularyExist || !rowExist)
            return NotFound();

        vocabulary[id - 1] = row;
        vocabulary[id - 1].Id = id;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult Delete(int? id)
    {
        var vocabularyExist = TryGetVocabulary(out var vocabulary);
        if (!vocabularyExist || id is null || !vocabulary.ContainsIndex(id.Value))
            return NotFound();

        var row = vocabulary.First(row => row.Id == id);
        return View(row);
    }

    // POST: Symbol/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vocabularyExist = TryGetVocabulary(out var vocabulary);
        if (!vocabularyExist || !vocabulary.ContainsIndex(id))
            return NotFound();

        vocabulary.RemoveInc(id);
        
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public IActionResult GetRandomSymbol(RandomRowViewModel randomRowViewModel)
    {
        var vocabularyExist = TryGetVocabulary(out var vocabulary);
        if (!vocabularyExist)
            return NotFound();
        
        var random = new Random();
        var randomIndex = random.Next(0, vocabulary.Count);
        var randoRow = vocabulary[randomIndex];
        var symbolViewModel = new RandomRowViewModel(){Row = randoRow, SearchOption = randomRowViewModel.SearchOption};
        TempData.Put("randomRowViewModel", symbolViewModel);
        return RedirectToAction("Random");
    }

    public IActionResult Random()
    {
        var randomRowViewModel = TempData.Get<RandomRowViewModel>("randomRowViewModel");
        return randomRowViewModel is null 
            ? RedirectToAction(nameof(Index), "Home") 
            : View(randomRowViewModel);
    }

    private bool TryGetVocabulary(out IList<Row> vocabulary)
    {
        vocabulary = new List<Row>();
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

    private IList<Row> GetVocabulary()
    {
        if (context.Vocabularies is null)
        {
            throw new NullReferenceException("Entity set 'VocabularyContext.Vocabularies' is null.");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var vocabulary = context.Vocabularies.FirstOrDefault(v => v.UserId == userId)!;
        return vocabulary.Data;
    }

    private bool TryGetRow(int? id, out Row? row)
    {
        row = null;
        var vocabularyExist = TryGetVocabulary(out var vocabulary);
        if (!vocabularyExist || !vocabulary.ContainsIndex(id))
        {
            return false;
        }

        row = vocabulary[id!.Value - 1];
        return true;
    }
} 