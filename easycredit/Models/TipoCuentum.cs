using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class TipoCuentum
    {
        public TipoCuentum()
        {
            Cuenta = new HashSet<Cuentum>();
        }

        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Cuentum> Cuenta { get; set; }
    }
}
