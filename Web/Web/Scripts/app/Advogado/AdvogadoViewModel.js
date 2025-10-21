// Arquivo: Web/Scripts/app/Advogado/AdvogadoViewModel.js (CÓDIGO COMPLETO E CORRIGIDO)

// Usa a nomenclatura correta: Nome do arquivo (AdvogadoViewModel) + função
var AdvogadoViewModel = (function ($) {

    // 1. Função de Carregamento Principal (Regra: nome de arquivo + _AoCarregarComponente())
    var AdvogadoViewModel_AoCarregarComponente = function () {
        // CORREÇÃO: LIGA A FUNÇÃO DE LIMPEZA AO SUBMIT DO FORMULÁRIO
        $('form').submit(AdvogadoViewModel_AoSubmeterFormulario);
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

    // 3. NOVO MÉTODO: Limpeza antes da Submissão (Desmascaramento)
    var AdvogadoViewModel_AoSubmeterFormulario = function () {

        // Limpar o CEP: remove o hífen e deixa só números
        var $cep = $('#cep');
        var cep_valor = $cep.val();
        if (cep_valor) {
            // Remove todos os caracteres não numéricos
            $cep.val(cep_valor.replace(/\D/g, ''));
        }

        // Limpar o Número: remove qualquer caracter que possa ter entrado, garantindo apenas dígitos
        var $numero = $('#numero');
        var numero_valor = $numero.val();
        if (numero_valor) {
            $numero.val(numero_valor.replace(/\D/g, ''));
        }

        // Retorna true para permitir que o formulário continue com o POST.
        return true;
    };


    // 4. Função de Confirmação para Exclusão (Regra: nome de arquivo + _Confirmar())
    var AdvogadoViewModel_Confirmar = function (id) {
        if (confirm("Tem certeza que deseja excluir este Advogado?")) {

            // Regra: Usar $post para comunicação com as rotas do controller
            $.post('/Advogado/Excluir', { pIntId: id }, function (data) {
                // Redireciona ou recarrega
                if (data.redirectUrl) {