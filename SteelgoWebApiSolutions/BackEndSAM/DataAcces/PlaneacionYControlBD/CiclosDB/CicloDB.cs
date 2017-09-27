using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.PlaneacionYControlBD.CiclosDB
{
    public class CicloDB
    {
        private static readonly object _mutex = new object();
        private static CicloDB _instance;
        public static CicloDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CicloDB();
                    }
                }
                return _instance;
            }
        }
        public object Etapas()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var etapas = ctx.Sam3_PYC_Etapa.Select(x => new { x.EtapaId, x.Etapa }).ToList();
                    return etapas;
                }

            }
            catch (Exception ex)
            {
                TransactionalInformation result = null; result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;

            }
        }
        public object FamiliaMaterial()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    string pw = "+PWHT";
                    //var familiaMateriales = ctx.Sam3_FamiliaMaterial.Select(x => new { x.FamiliaMaterialID, x.Nombre }).ToList();
                    //generar columnas para el datatable 
                    ObjetosSQL objSql = new ObjetosSQL();
                    DataTable dtNombresFamiliaMaterial = objSql.Tabla("Sam3_Ciclos_Get_NombresFamiliaMaterial");
                    DataTable dtNombresFamiliaAcero = objSql.Tabla("Sam3_Ciclos_Get_NombresFamiliaAcero");
                    //crear dataTable nuevo que contendra las combinaciones
                    DataTable dtCiclos = new DataTable();
                    DataRow drCiclos = null;
                    // string[] columnasFamiliaMaterial = dtNombresFamiliaMaterial.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                    // string[] columnasAceros = dtNombresFamiliaAcero.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();

                    foreach (DataColumn dcx in dtNombresFamiliaMaterial.Columns)
                    {
                        foreach (DataRow dr in dtNombresFamiliaMaterial.Rows)
                        {
                            //agegrar nueva columna de la familia material
                            DataColumn dcCilosFamiliaMaterial = new DataColumn(dcx.ColumnName);
                            dtCiclos.Columns.Add(dcCilosFamiliaMaterial);

                            //agegrar nueva columna de PWHT
                            DataColumn dcCilosPWHT = new DataColumn(dcx.ColumnName + pw);
                            dtCiclos.Columns.Add(dcCilosPWHT);

                            //crear la fila para ir colocando el IdFamiliaMaterial 
                            if (dcx.Ordinal == 0)
                            {
                                drCiclos = dtCiclos.NewRow();
                                drCiclos[dcx.ColumnName] = dr.Field<int>(dcx);
                                drCiclos[dcx.ColumnName + pw] = 0;
                                dtCiclos.Rows.Add(drCiclos);
                            }
                            else
                            {
                                drCiclos[dcx.ColumnName] = dr.Field<int>(dcx);
                                drCiclos[dcx.ColumnName + pw] = 0;

                            }
                            foreach (DataColumn dcx1 in dtNombresFamiliaAcero.Columns)
                            {
                                foreach (DataRow dr2 in dtNombresFamiliaAcero.Rows)
                                {
                                    ////comparar los valores del IdFamiliaMaterial con relacion a la Familia de Acero
                                    if (dr.Field<int>(dcx) == dr2.Field<int>(dcx1))
                                    {
                                        ////agegrar nueva columna de la familia material Aceros
                                        DataColumn dcCilosFamiliaMaterialAcero = new DataColumn(dcx + "_" + dcx1.ColumnName);
                                        dtCiclos.Columns.Add(dcCilosFamiliaMaterialAcero);

                                        ////agegrar nueva columna de PWHT Aceros
                                        DataColumn dcCilosPWHTAceros = new DataColumn(dcx + "_" + dcx1.ColumnName + pw);
                                        dtCiclos.Columns.Add(dcCilosPWHTAceros);

                                        ////colocar id de la columna
                                        drCiclos[dcx + "_" + dcx1.ColumnName] = dr2.Field<int>(dcx1);
                                        drCiclos[dcx + "_" + dcx1.ColumnName + pw] = 0;
                                    }
                                }
                            }
                        }
                    }
                    return dtCiclos;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = null; result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;

            }
        }
        public object FamiliaAcero()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var familiaAceros = ctx.Sam3_FamiliaAcero.Select(x =>
                            new { x.FamiliaMaterialID, x.FamiliaAceroID, x.Nombre }).ToList();
                    return familiaAceros;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = null; result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }

        }

    }
}