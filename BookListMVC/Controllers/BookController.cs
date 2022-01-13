using BookListMVC.Data;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Book Book { get; set; }

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Book>? objBookList = _db.Books.ToList();
            return View(objBookList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ViewBag.Message="Added";
            Book = new Book();
            if(id==null)
            {
                //create
                return View(Book);
            }
            //update
            Book = _db.Books.FirstOrDefault(u=>u.Id==id);
            if(Book==null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    //create
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var obj = _db.Books.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.Books.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}