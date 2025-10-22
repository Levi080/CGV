using Dominio;
using Repositorio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

#region CLASES MOCKADAS
// Simula a transação (para o Commit/Rollback)
public class MockDbTransaction
{
    public void Commit() { }
    public void Rollback() { }
}

// Simula o objeto de Comando, obrigatório para a instrução 'using' (IDisposable)
public class MockDbCommand : IDisposable
{
    public MockDbTransaction Transaction { get; } = new MockDbTransaction();
    public void Dispose() { }
}

// Classe Base Abstrata (Regra de Herança)
public abstract class MySqlRepositorio
{
    public MockDbCommand IniciarComando(bool usarTransacao)
    {
        return new MockDbCommand(); // Retorna o objeto que implementa IDisposable
    }

    public void AbrirConexao() { }
    public void FecharConexao() { }
}
// --- FIM CLASES MOCKADAS ---
#endregion

namespace Repositorio.Implementacao
{
    public class AdvogadoRepositorio : MySqlRepositorio, IAdvogadoRepositorio
    {
        // Simulação de Banco de Dados em Memória para CRUD
        private static List<Advogado> _advogados = new List<Advogado>();
        private static int _nextId = 1;


        public void IncluirAdvogado(Advogado pObjAdvogado)
        {
            AbrirConexao(); 

            using (var comando = IniciarComando(usarTransacao: true))
            {
                try
                {
                    pObjAdvogado.Id = _nextId++;
                    _advogados.Add(pObjAdvogado);

                    comando.Transaction.Commit(); 
                }
                catch (Exception e)
                {
                    comando.Transaction.Rollback(); 
                    throw e;
                }
                finally
                {
                    FecharConexao(); 
                }
            }
        }

        public void AtualizarAdvogado(Advogado pObjAdvogado)
        {
            var advogado = _advogados.Find(a => a.Id == pObjAdvogado.Id);

            if (advogado != null)
            {
                advogado.Nome = pObjAdvogado.Nome;
                advogado.Senioridade = pObjAdvogado.Senioridade;
                advogado.Logradouro = pObjAdvogado.Logradouro;
                advogado.Cep = pObjAdvogado.Cep;
                advogado.Bairro = pObjAdvogado.Bairro;
                advogado.Numero = pObjAdvogado.Numero;
                advogado.Complemento = pObjAdvogado.Complemento;
                
            }
        }

        public void ExcluirAdvogado(int pIntId)
        {
            _advogados.RemoveAll(a => a.Id == pIntId);
        }

        public IEnumerable<Advogado> ListarAdvogados(string pStrFiltro)
        {
            // Lógica de filtro simulada:
            if (string.IsNullOrEmpty(pStrFiltro))
                return _advogados;

            string filtro = pStrFiltro.ToLower();

            return _advogados.Where(a => a.Nome.ToLower().Contains(filtro));
        }

        public Advogado ObterAdvogado(int pIntId)
        {
            return _advogados.FirstOrDefault(a => a.Id == pIntId);
        }
    }
}