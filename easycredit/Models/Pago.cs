using System;
using System.Collections.Generic;

namespace easycredit.Models
{
    public partial class Pago
    {
        public int Id { get; set; }
        public double? CapitalInicial { get; set; }
        public DateTime? FechaPlanificada { get; set; }
        public DateTime? FechaEfectiva { get; set; }
        public double? Cuota { get; set; }
        public double? Interes { get; set; }
        public double? Amortizacion { get; set; }
        public int? Modalidad { get; set; }
        public string? CodigoComprobante { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaEditado { get; set; }
        public DateTime? FechaEliminado { get; set; }
        public int? UsuarioCreador { get; set; }
        public int? UsuarioEliminador { get; set; }
        public int? UsuarioEditor { get; set; }
        public bool? Active { get; set; }
        public double? Abono { get; set; }
        public int? CodigoPrestamo { get; set; }

        public virtual Prestamo? CodigoPrestamoNavigation { get; set; }
        public virtual ModalidadPago? ModalidadNavigation { get; set; }
    }
}
