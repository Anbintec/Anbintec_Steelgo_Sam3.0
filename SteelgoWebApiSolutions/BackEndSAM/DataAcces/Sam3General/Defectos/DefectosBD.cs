using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Defectos
{
    public class DefectosBd
    {
        private static readonly object _mutex = new object();
        private static DefectosBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DefectosBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static DefectosBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DefectosBd();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Retorna la lista de los defectos dependiendo el tipo de prueba
        /// </summary>
        /// <param name="lenguaje">lenguaje de defectos a retornar</param>
        /// <param name="TipoPrueba">descripcion del tipo de prueba a filtrar</param>
        /// <returns> objeto del tipo Sam3_Steelgo_Get_Defectos_Result</returns>
        public object listadoDefectos(string lenguaje, string TipoPrueba)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_Defectos_Result> lista = ctx.Sam3_Steelgo_Get_Defectos(lenguaje, TipoPrueba).ToList();
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
    }
}