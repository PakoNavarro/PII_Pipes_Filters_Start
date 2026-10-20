//------------------------------------------------------------------------------
// <copyright file="PipeSerial.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucu.Poo.PipesAndFilters;

namespace Ucu.Poo.PipesAndFilters.Pipes
{
    /// <summary>
    /// un pipe que recibe una imagen, le aplica un filtro y la envía a la siguiente pipe.
    /// </summary>
    public class PipeSerial : IPipe
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PipeSerial"/>. Esta pipe recibe una imagen, le aplica un filtro y la
        /// envía a la siguiente pipe.
        /// </summary>
        /// <param name="filter">El filtro que se debe aplicar sobre la imagen.</param>
        /// <param name="nextPipe">La siguiente pipe.</param>
        public PipeSerial(IFilter filter, IPipe nextPipe)
        {
            this.NextPipe = nextPipe;
            this.Filter = filter;
        }

        /// <summary>
        /// Obtiene la próxima pipe.
        /// </summary>
        public IPipe NextPipe { get; protected set; }

        /// <summary>
        /// Obtiene el filtro que aplica esta pipe.
        /// </summary>
        public IFilter Filter { get; protected set; }

        /// <summary>
        /// Recibe una imagen, le aplica un filtro y la envía al siguiente pipe.
        /// </summary>
        /// <param name="picture">Imagen a la cual se debe aplicar el filtro.</param>
        /// <returns>La imagen retornada por el siguiente pipe.</returns>
        public IPicture Send(IPicture picture)
        {
            picture = this.Filter.Filter(picture);
            return this.NextPipe.Send(picture);
        }
    }
}
