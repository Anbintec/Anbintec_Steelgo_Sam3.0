using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class ValidarItemCodeNUBd
    {
        private static readonly object _mutex = new object();
        private static ValidarItemCodeNUBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ValidarItemCodeNUBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ValidarItemCodeNUBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidarItemCodeNUBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Valida si un item code tiene Numero unico
        /// para saber si se puede cancelar un item code
        /// </summary>
        /// <param name="folioAvisoLlegadaID">folio de llegada</param>
        /// <param name="folioCuantificacionID">folio cuantificacion</param>
        /// <param name="ItemCode">item code</param>
        /// <param name="bultoID">id del bulto</param>
        /// <returns>tru o false</returns>
        public object ValidarItemCode(string folioAvisoLlegadaID, string folioCuantificacionID, string ItemCode, string bultoID, string detalleBulto, Sam3_Usuario usuario)
        {
            try
            {
                bool tieneNU = false;
                using (SamContext ctx = new SamContext())
                {
                    if (ItemCode.Contains("Bulto"))
                    {
                        List<bool> listaNU = new List<bool>();
                        listaNU.AddRange(from bic in ctx.Sam3_Rel_Bulto_ItemCode
                                         where bic.BultoID.ToString() == bultoID && bic.Activo
                                         select bic.TieneNumerosUnicos);

                        if (listaNU.Contains(true))
                        {
                            tieneNU = true;
                        }
                    }
                    else if(detalleBulto == "1")//Esta en el detalle Bulto
                    {
                        tieneNU = (from bulto in ctx.Sam3_Rel_Bulto_ItemCode
                                   where bulto.Activo && bulto.BultoID.ToString() == bultoID 
                                   && bulto.ItemCodeID.ToString() == ItemCode
                                   select bulto.TieneNumerosUnicos).AsParallel().FirstOrDefault();
                    }
                    else
                    {
                        int itemCodeID = (from ic in ctx.Sam3_ItemCode where ic.Codigo == ItemCode && ic.Activo select ic.ItemCodeID).AsParallel().FirstOrDefault();
                        
                        tieneNU = (from fcic in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                   where fcic.ItemCodeID == itemCodeID && fcic.FolioCuantificacionID.ToString() == folioCuantificacionID && fcic.Activo
                                   select fcic.TieneNumerosUnicos).AsParallel().FirstOrDefault();
                    }
                    return tieneNU;
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
        /// Eliminar/Cancelar un Item Code de grid de materiales o bulto
        /// </summary>
        /// <param name="folioAvisoLlegadaID">folio de llegada</param>
        /// <param name="folioCuantificacionID">folio cuantificacion</param>
        /// <param name="BultoID">bulto id</param>
        /// <param name="ItemCode">item code</param>
        /// <param name="usuario">usuario regisstrado</param>
        /// <returns>estatus de exito o error</returns>
        public object EliminarItemCode(string folioAvisoLlegadaID, string folioCuantificacionID, string BultoID, string ItemCode, string detalleBulto, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int itemCodeID = ItemCode != null ? Convert.ToInt32(ItemCode) : 0;
                    string strItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;
                    if (strItemCode.Contains("Bulto"))
                    {
                        //Elimino de Rel Bulto
                        Sam3_Rel_Bulto_ItemCode relBulto = ctx.Sam3_Rel_Bulto_ItemCode.Where(x => x.Rel_Bulto_ItemCode_ID.ToString() == BultoID && x.Activo).AsParallel().SingleOrDefault();
                        if (relBulto != null)
                        {
                            if (!ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.Rel_Bulto_ItemCode_ID == relBulto.Rel_Bulto_ItemCode_ID && x.Activo).Any())
                            {
                                relBulto.Activo = false;
                                relBulto.UsuarioModificacion = usuario.UsuarioID;
                                relBulto.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("El ItemCode ya cuenta con Números únicos");
                            }
                        }
                        //Elimino de bulto 
                        Sam3_Bulto bulto = (from rbi in ctx.Sam3_Rel_Bulto_ItemCode
                                        join b in ctx.Sam3_Bulto on rbi.BultoID equals b.BultoID
                                        where rbi.Rel_Bulto_ItemCode_ID.ToString() == BultoID
                                        && b.FolioCuantificacionID.ToString() == folioCuantificacionID
                                        select b).AsParallel().SingleOrDefault();
                            //ctx.Sam3_Bulto
                            //.Where(x => x.BultoID.ToString() == BultoID && x.FolioCuantificacionID.ToString() == folioCuantificacionID && x.Activo).AsParallel().SingleOrDefault();

                        bulto.Activo = false;
                        bulto.UsuarioModificacion = usuario.UsuarioID;
                        bulto.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();
                    }
                    else if(detalleBulto == "1") //esta en el detalle bulto
                    {
                        //Elimino de Rel Bulto
                        Sam3_Rel_Bulto_ItemCode bulto = ctx.Sam3_Rel_Bulto_ItemCode
                            .Where(x => x.Rel_Bulto_ItemCode_ID.ToString() == BultoID && x.Activo).AsParallel().SingleOrDefault();

                        if (!ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.Rel_Bulto_ItemCode_ID == bulto.Rel_Bulto_ItemCode_ID && x.Activo).Any())
                        {
                            bulto.Activo = false;
                            bulto.UsuarioModificacion = usuario.UsuarioID;
                            bulto.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("El ItemCode ya cuenta con Números únicos");
                        }
                    }
                    else
                    {
                        //Elimino de Rel FolioCuantificacion_ItemCode
                        Sam3_Rel_FolioCuantificacion_ItemCode itemCode = ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                            .Where(x => x.Rel_FolioCuantificacion_ItemCode_ID == itemCodeID && x.FolioCuantificacionID.ToString() == folioCuantificacionID)
                            .AsParallel().SingleOrDefault();

                        if (!ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.Rel_FolioCuantificacion_ItemCode_ID == itemCode.Rel_FolioCuantificacion_ItemCode_ID && x.Activo).Any())
                        {
                            itemCode.Activo = false;
                            itemCode.UsuarioModificacion = usuario.UsuarioID;
                            itemCode.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("El ItemCode ya cuenta con Números únicos");
                        }
                    }

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
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
    }
}