//------------------------------------------------------------------------------
// <copyright file="PipeFork.cs" company="Universidad Católica del Uruguay">
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
    /// Un pipe que recibe una imagen, la clona, y luego envía la original por
    /// un pipe y la clonada por otro.
    /// </summary>
    public class PipeFork : IPipe
    {
        private IPipe next2Pipe;
        private IPipe nextPipe;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PipeFork"/>.
        /// </summary>
        /// <param name="nextPipe">Siguiente pipe con filtro.</param>
        /// <param name="next2Pipe">Siguiente pipe sin filtro.</param>
        public PipeFork(IPipe nextPipe, IPipe next2Pipe)
        {
            this.next2Pipe = next2Pipe;
            this.nextPipe = nextPipe;
        }

        /// <summary>
        /// El pipe recibe una imagen, construye un duplicado,
        /// y envía la original por un pipe y el duplicado por otro.
        /// </summary>
        /// <param name="picture">imagen a filtrar y enviar a los siguientes
        /// pipes.</param>
        /// <returns>La imagen retornada por el pipe al que se envió la imagen original, o <c>null</c> si
        /// <paramref name="picture"/> es <c>null</c>.</returns>
        public IPicture Send(IPicture picture)
        {
            if (picture == null)
            {
                return null;
            }

            this.next2Pipe.Send(picture.Clone());
            return this.nextPipe.Send(picture);
        }
    }
}
