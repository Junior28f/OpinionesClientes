using Domain.Base;
using Domain.Entities;
using OpinionesClientes.Data;
using System;
using System.Linq;

namespace Application.Service
{
    public class ClienteService
    {
        private readonly string _pathFile;

        public ClienteService(string pathFile)
        {
            _pathFile = pathFile;
        }

        public void SaveClientesToDatabase()
        {
            try
            {
                // Leer el CSV directamente a objetos Cliente
                var clientesCsv = ReadCsv.ReadFile<Cliente>(_pathFile);

                if (clientesCsv == null || !clientesCsv.Any())
                {
                    Console.WriteLine("CSV de clientes vacío o no leído correctamente.");
                    return;
                }

                using (var context = new DContext())
                {
                    // Obtener los IdCliente y Emails ya existentes en la base
                    var clientesExistentesIds = context.Clientes
                        .Select(c => c.IdCliente)
                        .ToHashSet();

                    var clientesExistentesEmails = context.Clientes
                        .Select(c => c.Email)
                        .ToHashSet();

                    // Filtrar los nuevos clientes que no están en la base
                    var clientesNuevos = clientesCsv
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(r.IdCliente) &&    // Validar que tenga IdCliente
                            !string.IsNullOrWhiteSpace(r.Email) &&       // Validar que tenga Email
                            !clientesExistentesIds.Contains(r.IdCliente) && // No duplicar IdCliente
                            !clientesExistentesEmails.Contains(r.Email))   // No duplicar Email
                        .ToList();

                    // Auditar duplicados (ya existen en la base)
                    var clientesDuplicados = clientesCsv
                        .Where(r => clientesExistentesIds.Contains(r.IdCliente) || clientesExistentesEmails.Contains(r.Email))
                        .Select(r => $"IdCliente: {r.IdCliente}, Email: {r.Email}")
                        .Distinct()
                        .ToList();

                    if (clientesDuplicados.Any())
                    {
                        Console.WriteLine("Clientes ya existentes en la base:");
                        foreach (var cliente in clientesDuplicados)
                            Console.WriteLine(cliente);
                    }

                    if (clientesNuevos.Any())
                    {
                        context.Clientes.AddRange(clientesNuevos);
                        context.SaveChanges();
                        Console.WriteLine($"{clientesNuevos.Count} clientes insertados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Todos los clientes del CSV ya existen en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sincronizando clientes: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }
}
