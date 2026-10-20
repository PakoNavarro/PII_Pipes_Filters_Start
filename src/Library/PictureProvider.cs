//------------------------------------------------------------------------------
// <copyright file="PictureProvider.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.IO;
using SkiaSharp;

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// Esta clase permite leer y guardar imágenes desde archivos.
    /// </summary>
    public class PictureProvider
    {
        /// <summary>
        /// Lee una imagen desde un archivo.
        /// </summary>
        /// <param name="path">El path del archivo desde el cual leer la imagen.</param>
        /// <returns>La imagen leída.</returns>
        public IPicture GetPicture(string path)
        {
            Picture picture = new Picture(1, 1);
            using (SKBitmap image = SKBitmap.Decode(path))
            {
                picture.Resize(image.Width, image.Height);
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        SKColor color = image.GetPixel(x, y);
                        picture.SetColor(
                            x,
                            y,
                            System.Drawing.Color.FromArgb(
                                color.Alpha, color.Red, color.Green, color.Blue));
                    }
                }
            }

            return picture;
        }

        /// <summary>
        /// Guarda una imagen en un archivo. El formato se determina a partir de
        /// la extensión del archivo.
        /// </summary>
        /// <param name="picture">La imagen a guardar.</param>
        /// <param name="path">El path del archivo donde guardar la
        /// imagen.</param>
        public void SavePicture(IPicture picture, string path)
        {
            if (picture == null)
            {
                return;
            }

            int width = picture.Width;
            int height = picture.Height;
            SKImageInfo info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            using (SKBitmap image = new SKBitmap(info))
            {
                for (int y = 0; y < picture.Height; y++)
                {
                    for (int x = 0; x < picture.Width; x++)
                    {
                        System.Drawing.Color c = picture.GetColor(x, y);
                        image.SetPixel(x, y, new SKColor(c.R, c.G, c.B, c.A));
                    }
                }

                using (SKFileWStream stream = new SKFileWStream(path))
                {
                    image.Encode(stream, GetFormat(path), 100);
                }
            }
        }

        private static SKEncodedImageFormat GetFormat(string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".png":
                    return SKEncodedImageFormat.Png;
                case ".webp":
                    return SKEncodedImageFormat.Webp;
                default:
                    return SKEncodedImageFormat.Jpeg;
            }
        }
    }
}
