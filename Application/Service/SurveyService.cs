using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;

namespace Application.Service
{
    public class SurveyService
    {
        private readonly string _pathFile;

        public SurveyService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void ReadSurvey()
        {
            try
            {
                var records = ReadCsv.ReadFile<Survey>(_pathFile);

                if (records == null || !records.Any())
                {
                    Console.WriteLine("CSV de encuestas vacío o no leído correctamente.");
                    return;
                }

                // Map de clasificaciones como string (IdClasificacion es VARCHAR en DB)
                var clasificacionMap = new Dictionary<string, string>
                {
                    { "Positiva", "1" },
                    { "Neutra", "2" },
                    { "Negativa", "3" }
                };

                using (var context = new DContext())
                {
                    // Insertar clasificaciones si no existen
                    foreach (var kvp in clasificacionMap)
                    {
                        string idClasificacion = kvp.Value;

                        if (!context.ClasificacionOpinion.Any(c => c.IdClasificacion == idClasificacion))
                        {
                            context.ClasificacionOpinion.Add(new ClasificacionOpinion
                            {
                                IdClasificacion = idClasificacion,
                                ClasificacionNombre = kvp.Key
                            });
                        }
                    }
                    context.SaveChanges();

                    // Procesar registros del CSV
                    foreach (var r in records)
                    {
                        if (r.PuntajeSatisfaccion < 1 || r.PuntajeSatisfaccion > 5)
                        {
                            Console.WriteLine($"⚠ Puntaje inválido en ID {r.IdOpinion}: {r.PuntajeSatisfaccion} → asignando valor por defecto 3");
                            r.PuntajeSatisfaccion = 3;
                        }

                        // Mapear ClasificacionNombre a IdClasificacion, usar "2" (Neutra) por defecto
                        if (clasificacionMap.TryGetValue(r.ClasificacionNombre, out var id))
                        {
                            r.IdClasificacion = id;
                        }
                        else
                        {
                            r.IdClasificacion = "2"; // Neutra por defecto
                            r.ClasificacionNombre = "Neutra";
                        }
                    }

                    // Insertar solo nuevas encuestas
                    var existingIds = context.Survey.Select(s => s.IdOpinion).ToHashSet();
                    var newSurveys = records.Where(r => !existingIds.Contains(r.IdOpinion)).ToList();

                    if (newSurveys.Any())
                    {
                        context.Survey.AddRange(newSurveys);
                        context.SaveChanges();
                        Console.WriteLine($"{newSurveys.Count} encuestas insertadas correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Todas las encuestas ya existen en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer archivo: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }
}
