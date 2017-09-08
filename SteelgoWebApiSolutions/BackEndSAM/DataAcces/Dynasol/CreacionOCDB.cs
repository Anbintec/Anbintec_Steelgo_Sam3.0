using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.CreacionOC;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class CreacionOCDB
    {

        private static readonly object _mutex = new object();
        private static CreacionOCDB _instance;
        public static CreacionOCDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CreacionOCDB();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerClienteByOrdenCompraID(int OrdenCompraID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ClienteClass> ListaCliente = new List<ClienteClass>();
                    ListaCliente.Add(new ClienteClass());
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
                        ListaMonedas.Add(new MonedaClass
                        {
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


        public object ValidaCreacionOC(string ordenCompra, int clienteID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    
                    ObjectParameter op = new ObjectParameter("Retorna", typeof(string));
                    op.Value = null;
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var res = ctx.Sam3_OrdenCompra_Cobrar_GET_ValidaCreacion(ordenCompra,clienteID, oMyString);
                    var data = oMyString.Value.ToString();
                    

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");

                    if (data.Equals("ok"))
                        result.ReturnMessage.Add("nueva");
                    else
                        result.ReturnMessage.Add("existe");

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

        public object GuardaOC(DataTable Datos, int clienteID, string OrdenCompra, int moneda, int monedaPago, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", UsuarioID.ToString() },
                        { "@OrdenCompra", OrdenCompra.ToString() },
                        { "@ClienteID", clienteID.ToString() },
                        { "@MonedaID", moneda.ToString() },
                        { "@MonedaPagoID", monedaPago.ToString() }
                    };
                    _SQL.Ejecuta(Stords.GUARDACAPTURA_CREACIONOC, Datos, "@DATOS", Parametros);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    //result.ReturnMessage.Add(cerrada + "");
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