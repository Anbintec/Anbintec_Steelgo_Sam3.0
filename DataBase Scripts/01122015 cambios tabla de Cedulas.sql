ALTER TABLE Sam3_Cedula
DROP CONSTRAINT [UQ__Sam3_Ced__218A5B7FE23F4AD7]

--Creo tabla equivalencias espesor
CREATE TABLE Sam3_EquivalenciaEspesor
(
EquivalenciaEspesorID int Primary Key NOT NULL Identity(1,1),
Sam2_EspesorID int NOT NULL,
Sam3_EspesorID int NOT NULL,
Activo bit NOT NULL,
FechaModificacion datetime,
UsuarioModificacion int
)

--Creo tabla Espesor en Sam 3
CREATE TABLE Sam3_Espesor
(
	EspesorID int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	DiametroID int NULL,
	CedulaID int NULL,
	Valor decimal(10, 4) NOT NULL,
	Activo int NOT NULL,
	FechaModificacion datetime,
	UsuarioModificacion int
)

--Eliminar llave foranea de cedulas
ALTER TABLE Sam3_ItemCodeSteelgo
DROP CONSTRAINT [FK__Sam3_Item__Cedul__4341E1B1]

--Eliminar tabla de cedulas
DROP TABLE Sam3_Cedula

--Crear tabla de Cedula Sam 3
CREATE TABLE Sam3_Cedula(
	[CedulaID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Orden] [int] NULL,
	[VerificadoPorCalidad] [bit] NOT NULL CONSTRAINT [DF_Cedula_VerificadoPorCalidad]  DEFAULT ((0)),
	Activo bit NOT NULL,
	UsuarioModificacion int,
	FechaModificacion datetime
	)

	--Creo tabla equivalencias Cedulas
	CREATE TABLE Sam3_EquivalenciaCedula
	(
	EquivalenciaCedulaID int IDENTITY(1,1) Primary Key NOT NULL,
	Sam2_CedulaID int NOT NULL,
	Sam3_CedulaID int NOT NULL,
	Activo bit NOT NULL,
	UsuarioModificacion int,
	FechaModificacion datetime 
	)

--insertar datos en cedula
INSERT INTO Sam3_Cedula (Codigo, VerificadoPorCalidad, Orden, Activo)
SELECT Codigo, VerificadoPorCalidad, Orden, 1 FROM sam2.sam.dbo.Cedula 

--agrego Equivalencias de Cedulas
INSERT INTO Sam3_EquivalenciaCedula (Sam2_CedulaID, Sam3_CedulaID, Activo)
SELECT s2c.CedulaID, s3c.CedulaID, 1 FROM Sam3_Cedula s3c
INNER JOIN sam2.sam.dbo.Cedula s2c on s3c.Codigo = s2c.Codigo

--Agrego datos de espesor en Sam 3
INSERT INTO Sam3_Espesor (DiametroID, CedulaID, Valor, Activo)
SELECT di.Sam3_DiametroID, c.Sam3_CedulaID, Valor, 1 FROM sam2.sam.dbo.Espesor es
INNER JOIN Sam3_EquivalenciaDiametro di on es.DiametroID = di.Sam2_DiametroID 
INNER JOIN Sam3_EquivalenciaCedula c on es.CedulaID = c.Sam2_CedulaID


DBCC CHECKIDENT ('Sam3_EquivalenciaEspesor', RESEED, 0);
--Agrego equivalencias de espesor
INSERT INTO Sam3_EquivalenciaEspesor (Sam2_EspesorID, Sam3_EspesorID, Activo)
SELECT a.EspesorID, b.EspesorID, 1 FROM sam2.sam.dbo.Espesor a
INNER JOIN Sam3_Espesor b on a.EspesorID = b.EspesorID


--Crear tabla de catalogo de cedulas

CREATE TABLE Sam3_CatalogoCedulas
  (
  CatalogoCedulasID int Primary Key Identity(1,1),
  DiametroID int FOREIGN KEY REFERENCES Sam3_Diametro(DiametroID),
  CedulaA int,
  CedulaB int,
  CedulaC int,
  EspesorIn decimal(7,4) NOT NULL,
  EspesorID int NOT NULL FOREIGN KEY REFERENCES Sam3_Espesor(EspesorID),
  Activo bit NOT NULL,
  UsuarioModificacion int,
  FechaModificacion datetime
  )

  --Insertar una cedula
INSERT INTO Sam3_CatalogoCedulas VALUES (16, NULL, 6, 105, 685.0541 , 1, 1, NULL, NULL)


UPDATE Sam3_ItemCodeSteelgo
SET CedulaID = 1

--Agrego llave foranea para ICS
ALTER TABLE [dbo].Sam3_ItemCodeSteelgo  WITH CHECK ADD FOREIGN KEY(CedulaID)
REFERENCES [dbo].Sam3_CatalogoCedulas (CatalogoCedulasID)
GO
