begin tran 
begin try 
	insert into [steelgo-sam3].[dbo].[Sam3_Propiedad] values(29,'foliocuantificacion',1,NULL,NULL),(29,'itemcode',1,NULL,NULL),(29,'mostrar',1,NULL,NULL),(29,'numerounico',1,NULL,NULL),(29,'proyecto',1,NULL,NULL)
	insert into [steelgo-sam3].[dbo].[Sam3_Rel_Perfil_Propiedad_Pagina] values(1,182,16,1,1,1,1,NULL,NULL),(1,183,16,1,1,0,1,NULL,NULL),(1,184,16,1,1,0,1,NULL,NULL),(1,185,16,1,1,0,1,NULL,NULL),(1,186,16,1,1,1,1,NULL,NULL)
commit tran 
end try 
begin catch 
        exec [dbo].[Sam3_GetErrorInfo] 
        rollback tran 
end catch