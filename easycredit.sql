
create database easycredit;

use easycredit;

create table tipoUsuario(
id int PRIMARY KEY IDENTITY(1,1),
tipo VARCHAR(50),
descripcion VARCHAR(100),
	fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 
);


create table usuarios(
id int PRIMARY KEY IDENTITY(1,1),
usuario VARCHAR(50),
clave VARCHAR(50),
tipoID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 

FOREIGN KEY (tipoID) REFERENCES tipoUsuario(id)
);

create table tipoCliente(
	id int PRIMARY KEY IDENTITY(1,1),
	tipo VARCHAR(50),
	descripcion VARCHAR(100),
	fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 
);

create  table cliente(
id int PRIMARY KEY IDENTITY(1,1),
nombre VARCHAR(50),
apellido VARCHAR(50),
cedula VARCHAR(20),
direccion varchar(100),
telefono varchar(15),
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 
);

create table clienteTipoCliente(
id int PRIMARY KEY IDENTITY(1,1),
clienteID int,
tipoID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 

FOREIGN KEY (tipoID) REFERENCES tipoCliente(id),
FOREIGN KEY (clienteID) REFERENCES cliente(id)
);

create table tipoGarantia(
	id int PRIMARY KEY IDENTITY(1,1),
	tipo VARCHAR(50),
	descripcion VARCHAR(100),
	fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 
);

create table garantia(
id int primary key identity(1,1),
codigo varchar(30),
valor float,
ubicacion varchar(100),
tipoID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 

FOREIGN KEY (tipoID) REFERENCES tipoGarantia(id)
);

create table prestamo(
id int PRIMARY KEY IDENTITY(1,1),
codigo varchar(20),
monto float,
plazo int,
tazaInteres float,
fechaSolicitud Date,
fechaAprovacion Date,
fechaInicio Date,
fechaTermino Date,
clienteID int,
garanteID int ,
garantiaID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 ,

FOREIGN KEY (clienteID) REFERENCES cliente(id),
FOREIGN KEY (garanteID) REFERENCES cliente(id),
FOREIGN KEY (garantiaID) REFERENCES garantia(id)
);

create table inversion(
id int PRIMARY KEY IDENTITY(1,1),
codigo varchar(20),
monto float,
tazaInteres float,
plazo int,
fechaInicio Date,
fechaTermino Date,
clienteID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 ,
FOREIGN KEY (clienteID) REFERENCES cliente(id)
);

create table cronogramaPrestamo(
id int primary key identity(1,1),
fechaInicio date,
fechaTermino date,
fechaPlanificada date,
prestamoID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1,
FOREIGN KEY (prestamoID) REFERENCES prestamo(id)
);
create table cronogramaInversion(
id int primary key identity(1,1),
fechaInicio date,
fechaTermino date,
fechaPlanificada date,
inversionID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1,
FOREIGN KEY (inversionID) REFERENCES inversion(id)
);

create table modalidadPago(
id int primary key identity(1,1),
tipo varchar(60),
descripcion varchar(100),
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 

);

create table pago(
id int primary key identity(1,1),
capitalInicial float,
fechaPlanificada date,
fechaEfectiva date,
cuota float,
interes float,
amortizacion float,
modalidad int,
codigoComprobante varchar(30),
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1, 

FOREIGN KEY (modalidad) REFERENCES modalidadPago(id)
);

create table tipoCuenta(
id int primary key identity(1,1),
tipo varchar(60),
descripcion varchar(100),
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 

);

create table cuenta(
id int primary key identity(1,1),
banco varchar(50),
cuenta varchar(50),
tipoID int,
clienteID int,
fecha_creado datetime,
	fecha_editado datetime,
	fecha_eliminado datetime,
	usuario_creador int,
	usuario_eliminador int,
	usuario_editor int,
	active bit default 1 ,
FOREIGN KEY (tipoID) REFERENCES tipoCuenta(id),
FOREIGN KEY (clienteID) REFERENCES cliente(id)
);
