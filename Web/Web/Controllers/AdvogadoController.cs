using System.Web.Mvc;
using System.Linq;
using Repositorio.Interface;
using Web.ViewModels;
using Dominio;

namespace Web.Controllers
{
    public class AdvogadoController : Controller
    {
        private readonly IAdvogadoRepositorio _advogadoRepositorio;

        // Construtor para Injeção de Dependência
        public AdvogadoController(IAdvogadoRepositorio pObjAdvogadoRepositorio)
        {
            _advogadoRepositorio = pObjAdvogadoRepositorio;
        }

        public ActionResult Index(string pStrFiltro)
        {
            var listaAdvogados = _advogadoRepositorio.ListarAdvogados(pStrFiltro);

            // Mapeamento (Dominio -> ViewModel)
            var viewModels = listaAdvogados.Select(a => new AdvogadoViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Senioridade = a.Senioridade,
                Logradouro = a.Logradouro,
                Bairro = a.Bairro,
                Estado = a.Estado,
                Cep = a.Cep,
                Numero = a.Numero.ToString(),
                Complemento = a.Complemento
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public ActionResult Formulario(int? pIntId)
        {
            AdvogadoViewModel viewModel = new AdvogadoViewModel();

            if (pIntId.HasValue && pIntId.Value > 0)
            {
                // Regra de Método para Obter Dados: Obterxxx
                var advogado = _advogadoRepositorio.ObterAdvogado(pIntId.Value);

                if (advogado != null)
                {
                    // Mapeamento (Dominio -> ViewModel)
                    viewModel.Id = advogado.Id;
                    viewModel.Nome = advogado.Nome;
                    viewModel.Senioridade = advogado.Senioridade;
                    viewModel.Logradouro = advogado.Logradouro;
                    viewModel.Bairro = advogado.Bairro;
                    viewModel.Estado = advogado.Estado;
                    viewModel.Cep = advogado.Cep;
                    viewModel.Numero = advogado.Numero.ToString();
                    viewModel.Complemento = advogado.Complemento;
                }
            }

            return View(new AdvogadoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Formulario(AdvogadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Mapeamento (ViewModel -> Dominio)
                var advogado = new Advogado
                {
                    Id = viewModel.Id,
                    Nome = viewModel.Nome,
                    Senioridade = viewModel.Senioridade,
                    Logradouro = viewModel.Logradouro,
                    Bairro = viewModel.Bairro,
                    Estado = viewModel.Estado,
                    Cep = viewModel.Cep,
                    // Conversão de volta para INT
                    Numero = int.TryParse(viewModel.Numero, out int num) ? num : 0,
                    Complemento = viewModel.Complemento
                };

                if (advogado.Id == 0)
                    _advogadoRepositorio.IncluirAdvogado(advogado);              
                else
                    _advogadoRepositorio.AtualizarAdvogado(advogado);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Excluir(int pIntId)
        {
            // Regra de Método para exclusão: Excluirxxxx
            _advogadoRepositorio.ExcluirAdvogado(pIntId);
            return RedirectToAction("Index");
        }
    }
}