//------------------------------------------------------------------------------
// <copyright file="IFilter.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// Un filtro que recibe, procesa y retorna imágenes. El filtro puede
    /// retornar la misma imagen o una nueva imagen creada por él.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Procesa la imagen pasada por parámetro y retorna la misma imagen o
        /// una nueva.
        /// </summary>
        /// <param name="image">La imagen a la cual se le va a aplicar el
        /// filtro.</param>
        /// <returns>La misma imagen o una nueva imagen creada por el
        /// filtro.</returns>
        IPicture Filter(IPicture image);
    }
}
