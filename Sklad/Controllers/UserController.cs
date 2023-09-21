using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Sklad.Models.EF;

namespace Sklad.Controllers
{
    public class UserController : Controller
    {
        private EFContext _context;

        public UserController(EFContext con)
        {
            _context = con;
        }
        public IActionResult UserIndex()
        {
            return View(_context.Products.Where(p => p.CanBeGiven && p.CountToGive>0).ToList());
        }
    }
}
