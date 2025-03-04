using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TratamientoBL
    {
        private readonly TratamientoDAL _tratamientoDAL;
        

        public TratamientoBL(TratamientoDAL tratamientoDAL)
        {
            _tratamientoDAL = tratamientoDAL;
            
        }

        public List<TratamientoCLS> ListarTratamiento()
        {
            return _tratamientoDAL.ListarTratamiento();
        }

        public TratamientoCLS? RecuperarTratamiento(int idTratamiento)
        {
            return _tratamientoDAL.RecuperarTratamiento(idTratamiento);
        }


        public void EliminarTratamiento(int idTratamiento)
        {
            _tratamientoDAL.EliminarTratamiento(idTratamiento);

        }

        public void GuardarTratamiento(TratamientoCLS tratamiento)
        {
            _tratamientoDAL.GuardarTratamiento(tratamiento);
        }


    }
}
