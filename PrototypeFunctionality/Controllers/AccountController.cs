using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeFunctionality.Models;
using PrototypeFunctionality.ViewModels;

namespace PrototypeFunctionality.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                            .PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.IsRememberMe, false);

                if (result.Succeeded)
                {
                    var loginUserInfo = await _userManager.FindByEmailAsync(loginModel.Email);
                    TempData["LoginNotifaction"] = "Login Successfull";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                    {
                        if (loginUserInfo.UserTypeId == (int)UserType.Employee)
                            return RedirectToAction("Index", "Employee");

                        return RedirectToAction("Index", "Product");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                var employee = new ApplicationUser 
                { 
                    FirstName = employeeModel.FirstName,
                    LastName = employeeModel.LastName,
                    UserTypeId = employeeModel.UserTypeId,
                    UserName = employeeModel.Email, 
                    Email = employeeModel.Email, 
                    City = employeeModel.City 
                };

                var createEmployeeResult = await _userManager.CreateAsync(employee, employeeModel.Password);
				var setEmployeeRole = await _userManager.AddToRoleAsync(employee, "Employee");

				if (createEmployeeResult.Succeeded && setEmployeeRole.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["EmployeeRegistrationNotifaction"] = "Employee Registration Successfull";
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in createEmployeeResult.Errors)
					ModelState.AddModelError("", error.Description);
			}

            return View(employeeModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}