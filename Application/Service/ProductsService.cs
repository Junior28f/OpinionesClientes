using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class ProductsService
    {
        private readonly string _pathFile;

        public ProductsService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void ReadProducts()
        {
            try
            {
                var productsCsv = ReadCsv.ReadFile<Products>(_pathFile);

                if (productsCsv == null || !productsCsv.Any())
                {
                    Console.WriteLine("CSV de productos vacío o no leído correctamente.");
                    return;
                }

                using (var context = new DContext())
                {
                    // Obtener los IDs de productos ya existentes
                    var productosExistentes = context.Products
                        .Select(p => p.IdProducto)
                        .ToHashSet();

                    // Filtrar productos que no estén en la base de datos
                    var productosNuevos = productsCsv
                        .Where(p => !productosExistentes.Contains(p.IdProducto))
                        .ToList();

                    // Auditar duplicados
                    var productosDuplicados = productsCsv
                        .Where(p => productosExistentes.Contains(p.IdProducto))
                        .Select(p => p.IdProducto)
                        .Distinct()
                        .ToList();

                    if (productosDuplicados.Any())
                    {
                        Console.WriteLine("Productos ya existentes en la base:");
                        foreach (var id in productosDuplicados)
                            Console.WriteLine($"IdProducto duplicado: {id}");
                    }

                    if (productosNuevos.Any())
                    {
                        context.Products.AddRange(productosNuevos);
                        context.SaveChanges();
                        Console.WriteLine($"{productosNuevos.Count} productos insertados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Todos los productos del CSV ya existen en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sincronizando productos: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }
}
