begin tran
begin try

INSERT INTO [dbo].[Sam3_EquivalenciaFamiliaAcero]
           ([Sam2_FamiliaAceroID]
           ,[Sam3_FamiliaAceroID]
           ,[Activo])
     VALUES
           (1
           ,1
           ,1)

INSERT INTO [dbo].[Sam3_EquivalenciaFamiliaAcero]
           ([Sam2_FamiliaAceroID]
           ,[Sam3_FamiliaAceroID]
           ,[Activo])
     VALUES
           (1
           ,2
           ,1)

commit tran
end try
begin catch
	rollback tran
end catch