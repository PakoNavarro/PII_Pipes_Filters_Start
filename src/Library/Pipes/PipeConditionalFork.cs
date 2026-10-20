//------------------------------------------------------------------------------
// <copyright file="PipeConditionalFork.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.PipesAndFilters.Pipes
{
    /// <summary>
    /// Un pipe que aplica un filtro condicional a la imagen y, según el
    /// resultado del filtro, la envía por un pipe o por otro.
    /// </summary>
    public class PipeConditionalFork : IPipe
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="PipeConditionalFork"/>.
        /// </summary>
        /// <param name="filter">El filtro condicional que decide el
        /// camino.</param>
        /// <param name="truePipe">El pipe al que se envía la imagen si el
        /// resultado del filtro es <c>true</c>.</param>
        /// <param name="falsePipe">El pipe al que se envía la imagen si el
        /// resultado del filtro es <c>false</c>.</param>
        public PipeConditionalFork(IConditionalFilter filter, IPipe truePipe, IPipe falsePipe)
        {
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentNullException.ThrowIfNull(truePipe);
            ArgumentNullException.ThrowIfNull(falsePipe);

            this.Filter = filter;
            this.TruePipe = truePipe;
            this.FalsePipe = falsePipe;
        }

        /// <summary>
        /// Obtiene el filtro condicional que decide el camino.
        /// </summary>
        public IConditionalFilter Filter { get; private set; }

        /// <summary>
        /// Obtiene el pipe usado cuando el resultado del filtro es
        /// <c>true</c>.
        /// </summary>
        public IPipe TruePipe { get; private set; }

        /// <summary>
        /// Obtiene el pipe usado cuando el resultado del filtro es
        /// <c>false</c>.
        /// </summary>
        public IPipe FalsePipe { get; private set; }

        /// <summary>
        /// Aplica el filtro condicional a la imagen y la envía al pipe que
        /// corresponde según el resultado.
        /// </summary>
        /// <param name="picture">La imagen a procesar.</param>
        /// <returns>La imagen retornada por el pipe elegido.</returns>
        public IPicture Send(IPicture picture)
        {
            picture = this.Filter.Filter(picture);
            if (this.Filter.Result)
            {
                return this.TruePipe.Send(picture);
            }

            return this.FalsePipe.Send(picture);
        }
    }
}
