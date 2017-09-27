using BackEndSAM.Models.ConfiguracionSoldadura.PQR;
using BackEndSAM.Models.ConfiguracionSoldadura.WPS;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BackEndSAM.DataAcces.ConfiguracionSoldadura
{
    public class WPSBd
    {


        private static readonly object _mutex = new object();
        private static WPSBd _instance;


        public static WPSBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new WPSBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerCatalogoWPS(int TipoDato)
        {
            using (SamContext ctx = new SamContext())
            {
                List<WPS> data = (from WPS in ctx.Sam3_Soldadura_WPS(TipoDato)
                                         select new WPS
                                         {
                                             Accion = 2,
                                             WPSID = WPS.WPSID,
                                             WPSNombre = WPS.WPSNombre,
                                             WPSNombreOriginal = WPS.WPSNombre,

                                             PQRRaizId = Convert.ToInt32(WPS.PQRRaizId),
                                             NombrePQRRaiz = WPS.NombrePQRRaiz,
                                             PQRRellenoId = Convert.ToInt32(WPS.PQRRellenoId),
                                             NombrePQRRelleno = WPS.NombrePQRRelleno,

                                             GrupoPRaiz = WPS.GrupoMaterialBase1RaizU + " " + WPS.GrupoMaterialBase1RaizD,
                                             GrupoPRelleno = WPS.GrupoMaterialBase1RellenoU + " " + WPS.GrupoMaterialBase1RellenoD,

                                             ProcesoSoldaduraRaiz = WPS.ProcesoSoldaduraRaiz,
                                             ProcesoSoldaduraRelleno = WPS.ProcesoSoldaduraRelleno,

                                             RaizEspesorRaiz = WPS.RaizEspesorRaiz.GetValueOrDefault(),
                                             RaizEspesorRelleno = WPS.RaizEspesorRelleno,
                                             RellenoEspesorRaiz = WPS.RellenoEspesorRaiz.GetValueOrDefault(),
                                             RellenoEspesorRelleno = WPS.RellenoEspesorRelleno,
                                             EspesorMaximoRaiz =WPS.EspesorMaximoRaiz.GetValueOrDefault(),
                                             EspesorMaximoRelleno = WPS.EspesorMaximoRelleno.GetValueOrDefault(),
                                             PWHTRaizId = Convert.ToInt32(WPS.PWHTId),
                                             PWHTRaiz = WPS.PWHTId,
                                             PWHTRellenoId = Convert.ToInt32(WPS.PWHTId),
                                             PWHTRelleno = WPS.PWHTId,

                                             PREHEATRaizId = Convert.ToInt32(WPS.PREHEATId),
                                             PREHEATRaiz = WPS.PREHEATId,
                                             PREHEATRellenoId = Convert.ToInt32(WPS.PREHEATId),
                                             PREHEATRelleno = WPS.PREHEATId,
                                             CVNRaiz = WPS.CVN.GetValueOrDefault(),
                                             CVNRelleno = WPS.CVN.GetValueOrDefault(),

                                             GrupoMaterialBase1RaizD = WPS.GrupoMaterialBase1RaizD,
                                             GrupoMaterialBase1RaizDID = WPS.GrupoMaterialBase1RaizDID.GetValueOrDefault(),
                                             GrupoMaterialBase1RaizU = WPS.GrupoMaterialBase1RaizU,
                                             GrupoMaterialBase1RaizUID = WPS.GrupoMaterialBase1RaizUID.GetValueOrDefault(),
                                             GrupoMaterialBase1RellenoD = WPS.GrupoMaterialBase1RellenoD,
                                             GrupoMaterialBase1RellenoDID = WPS.GrupoMaterialBase1RellenoDID.GetValueOrDefault(),
                                             GrupoMaterialBase1RellenoU = WPS.GrupoMaterialBase1RellenoU,
                                             GrupoMaterialBase1RellenoUID = WPS.GrupoMaterialBase1RellenoUID.GetValueOrDefault(),

                                             EspesorMaximo = WPS.EspesorMaximo.GetValueOrDefault(),
                                             EspesorMinimo = WPS.EspesorMinimo.GetValueOrDefault(),

                                             listadoRaizPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),
                                             listadoRellenoPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),
                                             RowOk = true,
                                             EditadoUsuario = false,
                                             RegistrosWPS = WPS.RegistrosWPS.GetValueOrDefault(), 
                                             QuitarCVN = WPS.QuitarCVN.GetValueOrDefault() ? 1 : 0

                                         }).AsParallel().ToList();
                data.Insert(0, new WPS());

                return data.OrderBy(x => x.WPSNombre).ToList<WPS>();
            }
        }
        public object ObtenerWPS(int TipoDato, Sam3_Usuario usuario)
        {

            using (SamContext ctx = new SamContext())
            {
                List<WPS> data = (from WPS in ctx.Sam3_Soldadura_WPS(TipoDato)
                                  select new WPS
                                  {
                                      Accion = 2,
                                      WPSID = WPS.WPSID,
                                      WPSNombre = WPS.WPSNombre,
                                      WPSNombreOriginal = WPS.WPSNombre,

                                      PQRRaizId = Convert.ToInt32(WPS.PQRRaizId),
                                      NombrePQRRaiz = WPS.NombrePQRRaiz,
                                      PQRRellenoId = Convert.ToInt32(WPS.PQRRellenoId),
                                      NombrePQRRelleno = WPS.NombrePQRRelleno,

                                      GrupoPRaiz = WPS.GrupoMaterialBase1RaizU + " " + WPS.GrupoMaterialBase1RaizD,
                                      GrupoPRelleno = WPS.GrupoMaterialBase1RellenoU + " " + WPS.GrupoMaterialBase1RellenoD,

                                      ProcesoSoldaduraRaiz = WPS.ProcesoSoldaduraRaiz,
                                      ProcesoSoldaduraRelleno = WPS.ProcesoSoldaduraRelleno,

                                      RaizEspesorRaiz = WPS.RaizEspesorRaiz.GetValueOrDefault(),
                                      RaizEspesorRelleno = WPS.RaizEspesorRelleno,
                                      RellenoEspesorRaiz = WPS.RellenoEspesorRaiz.GetValueOrDefault(),
                                      RellenoEspesorRelleno = WPS.RellenoEspesorRelleno,
                                      EspesorMaximoRaiz = WPS.EspesorMaximoRaiz.GetValueOrDefault(),
                                      EspesorMaximoRelleno = WPS.EspesorMaximoRelleno.GetValueOrDefault(),

                                      PWHTRaizId = Convert.ToInt32(WPS.PWHTId),
                                      PWHTRaiz = WPS.PWHTId,
                                      PWHTRellenoId = Convert.ToInt32(WPS.PWHTId),
                                      PWHTRelleno = WPS.PWHTId,

                                      CVNRaiz = WPS.CVN.GetValueOrDefault(),
                                      CVNRelleno = WPS.CVN.GetValueOrDefault(),

                                      PREHEATRaizId = Convert.ToInt32(WPS.PREHEATId),
                                      PREHEATRaiz = WPS.PREHEATId,
                                      PREHEATRellenoId = Convert.ToInt32(WPS.PREHEATId),
                                      PREHEATRelleno = WPS.PREHEATId,

                                      GrupoMaterialBase1RaizD = WPS.GrupoMaterialBase1RaizD,
                                      GrupoMaterialBase1RaizDID = WPS.GrupoMaterialBase1RaizDID.GetValueOrDefault(),
                                      GrupoMaterialBase1RaizU = WPS.GrupoMaterialBase1RaizU,
                                      GrupoMaterialBase1RaizUID = WPS.GrupoMaterialBase1RaizUID.GetValueOrDefault(),
                                      GrupoMaterialBase1RellenoD = WPS.GrupoMaterialBase1RellenoD,
                                      GrupoMaterialBase1RellenoDID = WPS.GrupoMaterialBase1RellenoDID.GetValueOrDefault(),
                                      GrupoMaterialBase1RellenoU = WPS.GrupoMaterialBase1RellenoU,
                                      GrupoMaterialBase1RellenoUID = WPS.GrupoMaterialBase1RellenoUID.GetValueOrDefault(),

                                      EspesorMaximo = WPS.EspesorMaximo.GetValueOrDefault(),
                                      EspesorMinimo = WPS.EspesorMinimo.GetValueOrDefault(),

                                      listadoRaizPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),
                                      listadoRellenoPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),
                                      RowOk= true,
                                      EditadoUsuario = false,
                                      RegistrosWPS = WPS.RegistrosWPS.GetValueOrDefault(),
                                      QuitarCVN = Convert.ToInt32(WPS.QuitarCVN)
                                  }).AsParallel().ToList();
                return data.OrderBy(x => x.WPSNombre).ToList<WPS>();
            }


        }

        //public object ObtenerJuntaWPS(int juntaID, Sam3_Usuario usuario)
        //{
        //    using (SamContext ctx = new SamContext())
        //    {
        //        List<WPS> data = (from WPS in ctx.Sam3_Soldadura_Get_WPSXJunta(juntaID, usuario)
        //                          select new WPS
        //                          {
        //                              Accion = 2,
        //                              WPSID = WPS.WPSID,
        //                              WPSNombre = WPS.WPSNombre,


        //                              PQRRellenoId = Convert.ToInt32(WPS.PQRRellenoId),
        //                              NombrePQRRelleno = WPS.NombrePQRRelleno,

        //                              EspesorMaximo = WPS.EspesorMaximo.GetValueOrDefault(),
        //                              EspesorMinimo = WPS.EspesorMinimo.GetValueOrDefault(),

        //                              listadoRaizPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),
        //                              listadoRellenoPQR = (List<DetallePQR>)PQRBd.ObtenerListadoPQRActivos(),

        //                          }).AsParallel().ToList();
        //        return data.OrderBy(x => x.WPSNombre).ToList<WPS>();
        //    }


        //}



        public object EliminaWPS(int TipoDeDato, int WPSIdentificador, int IdUsuario)
        {

            using (SamContext ctx = new SamContext())
            {
                //ObjectResult<Sam3_Soldadura_WPS_Result> ColeccionObjetResult = ctx.Sam3_Soldadura_WPS(TipoDeDato, null, null, null, null, null, null, null, null, null, null, null, IdUsuario, WPSIdentificador);

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;



            }




        }



        public object AgregarWPS(DataTable dtDetalleWPS,DataTable dtGruposP, Sam3_Usuario usuario)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    dtDetalleWPS.Columns.Remove("gruposCorrectos");
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() } };
                    int ID = _SQL.EjecutaInsertUpdate(Stords.GUARDAWPS, dtDetalleWPS, "@Tabla",dtGruposP, "@TablaGrupoP", parametro);


                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    if (ID > 0)
                    {
                        result.ReturnMessage.Add(ID.ToString());
                    }
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;

                }
            }
            catch (Exception ex)
            {

                TransactionalInformation lista = new TransactionalInformation();
                lista.ReturnMessage.Add(ex.Message);
                lista.ReturnCode = 500;
                lista.ReturnStatus = false;
                lista.IsAuthenicated = true;

                return lista;
            }




        }



        public object ValidarExisteWPS(int WPSID, string nombre)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectParameter op = new ObjectParameter("Retorna", typeof(string));
                    op.Value = null;
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var res = ctx.Sam3_Soldadura_WPS_Existe(WPSID, nombre, oMyString);
                    var data = oMyString.Value.ToString();
                    TransactionalInformation result = new TransactionalInformation();
                    if (data.Equals("ok"))
                    {
                        result.ReturnMessage.Add("OK");
                    }
                    else
                    {
                        result.ReturnMessage.Add("Error");
                    }

                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation lista = new TransactionalInformation();
                lista.ReturnMessage.Add(ex.Message);
                lista.ReturnCode = 500;
                lista.ReturnStatus = false;
                lista.IsAuthenicated = true;

                return lista;
            }

        }


    }
}