using Dominio.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.ViewModels
{
    public class AdvogadoViewModel
    {
        public AdvogadoViewModel()
        {
            // Inicializar as listas como vazias para evitar NullReferenceException
            SenioridadeList = new SelectList(new List<SelectListItem>());
            EstadoList = new SelectList(new List<SelectListItem>());
        }

        public int Id { get; set; }

        // Nome do Advogado
        [Required(ErrorMessage = "O campo Nome do Advogado é obrigatório.")]
        [Display(Name = "Nome do Advogado")]
        public string Nome { get; set; }

        // Senioridade
        [Required(ErrorMessage = "A Senioridade é obrigatória.")]
        [Display(Name = "Senioridade")]
        public SenioridadeEnum Senioridade { get; set; }

        // Endereço - Logradouro
        [Required(ErrorMessage = "O Logradouro é obrigatório.")]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        // Endereço - Bairro
        [Required(ErrorMessage = "O Bairro é obrigatório.")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        // Endereço - Estado (Combo)
        [Required(ErrorMessage = "O Estado é obrigatório.")]
        [Display(Name = "Estado")]
        public EstadoEnum Estado { get; set; }

        // Endereço - CEP (Máscara)
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        // Endereço - Número
        [Required(ErrorMessage = "O Número é obrigatório.")]
        [Display(Name = "Número")]
        // Usamos string aqui porque o input virá como texto, e o jQuery/máscara tratará para só aceitar números.
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        public SelectList SenioridadeList { get; set; }
        public SelectList EstadoList { get; set; }
    }
}