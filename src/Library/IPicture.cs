//------------------------------------------------------------------------------
// <copyright file="IPicture.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// Representa una imagen. Las imágenes son una matriz bidimensional de
    /// colores. Adicionalmente, se proveen operaciones para obtener las
    /// dimensiones de la imagen, modificar el tamaño de la imagen, y copiar la
    /// imagen a una nueva.
    /// </summary>
    public interface IPicture
    {
        /// <summary>
        /// Retorna el ancho de la imagen.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Retorna el alto de la imagen.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Retorna el color del punto localizado en la posición (x, y) de la
        /// imagen. La esquina superior izquierda de la matriz corresponde al
        /// punto (0,0) y el inferior derecho al (Width - 1, Height -1).
        /// </summary>
        /// <param name="x">La coordenada x del punto.</param>
        /// <param name="y">La coordenada y del punto.</param>
        /// <returns>El color del punto.</returns>
        Color GetColor(int x, int y);

        /// <summary>
        /// Establece el color del punto localizado en la posición (x, y) de la
        /// imagen. La esquina superior izquierda de la matriz corresponde al
        /// punto (0,0) y el inferior derecho al (Width - 1, Height -1).
        /// </summary>
        /// <param name="x">La coordenada x del punto.</param>
        /// <param name="y">La coordenada y del punto.</param>
        /// <param name="color">El color a usar para el punto.</param>
        void SetColor(int x, int y, Color color);

        /// <summary>
        /// Modifica el tamaño de la imagen.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Los colores de la imagen deben preservarse.
        /// </para>
        /// <para>
        /// Si la imagen tiene nuevos puntos, estos comienzan en color negro.
        /// </para>
        /// </remarks>
        /// <param name="width">El nuevo ancho de la imagen.</param>
        /// <param name="height">El nuevo largo de la imagen.</param>
        void Resize(int width, int height);

        /// <summary>
        /// Copia la imagen en una nueva imagen.
        /// </summary>
        /// <returns>Una copia de la imagen.</returns>
        IPicture Clone();
    }
}