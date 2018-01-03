using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace BackEndSAM.DataAcces
{
    /// <summary>
    /// operaciones sobre la entidad ItemCode
    /// </summary>
    public class ItemCodeBd
    {
        private static readonly object _mutex = new object();
        private static ItemCodeBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ItemCodeBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ItemCodeBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemCodeBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene los item codes para el combo del grid de materiales
        /// Que no tengan orden de Recepcion ni Numeros unicos
        /// </summary>
        /// <param name="tipoPackingListID">tipo Packing List: 1 Tubo, 2 Accesorio</param>
        /// <returns>lista de item codes</returns>
        public object ObtenerItemCode(int tipoPackingListID, Sam3_Usuario usuario, int paginaID, string idioma, int proyectoID,string datosBuscar)
        {
            try
            {
                // tpo packinglist = 3 traer todos los itemcodes
                List<BackEndSAM.Models.ItemCode> IC = new List<BackEndSAM.Models.ItemCode>();
                List<BackEndSAM.Models.ItemCode> itemCodeS2 = new List<BackEndSAM.Models.ItemCode>();

                //ReplicarItemCodesSam2(proyectoID, usuario);

                using (SamContext ctx = new SamContext())
                {
                    using (DatabaseManager.Sam2.Sam2Context ctx2 = new DatabaseManager.Sam2.Sam2Context())
                    {

                        IC.Add(new BackEndSAM.Models.ItemCode { ItemCodeID = "0", Codigo = "Bulto", D1 = 0, D2 = 0 });

                        if (tipoPackingListID == 3)
                        {
                            itemCodeS2 = (from ic in ctx.Sam3_ItemCode
                                          join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                          join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                          join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                          where ic.Activo && rid.Activo
                                          && ic.ProyectoID == proyectoID && ic.Codigo.Contains(datosBuscar)
                                          select new BackEndSAM.Models.ItemCode
                                          {
                                              ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                              Codigo = ic.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                              D1 = d1.Valor,
                                              D2 = d2.Valor
                                          }).AsParallel().Distinct().ToList();
                        }
                        else
                        {
                            itemCodeS2 = (from ic in ctx.Sam3_ItemCode
                                          join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                          join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                          join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                          where ic.Activo && rid.Activo
                                          && ic.TipoMaterialID == tipoPackingListID
                                          && ic.ProyectoID == proyectoID && ic.Codigo.Contains(datosBuscar)
                                          select new BackEndSAM.Models.ItemCode
                                          {
                                              ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                              Codigo = ic.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                              D1 = d1.Valor,
                                              D2 = d2.Valor
                                          }).AsParallel().Distinct().ToList();
                        }
                    }
                }

                IC.AddRange(itemCodeS2);
                return IC;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los ItemCodes del Proyecto SAM2. {0}", proyectoID));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Guardar nnuevo Item Code
        /// </summary>
        /// <param name="DatosItemCode">datos capturados por el usuario en el modal</param>
        /// <param name="usuario">usuario registrado</param>
        /// <returns>status exito o error</returns>
        public object GuardarItemCodePopUp(ItemCodeJson DatosItemCode, Sam3_Usuario usuario)
        {
            try
            {
                int sam2_ProyectoID = 0;

                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var sam2_tran = ctx2.Database.BeginTransaction())
                            {
                                //Inserta en Sam 2
                                DatabaseManager.Sam2.ItemCode itemS2;
                                sam2_ProyectoID = (from eq in ctx.Sam3_EquivalenciaProyecto
                                                   where eq.Activo
                                                   && eq.Sam3_ProyectoID == DatosItemCode.ProyectoID
                                                   select eq.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                                decimal Diametro1 = string.IsNullOrEmpty(DatosItemCode.Diametro1.ToString()) ? 0 : Convert.ToDecimal(DatosItemCode.Diametro1);
                                decimal Diametro2 = string.IsNullOrEmpty(DatosItemCode.Diametro2.ToString()) ? 0 : Convert.ToDecimal(DatosItemCode.Diametro2);

                                if (!ctx2.ItemCode.Where(x => x.Codigo == DatosItemCode.ItemCode && x.ProyectoID == sam2_ProyectoID).Any())
                                {
                                    itemS2 = new DatabaseManager.Sam2.ItemCode();
                                    itemS2.ProyectoID = sam2_ProyectoID;
                                    itemS2.TipoMaterialID = DatosItemCode.TipoPackingList;
                                    itemS2.Codigo = DatosItemCode.ItemCode;
                                    itemS2.ItemCodeCliente = DatosItemCode.ItemCodeCliente;
                                    itemS2.DescripcionEspanol = DatosItemCode.Descripcion;
                                    itemS2.DescripcionIngles = DatosItemCode.DescripcionIngles;
                                    itemS2.FechaModificacion = DateTime.Now;
                                    itemS2.Peso = DatosItemCode.Peso;
                                    itemS2.DescripcionInterna = DatosItemCode.DescripcionInterna;
                                    itemS2.Diametro1 = Diametro1;
                                    itemS2.Diametro2 = Diametro2;
                                    //itemS2.FamiliaAceroID = (from eq in ctx.Sam3_EquivalenciaFamiliaAcero
                                    //                         where eq.Activo
                                    //                         && eq.Sam3_FamiliaAceroID == DatosItemCode.FamiliaID
                                    //                         select eq.Sam2_FamiliaAceroID).AsParallel().SingleOrDefault();

                                    ctx2.ItemCode.Add(itemS2);
                                    ctx2.SaveChanges();
                                }
                                else
                                {
                                    itemS2 = ctx2.ItemCode.Where(x => x.Codigo == DatosItemCode.ItemCode && x.ProyectoID == sam2_ProyectoID).AsParallel().SingleOrDefault();
                                }


                                int diam1 = Convert.ToInt32(DatosItemCode.Diametro1ID);
                                int diam2 = Convert.ToInt32(DatosItemCode.Diametro2ID);

                                if (diam1 == 0)
                                {
                                    Sam3_Diametro diametro1;
                                    //Se verifica que si exista el diametro
                                    //Si existe se asigna el valor
                                    if (ctx.Sam3_Diametro.Where(x => x.Valor == Diametro1).Any())
                                    {
                                        diam1 = ctx.Sam3_Diametro.Where(x => x.Valor == Diametro1).Select(x => x.DiametroID).AsParallel().SingleOrDefault();
                                    }
                                    else
                                    {
                                        diametro1 = new Sam3_Diametro();
                                        diametro1.Valor = Diametro1;
                                        diametro1.VerificadoPorCalidad = true;
                                        diametro1.Activo = true;
                                        diametro1.UsuarioModificacion = usuario.UsuarioID;
                                        diametro1.FechaModificacion = DateTime.Now;
                                        ctx.Sam3_Diametro.Add(diametro1);
                                        ctx.SaveChanges();
                                        diam1 = diametro1.DiametroID;
                                    }
                                }

                                if (diam2 == 0)
                                {
                                    Sam3_Diametro diametro2;
                                    //Se verifica que si exista el diametro
                                    //Si existe se asigna el valor
                                    if (ctx.Sam3_Diametro.Where(x => x.Valor == Diametro2).Any())
                                    {
                                        diam2 = ctx.Sam3_Diametro.Where(x => x.Valor == Diametro2).Select(x => x.DiametroID).AsParallel().SingleOrDefault();
                                    }
                                    else
                                    {
                                        diametro2 = new Sam3_Diametro();
                                        diametro2.Valor = Diametro2;
                                        diametro2.VerificadoPorCalidad = true;
                                        diametro2.Activo = true;
                                        diametro2.UsuarioModificacion = usuario.UsuarioID;
                                        diametro2.FechaModificacion = DateTime.Now;
                                        ctx.Sam3_Diametro.Add(diametro2);
                                        ctx.SaveChanges();
                                        diam2 = diametro2.DiametroID;
                                    }
                                }

                                Sam3_ItemCode itemS3;
                                //Inserta en Sam 3
                                if (!ctx.Sam3_ItemCode.Where(x => x.Codigo == DatosItemCode.ItemCode && x.ProyectoID == DatosItemCode.ProyectoID).Any())
                                {
                                    itemS3 = new Sam3_ItemCode();
                                    itemS3.ProyectoID = DatosItemCode.ProyectoID;
                                    itemS3.TipoMaterialID = DatosItemCode.TipoPackingList;//
                                    itemS3.Codigo = DatosItemCode.ItemCode;//
                                    itemS3.ItemCodeCliente = DatosItemCode.ItemCodeCliente;
                                    itemS3.DescripcionEspanol = DatosItemCode.Descripcion;//
                                    //itemS3.FamiliaAceroID = DatosItemCode.FamiliaID;//
                                    itemS3.Activo = true;
                                    itemS3.UsuarioModificacion = usuario.UsuarioID;
                                    itemS3.FechaModificacion = DateTime.Now;
                                    itemS3.TipoUsoID = Convert.ToInt32(DatosItemCode.TipoUsoID) == -1 ? 1 : Convert.ToInt32(DatosItemCode.TipoUsoID);
                                    ctx.Sam3_ItemCode.Add(itemS3);
                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    itemS3 = ctx.Sam3_ItemCode.Where(x => x.Codigo == DatosItemCode.ItemCode && x.ProyectoID == DatosItemCode.ProyectoID).AsParallel().SingleOrDefault();
                                }

                                if (!ctx.Sam3_EquivalenciaItemCode.Where(x => x.Sam2_ItemCodeID == itemS2.ItemCodeID
                                       && x.Sam3_ItemCodeID == itemS3.ItemCodeID && x.Activo).Any())
                                {
                                    //Inserta en equivalencia
                                    Sam3_EquivalenciaItemCode equiv = new Sam3_EquivalenciaItemCode();
                                    equiv.Sam2_ItemCodeID = itemS2.ItemCodeID;
                                    equiv.Sam3_ItemCodeID = itemS3.ItemCodeID;
                                    equiv.Activo = true;
                                    equiv.UsuarioModificacion = usuario.UsuarioID;
                                    equiv.FechaModificacion = DateTime.Now;

                                    ctx.Sam3_EquivalenciaItemCode.Add(equiv);
                                    ctx.SaveChanges();
                                }

                                bool existeItemCode = ctx.Sam3_ItemCode.Where(x => x.Codigo == DatosItemCode.ItemCode && x.Activo && x.ProyectoID == DatosItemCode.ProyectoID).Any();
                                if (existeItemCode)
                                {
                                    int itemID = ctx.Sam3_ItemCode
                                        .Where(x => x.Codigo == DatosItemCode.ItemCode && x.Activo && x.ProyectoID == DatosItemCode.ProyectoID).Select(x => x.ItemCodeID).FirstOrDefault();

                                    if (!ctx.Sam3_Rel_ItemCode_Diametro.Where(x => x.ItemCodeID == itemID
                                            && x.Diametro1ID == diam1 && x.Diametro2ID == diam2 && x.Activo).AsParallel().Any())
                                    {
                                        Sam3_Rel_ItemCode_Diametro relItemCodeDiametro = new Sam3_Rel_ItemCode_Diametro();
                                        relItemCodeDiametro.ItemCodeID = itemS3.ItemCodeID;
                                        relItemCodeDiametro.Diametro1ID = diam1;
                                        relItemCodeDiametro.Diametro2ID = diam2;
                                        relItemCodeDiametro.UsuarioModificacion = usuario.UsuarioID;
                                        relItemCodeDiametro.FechaModificacion = DateTime.Now;
                                        relItemCodeDiametro.Activo = true;
                                        ctx.Sam3_Rel_ItemCode_Diametro.Add(relItemCodeDiametro);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        throw new Exception("El item code ya existe con esos diametros.");
                                    }
                                }

                                sam2_tran.Commit();
                            } //tran sam2
                        } //ctx Sam 2
                        sam3_tran.Commit();
                    } //tran sam3
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                } //ctx sam 3
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Obtener el detalle de un item code al seleccionarlo en el grid
        /// </summary>
        /// <param name="itemCode">item code seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>objeto con la informacion del item code</returns>
        public object ObtenerDetalleItemCode(string itemCode)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int RelItemID = Convert.ToInt32(itemCode);
                    bool tieneICS = (from rid in ctx.Sam3_Rel_ItemCode_Diametro
                                     join rii in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rii.Rel_ItemCode_Diametro_ID
                                     where rid.Rel_ItemCode_Diametro_ID == RelItemID
                                     && rid.Activo && rii.Activo
                                     select rii).AsParallel().Any();

                    ItemCodeJson detalle = new ItemCodeJson();

                    if (tieneICS)
                    {
                        detalle = (from r in ctx.Sam3_ItemCode
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                                   join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                   join rics in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rics.Rel_ItemCodeSteelgo_Diametro_ID
                                   join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   where r.Activo && rid.Activo && riit.Activo && rics.Activo && ics.Activo && d1.Activo && d2.Activo
                                   && rid.Rel_ItemCode_Diametro_ID == RelItemID
                                   select new ItemCodeJson
                                   {
                                       ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                       ItemCode = r.Codigo, 
                                       Descripcion = r.DescripcionEspanol,
                                       Diametro1 = d1.Valor,
                                       Diametro2 = d2.Valor,
                                       FamiliaAcero = (from f in ctx.Sam3_FamiliaAcero where f.FamiliaAceroID == ics.FamiliaAceroID && f.Activo select f.Nombre).FirstOrDefault(),
                                       ItemCodeSteelgoID = riit.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                       ItemCodeSteelgo = ics.Codigo,
                                       TipoAcero = (from rics2 in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                    join itcs in ctx.Sam3_ItemCodeSteelgo on rics2.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                    join rid2 in ctx.Sam3_Rel_ItemCode_Diametro on rics2.Rel_ItemCode_Diametro_ID equals rid2.Rel_ItemCode_Diametro_ID
                                                    join it in ctx.Sam3_ItemCode on rid2.ItemCodeID equals it.ItemCodeID
                                                    join fa in ctx.Sam3_FamiliaAcero on itcs.FamiliaAceroID equals fa.FamiliaAceroID
                                                    join fm in ctx.Sam3_FamiliaMaterial on fa.FamiliaMaterialID equals fm.FamiliaMaterialID
                                                    where rics2.Activo && itcs.Activo && rid2.Activo && it.Activo && fa.Activo && fm.Activo
                                                    && rid2.ItemCodeID == r.ItemCodeID
                                                    select fm.Nombre).FirstOrDefault(),
                                       ItemCodeOrigenID = r.ItemCodeID,
                                       TipoPackingList = r.TipoMaterialID,
                                       TextoTipoPackingList = (from tm in ctx.Sam3_TipoMaterial
                                                               where tm.TipoMaterialID == r.TipoMaterialID
                                                               select tm.Nombre).FirstOrDefault()
                                   }).AsParallel().SingleOrDefault();

                        if (detalle != null)
                        {
                            string diametro = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                               join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                               && ricsd.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                               && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                              && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                              && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            detalle.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }
                    else
                    {
                        detalle = (from r in ctx.Sam3_ItemCode
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   where r.Activo && rid.Activo && d1.Activo && d2.Activo
                                    && rid.Rel_ItemCode_Diametro_ID == RelItemID
                                   select new ItemCodeJson
                                   {
                                       ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                       ItemCode = r.Codigo,
                                       Descripcion = r.DescripcionEspanol,
                                       Diametro1 = d1.Valor,
                                       Diametro2 = d2.Valor,
                                       ItemCodeOrigenID = r.ItemCodeID,
                                       TipoPackingList = r.TipoMaterialID,
                                       TextoTipoPackingList = (from tm in ctx.Sam3_TipoMaterial
                                                               where tm.TipoMaterialID == r.TipoMaterialID
                                                               select tm.Nombre).FirstOrDefault()
                                   }).AsParallel().SingleOrDefault();
                    }
                    return detalle;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Obtener el detalle de un item code al seleccionarlo en el grid
        /// </summary>
        /// <param name="itemCode">item code seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>objeto con la informacion del item code</returns>
        public object ObtenerDetalleItemCode(string itemCode, string Colada, string MM, int Cantidad)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int RelItemID = Convert.ToInt32(itemCode);
                    bool tieneICS = (from rid in ctx.Sam3_Rel_ItemCode_Diametro
                                     join rii in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rii.Rel_ItemCode_Diametro_ID
                                     where rid.Rel_ItemCode_Diametro_ID == RelItemID
                                     && rid.Activo && rii.Activo
                                     select rii).AsParallel().Any();

                    ItemCodeJson detalle = new ItemCodeJson();

                    if (tieneICS)
                    {
                        int? valor = MM == ""|| MM ==null ? 0 : int.Parse(MM);
                        detalle = (from r in ctx.Sam3_ItemCode
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                                   join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                   join rics in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rics.Rel_ItemCodeSteelgo_Diametro_ID
                                   join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   where r.Activo && rid.Activo && riit.Activo && rics.Activo && ics.Activo && d1.Activo && d2.Activo
                                   && rid.Rel_ItemCode_Diametro_ID == RelItemID
                                   select new ItemCodeJson
                                   {
                                       ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                       ItemCode = r.Codigo,
                                       Descripcion = r.DescripcionEspanol,
                                       Diametro1 = d1.Valor,
                                       Diametro2 = d2.Valor,
                                       FamiliaAcero = (from f in ctx.Sam3_FamiliaAcero where f.FamiliaAceroID == ics.FamiliaAceroID && f.Activo select f.Nombre).FirstOrDefault(),
                                       ItemCodeSteelgoID = riit.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                       ItemCodeSteelgo = ics.Codigo,
                                       TipoAcero = (from rics2 in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                    join itcs in ctx.Sam3_ItemCodeSteelgo on rics2.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                    join rid2 in ctx.Sam3_Rel_ItemCode_Diametro on rics2.Rel_ItemCode_Diametro_ID equals rid2.Rel_ItemCode_Diametro_ID
                                                    join it in ctx.Sam3_ItemCode on rid2.ItemCodeID equals it.ItemCodeID
                                                    join fa in ctx.Sam3_FamiliaAcero on itcs.FamiliaAceroID equals fa.FamiliaAceroID
                                                    join fm in ctx.Sam3_FamiliaMaterial on fa.FamiliaMaterialID equals fm.FamiliaMaterialID
                                                    where rics2.Activo && itcs.Activo && rid2.Activo && it.Activo && fa.Activo && fm.Activo
                                                    && rid2.ItemCodeID == r.ItemCodeID
                                                    select fm.Nombre).FirstOrDefault(),
                                       ItemCodeOrigenID = r.ItemCodeID,
                                       TipoPackingList = r.TipoMaterialID,
                                       TextoTipoPackingList = (from tm in ctx.Sam3_TipoMaterial
                                                               where tm.TipoMaterialID == r.TipoMaterialID
                                                               select tm.Nombre).FirstOrDefault(),
                                       Cantidad=Cantidad,
                                       MM= valor,
                                       ColadaNombre=Colada
                                   }).AsParallel().SingleOrDefault();

                        if (detalle != null)
                        {
                            string diametro = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                               join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                               && ricsd.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                               && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                              && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                              join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                              && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            detalle.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }
                    else
                    {
                        int? valor = MM == "" || MM == null ? 0 : int.Parse(MM);
                        detalle = (from r in ctx.Sam3_ItemCode
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   where r.Activo && rid.Activo && d1.Activo && d2.Activo
                                    && rid.Rel_ItemCode_Diametro_ID == RelItemID
                                   select new ItemCodeJson
                                   {
                                       ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                       ItemCode = r.Codigo,
                                       Descripcion = r.DescripcionEspanol,
                                       Diametro1 = d1.Valor,
                                       Diametro2 = d2.Valor,
                                       ItemCodeOrigenID = r.ItemCodeID,
                                       TipoPackingList = r.TipoMaterialID,
                                       TextoTipoPackingList = (from tm in ctx.Sam3_TipoMaterial
                                                               where tm.TipoMaterialID == r.TipoMaterialID
                                                               select tm.Nombre).FirstOrDefault(),
                                       Cantidad = Cantidad,
                                       MM = valor,
                                       ColadaNombre = Colada
                                   }).AsParallel().SingleOrDefault();
                    }
                    return detalle;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> Ids)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ItemCode> registros = new List<Sam3_ItemCode>();

                    if (proyectoID > 0)
                    {
                        registros = (from it in ctx.Sam3_ItemCode
                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                     join p in ctx.Sam3_Proyecto on it.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where it.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && Ids.Contains(rid.Rel_ItemCode_Diametro_ID)
                                     select it).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from it in ctx.Sam3_ItemCode
                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                     join p in ctx.Sam3_Proyecto on it.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where it.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && Ids.Contains(rid.Rel_ItemCode_Diametro_ID)
                                     select it).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                     where p.Activo
                                     && p.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                               join riit in ctx.Sam3_Rel_Incidencia_ItemCode on rid.Rel_ItemCode_Diametro_ID equals riit.ItemCodeID
                               join inc in ctx.Sam3_Incidencia on riit.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where riit.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   TipoIncidencia = tpi.Nombre,
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

                    if (ActivarFolioConfiguracionIncidencias)
                    {
                        foreach (ListadoIncidencias item in listado)
                        {
                            if (!string.IsNullOrEmpty(item.FolioConfiguracionIncidencia))
                            {
                                string[] elemntos = item.FolioConfiguracionIncidencia.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                item.FolioConfiguracionIncidencia = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                            else
                            {
                                item.FolioConfiguracionIncidencia = item.FolioIncidenciaID.ToString();
                            }
                        }
                    }
                }
                return listado;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }

        /// <summary>
        /// Obtiene los diametros
        /// </summary>
        /// <returns>lista de diametros</returns>
        public object ObtenerDiametros(Sam3_Usuario usuario)
        {
            try
            {
                List<ListaDiametros> registros = new List<ListaDiametros>();

                using (SamContext ctx = new SamContext())
                {
                    registros = (from r in ctx.Sam3_Diametro
                                 where r.Activo
                                 select new ListaDiametros
                                 {
                                     id = r.DiametroID,
                                     value = r.Valor
                                 }).ToList();

                    registros.OrderBy(x => x.value).ToList();

                    List<ListaCombos> diametros = (from r in registros.OrderBy(x => x.value)
                                                   select new ListaCombos
                                                   {
                                                       id = r.id.ToString(),
                                                       value = r.value.ToString()
                                                   }).AsParallel().ToList();

                    return diametros;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los diametros"));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        /// <summary>
        /// Obtiene los diametros
        /// </summary>
        /// <returns>lista de diametros</returns>
        public object ObtenerDiametroCero(int Diametro, Sam3_Usuario usuario)
        {
            try
            {
                ListaCombos registro = new ListaCombos();

                using (SamContext ctx = new SamContext())
                {
                    registro = (from r in ctx.Sam3_Diametro
                                where r.Activo && r.Valor == Diametro
                                select new ListaCombos
                                {
                                    id = r.DiametroID.ToString(),
                                    value = r.Valor.ToString()
                                }).FirstOrDefault();

                    return registro;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los diametros"));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerItemCodeCatalogoMTR(int proyectoID, Sam3_Usuario usuario, string busqueda = "")
        {
            try
            {
                // tpo packinglist = 3 traer todos los itemcodes
                List<BackEndSAM.Models.ItemCode> IC = new List<BackEndSAM.Models.ItemCode>();

                using (SamContext ctx = new SamContext())
                {
                    using (DatabaseManager.Sam2.Sam2Context ctx2 = new DatabaseManager.Sam2.Sam2Context())
                    {
                        #region Filtros
                        //traemos la informacion de los proyectos y patios del usuario
                        List<int> proyectos;
                        List<int> patios;
                        UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);


                        IC = (from ic in ctx.Sam3_ItemCode
                                      join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                      join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                      join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                      where ic.Activo && rid.Activo
                                      && proyectos.Contains(ic.ProyectoID)
                                      && ic.ProyectoID == proyectoID
                                      select new BackEndSAM.Models.ItemCode
                                      {
                                          ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                          Codigo = ic.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                          D1 = d1.Valor,
                                          D2 = d2.Valor
                                      }).AsParallel().Distinct().ToList();
                        #endregion
                    }
                }

                if (busqueda != string.Empty)
                {
                    IC = IC.Where(x => x.Codigo.Contains(busqueda)).ToList();
                }

                return IC;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los ItemCodes del Proyecto SAM2. {0}"));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        private object ReplicarItemCodesSam2(int proyectoID, Sam3_Usuario usuario)
        {
            try
            {
                List<BackEndSAM.Models.ItemCode> listado = new List<BackEndSAM.Models.ItemCode>();
                int sam2_ProyectoID = 0;
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        sam2_ProyectoID = ctx.Sam3_EquivalenciaProyecto.Where(x => x.Sam3_ProyectoID == proyectoID && x.Activo)
                            .Select(x => x.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                        List<int> itemCodesSam2EnSam3 = (from eqit in ctx.Sam3_EquivalenciaItemCode
                                                         join it in ctx.Sam3_ItemCode on eqit.Sam3_ItemCodeID equals it.ItemCodeID
                                                         join p in ctx.Sam3_Proyecto on it.ProyectoID equals p.ProyectoID
                                                         where eqit.Activo && it.Activo && p.Activo
                                                         && p.ProyectoID == proyectoID
                                                         select eqit.Sam2_ItemCodeID).AsParallel().Distinct().ToList();

                        List<DatabaseManager.Sam2.ItemCode> itemsAReplicar = (from it2 in ctx2.ItemCode
                                                                              where !itemCodesSam2EnSam3.Contains(it2.ItemCodeID)
                                                                              && it2.ProyectoID == sam2_ProyectoID
                                                                              select it2).AsParallel().Distinct().ToList();

                        if (itemsAReplicar.Count() > 0)
                        {
                            itemsAReplicar.ForEach(x => InsertarItemCodeEnSam3(x, usuario, proyectoID));
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los ItemCodes del Proyecto SAM2. {0}"));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        private void InsertarItemCodeEnSam3(DatabaseManager.Sam2.ItemCode itemSam2, Sam3_Usuario ususario, int sam3_ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            int sam3_FamiliaAceroID = 0;
                            if (itemSam2.FamiliaAceroID != null)
                            {
                                #region Familia material y acero
                                if (ctx.Sam3_EquivalenciaFamiliaAcero.Where(x => x.Sam2_FamiliaAceroID == itemSam2.FamiliaAceroID && x.Activo).Any())
                                {
                                    sam3_FamiliaAceroID = ctx.Sam3_EquivalenciaFamiliaAcero.Where(x => x.Sam2_FamiliaAceroID == itemSam2.FamiliaAceroID && x.Activo)
                                        .Select(x => x.Sam3_FamiliaAceroID).SingleOrDefault();
                                }
                                else
                                {
                                    int sam3_FamiliaMaterialID = 0;

                                    DatabaseManager.Sam2.FamiliaMaterial sam2_FamiliaMaterial = ctx2.FamiliaAcero.Where(x => x.FamiliaAceroID == itemSam2.FamiliaAceroID)
                                        .Select(x => x.FamiliaMaterial).AsParallel().SingleOrDefault();

                                    if (ctx.Sam3_EquivalenciaFamiliaMaterial.Where(x => x.Sam2_FamiliaMaterialID == sam2_FamiliaMaterial.FamiliaMaterialID && x.Activo).Any())
                                    {
                                        sam3_FamiliaMaterialID = ctx.Sam3_EquivalenciaFamiliaMaterial.Where(x => x.Sam2_FamiliaMaterialID == sam2_FamiliaMaterial.FamiliaMaterialID && x.Activo)
                                            .Select(x => x.Sam3_FamiliaMaterialID).SingleOrDefault();
                                    }
                                    else
                                    {
                                        //insertar la nueva familia de material
                                        Sam3_FamiliaMaterial familiaMaterialS3 = new Sam3_FamiliaMaterial();
                                        familiaMaterialS3.Activo = true;
                                        familiaMaterialS3.Descripcion = sam2_FamiliaMaterial.Descripcion;
                                        familiaMaterialS3.FechaModificacion = DateTime.Now;
                                        familiaMaterialS3.Nombre = sam2_FamiliaMaterial.Nombre;
                                        familiaMaterialS3.UsuarioModificacion = ususario.UsuarioID;
                                        ctx.Sam3_FamiliaMaterial.Add(familiaMaterialS3);
                                        ctx.SaveChanges();

                                        sam3_FamiliaMaterialID = familiaMaterialS3.FamiliaMaterialID;
                                        //insertar la equivalencia 
                                        Sam3_EquivalenciaFamiliaMaterial equivalencia = new Sam3_EquivalenciaFamiliaMaterial
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_FamiliaMaterialID = sam2_FamiliaMaterial.FamiliaMaterialID,
                                            Sam3_FamiliaMaterialID = sam3_FamiliaMaterialID,
                                            UsuarioModificacion = ususario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaFamiliaMaterial.Add(equivalencia);
                                        ctx.SaveChanges();
                                    }

                                    DatabaseManager.Sam2.FamiliaAcero familiaS2 = ctx2.FamiliaAcero.Where(x => x.FamiliaAceroID == itemSam2.FamiliaAceroID).SingleOrDefault();
                                    //insertar Familia de acero
                                    Sam3_FamiliaAcero familiaAceroS3 = new Sam3_FamiliaAcero();
                                    familiaAceroS3.Activo = true;
                                    familiaAceroS3.Descripcion = familiaS2.Descripcion;
                                    familiaAceroS3.FamiliaMaterialID = sam3_FamiliaMaterialID;
                                    familiaAceroS3.FechaModificacion = DateTime.Now;
                                    familiaAceroS3.Nombre = familiaS2.Nombre;
                                    familiaAceroS3.UsuarioModificacion = ususario.UsuarioID;
                                    familiaAceroS3.VerificadoPorCalidad = familiaS2.VerificadoPorCalidad;
                                    ctx.Sam3_FamiliaAcero.Add(familiaAceroS3);
                                    ctx.SaveChanges();

                                    sam3_FamiliaAceroID = familiaAceroS3.FamiliaAceroID;

                                    // insertar la equivalencia
                                    Sam3_EquivalenciaFamiliaAcero equivalenciaFamiliaAcero = new Sam3_EquivalenciaFamiliaAcero
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        Sam3_FamiliaAceroID = sam3_FamiliaAceroID,
                                        Sam2_FamiliaAceroID = familiaS2.FamiliaAceroID,
                                        UsuarioModificacion = ususario.UsuarioID
                                    };
                                    ctx.Sam3_EquivalenciaFamiliaAcero.Add(equivalenciaFamiliaAcero);
                                    ctx.SaveChanges();

                                }
                                #endregion
                            }

                            #region ItemCode y equivalencia
                            Sam3_ItemCode itemSam3 = new Sam3_ItemCode();
                            itemSam3.Activo = true;
                            itemSam3.Codigo = itemSam2.Codigo;
                            itemSam3.DescripcionEspanol = itemSam2.DescripcionEspanol;
                            itemSam3.DescripcionIngles = itemSam2.DescripcionIngles;
                            itemSam3.DescripcionInterna = itemSam2.DescripcionInterna;
                            if (sam3_FamiliaAceroID > 0)
                            {
                                itemSam3.FamiliaAceroID = sam3_FamiliaAceroID;
                            }
                            itemSam3.FechaModificacion = DateTime.Now;
                            itemSam3.ItemCodeCliente = itemSam2.ItemCodeCliente;
                            itemSam3.Peso = itemSam2.Peso;
                            itemSam3.ProyectoID = sam3_ProyectoID;
                            itemSam3.TipoMaterialID = itemSam2.TipoMaterialID;
                            itemSam3.TipoUsoID = ctx.Sam3_TipoUso.Where(x => x.Nombre == "Tipo Uso Default" && x.Activo).Select(x => x.TipoUsoID).SingleOrDefault();
                            itemSam3.UsuarioModificacion = ususario.UsuarioID;
                            ctx.Sam3_ItemCode.Add(itemSam3);
                            ctx.SaveChanges();
                            // insertar la equivalencia de ItemCode
                            Sam3_EquivalenciaItemCode equivalenciaItemCode = new Sam3_EquivalenciaItemCode
                            {
                                Activo = true,
                                Sam2_ItemCodeID = itemSam2.ItemCodeID,
                                Sam3_ItemCodeID = itemSam3.ItemCodeID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = ususario.UsuarioID
                            };
                            ctx.Sam3_EquivalenciaItemCode.Add(equivalenciaItemCode);
                            ctx.SaveChanges();
                            #endregion

                            //insertar relaciones de diametros
                            int sam3_diametro1ID = 0;
                            int sam3_diametro2ID = 0;
                            decimal sam2_diametro1 = (from nu in ctx2.NumeroUnico
                                                      where nu.ItemCodeID == itemSam2.ItemCodeID
                                                      && nu.Diametro1 != null
                                                      select nu.Diametro1).FirstOrDefault() != null ?
                                                    (from nu in ctx2.NumeroUnico
                                                     where nu.ItemCodeID == itemSam2.ItemCodeID
                                                     && nu.Diametro1 != null
                                                     select nu.Diametro1).FirstOrDefault() : 0;

                            decimal sam2_diametro2 = (from nu in ctx2.NumeroUnico
                                                      where nu.ItemCodeID == itemSam2.ItemCodeID
                                                      && nu.Diametro2 != null
                                                      select nu.Diametro2).FirstOrDefault() != null ?
                                                    (from nu in ctx2.NumeroUnico
                                                     where nu.ItemCodeID == itemSam2.ItemCodeID
                                                     && nu.Diametro2 != null
                                                     select nu.Diametro2).FirstOrDefault() : 0;

                            #region Diametro1
                            if (ctx.Sam3_Diametro.Where(x => x.Valor == sam2_diametro1 && x.Activo).Any())
                            {
                                sam3_diametro1ID = ctx.Sam3_Diametro.Where(x => x.Valor == sam2_diametro1 && x.Activo)
                                    .Select(x => x.DiametroID).SingleOrDefault();
                            }
                            else
                            {
                                #region insertar diametro y equivalencia 1
                                DatabaseManager.Sam2.Diametro d1S2 = ctx2.Diametro.Where(x => x.Valor == sam2_diametro1).SingleOrDefault();
                                Sam3_Diametro d1S3 = new Sam3_Diametro
                                {
                                    Activo = true,
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModificacion = ususario.UsuarioID,
                                    Valor = d1S2.Valor,
                                    VerificadoPorCalidad = d1S2.VerificadoPorCalidad
                                };
                                ctx.Sam3_Diametro.Add(d1S3);
                                ctx.SaveChanges();
                                sam3_diametro1ID = d1S3.DiametroID;
                                //equivalencia
                                Sam3_EquivalenciaDiametro equivalenciaDiametro = new Sam3_EquivalenciaDiametro
                                {
                                    Activo = true,
                                    Sam2_DiametroID = d1S2.DiametroID,
                                    Sam3_DiametroID = d1S3.DiametroID,
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModificacion = ususario.UsuarioID
                                };
                                ctx.Sam3_EquivalenciaDiametro.Add(equivalenciaDiametro);
                                ctx.SaveChanges();
                                #endregion
                            }
                            #endregion

                            #region Diametro2
                            if (ctx.Sam3_Diametro.Where(x => x.Valor == sam2_diametro2 && x.Activo).Any())
                            {
                                sam3_diametro2ID = ctx.Sam3_Diametro.Where(x => x.Valor == sam2_diametro2 && x.Activo)
                                    .Select(x => x.DiametroID).SingleOrDefault();
                            }
                            else
                            {
                                #region insertar diametro y equivalencia 2
                                DatabaseManager.Sam2.Diametro d2S2 = ctx2.Diametro.Where(x => x.Valor == sam2_diametro2).SingleOrDefault();
                                Sam3_Diametro d2S3 = new Sam3_Diametro
                                {
                                    Activo = true,
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModificacion = ususario.UsuarioID,
                                    Valor = d2S2.Valor,
                                    VerificadoPorCalidad = d2S2.VerificadoPorCalidad
                                };
                                ctx.Sam3_Diametro.Add(d2S3);
                                ctx.SaveChanges();
                                sam3_diametro2ID = d2S3.DiametroID;
                                //equivalencia
                                Sam3_EquivalenciaDiametro equivalenciaDiametro = new Sam3_EquivalenciaDiametro
                                {
                                    Activo = true,
                                    Sam2_DiametroID = d2S2.DiametroID,
                                    Sam3_DiametroID = d2S3.DiametroID,
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModificacion = ususario.UsuarioID
                                };
                                ctx.Sam3_EquivalenciaDiametro.Add(equivalenciaDiametro);
                                ctx.SaveChanges();
                                #endregion
                            }
                            #endregion

                            #region insertar relacion de itemCode con diametros
                            Sam3_Rel_ItemCode_Diametro relItemCodeDiametro = new Sam3_Rel_ItemCode_Diametro
                            {
                                Activo = true,
                                Diametro1ID = sam3_diametro1ID,
                                Diametro2ID = sam3_diametro2ID,
                                ItemCodeID = itemSam3.ItemCodeID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = ususario.UsuarioID
                            };
                            ctx.Sam3_Rel_ItemCode_Diametro.Add(relItemCodeDiametro);
                            ctx.SaveChanges();
                            #endregion
                        }
                        ctx_tran.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                //TransactionalInformation result = new TransactionalInformation();
                //result.ReturnMessage.Add(string.Format("Error al obtener los ItemCodes del Proyecto SAM2. {0}"));
                //result.ReturnCode = 500;
                //result.ReturnStatus = false;
                //result.IsAuthenicated = true;

                //return result;
            }
        }

        public object ObtenerDiametrosItemCode(int itemCodeID, int proyectoID, Sam3_Usuario usuario)
        {
            try
            {
                string diametro1 = "";
                string diametro2 = "";
                List<object> result = new List<object>();
                using (SamContext ctx = new SamContext())
                {
                    diametro1 = (from it in ctx.Sam3_ItemCode
                                 join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                 join d in ctx.Sam3_Diametro on rid.Diametro1ID equals d.DiametroID
                                 where it.Activo && rid.Activo
                                 && it.ProyectoID == proyectoID
                                 && rid.Rel_ItemCode_Diametro_ID == itemCodeID
                                 select d.Valor.ToString()).AsParallel().SingleOrDefault();

                    diametro2 = (from it in ctx.Sam3_ItemCode
                                 join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                 join d in ctx.Sam3_Diametro on rid.Diametro2ID equals d.DiametroID
                                 where it.Activo && rid.Activo
                                 && it.ProyectoID == proyectoID
                                 && rid.Rel_ItemCode_Diametro_ID == itemCodeID
                                 select d.Valor.ToString()).AsParallel().SingleOrDefault();

                    result.Add(diametro1);
                    result.Add(diametro2);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(string.Format("Error al obtener los ItemCodes del Proyecto SAM2. {0}"));
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}