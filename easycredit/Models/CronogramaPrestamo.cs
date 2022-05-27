using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class CronogramaPrestamo
    {
        public int Id { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public DateTime? FechaPlanificada { get; set; }
        public int? PrestamoId { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual Prestamo? Prestamo { get; set; }
    }
}
