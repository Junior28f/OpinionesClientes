using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class FuenteDatosService
    {
        private readonly string _pathFile;

        public FuenteDatosService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void ReadFuenteDatos()
        {
            try
            {
                var records = ReadCsv.ReadFile<FuenteDatos>(_pathFile);

                if (records == null || !records.Any())
                {
                    Console.WriteLine("CSV de FuenteDatos vacío o no leído correctamente.");
                    return;
                }

                using (var context = new DContext())
                {
                    // IDs existentes en la tabla FuenteDatos
                    var existingIds = context.FuenteDatos
                        .Select(f => f.IdFuente)
                        .ToHashSet();

                    // IDs válidos de TipoFuente (FK)
                    var validTipoFuenteIds = context.TipoFuente
                        .Select(t => t.IdTipoFuente)
                        .ToHashSet();

                    // Filtrar registros nuevos y válidos
                    var newRecords = records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(r.IdFuente) &&             // no vacío
                            !existingIds.Contains(r.IdFuente) &&                  // no duplicado
                            validTipoFuenteIds.Contains(r.IdTipoFuente))          // FK válida
                        .Select(r => new FuenteDatos
                        {
                            IdFuente = r.IdFuente,
                            NombreFuente = r.NombreFuente,
                            IdTipoFuente = r.IdTipoFuente
                        })
                        .ToList();

                    if (newRecords.Any())
                    {
                        context.FuenteDatos.AddRange(newRecords);
                        context.SaveChanges();
                        Console.WriteLine($"{newRecords.Count} registros de FuenteDatos guardados en la base de datos.");
                    }
                    else
                    {
                        Console.WriteLine("No hay nuevos registros válidos para insertar en FuenteDatos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando FuenteDatos: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }
}
