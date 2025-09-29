using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text; 

namespace Domain.Base
{
    public class ReadCsv
    {
        public static List<T> ReadFile<T>(string pathFile) where T : class
        {
            try
            {
                if (File.Exists(pathFile))
                {
                    // 👇 Usa Encoding.Latin1 o Encoding.UTF8 según tu CSV
                    using var reader = new StreamReader(pathFile, Encoding.Latin1);

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null,
                        MissingFieldFound = null
                    };

                    using var csv = new CsvReader(reader, config);

                    var records = csv.GetRecords<T>().ToList();
                    return records;
                }
                else
                {
                    throw new FileNotFoundException($"El archivo no existe: {pathFile}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                throw;
            }
        }
    }
}
