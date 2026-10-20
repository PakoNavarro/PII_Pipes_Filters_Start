//------------------------------------------------------------------------------
// <copyright file="FilterEdgeDetectionConvolution.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro de convolución que resalta los bordes de la imagen recibida y
    /// oscurece las zonas de color uniforme. Basado en
    /// https://en.wikipedia.org/wiki/Kernel_(image_processing).
    /// </summary>
    public class FilterEdgeDetectionConvolution : FilterConvolution
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterEdgeDetectionConvolution"/>.
        /// </summary>
        public FilterEdgeDetectionConvolution()
            : base(
                new int[,]
                {
                    { -1, -1, -1 },
                    { -1, 8, -1 },
                    { -1, -1, -1 },
                },
                1,
                0)
        {
        }
    }
}
