using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteTipoClientes = new HashSet<ClienteTipoCliente>();
            Cuenta = new HashSet<Cuentum>();
            Inversions = new HashSet<Inversion>();
            PrestamoClientes = new HashSet<Prestamo>();
            PrestamoGarantes = new HashSet<Prestamo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Cedula { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<ClienteTipoCliente> ClienteTipoClientes { get; set; }
        public virtual ICollection<Cuentum> Cuenta { get; set; }
        public virtual ICollection<Inversion> Inversions { get; set; }
        public virtual ICollection<Prestamo> PrestamoClientes { get; set; }
        public virtual ICollection<Prestamo> PrestamoGarantes { get; set; }
    }
}
