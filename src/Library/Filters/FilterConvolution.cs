//------------------------------------------------------------------------------
// <copyright file="FilterConvolution.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Clase base para los filtros de convolución. Calcula el color de cada
    /// pixel a partir de los colores de sus vecinos, ponderados por un kernel
    /// de 3x3, divididos entre un divisor y sumados a un complemento. Ver
    /// https://en.wikipedia.org/wiki/Kernel_(image_processing).
    /// </summary>
    /// <remarks>
    /// Para crear un nuevo filtro de convolución basta con heredar de esta
    /// clase y pasar al constructor el kernel, el divisor y el complemento. El
    /// algoritmo de convolución se reutiliza sin cambios.
    /// </remarks>
    public abstract class FilterConvolution : IFilter
    {
        private const int KernelSize = 3;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterConvolution"/>.
        /// </summary>
        /// <param name="kernel">El kernel de 3x3 del filtro. El elemento
        /// <c>kernel[x, y]</c> es el peso del vecino ubicado en la columna
        /// <c>x</c> y la fila <c>y</c>; el centro <c>kernel[1, 1]</c> es el peso
        /// del pixel que se filtra.</param>
        /// <param name="divider">El valor entre el cual se divide la suma
        /// ponderada. No puede ser cero.</param>
        /// <param name="complement">El valor que se suma luego de
        /// dividir.</param>
        protected FilterConvolution(int[,] kernel, int divider, int complement)
        {
            ArgumentNullException.ThrowIfNull(kernel);

            if (kernel.GetLength(0) != KernelSize || kernel.GetLength(1) != KernelSize)
            {
                throw new ArgumentException("El kernel debe ser de 3x3.", nameof(kernel));
            }

            if (divider == 0)
            {
                throw new ArgumentException("El divisor no puede ser cero.", nameof(divider));
            }

            this.Kernel = (int[,])kernel.Clone();
            this.Divider = divider;
            this.Complement = complement;
        }

        /// <summary>
        /// Obtiene o asigna el kernel del filtro.
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
        /// Procesa la imagen pasada por parámetro mediante el kernel, y retorna
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

        private static int Normalize(int value)
        {
            return Math.Min(255, Math.Abs(value));
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

            redFinal = Normalize((redFinal / this.Divider) + this.Complement);
            greenFinal = Normalize((greenFinal / this.Divider) + this.Complement);
            blueFinal = Normalize((blueFinal / this.Divider) + this.Complement);

            return Color.FromArgb(redFinal, greenFinal, blueFinal);
        }

        private Color[,] CreateSample(IPicture image, int x, int y)
        {
            Color[,] sample = new Color[KernelSize, KernelSize];

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
