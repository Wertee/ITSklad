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
using Microsoft.IdentityModel.Tokens;
using Sklad.Infrastructure;
using Sklad.Models.EF;
using Sklad.Models.Filters;

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

        public IActionResult Index(ProductFilter filter)
        {

            var products = _context.Products.Where(p => p.CurrentCount > 0).AsQueryable();
            if (!filter.Name.IsNullOrEmpty())
                products = filter.ExactMatch 
                    ? products.Where(p => p.Name == filter.Name)
                    : products.Where(p => p.Name.Contains(filter.Name));
            if (!filter.Description.IsNullOrEmpty())
                products = filter.ExactMatch
                    ? products.Where(p => p.Description == filter.Description)
                    : products.Where(p=>p.Description.Contains(filter.Description));
            if (!filter.Applicant.IsNullOrEmpty())
                products = filter.ExactMatch 
                    ? products.Where(p => p.Applicant == filter.Applicant)
                    : products.Where(p=>p.Applicant.Contains(filter.Applicant));
            if (filter.YearOfIncome != 0)
                products = products.Where(p => p.YearOfIncome == filter.YearOfIncome);

            var foundProducts = products.OrderBy(p => p.Name).Include(p=>p.Category).ToList();
            
            return View(foundProducts);
        }


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
                ModelState.AddModelError("", "Необходимо ввести количество");
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
                ModelState.AddModelError("", "Нельзя выдать больше чем есть на складе");
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

            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            
            return View();
        }
        [HttpPost]
        public IActionResult NewProduct(Product prod)
        {
            if (prod != null && prod.CurrentCount > 0)
            {
                _context.Products.Add(prod);
                _context.SaveChanges();
                _context.Incomes.Add(new Income() { ProductId = prod.Id, Count = prod.CurrentCount, Date = DateTime.Now });
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
                
                var categories = _context.Categories.ToList();
                ViewBag.Categories = categories;
                
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
                    ModelState.AddModelError("", "Введите корректное количество");
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

        public ViewResult Incomes(IncomeFilter filter)
        {
            var incomes = _context.Incomes.Include(x => x.Product).AsQueryable();
            if (!filter.Name.IsNullOrEmpty())
                incomes = filter.ExactMatch 
                    ? incomes.Where(p => p.Product.Name == filter.Name)
                    : incomes.Where(p => p.Product.Name.Contains(filter.Name));
            if (!filter.Description.IsNullOrEmpty())
                incomes = filter.ExactMatch
                    ? incomes.Where(p => p.Product.Description == filter.Description)
                    : incomes.Where(p=>p.Product.Description.Contains(filter.Description));
            if (filter.YearOfIncome != 0)
                incomes = incomes.Where(p => p.Date.Year == filter.YearOfIncome);
            return View(incomes.ToList());
        } 

        public ViewResult Outcomes(OutcomeFilter filter)
        {
            var outcomes = _context.Outcomes.Include(x => x.Product).AsQueryable();
            if (!filter.Name.IsNullOrEmpty())
                outcomes = filter.ExactMatch 
                    ? outcomes.Where(p => p.Product.Name == filter.Name)
                    : outcomes.Where(p => p.Product.Name.Contains(filter.Name));
            if (!filter.Description.IsNullOrEmpty())
                outcomes = filter.ExactMatch
                    ? outcomes.Where(p => p.Product.Description == filter.Description)
                    : outcomes.Where(p=>p.Product.Description.Contains(filter.Description));
            if (!filter.Recipient.IsNullOrEmpty())
                outcomes = filter.ExactMatch
                    ? outcomes.Where(p => p.Name == filter.Recipient)
                    : outcomes.Where(p => p.Name.Contains(filter.Recipient));
            if (filter.YearOfOutcome != 0)
                outcomes = outcomes.Where(p => p.Date.Year == filter.YearOfOutcome);
            return View(outcomes);
        } 

        

    }
}
