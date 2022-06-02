using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using easycredit.Models;

namespace easycredit.Data
{
    public partial class easycreditContext : DbContext
    {
        public easycreditContext()
        {
        }

        public easycreditContext(DbContextOptions<easycreditContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<ClienteTipoCliente> ClienteTipoClientes { get; set; } = null!;
        public virtual DbSet<CronogramaInversion> CronogramaInversions { get; set; } = null!;
        public virtual DbSet<CronogramaPrestamo> CronogramaPrestamos { get; set; } = null!;
        public virtual DbSet<Cuentum> Cuenta { get; set; } = null!;
        public virtual DbSet<Garantium> Garantia { get; set; } = null!;
        public virtual DbSet<Inversion> Inversions { get; set; } = null!;
        public virtual DbSet<ModalidadPago> ModalidadPagos { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<Prestamo> Prestamos { get; set; } = null!;
        public virtual DbSet<TipoCliente> TipoClientes { get; set; } = null!;
        public virtual DbSet<TipoCuentum> TipoCuenta { get; set; } = null!;
        public virtual DbSet<TipoGarantium> TipoGarantia { get; set; } = null!;
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellido");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.TipoId).HasColumnName("tipoID");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("Fk_tipoCliente");
            });

            modelBuilder.Entity<ClienteTipoCliente>(entity =>
            {
                entity.ToTable("clienteTipoCliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClienteId).HasColumnName("clienteID");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.TipoId).HasColumnName("tipoID");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.ClienteTipoClientes)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__clienteTi__clien__2B3F6F97");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.ClienteTipoClientes)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__clienteTi__activ__2A4B4B5E");
            });

            modelBuilder.Entity<CronogramaInversion>(entity =>
            {
                entity.ToTable("cronogramaInversion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.FechaPlanificada)
                    .HasColumnType("date")
                    .HasColumnName("fechaPlanificada");

                entity.Property(e => e.FechaTermino)
                    .HasColumnType("date")
                    .HasColumnName("fechaTermino");

                entity.Property(e => e.InversionId).HasColumnName("inversionID");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Inversion)
                    .WithMany(p => p.CronogramaInversions)
                    .HasForeignKey(d => d.InversionId)
                    .HasConstraintName("FK__cronogram__inver__4316F928");
            });

            modelBuilder.Entity<CronogramaPrestamo>(entity =>
            {
                entity.ToTable("cronogramaPrestamo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.FechaPlanificada)
                    .HasColumnType("date")
                    .HasColumnName("fechaPlanificada");

                entity.Property(e => e.FechaTermino)
                    .HasColumnType("date")
                    .HasColumnName("fechaTermino");

                entity.Property(e => e.PrestamoId).HasColumnName("prestamoID");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Prestamo)
                    .WithMany(p => p.CronogramaPrestamos)
                    .HasForeignKey(d => d.PrestamoId)
                    .HasConstraintName("FK__cronogram__prest__3F466844");
            });

            modelBuilder.Entity<Cuentum>(entity =>
            {
                entity.ToTable("cuenta");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Banco)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("banco");

                entity.Property(e => e.ClienteId).HasColumnName("clienteID");

                entity.Property(e => e.Cuenta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cuenta");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.TipoId).HasColumnName("tipoID");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__cuenta__clienteI__5165187F");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__cuenta__tipoID__5070F446");
            });

            modelBuilder.Entity<Garantium>(entity =>
            {
                entity.ToTable("garantia");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.TipoId).HasColumnName("tipoID");

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ubicacion");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.Property(e => e.Valor).HasColumnName("valor");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Garantia)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__garantia__active__31EC6D26");
            });

            modelBuilder.Entity<Inversion>(entity =>
            {
                entity.ToTable("inversion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClienteId).HasColumnName("clienteID");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.FechaTermino)
                    .HasColumnType("date")
                    .HasColumnName("fechaTermino");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.Property(e => e.Plazo).HasColumnName("plazo");

                entity.Property(e => e.TazaInteres).HasColumnName("tazaInteres");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Inversions)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__inversion__clien__3B75D760");
            });

            modelBuilder.Entity<ModalidadPago>(entity =>
            {
                entity.ToTable("modalidadPago");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("pago");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abono).HasColumnName("abono");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Amortizacion).HasColumnName("amortizacion");

                entity.Property(e => e.CapitalInicial).HasColumnName("capitalInicial");

                entity.Property(e => e.CodigoComprobante)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("codigoComprobante");

                entity.Property(e => e.CodigoPrestamo).HasColumnName("codigoPrestamo");

                entity.Property(e => e.Cuota).HasColumnName("cuota");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEfectiva)
                    .HasColumnType("date")
                    .HasColumnName("fechaEfectiva");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.FechaPlanificada)
                    .HasColumnType("date")
                    .HasColumnName("fechaPlanificada");

                entity.Property(e => e.Interes).HasColumnName("interes");

                entity.Property(e => e.Modalidad).HasColumnName("modalidad");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.CodigoPrestamoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.CodigoPrestamo)
                    .HasConstraintName("FK__pago__codigoPres__17036CC0");

                entity.HasOne(d => d.ModalidadNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.Modalidad)
                    .HasConstraintName("FK__pago__modalidad__49C3F6B7");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("prestamo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClienteId).HasColumnName("clienteID");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.FechaAprovacion)
                    .HasColumnType("date")
                    .HasColumnName("fechaAprovacion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.FechaSolicitud)
                    .HasColumnType("date")
                    .HasColumnName("fechaSolicitud");

                entity.Property(e => e.FechaTermino)
                    .HasColumnType("date")
                    .HasColumnName("fechaTermino");

                entity.Property(e => e.GaranteId).HasColumnName("garanteID");

                entity.Property(e => e.GarantiaId).HasColumnName("garantiaID");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.Property(e => e.Plazo).HasColumnName("plazo");

                entity.Property(e => e.Saldado)
                    .HasColumnName("saldado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TazaInteres).HasColumnName("tazaInteres");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.PrestamoClientes)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__prestamo__client__35BCFE0A");

                entity.HasOne(d => d.Garante)
                    .WithMany(p => p.PrestamoGarantes)
                    .HasForeignKey(d => d.GaranteId)
                    .HasConstraintName("FK__prestamo__garant__36B12243");

                entity.HasOne(d => d.Garantia)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.GarantiaId)
                    .HasConstraintName("FK__prestamo__garant__37A5467C");
            });

            modelBuilder.Entity<TipoCliente>(entity =>
            {
                entity.ToTable("tipoCliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");
            });

            modelBuilder.Entity<TipoCuentum>(entity =>
            {
                entity.ToTable("tipoCuenta");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");
            });

            modelBuilder.Entity<TipoGarantium>(entity =>
            {
                entity.ToTable("tipoGarantia");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.ToTable("tipoUsuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creado");

                entity.Property(e => e.FechaEditado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_editado");

                entity.Property(e => e.FechaEliminado)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_eliminado");

                entity.Property(e => e.TipoId).HasColumnName("tipoID");

                entity.Property(e => e.Usuario1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usuario");

                entity.Property(e => e.UsuarioCreador).HasColumnName("usuario_creador");

                entity.Property(e => e.UsuarioEditor).HasColumnName("usuario_editor");

                entity.Property(e => e.UsuarioEliminador).HasColumnName("usuario_eliminador");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__usuarios__active__06CD04F7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
