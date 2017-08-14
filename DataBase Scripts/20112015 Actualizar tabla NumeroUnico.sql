begin tran
begin try
	alter table Sam3_NumeroUnico add  EstatusFisico varchar(100) NULL,EstatusDocumental varchar(100) NULL,TipoUsoID int NULL
	ALTER TABLE Sam3_NumeroUnico ADD CONSTRAINT FK_NumeroUnico_TipoUso
	FOREIGN KEY (TipoUsoID) REFERENCES dbo.Sam3_TipoUso(TipoUsoID);
commit tran
end try
begin catch
	rollback tran
end catch



