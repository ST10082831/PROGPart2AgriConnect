using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeFunctionality.Models;
using PrototypeFunctionality.ViewModels;

namespace PrototypeFunctionality.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductCategoryController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PrototypeFunctionalityDbContext _dbContext;

        public ProductCategoryController(PrototypeFunctionalityDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loginFarmerId = _signInManager.UserManager.GetUserId(User);
            var productCategories = await _dbContext.ProductCategories
                                    .Where(productCategory => productCategory.ApplicationUserId == loginFarmerId)
                                    .ToListAsync();

            return View(productCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductCategoryCreateModel productCategoryModel)
        {
            productCategoryModel.ApplicationUserId = _signInManager.UserManager.GetUserId(User)!;

            if (ModelState.IsValid)
            {
                var productCategory = new ProductCategory
                {
                    Name = productCategoryModel.Name,
                    ApplicationUserId = productCategoryModel.ApplicationUserId
                };

                await _dbContext.ProductCategories.AddAsync(productCategory);
                var saveProductCategory = await _dbContext.SaveChangesAsync();

                if (saveProductCategory > 0)
                {
                    TempData["ProductCategoryCreateNotifaction"] = "Product Category Create Successfull";
                    return RedirectToAction("Index", "ProductCategory");
                }

                ModelState.AddModelError("", "Product category cannot saved! Please, try again.");
            }

            return View(productCategoryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existProductCategory = await _dbContext.ProductCategories
                                       .Where(productCategory => productCategory.Id == id)
                                       .FirstOrDefaultAsync();

            if (existProductCategory is null)
                return View(new ProductCategoryEditModel());

            var productCategory = new ProductCategoryEditModel
            {
                Name = existProductCategory.Name,
                ApplicationUserId = existProductCategory.ApplicationUserId
            };

            return View(productCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategoryEditModel productCategoryEditModel)
        {
            if(ModelState.IsValid)
            {
                var productCategory = new ProductCategory
                {
                    Id = productCategoryEditModel.Id,
                    Name = productCategoryEditModel.Name,
                    ApplicationUserId = productCategoryEditModel.ApplicationUserId
                };

                _dbContext.ProductCategories.Update(productCategory);
                var editProductCategory = await _dbContext.SaveChangesAsync() > 0;

                if(editProductCategory)
                {
                    TempData["ProductCategoryEditNotifaction"] = "Product Category Edit Successfull";
                    return RedirectToAction("Index", "ProductCategory");
                }

                return View(productCategoryEditModel);
            }

            return View(productCategoryEditModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var existProductCatgory = await _dbContext.ProductCategories
                              .Where(productCategory => productCategory.Id == id)
                              .FirstOrDefaultAsync();

            if (existProductCatgory is null)
                return BadRequest("Product categories cannot found! Please, try again.");

            _dbContext.ProductCategories.Remove(existProductCatgory);
            var deleteProductCategory = await _dbContext.SaveChangesAsync() > 0;

            if (deleteProductCategory)
            {
                TempData["ProductCategoryDeleteNotifaction"] = "Product Category Delete Successfull";
                return RedirectToAction("Index", "ProductCategory");
            }

            return RedirectToAction("Index", "ProductCategory");
        }
    }
}