using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeFunctionality.Models;
using PrototypeFunctionality.ViewModels;

namespace PrototypeFunctionality.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PrototypeFunctionalityDbContext _dbContext;

        public ProductController(PrototypeFunctionalityDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Index()
        {
            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            var products = await _dbContext.Products
                           .Include(product => product.ProductCategory)
                           .Where(product => product.ApplicationUserId == loginFarmerId)
                           .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            ViewBag.ProductCategories = await _dbContext.ProductCategories
                                        .Where(productCategory => productCategory.ApplicationUserId == loginFarmerId)
                                        .Select(s => new PrototypeFunctionality.ViewModels.SelectModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductCreateModel productModel)
        {
            productModel.ApplicationUserId = _signInManager.UserManager.GetUserId(User)!;

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productModel.Name,
                    ProductCategoryId = productModel.ProductCategoryId,
                    ProductionDate = productModel.ProductionDate,
                    Details = productModel.Details,
                    ApplicationUserId = productModel.ApplicationUserId
                };

                await _dbContext.Products.AddAsync(product);
                var saveProduct = await _dbContext.SaveChangesAsync();

                if (saveProduct > 0)
                {
                    TempData["ProductCreateNotifaction"] = "Product Create Successfull";
                    return RedirectToAction("Index", "Product");
                }

                ModelState.AddModelError("", "Product cannot saved! Please, try again.");
            }

            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            ViewBag.ProductCategories = await _dbContext.ProductCategories
                                        .Where(productCategory => productCategory.ApplicationUserId == loginFarmerId)
                                        .Select(s => new SelectModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToListAsync();

            return View(productModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existProduct = await _dbContext.Products
                               .Where(product => product.Id == id)
                               .FirstOrDefaultAsync();

            if (existProduct is null)
                return View(new ProductEditModel());

            var product = new ProductEditModel
            {
                Id = existProduct.Id,
                Name = existProduct.Name,
                ProductCategoryId = existProduct.ProductCategoryId,
                Details = existProduct.Details,
                ApplicationUserId = existProduct.ApplicationUserId
            };

            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            ViewBag.ProductCategories = await _dbContext.ProductCategories
                                        .Where(productCategory => productCategory.ApplicationUserId == loginFarmerId)
                                        .Select(s => new PrototypeFunctionality.ViewModels.SelectModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToListAsync();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditModel productEditModel)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = productEditModel.Id,
                    Name = productEditModel.Name,
                    ProductCategoryId = productEditModel.ProductCategoryId,
                    Details = productEditModel.Details,
                    ApplicationUserId = productEditModel.ApplicationUserId
                };

                _dbContext.Products.Update(product);
                var editProduct = await _dbContext.SaveChangesAsync() > 0;

                if (editProduct)
                {
                    TempData["ProductEditNotifaction"] = "Product Edit Successfull";
                    return RedirectToAction("Index", "Product");
                }
            }

            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            ViewBag.ProductCategories = await _dbContext.ProductCategories
                                        .Where(productCategory => productCategory.ApplicationUserId == loginFarmerId)
                                        .Select(s => new PrototypeFunctionality.ViewModels.SelectModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToListAsync();

            return View(productEditModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var existProduct = await _dbContext.Products
                              .Where(product => product.Id == id)
                              .FirstOrDefaultAsync();

            if (existProduct is null)
                return BadRequest("Product cannot found! Please, try again.");

            _dbContext.Products.Remove(existProduct);
            var deleteProduct = await _dbContext.SaveChangesAsync() > 0;

            if (deleteProduct)
            {
                TempData["ProductDeleteNotifaction"] = "Product Delete Successfull";
                return RedirectToAction("Index", "Product");
            }

            return RedirectToAction("Index", "Product");
        }
    }
}