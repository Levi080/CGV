var AdvogadoViewModel = (function ($) {

    var AdvogadoViewModel_AoCarregarComponente = function () {
    };

    var AdvogadoViewModel_FormatarCampos = function () {
        $.validator.unobtrusive.parse('#formSalvarAdvogado');

        $('#cep').mask('00000-000');

        $('#numero').on('keydown', function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)) {
                return;
            }
            e.preventDefault();
        });

        $('#formSalvarAdvogado').on('submit', AdvogadoViewModel_AoSubmeterFormularioAjax);
    };

    var AdvogadoViewModel_AoSubmeterFormularioAjax = function (e) {
        e.preventDefault();

        var $form = $(this);
        AdvogadoViewModel_AoLimparCampos($form);

        if ($form.valid()) {

            var $btnSalvar = $form.find('button[type="submit"]');
            $btnSalvar.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Salvando...');

            $.ajax({
                url: $form.attr('action'),
                type: $form.attr('method'),
                data: $form.serialize(),

                success: function (data, status, xhr) {
                    window.location.reload();
                },
                error: function (xhr) {
                    if (xhr.status === 400) {
                        $('#formularioContainer').html(xhr.responseText);
                        AdvogadoViewModel_FormatarCampos();
                    } else {
                        alert("Ocorreu um erro inesperado ao salvar: " + xhr.statusText);
                    }
                },
                complete: function () {
                    $btnSalvar.prop('disabled', false).html('<i class="fas fa-save"></i> Salvar');
                }
            });
        }
    };

    var AdvogadoViewModel_AoLimparCampos = function ($form) {

        var $cep = $form.find('#cep');
        var cep_valor = $cep.val();
        if (cep_valor) {
            $cep.val(cep_valor.replace(/\D/g, ''));
        }

        var $numero = $form.find('#numero');
        var numero_valor = $numero.val();
        if (numero_valor) {
            $numero.val(numero_valor.replace(/\D/g, ''));
        }
    };

    var AdvogadoViewModel_Confirmar = function (id) {
        if (confirm("Tem certeza que deseja excluir este Advogado?")) {

            $.post('/Advogado/Excluir', { pIntId: id }, function (data) {
                if (data && data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                } else {
                    window.location.reload();
                }
            }).fail(function () {
                alert("Erro ao tentar excluir o Advogado.");
            });
        }
    };

    return {
        AdvogadoViewModel_AoCarregarComponente: AdvogadoViewModel_AoCarregarComponente,
        AdvogadoViewModel_FormatarCampos: AdvogadoViewModel_FormatarCampos,
        AdvogadoViewModel_AoLimparCampos: AdvogadoViewModel_AoLimparCampos,
        AdvogadoViewModel_Confirmar: AdvogadoViewModel_Confirmar
    };
})(jQuery);

var AdvogadoViewModel_AoCarregarComponente = AdvogadoViewModel.AdvogadoViewModel_AoCarregarComponente;
var AdvogadoViewModel_Confirmar = AdvogadoViewModel.AdvogadoViewModel_Confirmar;
var AdvogadoViewModel_FormatarCampos = AdvogadoViewModel.AdvogadoViewModel_FormatarCampos;