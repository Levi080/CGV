using Dominio;
using Dominio.Enums;
using Repositorio.Interface;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.Util;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AdvogadoController : Controller
    {
        private readonly IAdvogadoRepositorio _advogadoRepositorio;

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
                var advogado = _advogadoRepositorio.ObterAdvogado(pIntId.Value);

                if (advogado != null)
                {
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

            viewModel.SenioridadeList = SelectListItemConverter.CreateSelectList<SenioridadeEnum>();
            viewModel.EstadoList = SelectListItemConverter.CreateSelectList<EstadoEnum>();

            return PartialView("_FormularioParcial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(AdvogadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro inesperado ao salvar: " + ex.Message);
                }
            }

            return View("_FormularioParcial", viewModel);
        }

        [HttpPost]
        public ActionResult Excluir(int pIntId)
        {
            try
            {
                _advogadoRepositorio.ExcluirAdvogado(pIntId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Erro interno ao excluir: " + ex.Message });

            }

        }
    }
}