//------------------------------------------------------------------------------
// <copyright file="IPipe.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// un pipe a traves de la cual pasa una imagen.
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        /// Envía un imagen a traves de El pipe.
        /// </summary>
        /// <param name="picture">La imagen a enviar.</param>
        /// <returns>La imagen enviada.</returns>
        IPicture Send(IPicture picture);
    }
}
