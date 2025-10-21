// Arquivo: Web/Scripts/app/Advogado/AdvogadoViewModel.js

// Usa a nomenclatura correta: Nome do arquivo (AdvogadoViewModel) + função
var AdvogadoViewModel = (function ($) {

    // 1. Função de Carregamento Principal (Regra: nome de arquivo + _AoCarregarComponente())
    var AdvogadoViewModel_AoCarregarComponente = function () {
        AdvogadoViewModel_FormatarCampos();
    };

    // 2. Função de Formatação (Regra: nome de arquivo + _FormatarCampos(formulario))
    var AdvogadoViewModel_FormatarCampos = function () {
        // Aplicação das máscaras
        $('#cep').mask('00000-000');

        // Impede letras no campo Número, garantindo apenas dígitos.
        $('#numero').on('keydown', function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)) {
                return;
            }
            e.preventDefault();
        });
    };

    // 3. Função de Confirmação para Exclusão (Regra: nome de arquivo + _Confirmar())
    var AdvogadoViewModel_Confirmar = function (id) {
        if (confirm("Tem certeza que deseja excluir este Advogado?")) {

            // Regra: Usar $post para comunicação com as rotas do controller
            $.post('/Advogado/Excluir', { pIntId: id }, function (data) {
                // Redireciona ou recarrega
                if (data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                } else {
                    window.location.reload();
                }
            }).fail(function () {
                alert("Erro ao tentar excluir o Advogado.");
            });
        }
    };

    // Retorna as funções públicas para acesso do HTML
    return {
        AdvogadoViewModel_AoCarregarComponente: AdvogadoViewModel_AoCarregarComponente,
        AdvogadoViewModel_FormatarCampos: AdvogadoViewModel_FormatarCampos,
        AdvogadoViewModel_Confirmar: AdvogadoViewModel_Confirmar
    };
})(jQuery);

// Permite o acesso global no HTML (necessário para chamar as funções diretamente)
var AdvogadoViewModel_AoCarregarComponente = AdvogadoViewModel.AdvogadoViewModel_AoCarregarComponente;
var AdvogadoViewModel_Confirmar = AdvogadoViewModel.AdvogadoViewModel_Confirmar;