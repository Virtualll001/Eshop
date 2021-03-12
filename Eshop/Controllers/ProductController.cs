using Eshop.Business.Interfaces;
using Eshop.Classes;
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
    }
}
