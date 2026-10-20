//------------------------------------------------------------------------------
// <copyright file="FilterHasFace.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.IO;
using Ucu.Poo.Cognitive;

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro condicional que detecta si la imagen contiene una cara. El
    /// resultado queda en <see cref="Result"/>: <c>true</c> si se encontró una
    /// cara y <c>false</c> en caso contrario.
    /// </summary>
    /// <remarks>
    /// Si se encuentra una cara, el filtro retorna la imagen con la cara
    /// marcada; si no, retorna la imagen recibida sin cambios.
    /// </remarks>
    public class FilterHasFace : IConditionalFilter
    {
        private const string RecognizedSuffix = "_r_";

        private readonly PictureProvider provider = new PictureProvider();

        private readonly CognitiveFace faceApi;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterHasFace"/>.
        /// </summary>
        /// <param name="faceApi">El servicio de reconocimiento de caras a
        /// utilizar.</param>
        public FilterHasFace(CognitiveFace faceApi)
        {
            ArgumentNullException.ThrowIfNull(faceApi);

            this.faceApi = faceApi;
        }

        /// <summary>
        /// Obtiene un valor que indica si la última imagen procesada contenía
        /// una cara.
        /// </summary>
        public bool Result { get; private set; }

        /// <summary>
        /// Detecta si la imagen recibida contiene una cara y deja el resultado
        /// en <see cref="Result"/>.
        /// </summary>
        /// <param name="image">La imagen a analizar.</param>
        /// <returns>La imagen con la cara marcada si se encontró una cara; la
        /// imagen recibida en caso contrario.</returns>
        public IPicture Filter(IPicture image)
        {
            this.Result = false;
            if (image == null)
            {
                return null;
            }

            // El servicio trabaja con archivos, así que guardamos la imagen en
            // un archivo temporal. Si encuentra la cara, genera otro archivo
            // con el sufijo "_r_" que contiene la cara marcada.
            string name = Guid.NewGuid().ToString();
            string directory = System.IO.Path.GetTempPath();
            string tempPath = System.IO.Path.Combine(directory, name + ".jpg");
            string markedPath = System.IO.Path.Combine(directory, name + RecognizedSuffix + ".jpg");

            try
            {
                this.provider.SavePicture(image, tempPath);
                CognitiveFace.RecognitionResult result = this.faceApi.Recognize(tempPath, true);
                this.Result = result.Success && result.FaceFound;

                if (this.Result && File.Exists(markedPath))
                {
                    return this.provider.GetPicture(markedPath);
                }

                return image;
            }
            finally
            {
                DeleteIfExists(tempPath);
                DeleteIfExists(markedPath);
            }
        }

        private static void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
