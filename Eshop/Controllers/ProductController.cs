using Eshop.Business.Interfaces;
using Eshop.Classes;
using Eshop.Extensions;
using Eshop.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Controllers
{
    [ExceptionsToMessageFilter]
    public class ProductController : Controller
    {
        private readonly ICategoryManager categoryManager;
        private readonly IProductManager productManager;

        public ProductController(ICategoryManager categoryManager, IProductManager productManager)
        {
            this.categoryManager = categoryManager;
            this.productManager = productManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageProduct(ManageProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.FormCaption = model.Product.ProductId == 0
                    ? "Nový produkt"
                    : "Editace produktu";
                model.AvailableCategories = categoryManager.GetCategoriesWithoutChildCategories();
                this.AddFlashMessage("Špatné parametry výrobku!", FlashMessageType.Danger);

                //if (!string.IsNullOrEmpty(url))
                //    model.PostedCategories = productManager.FindAssignedCategoriesToProduct(
                //        model.AvailableCategories, model.Product.CategoryProducts.ToList(), model.PostedCategories);

                return View(model);
            }
            var availableCategories = categoryManager.GetCategoriesWithoutChildCategories();

            // najdi ze všech dostupných kategorií ty, které jsou označené (PostedCategories[index] == true)
            int[] selectedIdCategories = availableCategories
                .Where(c => model.PostedCategories[availableCategories.IndexOf(c)])
                .Select(c => c.CategoryId)
                .ToArray();

            // uložení produktu i s jeho vazbami
            productManager.SaveProduct(model.Product);
            categoryManager.UpdateProductCategories(model.Product.ProductId, selectedIdCategories);
            this.AddFlashMessage("Produkt byl úspěšně uložen", FlashMessageType.Success);
            return RedirectToAction(actionName: "Administration", controllerName: "Account");
        }
    }
}
