using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
namespace CapaDatos
{
    public class PacienteDAL : CadenaDAL
    {
       public List<PacienteCLS> ListarPaciente()
        {
            List<PacienteCLS> lista = new List<PacienteCLS>();

            using (SqlConnection cn = new SqlConnection(this.cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from PACIENTES where BHABILITADO = 1", cn))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            PacienteCLS opacienteCLS;
                            lista = new List<PacienteCLS>();
                            while (dr.Read())
                            {
                                opacienteCLS = new PacienteCLS();
                                opacienteCLS.idPaciente = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                                opacienteCLS.nombre = dr.IsDBNull(1) ? "" : dr.GetString(1);
                                opacienteCLS.apellido = dr.IsDBNull(2) ? "" : dr.GetString(2);
                                opacienteCLS.fechaNacimiento = dr.IsDBNull(3) ? (DateTime?)null : dr.GetDateTime(3);
                                opacienteCLS.telefono = dr.IsDBNull(4) ? "" : dr.GetString(4);
                                opacienteCLS.email = dr.IsDBNull(5) ? "" : dr.GetString(5);
                                opacienteCLS.direccion = dr.IsDBNull(6) ? "" : dr.GetString(6);

                                lista.Add(opacienteCLS);
                            }
                        }


                    }

                }
                catch
                {
                    cn.Close();
                    lista = null;
                    throw;
                }
                
            }
            return lista;
        }



    }
}
