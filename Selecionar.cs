using System;
using System.Data;
using System.Data.SqlClient;

namespace Locadora.DBL
{
    class Selecionar : Conexao
    {
        private string _sqlTxt;
        private string _tabela;
        private string _colunas;
        private DataTable _dt = null;
        private string[] _condicoes;
        private string _condicao;
        private SqlConnection _conn;
        
        // -- PUBLICOS
        public void ExeSelecionar(string tabela, string colunas, string[] condicoes = null)
        {
            _tabela = tabela;
            _colunas = colunas;
            _condicoes = condicoes;
            GetSyntax();
            Execute();
        }

        public DataTable GetTabela()
        {
            return _dt;
        }

        // -- PRIVADOS
        private void GetSyntax()
        {
            // -- Limpa a strings caso tenha mais de 1 consulta por execução do Programa
            _condicao = string.Empty;

            // - Pegando as condições
            // - Para não ter um condição de pesquisa deixar [null] na passagem de parametros
            // - na chamada do função(Método) na ação principal
            if (_condicoes == null)
            {
                _sqlTxt = "SELECT " + _colunas + " FROM " + _tabela;
            }
            else
            {
                for (int i = 0; i < _condicoes.Length; i++)
                {
                    if(_condicoes[i].Contains("=") == true)
                    {
                        string[] col = _condicoes[i].Split('=');
                        _condicao = (i < _condicoes.Length - 1) ? _condicao + col[0] + "=@" + col[0] + ", " : _condicao + col[0] + "=@" + col[0];
                    }
                    else
                    {
                        string[] col = _condicoes[i].Split(':');
                        _condicao = (i < _condicoes.Length - 1) ? _condicao + col[0] + " LIKE @" + col[0] + ", " : _condicao + col[0] + " LIKE @" + col[0];
                    }
                }
                _sqlTxt = "SELECT " + _colunas + " FROM " + _tabela + " WHERE " + _condicao;
            }
        }

        private void Execute()
        {
            _conn = GetConexao();
            using (SqlCommand cmd = new SqlCommand(_sqlTxt, _conn))
            {
                try
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    if (_condicoes != null)
                    {
                        for (int i = 0; i < _condicoes.Length; i++)
                        {
                            if (_condicoes[i].Contains("=") == true)
                            {
                                string[] s = _condicoes[i].Split('=');
                                cmd.Parameters.AddWithValue("@" + s[0], s[1]);
                            }
                            else
                            {
                                string[] s = _condicoes[i].Split(':');
                                cmd.Parameters.AddWithValue("@" + s[0], s[1]);
                            }
                        }
                    }
                    adp.Fill(dt);
                    _dt = dt;
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
