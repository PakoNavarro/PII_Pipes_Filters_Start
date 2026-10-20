//------------------------------------------------------------------------------
// <copyright file="FilterSharpenConvolution.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro de convolución que retorna la imagen recibida con los bordes
    /// más nítidos. Basado en https://en.wikipedia.org/wiki/Kernel_(image_processing).
    /// </summary>
    public class FilterSharpenConvolution : FilterConvolution
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterSharpenConvolution"/>.
        /// </summary>
        public FilterSharpenConvolution()
            : base(
                new int[,]
                {
                    { 0, -1, 0 },
                    { -1, 5, -1 },
                    { 0, -1, 0 },
                },
                1,
                0)
        {
        }
    }
}
