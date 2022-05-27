using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class Garantium
    {
        public Garantium()
        {
            Prestamos = new HashSet<Prestamo>();
        }

        public int Id { get; set; }
        public string? Codigo { get; set; }
        public double? Valor { get; set; }
        public string? Ubicacion { get; set; }
        public int? TipoId { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual TipoGarantium? Tipo { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
