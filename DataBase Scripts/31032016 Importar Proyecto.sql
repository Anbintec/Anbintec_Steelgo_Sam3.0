------------------------------------------
------------------------------------------
------------------------------------------
---- CAMBIAR LOS VALORES INDICADOS CON ***
------------------------------------------
------------------------------------------
------------------------------------------
------------------------------------------
USE [steelgo-sam3]
BEGIN TRAN
BEGIN TRY

DECLARE @sam2_ProyectoID int,
		@sam2_PatioID int,
		@sam2_ColorID int,
		@sam2_ContactoID int,
		@sam2_ClienteID int,
		@sam3_ProyectoID int,
		@sam3_PatioID int,
		@sam3_ColorID int,
		@sam3_ContactoID int,
		@sam3_ClienteID int,
		@requierePermisoAduana bit,
		@requiereTipoPackingList bit

--------------------------------------------------------------------------------------------------------
--------------Inicializar variables
SET @sam2_ProyectoID = 1 --*** INDICAR EL ID (SAM 2) DEL PROYECTO QUE SE VA A IMPORTAR ***---
SET @requierePermisoAduana = 0 --*** INDICAR SI EL PROYECTO REQUIERE DE PERMISO DE ADUANA ***---
SET @requiereTipoPackingList = 0 --*** INDICAR SI EL PROYECTO REQUIERE TIPO DE PACKING LIST ***---

SET @sam2_ClienteID = ISNULL((
	SELECT ClienteID FROM SAM2.SAM.DBO.Proyecto
	WHERE ProyectoID = @sam2_ProyectoID
),0)

SET @sam2_ColorID = ISNULL((
	SELECT ColorID FROM SAM2.SAM.DBO.Proyecto
	WHERE ProyectoID = @sam2_ProyectoID
),0)

SET @sam2_ContactoID = ISNULL((
	SELECT ContactoID FROM SAM2.SAM.DBO.Proyecto
	WHERE ProyectoID = @sam2_ProyectoID
),0)

SET @sam2_PatioID = ISNULL((
	SELECT PatioID FROM SAM2.SAM.DBO.Proyecto
	WHERE ProyectoID = @sam2_ProyectoID
),0)
--------------------------------------------------------------------------------------------------------
-- insertar el patio
IF @sam2_PatioID > 0
BEGIN
	IF (SELECT Sam3_PatioID FROM Sam3_EquivalenciaPatio
		WHERE Sam2_PatioID = @sam2_PatioID) IS NULL
	BEGIN

		INSERT INTO [dbo].[Sam3_Patio]
				   ([Nombre]
				   ,[Propietario]
				   ,[Descripcion]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[RequierePermisoAduana])
			 SELECT
				   Nombre,
				   Propietario,
				   Descripcion,
				   1,
				   GETDATE(),
				   @requierePermisoAduana
		     FROM SAM2.SAM.DBO.Patio
			 WHERE PatioID = @sam2_PatioID

		SET @sam3_PatioID = SCOPE_IDENTITY()
		---- insertar la equivalencia del patio
		INSERT INTO Sam3_EquivalenciaPatio (Sam2_PatioID, Sam3_PatioID, Activo, FechaModificacion)
		VALUES (@sam2_PatioID, @sam3_PatioID, 1, GETDATE())

	END
	ELSE
		SET @sam3_PatioID = (
			SELECT Sam3_PatioID FROM Sam3_EquivalenciaPatio
			WHERE Sam2_PatioID = @sam2_PatioID
		)
END

--------------------------------------------------------------------------------------------------------
----------INSERTAR EL CLIENTE
IF @sam2_ClienteID > 0
BEGIN

	IF (SELECT ClienteID FROM Sam3_Cliente
		WHERE Sam2ClienteID = @sam2_ClienteID) IS NULL
	BEGIN
		INSERT INTO [dbo].[Sam3_Cliente]
				   ([Nombre]
				   ,[Direccion]
				   ,[Ciudad]
				   ,[Estado]
				   ,[Pais]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[Sam2ClienteID])
		 SELECT
			Nombre,
			Direccion,
			Ciudad,
			Estado,
			Pais,
			1,
			GETDATE(),
			ClienteID
		FROM SAM2.SAM.DBO.Cliente
		WHERE ClienteID = @sam2_ClienteID

		SET @sam3_ClienteID = SCOPE_IDENTITY()
	END
	ELSE
		SET @sam3_ClienteID = (
			SELECT ClienteID FROM Sam3_Cliente
			WHERE Sam2ClienteID = @sam2_ClienteID
		)
END

--------------------------------------------------------------------------------------------------------
----------INSERTAR CONTACTO
IF @sam2_ContactoID > 0
BEGIN
	IF (SELECT ContactoID FROM Sam3_Contacto
		WHERE Nombre = (
			SELECT Nombre FROM SAM2.SAM.DBO.Contacto
			WHERE ContactoID = @sam2_ContactoID
		)) IS NULL
	BEGIN
		INSERT INTO [dbo].[Sam3_Contacto]
				   ([Nombre]
				   ,[ApPaterno]
				   ,[ApMaterno]
				   ,[CorreoElectronico]
				   ,[TelefonoOficina]
				   ,[TelefonoParticular]
				   ,[TelefonoCelular]
				   ,[Activo]
				   ,[FechaModificacion])
			 SELECT
				   Nombre,
				   ApPaterno,
				   ApMaterno,
				   CorreoElectronico,
				   TelefonoOficina, 
				   TelefonoParticular,
				   TelefonoCelular,
				   1,
				   GETDATE()
			  FROM SAM2.SAM.DBO.Contacto
			  WHERE ContactoID = @sam2_ContactoID

		SET @sam3_ContactoID = SCOPE_IDENTITY()
	END 
	ELSE
		SET @sam3_ContactoID = (
			SELECT ContactoID FROM Sam3_Contacto
			WHERE Nombre = (
				SELECT Nombre FROM SAM2.SAM.DBO.Contacto
				WHERE ContactoID = @sam2_ContactoID
			)
			)
END 
--------------------------------------------------------------------------------------------------------
-----------INSERTAR COLOR
IF @sam2_ColorID > 0
BEGIN
	IF (SELECT ColorID FROM Sam3_Color
		WHERE Nombre = (
			SELECT Nombre FROM SAM2.SAM.DBO.COLOR
			WHERE ColorID = @sam2_ColorID
		)) IS NULL
	BEGIN
		INSERT INTO [dbo].[Sam3_Color]
				   ([Nombre]
				   ,[NombreIngles]
				   ,[CodigoHexadecimal]
				   ,[Activo]
				   ,[FechaModificacion])
			 SELECT
				   Nombre,
				   NombreIngles,
				   CodigoHexadecimal,
				   1,
				   GETDATE()
			  FROM SAM2.SAM.DBO.Color
			  WHERE ColorID = @sam2_ColorID

		SET @sam3_ColorID = SCOPE_IDENTITY()
	END
	ELSE
		SET @sam3_ColorID = (
			SELECT ColorID FROM Sam3_Color
			WHERE Nombre = (
				SELECT Nombre FROM SAM2.SAM.DBO.COLOR
				WHERE ColorID = @sam2_ColorID
		))
END
--------------------------------------------------------------------------------------------------------
--------INSERTAR EL PROYECTO
IF @sam2_ProyectoID > 0 AND @sam3_PatioID IS NOT NULL AND @sam3_ClienteID IS NOT NULL
	AND @sam3_ColorID IS NOT NULL AND @sam3_ContactoID IS NOT NULL
BEGIN
	IF (SELECT Sam3_proyectoID FROM Sam3_EquivalenciaProyecto
		WHERE Sam2_ProyectoID = @sam2_ProyectoID) IS NULL
	BEGIN
		INSERT INTO [dbo].[Sam3_Proyecto]
				   ([PatioID]
				   ,[ClienteID]
				   ,[ContactoID]
				   ,[ColorID]
				   ,[Nombre]
				   ,[Descripcion]
				   ,[FechaInicio]
				   ,[Activo]
				   ,[FechaModificacion])
			 SELECT
				   @sam3_PatioID,
				   @sam3_ClienteID,
				   @sam3_ContactoID,
				   @sam3_ColorID,
				   Nombre,
				   Descripcion,
				   FechaInicio,
				   1,
				   GETDATE()
			  FROM SAM2.SAM.DBO.Proyecto
			  WHERE ProyectoID = @sam2_ProyectoID
		
		SET @sam3_ProyectoID = SCOPE_IDENTITY()

		-----Insertamos la eqivalencia del proyecto
		INSERT INTO [dbo].[Sam3_EquivalenciaProyecto]
				   ([Sam2_ProyectoID]
				   ,[Sam3_ProyectoID]
				   ,[Activo]
				   ,[FechaModificacion])
			 VALUES
				   (@sam2_ProyectoID
				   ,@sam3_ProyectoID
				   ,1
				   ,GETDATE())

		-----Insertamos la configuracion del proyecto
		INSERT INTO [dbo].[Sam3_ProyectoConfiguracion]
				   ([ProyectoID]
				   ,[PrefijoNumeroUnico]
				   ,[PrefijoOrdenTrabajo]
				   ,[DigitosNumeroUnico]
				   ,[DigitosOrdenTrabajo]
				   ,[ToleranciaCortes]
				   ,[AnguloBisel]
				   ,[CuadroTubero]
				   ,[CuadroRaiz]
				   ,[CuadroRelleno]
				   ,[UsuarioModifica]
				   ,[FechaModificacion]
				   ,[ActualizaLocalizacion]
				   ,[CorreoPeqKgEsp]
				   ,[DigitosFolioPreparacion]
				   ,[ManejaReserva]
				   ,[Activo]
				   ,[TipoUsoID]
				   ,[RequiereTipoPackingList])
			 SELECT
				   @sam3_ProyectoID,
				   PrefijoNumeroUnico,
				   PrefijoOrdenTrabajo,
				   DigitosNumeroUnico,
				   DigitosOrdenTrabajo,
				   ToleranciaCortes,
				   AnguloBisel,
				   CuadroTubero,
				   CuadroRaiz,
				   CuadroRelleno, 
				   UsuarioModifica,
				   FechaModificacion,
				   ActualizaLocalizacion,
				   CorreoPeqKgEsp,
				   DigitosFolioPreparacion,
				   ManejaReserva,
				   1,
				   1, -- SE ESTABLECE COMO DEFAULT
				   @requiereTipoPackingList
			  FROM SAM2.SAM.DBO.ProyectoConfiguracion
			  WHERE ProyectoID = @sam2_ProyectoID
		
		------ Insertar los consecutivos del proyecto
		INSERT INTO [dbo].[Sam3_ProyectoConsecutivo]
				   ([ProyectoID]
				   ,[ConsecutivoFolioAvisollegada]
				   ,[ConsecutivoFolioEntradaMateiral]
				   ,[ConsecutivoFolioDescargaMaterial]
				   ,[ConsecutivoPaseSalida]
				   ,[ConsecutivoPermisoAduana]
				   ,[ConsucutivoFolioPackingList]
				   ,[ConsecutivoODT]
				   ,[ConsecutivoNumerounico]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[ConsecutivoOrdeRecepcion]
				   ,[ConsecutivoOrdenAlmacenaje])
			 SELECT
					@sam3_ProyectoID,
					0,
					0,
					0,
					0,
					0,
					0,
					ConsecutivoODT,
					ConsecutivoNumerounico,
					1,
					GETDATE(),
					0,
					0
			 FROM sam2.sam.dbo.ProyectoConsecutivo
			 WHERE ProyectoID = @sam2_ProyectoID
		
		----Insertar permiso para el usuario admin
		INSERT INTO [dbo].[Sam3_Rel_Usuario_Proyecto]
					([ProyectoID]
					,[UsuarioID]
					,[Activo]
					,[FechaModificacion])
		VALUES
			(@sam3_ProyectoID
			,1 -- Usuario Admin
			,1
			,GETDATE())
	END
END

	COMMIT TRAN
END TRY
BEGIN CATCH
	exec [dbo].[Sam3_GetErrorInfo]
	ROLLBACK TRAN
END CATCH