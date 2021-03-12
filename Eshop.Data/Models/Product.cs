using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eshop.Data.Models
{
    public class Product
    {
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vyplňte kód")]
        [StringLength(255, ErrorMessage = "Kód je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Kód produktu")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Vyplňte Url")]
        [StringLength(255, ErrorMessage = "Url je příliš dlouhá, max. 255 znaků")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Používejte jen malá písmena bez diakritiky nebo číslice")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Vyplňte titulek")]
        [StringLength(255, ErrorMessage = "Titulek je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Titulek produktu")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vyplňte krátký popis")]
        [StringLength(255, ErrorMessage = "Krátký popis je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Krátký popis")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Vyplňte popis")]
        [Display(Name = "Popis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vyplňte cenu")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nesmí být záporná")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Cena před slevou musí být větší než nula")]
        [Display(Name = "Cena před slevou")]
        public decimal? OldPrice { get; set; }

        [Required(ErrorMessage = "Vyplňte počet kusů na skladě")]
        [Range(0, int.MaxValue, ErrorMessage = "Počet kusů na skladě nesmí být záporný")]
        [Display(Name = "Skladem")]
        public int Stock { get; set; }
        
        //vlastnosti
        [Range(0, int.MaxValue, ErrorMessage = "Počet obrázků nesmí být záporný")]
        [Display(Name = "Obrázků produktu celkem")]
        public int ImagesCount { get; set; }

        [Display(Name = "Skrýt")]
        public bool Hidden { get; set; }

        public Product()
        {
            ImagesCount = 0;
            Hidden = false;
            CategoryProducts = new List<CategoryProduct>();
        }
    }
}
