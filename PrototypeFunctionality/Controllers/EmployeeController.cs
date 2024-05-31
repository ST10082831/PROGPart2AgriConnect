using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeFunctionality.Models;
using PrototypeFunctionality.ViewModels;

namespace PrototypeFunctionality.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PrototypeFunctionalityDbContext _dbContext;

        public EmployeeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            PrototypeFunctionalityDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>> Index()
        {
            var farmers = await _signInManager.UserManager.Users
                          .Where(user => user.UserTypeId != (int)UserType.Employee)
                          .ToListAsync();

            return View(farmers);
        }

        [HttpGet]
        public async Task<IActionResult> AddFarmer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(FarmerAddModel farmerModel)
        {
            if (ModelState.IsValid)
            {
                var farmer = new ApplicationUser
                {
                    FirstName = farmerModel.FirstName,
                    LastName = farmerModel.LastName,
                    UserTypeId = farmerModel.UserTypeId,
                    UserName = farmerModel.Email,
                    Email = farmerModel.Email,
                    City = farmerModel.City
                };

                var createFarmerResult = await _userManager.CreateAsync(farmer, farmerModel.Password);
                var setFarmerRole = await _userManager.AddToRoleAsync(farmer, "Farmer");

                if (createFarmerResult.Succeeded && setFarmerRole.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["FarmerCreateNotifaction"] = "Farmer Create Successfull";
                    return RedirectToAction("Index", "Employee");
                }

                foreach (var error in createFarmerResult.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(farmerModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                return BadRequest("Farmer id is not valid! Please, try again.");

            var existFarmer = await _userManager.FindByIdAsync(id);

            if (existFarmer is null)
                return BadRequest("Farmer annot found! Please, try again.");

            var deleteFarmerRole = await _userManager.RemoveFromRoleAsync(existFarmer, "Farmer");
            var deleteFarmer = await _userManager.DeleteAsync(existFarmer);            

            if (deleteFarmer.Succeeded && deleteFarmerRole.Succeeded)
            {
                TempData["FarmerDeleteNotifaction"] = "Farmer Delete Successfull";
                return RedirectToAction("Index", "Employee");
            }

            return RedirectToAction("Index", "Employee");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _dbContext.Products
                           .Include(Product => Product.ApplicationUser)
                           .Include(Product => Product.ProductCategory)
                           .Where(product => product.ApplicationUser.UserTypeId == (int)UserType.Farmer)
                           .ToListAsync();

            return View(products);
        }
    }
}