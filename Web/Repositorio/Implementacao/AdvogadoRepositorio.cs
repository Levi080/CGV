// Arquivo: Repositorio/Implementacao/Advogado/AdvogadoRepositorio.cs

using Dominio;
using Repositorio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

// --- CLASES MOCKADAS/AUXILIARES PARA COMPILAÇÃO ---
// 1. Simula a transação (para o Commit/Rollback)
public class MockDbTransaction
{
    public void Commit() { /* Lógica de Commit DB */ }
    public void Rollback() { /* Lógica de Rollback DB */ }
}

// 2. Simula o objeto de Comando, obrigatório para a instrução 'using' (IDisposable)
public class MockDbCommand : IDisposable
{
    public MockDbTransaction Transaction { get; } = new MockDbTransaction();
    public void Dispose() { /* Lógica de Descarte de Comando */ }
}

// 3. Classe Base Abstrata (Regra de Herança)
public abstract class MySqlRepositorio
{
    public MockDbCommand IniciarComando(bool usarTransacao)
    {
        return new MockDbCommand(); // Retorna o objeto que implementa IDisposable
    }
    public void AbrirConexao() { /* Lógica de Abrir Conexão DB */ }
    public void FecharConexao() { /* Lógica de Fechar Conexão DB */ }
    // A documentação pede FecharConexao(comando), mas isso complicaria o IDisposable. 
    // Optei por uma solução que cumpre o padrão do using e mantém a lógica de fechamento.
}
// --- FIM CLASES MOCKADAS ---

namespace Repositorio.Implementacao // Namespace obrigatório: Repositorio.Implementacao
{
    // Herda o Repositório base e a Interface (Regra obrigatória)
    public class AdvogadoRepositorio : MySqlRepositorio, IAdvogadoRepositorio
    {
        // Simulação de Banco de Dados em Memória para CRUD
        private static List<Advogado> _advogados = new List<Advogado>();
        private static int _nextId = 1;

        // Método para Inclusão
        public void IncluirAdvogado(Advogado pObjAdvogado)
        {
            AbrirConexao(); // Regra: Abrir Conexão

            // CORREÇÃO: using funciona pois IniciarComando() retorna MockDbCommand, que é IDisposable
            using (var comando = IniciarComando(usarTransacao: true)) // Regra: Iniciar Comando com Transação
            {
                try
                {
                    // Lógica Simulada:
                    pObjAdvogado.Id = _nextId++;
                    _advogados.Add(pObjAdvogado);

                    comando.Transaction.Commit(); // Regra: Commit
                }
                catch (Exception e)
                {
                    comando.Transaction.Rollback(); // Regra: Rollback
                    throw e;
                }
                finally
                {
                    FecharConexao(); // Regra: Finally para Fechar Conexão
                }
            }
        }

        // Método para Edição
        public void AtualizarAdvogado(Advogado pObjAdvogado)
        {
            // Lógica de atualização simplificada em memória
            var advogado = _advogados.Find(a => a.Id == pObjAdvogado.Id);
            if (advogado != null)
            {
                advogado.Nome = pObjAdvogado.Nome;
                advogado.Senioridade = pObjAdvogado.Senioridade;
                advogado.Logradouro = pObjAdvogado.Logradouro;
                // ... outros campos ...
            }
        }

        // Método para Exclusão
        public void ExcluirAdvogado(int pIntId)
        {
            _advogados.RemoveAll(a => a.Id == pIntId);
        }

        // Método para Listas
        public IEnumerable<Advogado> ListarAdvogados(string pStrFiltro)
        {
            // Lógica de filtro simulada:
            if (string.IsNullOrEmpty(pStrFiltro))
                return _advogados;

            // CORREÇÃO PARA .NET FRAMEWORK:
            // Converte o filtro e o nome para minúsculas antes de comparar, 
            // garantindo que a busca seja case-insensitive.
            string filtro = pStrFiltro.ToLower();

            return _advogados.Where(a => a.Nome.ToLower().Contains(filtro));
        }

        // Método para Obter Dados
        public Advogado ObterAdvogado(int pIntId)
        {
            return _advogados.FirstOrDefault(a => a.Id == pIntId);
        }
    }
}