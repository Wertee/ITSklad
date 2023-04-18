using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sklad.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sklad.Infrastructure;
using Sklad.Models.EF;

namespace Sklad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EFContext _context;

        public HomeController(EFContext con)
        {
            _context = con;
        }
        public IActionResult Index() => View(_context.Products.Where(x => x.CurrentCount>0).ToList());
        
        public IActionResult Outcome(int? id)
        {
            var prod = _context.Products.Find(id);
            if (prod != null)
                return View(new Outcome() { ProductId = prod.Id });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Outcome(Outcome outcome)
        {
            var prod = _context.Products.Find(outcome.ProductId);
            if (outcome.Count == 0)
            {
                ModelState.AddModelError("","Необходимо ввести количество");
                return View(outcome);
            }
            try
            {
                prod.CurrentCount = NewCount.OutcomeCount(prod.CurrentCount, outcome.Count);
                prod.CountToGive -= outcome.Count;
                if ((prod.CurrentCount == 0) || (prod.CountToGive <= 0))
                {
                    prod.CanBeGiven = false;
                    prod.CountToGive = 0;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Нельзя выдать больше чем есть на складе");
                return View(outcome);
            }
            
            _context.Outcomes.Add(outcome);
            _context.Products.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Income(int? id)
        {
            var prod = _context.Products.Find(id);
            if (prod != null)
                return View(new Income() { ProductId = prod.Id });
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public IActionResult Income(Income income)
        {
            var prod = _context.Products.Find(income.ProductId);
            if (prod == null | income.Count == 0)
            {
                ModelState.AddModelError("", "Не введено количество");
                return View(income);
            }
            prod.CurrentCount = NewCount.IncomeCount(prod.CurrentCount, income.Count);
            _context.Incomes.Add(income);
            _context.Products.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult NewProduct()
        {
            var yearsList = GetIncomeYears.GetYears();
            SelectList yearSelectList = new SelectList(yearsList);
            ViewBag.Years = yearSelectList;
            return View();
        }
        [HttpPost]
        public IActionResult NewProduct(Product prod)
        {
            if (prod != null && prod.CurrentCount>0)
            {
                _context.Products.Add(prod);
                _context.SaveChanges();
                _context.Incomes.Add(new Income() { ProductId = prod.Id, Count = prod.CurrentCount,Date = DateTime.Now});
                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Введите корректное количество");
                var yearsList = GetIncomeYears.GetYears();
                SelectList yearSelectList = new SelectList(yearsList);
                ViewBag.Years = yearSelectList;
                return View(prod);
            }
            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int? id)
        {
            var prod = _context.Products.Find(id);
            if (prod != null)
            {
                var yearsList = GetIncomeYears.GetYears();
                SelectList yearSelectList = new SelectList(yearsList);
                ViewBag.Years = yearSelectList;
                return View(prod);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditProduct(Product prod)
        {
            if (prod != null)
            {
                if (prod.CountToGive > prod.CurrentCount || (prod.CountToGive == 0 && prod.CanBeGiven))
                {
                    ModelState.AddModelError("","Введите корректное количество");
                    var yearsList = GetIncomeYears.GetYears();
                    SelectList yearSelectList = new SelectList(yearsList);
                    ViewBag.Years = yearSelectList;
                    return View(prod);
                }
                _context.Products.Update(prod);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        
        public IActionResult DeleteProduct(int? id)
        {
            var prod = _context.Products.Find(id);
            if (prod == null)
                return RedirectToAction("Index");
            _context.Products.Remove(prod);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ViewResult Incomes() => View(_context.Incomes.Include(x => x.Product).ToList());

        public ViewResult Outcomes() => View(_context.Outcomes.Include(x => x.Product).ToList());

    }
}
