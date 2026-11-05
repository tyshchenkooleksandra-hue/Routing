using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Route("products")]
        [Route("products/index", Order = 1)]
        [Route("items", Order = 1)]
        [Route("items/index", Order = 1)]
        public IActionResult Index(int? filterId, string filterName)
        {
            return View(productService.GetAll(filterId, filterName));
        }

        [HttpGet("products/{id:int}")]
        public IActionResult View(int id)
        {
            var product = productService.GetById(id);
            if (product == null)
            {
                return RedirectToProductNotFound();
            }

            return View(product);
        }

        [HttpGet("products/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = productService.GetById(id);
            if (product == null)
            {
                return RedirectToProductNotFound();
            }

            return View(product);
        }

        [HttpPost("products/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (!productService.Update(product))
            {
                return RedirectToProductNotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("products/create")]
        [HttpGet("products/new")]
        public IActionResult Create()
        {
            return View(new Product() { Id = productService.GetNextId() });
        }

        [HttpPost("products/create")]
        [HttpPost("products/new")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productService.Add(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("products/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!productService.Delete(id))
            {
                return RedirectToProductNotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("products/not-found")]
        public IActionResult ProductNotFound()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult RedirectToProductNotFound()
        {
            return RedirectToAction(nameof(ProductNotFound));
        }
    }
}
