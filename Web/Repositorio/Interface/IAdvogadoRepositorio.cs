using Dominio.Advogado;
using System.Collections.Generic;

namespace Repositorio.Interface
{
    public interface IAdvogadoRepositorio
    {
        void IncluirAdvogado(Advogado pObjAdvogado);

        void AtualizarAdvogado(Advogado pObjAdvogado);

        void ExcluirAdvogado(int pIntId);

        IEnumerable<Advogado> ListarAdvogados(string pStrFiltro);

        Advogado ObterAdvogado(int pIntId);
    }
}
