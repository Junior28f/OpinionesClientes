using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class ClasificacionOpinionService
    {
        private readonly string _pathFile;
        private readonly DContext _context;

        public ClasificacionOpinionService(string pathFile, DContext context)
        {
            _pathFile = pathFile;
            _context = context;
        }

        public void SaveClasificacionesToDatabase()
        {
            try
            {
                var records = ReadCsv.ReadFile<ClasificacionOpinion>(_pathFile);

                if (records == null || !records.Any())
                {
                    Console.WriteLine("CSV de clasificaciones vacío o no leído correctamente.");
                    return;
                }

                // Clasificaciones existentes
                var existingNames = _context.ClasificacionOpinion
                    .Select(c => c.ClasificacionNombre)
                    .ToHashSet();

                // Solo insertar nuevos nombres
                var newRecords = records
                    .Where(r => !string.IsNullOrWhiteSpace(r.ClasificacionNombre) &&
                                !existingNames.Contains(r.ClasificacionNombre))
                    .Select(r => new ClasificacionOpinion
                    {
                        IdClasificacion = string.IsNullOrWhiteSpace(r.IdClasificacion)
                            ? Guid.NewGuid().ToString()   // generar ID si no viene del CSV
                            : r.IdClasificacion,
                        ClasificacionNombre = r.ClasificacionNombre
                    })
                    .ToList();

                if (newRecords.Any())
                {
                    _context.ClasificacionOpinion.AddRange(newRecords);
                    _context.SaveChanges();
                    Console.WriteLine($"{newRecords.Count} clasificaciones guardadas en la base de datos.");
                }
                else
                {
                    Console.WriteLine("No hay nuevas clasificaciones para insertar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar clasificaciones: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
            }
        }
    }
}
