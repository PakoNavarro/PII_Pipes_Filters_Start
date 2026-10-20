//------------------------------------------------------------------------------
// <copyright file="FilterBlurConvolution.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro de convolución que retorna la imagen recibida con los bordes suavizados. Basado en
    /// https://en.wikipedia.org/wiki/Box_blur: cada pixel pasa a ser el promedio de sus vecinos.
    /// </summary>
    public class FilterBlurConvolution : FilterConvolution
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterBlurConvolution"/> con un kernel de unos, divisor 9 y
        /// complemento 0.
        /// </summary>
        public FilterBlurConvolution()
            : base(
                new int[,]
                {
                    { 1, 1, 1 },
                    { 1, 1, 1 },
                    { 1, 1, 1 },
                },
                9,
                0)
        {
        }
    }
}
