using Application.Service;
using System;
using System.IO;

namespace OpinionesClientes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ClienteService clienteService = new ClienteService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\clients.csv");
                ProductsService productsService = new ProductsService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\products.csv");
                FuenteDatosService fuenteDatosService = new FuenteDatosService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\fuente_datos.csv");
                SocialCommentsService socialCommentsService = new SocialCommentsService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\social_comments.csv");
                SurveyService surveyService = new SurveyService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\surveys_part1.csv");
                WebReviewsService webReviewsService = new WebReviewsService(@"C:\Users\TOSHIBA\Desktop\ElectivaBigData\web_reviews.csv");

                clienteService.SaveClientesToDatabase();
                productsService.ReadProducts();
                fuenteDatosService.ReadFuenteDatos();
                socialCommentsService.ReadSocialComments();
                surveyService.ReadSurvey();
                webReviewsService.SaveWebReviewsToDatabase();
            }
            catch (FileNotFoundException fex)
            {
                Console.WriteLine($"Error: {fex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }

            Console.ReadKey();
        }
    }
}
