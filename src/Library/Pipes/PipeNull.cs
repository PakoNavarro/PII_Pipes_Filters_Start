//------------------------------------------------------------------------------
// <copyright file="PipeNull.cs" company="Universidad Católica del Uruguay">
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
    /// un pipe que recibe una imagen, no hace nada con ella, y la retorna.
    /// </summary>
    public class PipeNull : IPipe
    {
        /// <summary>
        /// Retorna la imagen enviada a traves de El pipe.
        /// </summary>
        /// <param name="picture">La imagen a enviar.</param>
        /// <returns>La imagen enviada.</returns>
        public IPicture Send(IPicture picture)
        {
            return picture;
        }
    }
}
