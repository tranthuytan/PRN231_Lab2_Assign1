using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataBookStore.Models;
using Microsoft.AspNetCore.OData;

namespace ODataBookStore.Controllers
{
    public class BookController : ODataController
    {
        private BookStoreContext _context;
        public BookController(BookStoreContext context)
        {
            _context = context;
        }
        [EnableQuery(PageSize =2)]
        public IActionResult Get()
        {
            return Ok(_context.Books.Include(b=>b.Location).Include(b=>b.Press).AsQueryable());
        }
        [EnableQuery]
        public IActionResult Get(int key, string version)
        {
            var book = _context.Books.Include(b => b.Location).Include(b => b.Press).FirstOrDefault(b => b.Id == key);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Created(book);
        }
        [EnableQuery]
        public IActionResult Delete([FromBody]int key)
        {
            Book b = _context.Books.FirstOrDefault(b => b.Id == key);
            if (b == null)
                return NotFound();
            _context.Books.Remove(b);
            _context.SaveChanges();
            return Ok();
        }

    }
}
