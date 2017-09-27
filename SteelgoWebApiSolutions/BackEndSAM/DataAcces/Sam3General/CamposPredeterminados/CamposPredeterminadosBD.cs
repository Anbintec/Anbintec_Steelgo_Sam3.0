using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BackEndSAM.DataAcces.Sam3General.CamposPredeterminados
{
    public class CamposPredeterminadosBD
    {
        private static readonly object _mutex = new object();
        private static CamposPredeterminadosBD _instance;


        public static CamposPredeterminadosBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CamposPredeterminadosBD();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Retorna el valor del campo predeterminado tomando en consideración el identificador del mismo y el lenguaje
        /// </summary>
        /// <param name="lenguaje">lenguaje en que se retorna</param>
        /// <param name="id">identificador del Campo predeterminado</param>
        /// <returns></returns>
        public object ObtenerValorCampoPredeterminado(string lenguaje, int id)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var result = ctx.Sam3_Steelgo_Get_CampoPredeterminado(id, lenguaje, oMyString);
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
    }
}