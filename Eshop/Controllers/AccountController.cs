using Eshop.Classes;
using Eshop.Data.Models;
using Eshop.Extensions;
using Eshop.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                  ? Redirect(returnUrl)
                  : (IActionResult)RedirectToAction(nameof(HomeController.Index), "Home");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(model.Email) == null)
                {
                    // vytvoříme nový objekt typu ApplicationUser (uživatel), přidáme ho do databáze a přihlásíme ho
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);

                        return string.IsNullOrEmpty(returnUrl)
                            ? RedirectToAction("Index", "Home")
                            : RedirectToLocal(returnUrl);
                    }

                    AddErrors(result);
                }

                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je již zaregistrován" }));
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // kontrola na straně serveru, zda jsou všechny odeslané údaje obsažené ve viewmodelu v pořádku
            if (!ModelState.IsValid)
                return View(model);

            // pokus o přihlášení uživatele na základě zadaných údajů
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // pokud byly odeslány neplatné údaje, vrátíme uživatele k přihlašovacímu formuláři
            if (result.Succeeded)
            {
                this.AddFlashMessage(new FlashMessage("Přihlášení proběhlo úspěšně", FlashMessageType.Success));
                return RedirectToAction("Administration");
                //RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje");
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {       
            //6. lekce kapitola "Další akce"
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User)
                ?? throw new ApplicationException($"Nepodařilo se načíst uživatele s ID {userManager.GetUserId(User)}.");

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User)
                ?? throw new ApplicationException($"Nepodařilo se načíst uživatele s ID: {userManager.GetUserId(User)}.");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Administration");
        }
        public IActionResult Administration()
        {
            return View();
        }
    }
}
