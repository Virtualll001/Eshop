using Eshop.Business.Interfaces;
using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Eshop.Business.Managers
{
    public class ProductManager : IProductManager
    {
        private const string ProductImagePath = "wwwroot/images/products/";
        private IImageManager imageManager = new ImageManager(ProductImagePath);
        private const int ProductImageMaxWidth = 500;
        private const int ProductImageMaxHeight = 666;

        private IProductRepository productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public Product FindProductById(int id)
        {
            return productRepository.FindById(id);
        }
        public Product FindProductByUrl(string url)
        {
            return productRepository.FindByUrl(url);
        }
        public void SaveProduct(Product product)
        {
            var oldProduct = productRepository.FindById(product.ProductId);

            if (product.ProductId != 0)
                product.ProductId = 0;

            productRepository.Insert(product);

            if (oldProduct != null)
                CleanProduct(oldProduct);
        }

        private void CleanProduct(Product oldProduct, bool removeImages = false)
        {
            try
            {
                productRepository.Delete(oldProduct.ProductId);
            }
            catch (Exception)
            {
                oldProduct.CategoryProducts.Clear();    // odstraníme produkt z kategorií
                oldProduct.Hidden = true;               // a skryjeme jej
                productRepository.Update(oldProduct);
            }

            if (removeImages)
            {
                int imagesCount = oldProduct.ImagesCount;
                RemoveThumbnailFile(oldProduct.ProductId);  // odstranění miniatury

                for (int i = 0; i < imagesCount; i++)       // odstranění obrázků
                    RemoveImageFile(oldProduct.ProductId, i);
            }
        }

        public bool[] FindAssignedCategoriesToProduct(List<Category> availableCategories, List<CategoryProduct> assignedCategories, bool[] postedCategories)
        {
            for (int a = 0; a < availableCategories.Count; a++)
            {
                for (int p = 0; p < assignedCategories.Count; p++)
                {
                    if (availableCategories[a].CategoryId == assignedCategories[p].CategoryId)
                        postedCategories.SetValue(true, a);

                    break;
                }
            }

            return postedCategories;
        }

        private string GetImageFileName(int productId, int imageIndex, bool full = true)
        {
            if (!Directory.Exists($"{ProductImagePath}{productId}"))
                Directory.CreateDirectory(ProductImagePath + productId);

            string result = $"{productId}/{productId}_{imageIndex}";

            if (full)
                result = ProductImagePath + result + ".jpeg";

            return result;
        }
        private string GetThumbnailFileName(int productId, bool full = true)
        {
            if (!Directory.Exists($"{ProductImagePath}{productId}"))
                Directory.CreateDirectory(ProductImagePath + productId);

            string result = $"{productId}/{productId}_thumb";

            if (full)
                result = ProductImagePath + result + ".png";

            return result;
        }

        private void RemoveImageFile(int productId, int imageIndex)
        {
            string fileName = GetImageFileName(productId, imageIndex);
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        private void RemoveThumbnailFile(int productId)
        {
            string thumbfileName = GetThumbnailFileName(productId);
            if (File.Exists(thumbfileName))
                File.Delete(thumbfileName);
        }

        private void RenameImage(int oldProductId, int oldImageIndex, int productId, int imageIndex)
        {
            string oldPath = GetImageFileName(oldProductId, oldImageIndex);
            string newPath = GetImageFileName(productId, imageIndex);
            if (File.Exists(oldPath))
                File.Move(oldPath, newPath);
        }
        private void RenameProductImages(int oldProductId, int productId, int imagesCount)
        {
            if (imagesCount == 0)
                return;

            // přesun miniatury
            string oldThumbnailPath = GetThumbnailFileName(oldProductId);
            string newThumbnailPath = GetThumbnailFileName(productId);

            if (File.Exists(oldThumbnailPath))
                File.Move(oldThumbnailPath, newThumbnailPath);

            // přesun obrázků
            for (int i = 0; i < imagesCount; i++)
            {
                RenameImage(oldProductId, i, productId, i);
            }
        }
        public void SaveProductImages(Product product, List<IFormFile> images, int? oldProductID, int? oldImagesCount)
        {
            if (images == null)
                throw new Exception("Nepodařilo se nahrát obrázky!");

            int imagesCount = 0;

            // Přejmenování starých obrázků, pokud se změnilo ID produktu
            if (oldProductID.HasValue)
            {
                imagesCount = oldImagesCount.Value;
                RenameProductImages(oldProductID.Value, product.ProductId, imagesCount);
            }

            // nahrajeme další obrázky k produktu
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i] == null ||
                    !images[i].ContentType.ToLower().Contains("image"))
                    continue;

                imageManager.SaveImage(
                    images[i],
                    GetImageFileName(product.ProductId, i, full: false),
                    ImageManager.ImageExtension.Jpeg,
                    ProductImageMaxWidth, ProductImageMaxHeight
                                              );
                // první obrázek uložíme také jako miniaturu
                if (imagesCount == 0)
                {
                    imageManager.SaveImage(images[i],
                                           GetThumbnailFileName(product.ProductId, full: false),
                                           ImageManager.ImageExtension.Png,
                                           ProductImageMaxWidth, ProductImageMaxHeight
                                          );
                }

                imagesCount++;
            }

            product.ImagesCount = imagesCount;
            productRepository.Update(product);
        }
        public void RemoveProductImage(int productId, int imageIndex)
        {
            var product = productRepository.FindById(productId);

            if (imageIndex == 0)  // Pokud je to první obrázek, mažeme i miniaturu
            {
                RemoveThumbnailFile(productId);

                string secondImagePath = GetImageFileName(productId, 1); // Snažíme se vytvořit novou miniaturu z druhého obrázku
                if (File.Exists(secondImagePath))
                {
                    string thumbnailFileName = GetThumbnailFileName(product.ProductId);
                    imageManager.ResizeImage(thumbnailFileName, ProductThumbnailSize);
                }
            }

            // Mažeme obrázek
            RemoveImageFile(productId, imageIndex);

            // Přejmenování zbylých obrázků tak, aby šly za sebou
            for (int i = imageIndex + 1; i < product.ImagesCount; i++)
            {
                RenameImage(productId, i, productId, i - 1);
            }

            // Aktualizace počtu obrázků u produktu
            product.ImagesCount--;
            productRepository.Update(product);
        }
    }    
}
