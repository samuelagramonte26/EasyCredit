using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class Prestamo
    {
        public Prestamo()
        {
            CronogramaPrestamos = new HashSet<CronogramaPrestamo>();
        }

        public int Id { get; set; }
        public string? Codigo { get; set; }
        public double? Monto { get; set; }
        public int? Plazo { get; set; }
        public double? TazaInteres { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaAprovacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int? ClienteId { get; set; }
        public int? GaranteId { get; set; }
        public int? GarantiaId { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual Cliente? Garante { get; set; }
        public virtual Garantium? Garantia { get; set; }
        public virtual ICollection<CronogramaPrestamo> CronogramaPrestamos { get; set; }
    }
}
