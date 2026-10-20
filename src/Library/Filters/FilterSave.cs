//------------------------------------------------------------------------------
// <copyright file="FilterSave.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro que guarda en disco la imagen recibida y la retorna sin
    /// modificarla. Permite persistir el resultado de cualquier paso intermedio
    /// de una secuencia de pipes &amp; filters.
    /// </summary>
    public class FilterSave : IFilter
    {
        private readonly PictureProvider provider = new PictureProvider();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FilterSave"/>.
        /// </summary>
        /// <param name="path">El path del archivo donde se guarda la imagen. El
        /// formato se determina a partir de la extensión.</param>
        public FilterSave(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("El path no puede ser vacío.", nameof(path));
            }

            this.Path = path;
        }

        /// <summary>
        /// Obtiene el path del archivo donde se guarda la imagen.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Guarda la imagen recibida en <see cref="Path"/> y la retorna sin
        /// cambios.
        /// </summary>
        /// <param name="image">La imagen a guardar.</param>
        /// <returns>La misma imagen recibida.</returns>
        public IPicture Filter(IPicture image)
        {
            if (image == null)
            {
                return null;
            }

            this.provider.SavePicture(image, this.Path);
            return image;
        }
    }
}
