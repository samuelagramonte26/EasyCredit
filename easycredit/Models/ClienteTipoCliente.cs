using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class ClienteTipoCliente
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? TipoId { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual TipoCliente? Tipo { get; set; }
    }
}
