using BackEndSAM.Models.Fabricacion.Soldadura;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BackEndSAM.DataAcces.Fabricacion.Soldadura
{
    public class SoldaduraBD
    {

        private static readonly object _mutex = new object();
        private static SoldaduraBD _instance;

        public static SoldaduraBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SoldaduraBD();
                    }
                }
                return _instance;
            }
        }


        public object ObtenerTrabajosAdicionales(int juntaSpoolID)
        {
            try
            {
                List<TrabajosAdicionalesSoldadura> listaTrabajosAdicionalesSoldadura = new List<TrabajosAdicionalesSoldadura>();

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_TrabajoAdicional_Result> result = ctx.Sam3_Steelgo_Get_TrabajoAdicional("Soldadura").ToList();

                    foreach (Sam3_Steelgo_Get_TrabajoAdicional_Result item in result)
                    {
                        listaTrabajosAdicionalesSoldadura.Add(new TrabajosAdicionalesSoldadura
                        {
                            TrabajoAdicionalID = item.TrabajoAdicionalID,
                            TrabajoAdicional = item.NombreCorto,
                            SignoInformativo = item.SignoInformativo
                        });
                    }

                    return listaTrabajosAdicionalesSoldadura;
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

        public object ObtenerListadoWPS(int proyectoID, int procesoRaizID, int procesoRellenoID, decimal espesor,int pwht, string lenguaje)
        {
            try
            {
                List<WPS> listaWPS = new List<WPS>();
                listaWPS.Add(new WPS());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_Get_WPS_Proyecto_Result> result = ctx.Sam3_Soldadura_Get_WPS_Proyecto(proyectoID,procesoRaizID,procesoRellenoID,espesor,pwht, lenguaje).ToList();

                    foreach (Sam3_Soldadura_Get_WPS_Proyecto_Result item in result)
                    {
                        listaWPS.Add(new WPS
                        {
                            WPSID = item.WPSID,
                            WPSNombre = item.Certificado + " "+ item. WPSNombre,
                            EspesorMaximo = item.EspesorMaximo.GetValueOrDefault(),
                            EspesorMinimo = item.EspesorMinimo.GetValueOrDefault(),
                            GrupoMaterialBase1RaizD = item.GrupoMaterialBase1RaizD,
                            GrupoMaterialBase1RaizDID = item.GrupoMaterialBase1RaizDID.GetValueOrDefault(),
                            GrupoMaterialBase1RaizU = item.GrupoMaterialBase1RaizU,
                            GrupoMaterialBase1RaizUID = item.GrupoMaterialBase1RaizUID.GetValueOrDefault(),
                            GrupoMaterialBase1RellenoD = item.GrupoMaterialBase1RellenoD,
                            GrupoMaterialBase1RellenoDID = item.GrupoMaterialBase1RellenoDID.GetValueOrDefault(),
                            GrupoMaterialBase1RellenoU = item.GrupoMaterialBase1RellenoU,
                            GrupoMaterialBase1RellenoUID = item.GrupoMaterialBase1RellenoUID.GetValueOrDefault(),
                            NombrePQRRaiz = item.NombrePQRRaiz,
                            PQRRaizId = item.PQRRaizId.GetValueOrDefault(),
                            NombrePQRRelleno = item.NombrePQRRelleno,
                            PQRRellenoId = item.PQRRellenoId.GetValueOrDefault(),
                            ProcesoSoldaduraRaiz = item.ProcesoSoldaduraRaiz,
                            ProcesoSoldaduraRelleno = item.ProcesoSoldaduraRelleno,
                            PREHEAT = item.PREHEAT,
                            PWHT = item.PWHT,
                            Certificado = item.Certificado == ""? "1": item.Certificado,
                            SignoInformativo = item.Certificado == "" ? "MTrabajoAdicionalMas": ""
                        });
                    }
                    return listaWPS.OrderByDescending(x => x.Certificado).OrderBy(x => x.WPSNombre).ToList<WPS>();
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

        public object ObtenerListadoSoldadoresCertificados(int tipoProceso, int procesoSoldadura, decimal espesor, decimal diametro, string lenguaje)
        {
            try
            {
                List<ObreroSoldador> listaSoldadoresCertificados = new List<ObreroSoldador>();
                listaSoldadoresCertificados.Add(new ObreroSoldador());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_SoldadorCertificado_Result> result = ctx.Sam3_Steelgo_Get_SoldadorCertificado(espesor,procesoSoldadura,diametro,tipoProceso).ToList();
                    
                    foreach (Sam3_Steelgo_Get_SoldadorCertificado_Result item in result)
                    {
                        listaSoldadoresCertificados.Add(new ObreroSoldador
                        {
                            ObreroID = item.ObreroID,
                            Soldador = item.Soldador,
                            Certificado = item.Certificado
                        });
                    }
                    return listaSoldadoresCertificados.OrderBy(x => x.Soldador).ToList<ObreroSoldador>();
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

        public object ObtenerListadoColadas(int procesoSoldadura, int esRaiz, int wPSID, int familiaMaterialID, string lenguaje)
        {
            try
            {
                List<Consumible> listaSoldadoresCertificados = new List<Consumible>();
                listaSoldadoresCertificados.Add(new Consumible());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_Get_Colada_Result> result = ctx.Sam3_Soldadura_Get_Colada(procesoSoldadura, esRaiz, wPSID, familiaMaterialID, lenguaje).ToList();

                    foreach (Sam3_Soldadura_Get_Colada_Result item in result)
                    {
                        listaSoldadoresCertificados.Add(new Consumible
                        {
                            ConsumibleID = item.ConsumibleID,
                            Colada = item.Colada + " "+ item.Certificado,
                            Certificado = item.Certificado
                        });
                    }
                    return listaSoldadoresCertificados.OrderBy(x => x.Colada).ToList<Consumible>();
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

        public object ObtenerSoldadoresRaizCapturados(string idOrdenTrabajo, string ordenTrabajoSpoolID, int JuntaSoldaduraID, int esRaiz)
        {
            try
            {
                List<Soldadores> listaProcesosSoldadura = new List<Soldadores>();
               
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_GET_DetalleSoldadorColadas_Result> result = ctx.Sam3_Soldadura_GET_DetalleSoldadorColadas( JuntaSoldaduraID,esRaiz).ToList();

                    

                    foreach (Sam3_Soldadura_GET_DetalleSoldadorColadas_Result item in result)
                    {
                        listaProcesosSoldadura.Add(new Soldadores
                        {
                            Accion = 2,
                            Colada = item.Colada,
                            ColadaID = item.ColadaID,
                            ObreroID = item.ObreroID,
                            Observaciones = item.Comentario,
                            Soldador = item.Obrero,
                            JuntaSoldaduraID = item.JuntaSoldaduraID,
                            JuntaSoldaduraSoldadoID = item.JuntaSoldaduraSoldadoID,
                            ListaSoldador = new List<ObreroSoldador>()
                        });
                    }
                    return listaProcesosSoldadura;
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
        public object ObtenerProcesosSoldadura()
        {
            try
            {
                List<ProcesoSoldadura> listaProcesosSoldadura = new List<ProcesoSoldadura>();
                listaProcesosSoldadura.Add(new ProcesoSoldadura());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_PQR_ProcesoSoldadura_Result> result = ctx.Sam3_Cat_PQR_ProcesoSoldadura(2).ToList();

                    foreach (Sam3_Cat_PQR_ProcesoSoldadura_Result item in result)
                    {
                        listaProcesosSoldadura.Add(new ProcesoSoldadura
                        {
                            ProcesoSoldaduraID = item.ProcesoSoldaduraID,
                            Codigo = item.Codigo,
                        });
                    }
                    
                    return listaProcesosSoldadura;
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

        //public object ObtenerListadoSoldadoresCertificados(int procesoSoldaduraID, string tipoJunta, decimal diametro, decimal espesor, string cedula, int proceso, int proyecto)
        //{
        //    try
        //    {
        //        List<SoldadorRaizCertificado> listaTrabajosAdicionalesSoldadura = new List<SoldadorRaizCertificado>();

        //        using (SamContext ctx = new SamContext())
        //        {
        //            List<Sam3_Steelgo_Get_SoldadorCertificado_Result> result = ctx.Sam3_Steelgo_Get_SoldadorCertificado(espesor, procesoSoldaduraID, diametro, proyecto, proceso).ToList();

        //            foreach (Sam3_Steelgo_Get_SoldadorCertificado_Result item in result)
        //            {
        //                listaTrabajosAdicionalesSoldadura.Add(new SoldadorRaizCertificado
        //                {
        //                    ObreroID = item.ObreroID,
        //                    Soldador = item.Mostrar
        //                });
        //            }
        //            return listaTrabajosAdicionalesSoldadura;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;

        //        return result;
        //    }
        //}

        //public object ObtenerListadoRaiz(int procesoSoldaduraID, string tipoJunta, decimal diametro, decimal espesor, string cedula, int proceso, int proyecto)
        //{
        //    try
        //    {
        //        List<SoldadorRaizCertificado> listaTrabajosAdicionalesSoldadura = new List<SoldadorRaizCertificado>();

        //        using (SamContext ctx = new SamContext())
        //        {
        //            List<Sam3_Steelgo_Get_SoldadorCertificado_Result> result = ctx.Sam3_Steelgo_Get_SoldadorCertificado(espesor, procesoSoldaduraID, diametro, proyecto, proceso).ToList();

        //            foreach (Sam3_Steelgo_Get_SoldadorCertificado_Result item in result)
        //            {
        //                listaTrabajosAdicionalesSoldadura.Add(new SoldadorRaizCertificado
        //                {
        //                    ObreroID = item.ObreroID,
        //                    Soldador = item.Mostrar
        //                });
        //            }
        //            return listaTrabajosAdicionalesSoldadura;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;

        //        return result;
        //    }
        //}

        //public object ObtenerListadoRelleno(int procesoSoldaduraID, string tipoJunta, decimal diametro, decimal espesor, string cedula, int proceso, int proyecto)
        //{
        //    try
        //    {
        //        List<SoldadorRaizCertificado> listaTrabajosAdicionalesSoldadura = new List<SoldadorRaizCertificado>();

        //        using (SamContext ctx = new SamContext())
        //        {
        //            List<Sam3_Steelgo_Get_SoldadorCertificado_Result> result = ctx.Sam3_Steelgo_Get_SoldadorCertificado(espesor, procesoSoldaduraID, diametro, proyecto, proceso).ToList();

        //            foreach (Sam3_Steelgo_Get_SoldadorCertificado_Result item in result)
        //            {
        //                listaTrabajosAdicionalesSoldadura.Add(new SoldadorRaizCertificado
        //                {
        //                    ObreroID = item.ObreroID,
        //                    Soldador = item.Mostrar
        //                });
        //            }
        //            return listaTrabajosAdicionalesSoldadura;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;

        //        return result;
        //    }
        //}



        public object ObtenerListadoSoldadores(int tipo)
        {
            try
            {
                List<ObreroSoldador> listaSoldadura = new List<ObreroSoldador>();

                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Steelgo_Get_Obrero_Result> listresult = ctx.Sam3_Steelgo_Get_Obrero(tipo, "Soldador", null, null, null).ToList();

                    if(listresult.Count>0)
                    listaSoldadura.Add(new ObreroSoldador());

                    foreach (Sam3_Steelgo_Get_Obrero_Result item in listresult)
                    {
                        listaSoldadura.Add(new ObreroSoldador
                        {
                            ObreroID = item.ObreroID,
                            Soldador = item.Codigo,
                        });
                    }


                    return listaSoldadura;
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


        public object MostrarCapturaSoldadura(int avisoLlegadaID)
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

        public object AgregarDetalleCapturaSoldadura(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    return new object();

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

                    //ObjectParameter objectParameter = new ObjectParameter("Retorna", typeof(String));


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

        public object DetallaSoldaduraAdicional(int JuntaSoldaduraID, Sam3_Usuario usuario)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_Get_DetalleTrabajoAdicional_Result> listaDetallaTrabajoAdicionalJson = ctx.Sam3_Soldadura_Get_DetalleTrabajoAdicional(JuntaSoldaduraID).ToList();

                    List<TrabajosAdicionalesSoldadura> result = new List<TrabajosAdicionalesSoldadura>();
                    foreach (Sam3_Soldadura_Get_DetalleTrabajoAdicional_Result item in listaDetallaTrabajoAdicionalJson)
                    {
                        result.Add(new TrabajosAdicionalesSoldadura {
                            Accion = 2,
                            JuntaSoldaduraID = item.JuntaSoldaduraID,
                            SoldaduraTrabajoAdicionalID = item.SoldaduraTrabajoAdicionalID,
                            ObreroID = item.ObreroID,
                            Soldador = item.Soldador,
                            TrabajoAdicional = item.TrabajoAdicional,
                            TrabajoAdicionalID = item.TrabajoAdicionalID,
                            Observacion = item.Observacion
                            
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
      

        public object ObtenerDetalleSoldadura(string JuntaID, Sam3_Usuario usuario, string lenguaje)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_Get_DetalleJunta_Result> listaDetalleDatosJson = ctx.Sam3_Soldadura_Get_DetalleJunta(int.Parse(JuntaID), lenguaje).ToList();
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
                    List<Sam3_Steelgo_Get_JuntaSpool_Result> lista = ctx.Sam3_Steelgo_Get_JuntaSpool(sinCaptura, int.Parse(id), 2).ToList();
                    return lista.OrderBy(x => x.Etiqueta).ToList<Sam3_Steelgo_Get_JuntaSpool_Result>();
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

        public object ObtenerTrabajosXProyecto(Sam3_Usuario usuario, int idProyecto)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_Get_DetalleTrabajoAdicional_Result> lista = ctx.Sam3_Soldadura_Get_DetalleTrabajoAdicional(idProyecto).ToList();
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
                    List<Sam3_SteelGo_Get_Taller_Result> lista = ctx.Sam3_SteelGo_Get_Taller(idProyecto).ToList();

                    List<Taller> ListaTalleres = (from Talleres in ctx.Sam3_SteelGo_Get_Taller(idProyecto)
                                                                            select new Taller
                                                                            {
                                                                                Nombre= Talleres.Nombre,
                                                                                TallerID=Talleres.TallerID
                                                                            }).AsParallel().ToList().OrderBy(x => x.Nombre).ToList<Taller>();
                    ListaTalleres.Insert(0, new Taller());

                    return ListaTalleres;
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
                    List<Sam3_Steelgo_Get_TrabajoAdicional_Result> lista = ctx.Sam3_Steelgo_Get_TrabajoAdicional("Soldadura").ToList();
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

        public object InsertarCapturaSoldadura(DataTable dtDetalleCaptura, DataTable dtTrabajosAdicionales, DataTable dtSoldaduraSoldado, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    //ctx.Sam3_Armado_JuntaArmado()
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };

                    _SQL.Ejecuta(Stords.GUARDARCAPTURASOLDADURA, dtDetalleCaptura, "@Soldadura", dtSoldaduraSoldado, "@SoldaduraSoldador", dtTrabajosAdicionales, "@TrabajosAdicionales", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add("xd");
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
    }
}