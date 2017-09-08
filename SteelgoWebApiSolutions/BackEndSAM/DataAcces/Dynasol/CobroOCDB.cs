using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.CobroOC;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class CobroOCDB
    {
        private static readonly object _mutex = new object();
        private static CobroOCDB _instance;
        public static CobroOCDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CobroOCDB();
                    }
                }
                return _instance;
            }
        }


        public object ObtenerDetalleCobroOC(int OrdenCompra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Cobro> listaRecepcion = new List<Cobro>();
                    List<Sam3_OrdenCompra_GET_CobroOC_Result> result = ctx.Sam3_OrdenCompra_GET_CobroOC(OrdenCompra).ToList();
                    foreach (Sam3_OrdenCompra_GET_CobroOC_Result item in result)
                    {
                        listaRecepcion.Add(new Cobro
                        {
                            Accion = 2,
                            RevisionID = item.RevisionID,
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            ColadaID = item.ColadaID,
                            Partida = item.Partida,
                            ItemCode = item.ItemCode,
                            Descripcion = item.Descripcion,
                            MaterialNorma = item.MaterialNorma,
                            Diametro1 = (float)item.Diametro1,
                            Diametro2 = (float)item.Diametro2,                                                        
                            Colada = item.Colada,
                            CantC = item.CantC.GetValueOrDefault(),
                            CantG = item.CantG.GetValueOrDefault(),
                            CantS = item.CantS.GetValueOrDefault(),
                            Cobrado = item.Cobrado.GetValueOrDefault(),
                            Deficit = item.Deficit.GetValueOrDefault(),                            
                            RowOk = true,
                            ModificadoPorUsuario = false
                        });
                    }
                    return listaRecepcion;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerCatalogoMoneda()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<MonedaClass> ListaMonedas = new List<MonedaClass>();
                    List<Sam3_OrdenCompra_GET_CatalogoMonedas_Result> result = ctx.Sam3_OrdenCompra_GET_CatalogoMonedas().ToList();
                    ListaMonedas.Add(new MonedaClass());
                    foreach (Sam3_OrdenCompra_GET_CatalogoMonedas_Result item in result)
                    {
                        ListaMonedas.Add(new MonedaClass {
                            MonedaID = item.MonedaID,
                            Moneda = item.Moneda,
                            Descripcion = item.Descripcion
                        });
                    }
                    return ListaMonedas;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
        public object ObtenerClienteByOrdenCompraID(int OrdenCompraID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ClienteClass> ListaCliente = new List<ClienteClass>();
                    List<Sam3_OrdenCompra_GET_ClienteByOC_Result> result = ctx.Sam3_OrdenCompra_GET_ClienteByOC(OrdenCompraID).ToList();
                    foreach (Sam3_OrdenCompra_GET_ClienteByOC_Result item in result)
                    {
                        ListaCliente.Add(new ClienteClass
                        {
                            ClienteID = item.ClienteID,
                            Cliente = item.Cliente
                        });
                    }
                    return ListaCliente;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
     
        public object GuardaCliente(int OrdenCompraID, int usuarioID, string Nombre, string Direccion, string Ciudad, string Estado, string Pais)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //ObjectResult<int?> elementos = ctx.Sam3_OrdenCompra_SET_GuardaCliente(OrdenCompraID, usuarioID, Nombre, Direccion == null ? "" : Direccion, Ciudad == null ? "": Ciudad, Estado == null ? "" : Ciudad,  Pais == null ? "" : Pais);
                    var valor = 1;/*elementos.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];*/
                    return valor;
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

        public object GuardaOC(DataTable Datos,int OrdenCompraID,int cerrada, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectParameter op = new ObjectParameter("Retorna", typeof(string));
                    op.Value = null;
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var res = ctx.Sam3_OrdenCompra_Cobrar_GET_OrdenCompraCerrada(OrdenCompraID, oMyString);
                    var data = oMyString.Value.ToString();
                    
                    if (data.Equals("ok"))
                    {
                        cerrada = 1;
                    }


                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", UsuarioID.ToString() },
                        { "@Cerrar", cerrada.ToString() },
                        { "@OrdenCompraID", OrdenCompraID.ToString() }
                    };
                    _SQL.Ejecuta(Stords.GUARDACAPTURA_COBROOC, Datos, "@DATOS", Parametros);
                    


                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnMessage.Add(cerrada+"");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

    }
}