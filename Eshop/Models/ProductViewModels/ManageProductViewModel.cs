using Eshop.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models.ProductViewModels
{
    public class ManageProductViewModel
    {
        public Product Product { get; set; }

        private List<Category> availableCategories;

        public List<Category> AvailableCategories
        {
            get { return availableCategories; }
            set
            {
                availableCategories = value;
                PostedCategories = new bool[value.Count];
            }
        }

        [Required(ErrorMessage = "Musíte vybrat nejméně jednu kategorii pro produkt")]
        [Display(Name = "Kategorie")]
        public bool[] PostedCategories { get; set; }

        [Display(Name = "Nahrát obrázky")]
        public List<IFormFile> UploadedImages { get; set; }

        public string FormCaption { get; set; }

        public ManageProductViewModel()
        {
            Product = new Product();
            AvailableCategories = new List<Category>();
            PostedCategories = new bool[0];
            UploadedImages = new List<IFormFile>();
        }
    }
}
