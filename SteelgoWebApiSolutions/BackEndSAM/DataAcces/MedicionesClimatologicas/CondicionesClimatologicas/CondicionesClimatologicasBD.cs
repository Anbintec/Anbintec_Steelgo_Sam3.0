using BackEndSAM.Models.MedicionesClimatologicas;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.MedicionesClimatologicas.CondicionesClimatologicas
{
    public class CondicionesClimatologicasBD
    {
        private static readonly object _mutex = new object();
        private static CondicionesClimatologicasBD _instance;

        public static CondicionesClimatologicasBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CondicionesClimatologicasBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerEquiposDeToma(int usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_EquiposDeToma_Result> result = ctx.Sam3_Steelgo_Get_EquiposDeToma(usuario).ToList();
                    List<DetalleEquipos> listaDetalle = new List<DetalleEquipos>();
                    List<EquiposDeToma> listaEquipos = new List<EquiposDeToma>();
                    listaEquipos.Add(new EquiposDeToma());
                    listaEquipos[0].Equipos.Add(new ListasEquipos());



                    foreach (Sam3_Steelgo_Get_EquiposDeToma_Result item in result)
                    {
                        listaDetalle.Add(new DetalleEquipos
                        {
                            EquipoTomaID = item.EquipoTomaID,
                            NombreEquipo = item.NombreEquipo,
                            NombreCategoria = item.NombreCategoria
                        });
                    }

                    for (int i = 0; i < listaDetalle.Count; i++)
                    {
                        if (listaDetalle[i].NombreCategoria.Equals("Humedad"))
                        {
                            listaEquipos[0].Equipos[0].EquiposHumedad.Add(listaDetalle[i]);
                        }
                        else if (listaDetalle[i].NombreCategoria.Equals("Punto Rocio"))
                        {
                            listaEquipos[0].Equipos[0].EquiposPuntoRocio.Add(listaDetalle[i]);
                        }
                        else if (listaDetalle[i].NombreCategoria.Equals("Temperatura Ambiente"))
                        {
                            listaEquipos[0].Equipos[0].EquiposTemperaturaAmbiente.Add(listaDetalle[i]);
                        }
                        else if (listaDetalle[i].NombreCategoria.Equals("Campo X"))
                        {
                            listaEquipos[0].Equipos[0].EquiposCampoX.Add(listaDetalle[i]);
                        }
                    }

                    listaEquipos[0].Equipos[0].EquiposHumedad.Insert(0, new DetalleEquipos());
                    listaEquipos[0].Equipos[0].EquiposPuntoRocio.Insert(0, new DetalleEquipos());
                    listaEquipos[0].Equipos[0].EquiposTemperaturaAmbiente.Insert(0, new DetalleEquipos());
                    listaEquipos[0].Equipos[0].EquiposCampoX.Insert(0, new DetalleEquipos());

                    return listaEquipos;
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

        public object InsertarCapturaInspeccion(DataTable dtDetalleCaptura, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GuardarCondicionesClimatologias, dtDetalleCaptura, "@TablaCaptura", parametro);
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

        public object ObtenerCondicionClimatologica( string fechatoma, string horatoma, int patioid, int zonaid, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_GET_CC_Result> listadoElementos = ctx.Sam3_Pintura_GET_CC(fechatoma, horatoma, patioid, zonaid, lenguaje).ToList();

                    List<DetalleCondicionesClimatologicas> listaDetalleCondicionesClimatologicas = new List<DetalleCondicionesClimatologicas>();
                    if (listadoElementos.Count > 0)
                    {
                        foreach (Sam3_Pintura_GET_CC_Result item in listadoElementos)
                        {
                            listaDetalleCondicionesClimatologicas.Add( new DetalleCondicionesClimatologicas {
                                TempAmb=item.TempAmb,
                                NombreEquipoTemAmb=item.NombreEquipoTemAmb,
                                Humedad=item.Humedad,
                                NombreEquipoHumedad=item.NombreEquipoHumedad,
                                PuntoRocio=item.PuntoRocio,
                                NombreEquipoPuntoRocio=item.NombreEquipoPuntoRocio,
                                CampoX=item.CampoX,
                                NombreEquipoCampoX=item.NombreEquipoCampoX
                            });
                        }
                    }

                    return listaDetalleCondicionesClimatologicas;
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