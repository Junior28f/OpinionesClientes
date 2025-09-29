
using Domain.Base;

using Domain.Entities;
using OpinionesClientes.Data;

namespace Application.Service
{
    public class TipoFuentesService
    {
        private readonly DContext _context;

        public TipoFuentesService(DContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el IdTipoFuente correspondiente al nombre. Si no existe, lo crea.
        /// </summary>
        public int GetOrCreateTipoFuente(string nombreTipo)
        {
            if (string.IsNullOrWhiteSpace(nombreTipo))
                throw new ArgumentException("El nombre del tipo de fuente no puede estar vacío.");

            var tipo = _context.TipoFuente.FirstOrDefault(t => t.NombreTipo == nombreTipo);

            if (tipo != null)
                return tipo.IdTipoFuente;

            // Crear nuevo tipo
            tipo = new TipoFuente
            {
                NombreTipo = nombreTipo
            };

            _context.TipoFuente.Add(tipo);
            _context.SaveChanges();

            return tipo.IdTipoFuente;
        }
    }
}
