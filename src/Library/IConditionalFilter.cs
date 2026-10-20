//------------------------------------------------------------------------------
// <copyright file="IConditionalFilter.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// Un filtro que, además de procesar la imagen, deja disponible un
    /// resultado lógico de su última ejecución. Lo usan los pipes que deciden
    /// por qué camino seguir según ese resultado.
    /// </summary>
    public interface IConditionalFilter : IFilter
    {
        /// <summary>
        /// Obtiene el resultado de la última ejecución de <see
        /// cref="IFilter.Filter(IPicture)"/>.
        /// </summary>
        bool Result { get; }
    }
}
