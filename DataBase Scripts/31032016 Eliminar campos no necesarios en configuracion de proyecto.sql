USE [steelgo-sam3]
BEGIN TRAN
BEGIN TRY
	
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__24E777C3]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__25DB9BFC]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__26CFC035]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__28B808A7]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__29AC2CE0]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__2AA05119]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Digit__2B947552]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP CONSTRAINT [DF__Sam3_Proy__Gigit__27C3E46E]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosOrdeAlmacenaje]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioDescarga]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioAvisollegada]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [GigitosFolioPaseSalida]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioOrdenRecepcion]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioCuantifiacion]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioOrdenAlmacenaje]
	ALTER TABLE [dbo].[Sam3_ProyectoConfiguracion] DROP COLUMN [DigitosFolioPermisoAduana]

	COMMIT TRAN
END TRY
BEGIN CATCH
	exec [dbo].[Sam3_GetErrorInfo]
	ROLLBACK TRAN
END CATCH