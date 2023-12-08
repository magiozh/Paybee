using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LibraryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBookService _booksService;

        public CheckoutController(IBookService bookService)
        {
            _booksService = bookService;
        }

        public IActionResult Purchase(Guid id)
        {

            var book = _booksService.GetById(id);
            ViewBag.PurchaseAmount = book.Price;
            if (book == null) return NotFound();
            return View(book);
        }
        [HttpPost]
        public IActionResult Create(string stripeToken, Guid id) 
        {
            var book = _booksService.GetById(id);

            var chargeOptions = new ChargeCreateOptions()
            {
                //Amount = (long)(Convert.ToDouble(book.Price)*100),
                Amount = (long)(Convert.ToDouble(book.Price)),
                Currency = "sgd",
                Source = stripeToken,
                Metadata = new Dictionary<string, string>()
                {
                    {"BookId", book.Id.ToString() },
                    {"BookTitle", book.Title },
                    {"BookAuthor", book.Author }
                }
            };

            var service = new ChargeService();
            Charge charge = service.Create(chargeOptions);

            if(charge.Status == "succeeded")
            {
                return LocalRedirect("/");
            }
            return LocalRedirect("/");
        }

        public IActionResult Dea()
        {
            return LocalRedirect("/");
        }
    }
}