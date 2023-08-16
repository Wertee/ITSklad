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
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Ошибка записи в базу данных!");
            return View(category);
        }
    }

    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
            return RedirectToAction("Index");

        try
        {
            var productsWithCategory = _context.Products.Where(p => p.CategoryId == category.Id).ToList();
            foreach (var product in productsWithCategory)
            {
                product.CategoryId = null;
            }
            _context.SaveChanges();
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            
        }
        
        return RedirectToAction("Index");
    }

    public IActionResult EditCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
            return RedirectToAction("Index");
        return View(category);
    }

    [HttpPost]
    public IActionResult EditCategory(Category category)
    {
        if (category == null)
            return RedirectToAction("Index");
        _context.Categories.Update(category);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}