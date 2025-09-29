using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class WebReviewsService
    {
        private readonly string _pathFile;

        public WebReviewsService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void SaveWebReviewsToDatabase()
        {
            try
            {
                var reviewsCsv = ReadCsv.ReadFile<WebReviews>(_pathFile);

                if (reviewsCsv == null || !reviewsCsv.Any())
                {
                    Console.WriteLine("CSV de reseñas vacío o no leído correctamente.");
                    return;
                }

                using (var context = new DContext())
                {
                    // Obtener los IDs de reseñas existentes
                    var existingIds = context.WebReviews
                        .Select(w => w.IdReview)
                        .ToHashSet();

                    // Filtrar las reseñas que no existen en la base de datos
                    var newReviews = reviewsCsv
                        .Where(r => !existingIds.Contains(r.IdReview))
                        .ToList();

                    if (newReviews.Any())
                    {
                        context.WebReviews.AddRange(newReviews);
                        context.SaveChanges();
                        Console.WriteLine($"{newReviews.Count} reseñas insertadas correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Todas las reseñas del CSV ya existen en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sincronizando reseñas: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }
}
