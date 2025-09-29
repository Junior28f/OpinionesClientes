using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class SocialCommentsService
    {
        private readonly string _pathFile;

        public SocialCommentsService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void ReadSocialComments()
        {
            try
            {
                var commentsCsv = ReadCsv.ReadFile<SocialComments>(_pathFile);

                if (commentsCsv == null || !commentsCsv.Any())
                {
                    Console.WriteLine("CSV de comentarios vacío o no leído correctamente.");
                    return;
                }

                using (var context = new DContext())
                {
                    // IDs de clientes existentes en base de datos
                    var existingClientIds = context.Clientes
                        .Select(c => c.IdCliente.Trim())
                        .ToHashSet();

                    // Filtrar comentarios válidos con Cliente existente
                    var validComments = commentsCsv
                        .Where(c => !string.IsNullOrEmpty(c.IdCliente?.Trim())
                                    && existingClientIds.Contains(c.IdCliente.Trim()))
                        .Select(c =>
                        {
                            c.IdCliente = c.IdCliente.Trim();
                            c.IdProducto = c.IdProducto?.Trim();
                            return c;
                        })
                        .ToList();

                    if (!validComments.Any())
                    {
                        Console.WriteLine("No hay comentarios válidos para insertar (clientes no encontrados).");
                        return;
                    }

                    // Evitar duplicados usando combinación IdCliente + IdProducto + Fecha
                    var existingCommentsKeys = context.SocialComments
                        .Select(c => new { c.IdCliente, c.IdProducto, c.Fecha })
                        .AsEnumerable() // pasar a memoria
                        .Select(c => (c.IdCliente, c.IdProducto, c.Fecha)) // convertir a tupla
                        .ToHashSet();

                    var newComments = validComments
                        .Where(c => !existingCommentsKeys.Contains((c.IdCliente, c.IdProducto, c.Fecha)))
                        .ToList();

                    if (newComments.Any())
                    {
                        context.SocialComments.AddRange(newComments);
                        context.SaveChanges();
                        Console.WriteLine($"{newComments.Count} comentarios insertados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Todos los comentarios válidos ya existen en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sincronizando comentarios: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }
}
