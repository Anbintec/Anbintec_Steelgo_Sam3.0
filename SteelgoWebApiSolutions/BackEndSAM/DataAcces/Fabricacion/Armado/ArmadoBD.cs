using BackEndSAM.Models.Armado;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace BackEndSAM.DataAcces.ArmadoBD
{
    public class ArmadoBD
    {
        private static readonly object _mutex = new object();

        private static ArmadoBD _instance;
        public object MostrarCapturaArmado(int avisoLlegadaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public static ArmadoBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ArmadoBD();
                    }
                }
                return _instance;
            }
        }



        public object ObtenerIDOrdenTrabajo(Sam3_Usuario usuario, string ordentrabajo, int tipo, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_SpoolID_Result> lista = ctx.Sam3_Steelgo_Get_SpoolID(tipo, ordentrabajo, lenguaje).ToList();// Sam3_Steelgo_Get_SpoolID(tipo, ordentrabajo).ToList();
                    return lista;

                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerValorFecha(Sam3_Usuario usuario, string lenguaje, int idCampoPredeterminado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var result = ctx.Sam3_Steelgo_Get_CampoPredeterminado(idCampoPredeterminado, lenguaje, oMyString);
                    var data = oMyString.Value.ToString();
                    return data;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }



        public object listaNumeroUnicos(int juntaID, Sam3_Usuario usuario, int pagina, string sinCaptura)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Armado_Get_MaterialesSpool_Result> listaDetallaTrabajoAdicionalJson = ctx.Sam3_Armado_Get_MaterialesSpool(juntaID, int.Parse(sinCaptura), pagina).ToList();
                    return listaDetallaTrabajoAdicionalJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
        public object DetallaArmadoAdicional(int JuntaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Armado_Get_DetalleTrabajoAdicional_Result> listaDetallaTrabajoAdicionalJson = ctx.Sam3_Armado_Get_DetalleTrabajoAdicional(JuntaID).ToList();
                    return listaDetallaTrabajoAdicionalJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerDetalleArmado(DetalleCaptura JsonCaptura, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Armado_Get_Detalle_Result> listaDetalleDatosJson = ctx.Sam3_Armado_Get_Detalle(int.Parse(JsonCaptura.IdVal), lenguaje, JsonCaptura.JuntaID).ToList();
                    return listaDetalleDatosJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerJuntasXSpoolID(Sam3_Usuario usuario, string ordenTrabajo, string id, int sinCaptura)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_JuntaSpool_Result> lista = ctx.Sam3_Steelgo_Get_JuntaSpool(sinCaptura, int.Parse(id), 1).ToList();
                    List<JuntaSpool> listaJuntaSpool = new List<JuntaSpool>();
                    if (lista.Count > 0)
                    {
                        listaJuntaSpool.Add(new JuntaSpool());
                        foreach (Sam3_Steelgo_Get_JuntaSpool_Result item in lista)
                        {
                            JuntaSpool juntaSpool = new JuntaSpool
                            {
                                Etiqueta = item.Etiqueta,
                                JuntaSpoolID = item.JuntaSpoolID
                            };
                            listaJuntaSpool.Add(juntaSpool);
                        }
                    }
                    return listaJuntaSpool.OrderBy(x => x.Etiqueta).ToList<JuntaSpool>();
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerTuberoXProyecto(Sam3_Usuario usuario, int idProyecto, int tipo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_Obrero_Result> result = new List<Sam3_Steelgo_Get_Obrero_Result>();
                    result.Add(new Sam3_Steelgo_Get_Obrero_Result { Codigo = "", NombreCompleto = "", ObreroID = 0, TipoObrero = "" });
                    List<Sam3_Steelgo_Get_Obrero_Result> lista = ctx.Sam3_Steelgo_Get_Obrero(tipo, "Tubero", idProyecto, null, null).ToList();
                    foreach (Sam3_Steelgo_Get_Obrero_Result item in lista)
                    {
                        result.Add(new Sam3_Steelgo_Get_Obrero_Result
                        {
                            TipoObrero = item.TipoObrero,
                            ObreroID = item.ObreroID,
                            NombreCompleto = item.NombreCompleto,
                            Codigo = item.Codigo
                        });
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerTallerXPoryecto(Sam3_Usuario usuario, int idProyecto)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_SteelGo_Get_Taller_Result> result = new List<Sam3_SteelGo_Get_Taller_Result>();
                    result.Add(new Sam3_SteelGo_Get_Taller_Result
                    {
                        TallerID = 0,
                        Nombre = ""
                    });
                    List<Sam3_SteelGo_Get_Taller_Result> lista = ctx.Sam3_SteelGo_Get_Taller(idProyecto).ToList();
                    foreach (Sam3_SteelGo_Get_Taller_Result item in lista)
                    {
                        result.Add(new Sam3_SteelGo_Get_Taller_Result
                        {
                            Nombre = item.Nombre,
                            TallerID = item.TallerID
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }


        public object InsertarCapturaArmado(DataTable dtDetalleCaptura, DataTable dtTrabajosAdicionales, DataTable dtNumeroUnicoAsignado, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };

                    _SQL.Ejecuta(Stords.GUARDARCAPTURAARMADO, dtTrabajosAdicionales, "@TrabajosAdicionales", dtDetalleCaptura, "@Armado", dtNumeroUnicoAsignado, "@NumeroUnicoAsignado", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");

                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object listaTrabajosAdicionalesXJunta(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_TrabajoAdicional_Result> lista = ctx.Sam3_Steelgo_Get_TrabajoAdicional("Armado").ToList();
                    List<TrabajosAdicionalesXJunta> listaTrabajoAdicionalXJUnta = new List<TrabajosAdicionalesXJunta>();
                    if (lista.Count > 0)
                    {
                        listaTrabajoAdicionalXJUnta.Add(new TrabajosAdicionalesXJunta());
                        foreach (Sam3_Steelgo_Get_TrabajoAdicional_Result item in lista)
                        {
                            TrabajosAdicionalesXJunta trabajoAdicional = new TrabajosAdicionalesXJunta
                            {
                                NombreCorto = item.NombreCorto,
                                SignoInformativo = item.SignoInformativo,
                                TrabajoAdicionalID = item.TrabajoAdicionalID
                            };
                            listaTrabajoAdicionalXJUnta.Add(trabajoAdicional);
                        }
                    }
                    return listaTrabajoAdicionalXJUnta;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ActualizaDatos(DataTable dtInformacionActualizar, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Lenguaje", lenguaje } };

                    DataTable dt = _SQL.EjecutaDataAdapter(Stords.ARMADOACTUALIZARINFORMACION, dtInformacionActualizar, "@ArmadoActualizado", parametro);

                    //TransactionalInformation result = new TransactionalInformation();
                    //result.ReturnMessage.Add("Ok");

                    //result.ReturnCode = 200;
                    //result.ReturnStatus = true;
                    //result.IsAuthenicated = true;

                    return dt;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ValidarNumerosUnicos(DataTable dtValida)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    

                    DataTable dtSpoolsInvalidos= _SQL.EjecutaDataAdapter(Stords.GUARDARCAPTURAARMADOVALIDANUMEROSUNICOS, dtValida, "@NumerosUnicos");

                    List<string> listaSpoolInvalidos = new List<string>();

                    for (int i = 0; i < dtSpoolsInvalidos.Rows.Count; i++)
                    {
                        listaSpoolInvalidos.Add(dtSpoolsInvalidos.Rows[i][0].ToString());
                    }

                    return listaSpoolInvalidos;
                }
            }
            catch (Exception ex)
            {
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