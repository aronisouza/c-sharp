using System.Data.SqlClient;

namespace Locadora.DBL
{
    class Cadastrar : Conexao
    {
        private string _sqlText;
        private string _tabela;
        private string[] _syntax;
        private SqlConnection _conn;
        private int _retorno=0;
        private string col;
        private string val;

        public void ExeCadastro(string tabela, string[] syntax)
        {
            _tabela = tabela;
            _syntax = syntax;
            GetSyntax();
            Executar();
        }

        public int GetResut()
        {
            return _retorno;
        }

        // -- PRIVATE --
        private void GetSyntax()
        {
            col = string.Empty; val = string.Empty;
            for (int i = 0; i < _syntax.Length; i++)
            {
                string[] c = _syntax[i].Split('=');
                col = (i < _syntax.Length - 1) ? col + c[0] + ", " : col + c[0];
                val = (i < _syntax.Length - 1) ? val + "@" + c[0] + ", " : val + "@" + c[0];
            }
            _sqlText = "INSERT INTO " + _tabela + " (" + col + ") VALUES (" + val + ")";
        }

        private void Executar()
        {
            _conn = GetConexao();
            using (SqlCommand cmd = new SqlCommand(_sqlText, _conn))
            {
                for (int i = 0; i < _syntax.Length; i++)
                {
                    string[] v = _syntax[i].Split('=');
                    if (v[0] == "Valor") cmd.Parameters.AddWithValue("@" + v[0], decimal.Parse(v[1])); 
                    else cmd.Parameters.AddWithValue("@" + v[0], v[1]);
                }
                try
                {
                    _conn.Open();
                    _retorno = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    _conn.Close();
                }
            }
        }
    }
}
