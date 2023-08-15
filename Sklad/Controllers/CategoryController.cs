using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using Sklad.Models.EF;

namespace Sklad.Controllers;

public class CategoryController:Controller
{
    private readonly EFContext _context;

    public CategoryController(EFContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        if (category.Name.Length < 4)
        {
            ModelState.AddModelError("", "Слишком короткое название категории");
            return View(category);
        }
            
        try
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Ошибка записи в базу данных!");
            return View(category);
        }
    }
}