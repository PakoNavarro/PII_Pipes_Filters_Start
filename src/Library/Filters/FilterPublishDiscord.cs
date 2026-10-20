//------------------------------------------------------------------------------
// <copyright file="FilterPublishDiscord.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.IO;
using Ucu.Poo.Discord;

namespace Ucu.Poo.PipesAndFilters.Filters
{
    /// <summary>
    /// Un filtro que publica en un canal de Discord la imagen recibida y la
    /// retorna sin modificarla.
    /// </summary>
    /// <remarks>
    /// El <see cref="DiscordClient"/> se recibe ya creado y con la sesión
    /// iniciada; este filtro no es responsable de crearlo ni de liberarlo.
    /// </remarks>
    public class FilterPublishDiscord : IFilter
    {
        private readonly PictureProvider provider = new PictureProvider();

        private readonly DiscordClient client;

        private readonly ulong channelId;

        private readonly string message;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see
        /// cref="FilterPublishDiscord"/>.
        /// </summary>
        /// <param name="client">El cliente de Discord, con la sesión ya
        /// iniciada.</param>
        /// <param name="channelId">El identificador del canal donde se publica
        /// la imagen.</param>
        /// <param name="message">El mensaje que acompaña a la imagen.</param>
        public FilterPublishDiscord(DiscordClient client, ulong channelId, string message)
        {
            ArgumentNullException.ThrowIfNull(client);

            this.client = client;
            this.channelId = channelId;
            this.message = message;
        }

        /// <summary>
        /// Publica la imagen recibida en el canal de Discord y la retorna sin
        /// cambios.
        /// </summary>
        /// <param name="image">La imagen a publicar.</param>
        /// <returns>La misma imagen recibida.</returns>
        public IPicture Filter(IPicture image)
        {
            if (image == null)
            {
                return null;
            }

            // Discord publica archivos, así que primero guardamos la imagen en
            // un archivo temporal.
            string tempPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".jpg");

            try
            {
                this.provider.SavePicture(image, tempPath);
                this.client.SendImage(this.channelId, tempPath, this.message);
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }

            return image;
        }
    }
}
