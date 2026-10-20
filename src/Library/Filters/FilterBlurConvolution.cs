//------------------------------------------------------------------------------
// <copyright file="FilterBlurConvolution.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro de convolución que retorna la imagen recibida con los bordes suavizados. Basado en
    /// https://en.wikipedia.org/wiki/Box_blur utilizando el kernel
    /// https://wikimedia.org/api/rest_v1/media/math/render/svg/91256bfeece3344f8602e288d445e6422c8b8a1c.
    /// </summary>
    public class FilterBlurConvolution : IFilter
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterBlurConvolution"/> asignando el kernel, complemento, y
        /// divisor según
        /// https://wikimedia.org/api/rest_v1/media/math/render/svg/91256bfeece3344f8602e288d445e6422c8b8a1c.
        /// </summary>
        public FilterBlurConvolution()
        {
            this.Kernel = new int[3, 3];
            this.Complement = 0;
            this.Divider = 9;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    this.Kernel[x, y] = 1;
                }
            }
        }

        /// <summary>
        /// El kernel del filtro.
        /// </summary>
        protected int[,] Kernel { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor que se suma a cada componente del color luego de dividirlo entre
        /// <see cref="Divider"/>.
        /// </summary>
        protected int Complement { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor entre el cual se divide la suma de los colores ponderados por el
        /// <see cref="Kernel"/>.
        /// </summary>
        protected int Divider { get; set; }

        /// <summary>
        /// Procesa la imagen pasada por parámetro mediante un kernel, y retorna
        /// la imagen resultante.
        /// </summary>
        /// <param name="image">La imagen a la cual se le va a aplicar el
        /// filtro.</param>
        /// <returns>La imagen con el filtro aplicado.</returns>
        public IPicture Filter(IPicture image)
        {
            if (image == null)
            {
                return null;
            }

            IPicture result = image.Clone();
            Color[,] sample;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    sample = this.CreateSample(image, x, y);
                    result.SetColor(x, y, this.GetFilteredColor(sample));
                }
            }

            return result;
        }

        private Color GetFilteredColor(Color[,] sample)
        {
            int redFinal = 0;
            int greenFinal = 0;
            int blueFinal = 0;

            for (int x = 0; x < sample.GetLength(0); x++)
            {
                for (int y = 0; y < sample.GetLength(1); y++)
                {
                    redFinal += sample[x, y].R * this.Kernel[x, y];
                    greenFinal += sample[x, y].G * this.Kernel[x, y];
                    blueFinal += sample[x, y].B * this.Kernel[x, y];
                }
            }

            redFinal = Math.Abs((redFinal / this.Divider) + this.Complement);
            redFinal = Math.Min(255, redFinal);

            greenFinal = Math.Abs((greenFinal / this.Divider) + this.Complement);
            greenFinal = Math.Min(255, greenFinal);

            blueFinal = Math.Abs((blueFinal / this.Divider) + this.Complement);
            blueFinal = Math.Min(255, blueFinal);

            return Color.FromArgb(redFinal, greenFinal, blueFinal);
        }

        private Color[,] CreateSample(IPicture image, int x, int y)
        {
            Color[,] sample = new Color[3, 3];

            sample[0, 0] = image.GetColor(Math.Max(x - 1, 0), Math.Max(y - 1, 0));
            sample[1, 0] = image.GetColor(x, Math.Max(y - 1, 0));
            sample[2, 0] = image.GetColor(Math.Min(x + 1, image.Width - 1), Math.Max(y - 1, 0));
            sample[0, 1] = image.GetColor(Math.Max(x - 1, 0), y);
            sample[1, 1] = image.GetColor(x, y);
            sample[2, 1] = image.GetColor(Math.Min(x + 1, image.Width - 1), y);
            sample[0, 2] = image.GetColor(Math.Max(x - 1, 0), Math.Min(y + 1, image.Height - 1));
            sample[1, 2] = image.GetColor(x, Math.Min(y + 1, image.Height - 1));
            sample[2, 2] = image.GetColor(Math.Min(x + 1, image.Width - 1), Math.Min(y + 1, image.Height - 1));

            return sample;
        }
    }
}