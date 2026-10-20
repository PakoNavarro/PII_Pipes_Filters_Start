//------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Ucu.Poo.Cognitive;
using Ucu.Poo.Discord;
using Ucu.Poo.PipesAndFilters.Filters;
using Ucu.Poo.PipesAndFilters.Pipes;

namespace Ucu.Poo.PipesAndFilters
{
    /// <summary>
    /// Programa principal.
    /// </summary>
    public static class Program
    {
        private const string LukePath = @"luke.jpg";

        private const string BeerPath = @"beer.jpg";

        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
        {
            PictureProvider provider = new PictureProvider();

            RunExercise1(provider);
            RunExercise2(provider);
            RunBonus(provider);

            // Los ejercicios 3 y 4 usan servicios externos; solo se ejecutan si
            // están configuradas las variables de entorno correspondientes.
            DiscordClient discord = null;
            ulong channelId = 0;
            if (!TryLoginDiscord(out discord, out channelId))
            {
                return;
            }

            try
            {
                RunExercise3(provider, discord, channelId);

                string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_FACE_SUBSCRIPTION_KEY");
                if (string.IsNullOrWhiteSpace(subscriptionKey))
                {
                    Console.WriteLine("Falta la variable de entorno 'AZURE_FACE_SUBSCRIPTION_KEY'; se omite el ejercicio 4.");
                }
                else
                {
                    CognitiveFace faceApi = new CognitiveFace(subscriptionKey.Trim('"'));
                    RunExercise4(provider, discord, channelId, faceApi);
                }
            }
            finally
            {
                discord.Dispose();
            }
        }

        /// <summary>
        /// Ejercicio 1: la secuencia del primer diagrama. La imagen pasa por
        /// escala de grises, luego por negativo y termina en un
        /// <see cref="PipeNull"/>. Los pipes se crean en orden inverso porque
        /// cada uno necesita conocer al siguiente.
        /// </summary>
        private static void RunExercise1(PictureProvider provider)
        {
            PipeNull pipeNull = new PipeNull();
            PipeSerial pipeNegative = new PipeSerial(new FilterNegative(), pipeNull);
            PipeSerial pipeGrayscale = new PipeSerial(new FilterGrayscale(), pipeNegative);

            IPicture picture = provider.GetPicture(LukePath);
            IPicture result = pipeGrayscale.Send(picture);

            provider.SavePicture(result, @"luke.ex1.jpg");
            Console.WriteLine("Ejercicio 1: imagen guardada en luke.ex1.jpg");
        }

        /// <summary>
        /// Ejercicio 2: la misma secuencia, pero guardando la imagen después de
        /// cada paso usando <see cref="FilterSave"/> como un filtro más.
        /// </summary>
        private static void RunExercise2(PictureProvider provider)
        {
            PipeNull pipeNull = new PipeNull();
            PipeSerial pipeSaveNegative = new PipeSerial(new FilterSave(@"luke.ex2.2-negative.jpg"), pipeNull);
            PipeSerial pipeNegative = new PipeSerial(new FilterNegative(), pipeSaveNegative);
            PipeSerial pipeSaveGrayscale = new PipeSerial(new FilterSave(@"luke.ex2.1-grayscale.jpg"), pipeNegative);
            PipeSerial pipeGrayscale = new PipeSerial(new FilterGrayscale(), pipeSaveGrayscale);

            IPicture picture = provider.GetPicture(LukePath);
            pipeGrayscale.Send(picture);

            Console.WriteLine("Ejercicio 2: pasos intermedios guardados en luke.ex2.*.jpg");
        }

        /// <summary>
        /// Ejercicio 3: igual que el ejercicio 2, pero publicando en Discord la
        /// imagen de cada paso usando <see cref="FilterPublishDiscord"/>.
        /// </summary>
        private static void RunExercise3(PictureProvider provider, DiscordClient discord, ulong channelId)
        {
            PipeNull pipeNull = new PipeNull();
            PipeSerial pipePublishNegative = new PipeSerial(
                new FilterPublishDiscord(discord, channelId, "Paso 2: negativo"),
                pipeNull);
            PipeSerial pipeNegative = new PipeSerial(new FilterNegative(), pipePublishNegative);
            PipeSerial pipePublishGrayscale = new PipeSerial(
                new FilterPublishDiscord(discord, channelId, "Paso 1: escala de grises"),
                pipeNegative);
            PipeSerial pipeGrayscale = new PipeSerial(new FilterGrayscale(), pipePublishGrayscale);

            IPicture picture = provider.GetPicture(LukePath);
            pipeGrayscale.Send(picture);

            Console.WriteLine("Ejercicio 3: imágenes publicadas en Discord");
        }

        /// <summary>
        /// Ejercicio 4: la secuencia del segundo diagrama. Después de la escala
        /// de grises, <see cref="PipeConditionalFork"/> usa
        /// <see cref="FilterHasFace"/> para decidir: si hay una cara, publica la
        /// imagen en Discord; si no, le aplica el negativo. Se ejecuta con una
        /// imagen con cara y otra sin cara para recorrer ambos caminos.
        /// </summary>
        private static void RunExercise4(
            PictureProvider provider, DiscordClient discord, ulong channelId, CognitiveFace faceApi)
        {
            // Camino si hay una cara: publicar en Discord.
            PipeNull pipeNullTrue = new PipeNull();
            PipeSerial pipePublish = new PipeSerial(
                new FilterPublishDiscord(discord, channelId, "¡Encontré una cara!"),
                pipeNullTrue);

            // Camino si no hay una cara: aplicar el negativo.
            PipeNull pipeNullFalse = new PipeNull();
            PipeSerial pipeNegative = new PipeSerial(new FilterNegative(), pipeNullFalse);

            PipeConditionalFork pipeHasFace = new PipeConditionalFork(
                new FilterHasFace(faceApi), pipePublish, pipeNegative);
            PipeSerial pipeGrayscale = new PipeSerial(new FilterGrayscale(), pipeHasFace);

            IPicture withFace = pipeGrayscale.Send(provider.GetPicture(LukePath));
            provider.SavePicture(withFace, @"luke.ex4.jpg");

            IPicture withoutFace = pipeGrayscale.Send(provider.GetPicture(BeerPath));
            provider.SavePicture(withoutFace, @"beer.ex4.jpg");

            Console.WriteLine("Ejercicio 4: resultados guardados en luke.ex4.jpg y beer.ex4.jpg");
        }

        /// <summary>
        /// Ejercicio bonus: los filtros de convolución heredan de
        /// <see cref="FilterConvolution"/> y solo definen su kernel.
        /// </summary>
        private static void RunBonus(PictureProvider provider)
        {
            PipeNull pipeNull = new PipeNull();
            PipeSerial pipeSaveEdges = new PipeSerial(new FilterSave(@"beer.bonus.3-edges.jpg"), pipeNull);
            PipeSerial pipeEdges = new PipeSerial(new FilterEdgeDetectionConvolution(), pipeSaveEdges);
            PipeSerial pipeSaveSharpen = new PipeSerial(new FilterSave(@"beer.bonus.2-sharpen.jpg"), pipeNull);
            PipeSerial pipeSharpen = new PipeSerial(new FilterSharpenConvolution(), pipeSaveSharpen);
            PipeSerial pipeSaveBlur = new PipeSerial(new FilterSave(@"beer.bonus.1-blur.jpg"), pipeNull);
            PipeSerial pipeBlur = new PipeSerial(new FilterBlurConvolution(), pipeSaveBlur);

            // PipeFork envía una copia de la imagen por cada camino, así cada
            // filtro se aplica sobre la imagen original.
            PipeFork pipeForkSharpenEdges = new PipeFork(pipeSharpen, pipeEdges);
            PipeFork pipeForkAll = new PipeFork(pipeBlur, pipeForkSharpenEdges);

            pipeForkAll.Send(provider.GetPicture(BeerPath));

            Console.WriteLine("Bonus: filtros de convolución guardados en beer.bonus.*.jpg");
        }

        private static bool TryLoginDiscord(out DiscordClient discord, out ulong channelId)
        {
            discord = null;
            channelId = 0;

            string botToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            string channelText = Environment.GetEnvironmentVariable("CHANNEL_ID");
            if (string.IsNullOrWhiteSpace(botToken) || string.IsNullOrWhiteSpace(channelText))
            {
                Console.WriteLine("Faltan las variables de entorno 'DISCORD_BOT_TOKEN' y 'CHANNEL_ID'; se omiten los ejercicios 3 y 4.");
                return false;
            }

            if (!ulong.TryParse(channelText.Trim('"'), out channelId))
            {
                Console.WriteLine("La variable de entorno 'CHANNEL_ID' no es un número válido; se omiten los ejercicios 3 y 4.");
                return false;
            }

            discord = new DiscordClient();
            discord.Login(botToken.Trim('"'));
            return true;
        }
    }
}
