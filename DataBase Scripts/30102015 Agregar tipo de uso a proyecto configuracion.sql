  ALTER TABLE Sam3_ProyectoConfiguracion
  ADD TipoUsoID int NOT NULL FOREIGN KEY REFERENCES Sam3_TipoUso(TipoUsoID)