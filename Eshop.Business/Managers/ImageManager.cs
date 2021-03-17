using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Eshop.Business.Managers
{
    public class ImageManager : IImageManager
    {
        public enum ImageExtension { Bmp, Gif, Jpeg, Png }

        public string OutputDirectoryPath { get; private set; }

        public ImageManager(string outputDirectoryPath) => OutputDirectoryPath = outputDirectoryPath;

        private Image ResizeImage(Image image, int width = 0, int height = 0)
        {
            // System.Drawing defaultně zachová poměr stran, pokud je některý rozměr == 0
            if (width > 0 || height > 0)
            {
                Image newImage = new Bitmap(image, new Size(width, height));

                return newImage;
            }

            return image;
        }

        public void ResizeImage(string path, int width = 0, int height = 0)
        {
            Image image = Image.FromFile(path);
            ResizeImage(image, width, height);
        }

        /// <summary>
        /// Obalující metoda pro uložení obrázku ze vstupu.
        /// </summary>
        /// <param name="file">Nezpracovaný soubor s obrázkem k uložení.</param>
        /// <param name="fileName">Název souboru po uložení (bez přípony).</param>
        /// <param name="extension">Formát, ve kterém se má obrázek uložit.</param>
        /// <param name="width">Šířka obrázku po uložení (nepovinný parametr).</param>
        /// <param name="height">Výška obrázku po uložení (nepovinný parametr)</param>
        public void SaveImage(IFormFile file, string fileName, ImageExtension extension, int width = 0, int height = 0)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                Image image = Image.FromStream(stream);

                // System.Drawing defaultně zachová poměr stran, pokud je některý rozměr == 0
                Image newImage = ResizeImage(image, width, height);

                switch (extension)
                {
                    case ImageExtension.Bmp:
                        fileName += ".bmp";
                        break;

                    case ImageExtension.Gif:
                        fileName += ".gif";
                        break;

                    case ImageExtension.Jpeg:
                        fileName += ".jpeg";
                        break;

                    case ImageExtension.Png:
                        fileName += ".png";
                        break;

                    default:
                        return;
                }

                newImage.Save(OutputDirectoryPath + fileName);
            }
        }
    }
}
