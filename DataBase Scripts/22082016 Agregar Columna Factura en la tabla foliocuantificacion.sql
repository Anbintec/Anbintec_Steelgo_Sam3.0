ALTER TABLE Sam3_FolioCuantificacion ADD Factura varchar(max) null


update fc set  fc.Factura=fa.Factura from Sam3_FolioCuantificacion  fc
inner join Sam3_FolioAvisoEntrada fa
on fc.FolioAvisoEntradaID=fa.FolioAvisoEntradaID