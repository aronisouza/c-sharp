using System.Data.SqlClient;

namespace Locadora.DBL
{
    class Conexao
    {
        private SqlConnection conn = null;

        public SqlConnection GetConexao()
        {
            Connection();
            return conn;
        }

        private void Connection()
        {
            if (conn == null)
            {
                conn = new SqlConnection(@"Data Source=FILID\SQLSERVER14;Initial Catalog=Locadora;Integrated Security=True");
            }
        }

    }
}
