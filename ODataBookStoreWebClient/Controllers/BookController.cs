using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODataBookStore.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ODataBookStoreWebClient.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";

        public BookController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7057/odata/Books";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + $"?$expand=Location,Press");
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var list = temp.value;
            List<Book> items = ((JArray)list).Select(x => new Book
            {
                Id = (int)x["Id"],
                Author = (string)x["Author"],
                ISBN = (string)x["ISBN"],
                Title = (string)x["Title"],
                Price = (decimal)x["Price"],
                LocationName = (string)x["LocationName"],
                Press = new Press { Id = (int)x["Press"]["Id"], Name = (string)x["Press"]["Name"] }
            }).ToList();
            return View(items);
        }
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + $"/{id}?$expand=Location,Press");
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            Book book = new Book
            {
                Id = temp.Id,
                ISBN = temp.ISBN,
                Title = temp.Title,
                Author = temp.Author,
                Price = temp.Price,
                LocationName = temp.LocationName,
                PressId = temp.PressId,
                Location = new Address
                {
                    City = temp.Location.City,
                    Street = temp.Location.Street
                },
                Press = new Press
                {
                    Id = temp.Press.Id,
                    Name = temp.Press.Name,
                    Category = temp.Press.Category
                }
            };
            return View(book);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<Book>(ProductApiUrl, book);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + $"/{id}?$expand=Location,Press");
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            Book book = new Book
            {
                Id = temp.Id,
                ISBN = temp.ISBN,
                Title = temp.Title,
                Author = temp.Author,
                Price = temp.Price,
                LocationName = temp.LocationName,
                PressId = temp.PressId,
                Location = new Address
                {
                    City = temp.Location.City,
                    Street = temp.Location.Street
                },
                Press = new Press
                {
                    Id = temp.Press.Id,
                    Name = temp.Press.Name,
                    Category = temp.Press.Category
                }
            };
            return View("Edit", book);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<Book>(ProductApiUrl, book);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + $"/{id}?$expand=Location,Press");
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            Book book = new Book
            {
                Id = temp.Id,
                ISBN = temp.ISBN,
                Title = temp.Title,
                Author = temp.Author,
                Price = temp.Price,
                LocationName = temp.LocationName,
                PressId = temp.PressId,
                Location = new Address
                {
                    City = temp.Location.City,
                    Street = temp.Location.Street
                },
                Press = new Press
                {
                    Id = temp.Press.Id,
                    Name = temp.Press.Name,
                    Category = temp.Press.Category
                }
            };
            return View("Delete", book);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection? collection=null)
        {
            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl + $"/{id}");
            var strData = response.IsSuccessStatusCode;
            if (strData == false)
                return View("Delete");
            return RedirectToAction("Index");
        }
    }
}
