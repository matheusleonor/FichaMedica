$.ajax({
    url: "/Funcoes/GetFormData",
    method: "GET",
    dataType: "json",
    success: function (data) {
        var select = $("#selectFuncao"); // Use o ID correto do seu elemento select
        select.empty();
        $.each(data, function (index, item) {
            select.append($("<option>").val(item.funcaoId).text(item.nomeFuncao)); // Certifique-se de que os nomes das propriedades estejam corretos
        });
    }
});
function MostrarErro() {
    $('#entrarContaModal').modal('show');
}


function formatCpf(input) {
    var value = input.value.replace(/\D/g, ''); // Remove caracteres não numéricos
    if (value.length > 3) {
        value = value.substring(0, 3) + '.' + value.substring(3);
    }
    if (value.length > 7) {
        value = value.substring(0, 7) + '.' + value.substring(7);
    }
    if (value.length > 11) {
        value = value.substring(0, 11) + '-' + value.substring(11, 13);
    }
    input.value = value;
}

function formatPhoneNumber(phoneInput) {
    let phone = phoneInput.value.replace(/\D/g, ''); // Remove non-digit characters
    if (phone.length === 11) {
        phoneInput.value = phone.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
    } else if (phone.length === 10) {
        phoneInput.value = phone.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
    }
}


function applyBold() {
    let textArea = document.getElementById("TextoRico");
    let selectedText = textArea.value.substring(textArea.selectionStart, textArea.selectionEnd);
    let newText = `<strong>${selectedText}</strong>`;
    let updatedText = textArea.value.substring(0, textArea.selectionStart) + newText + textArea.value.substring(textArea.selectionEnd);
    textArea.value = updatedText;
}

function applyItalic() {
    let textArea = document.getElementById("TextoRico");
    let selectedText = textArea.value.substring(textArea.selectionStart, textArea.selectionEnd);
    let newText = `<em>${selectedText}</em>`;
    let updatedText = textArea.value.substring(0, textArea.selectionStart) + newText + textArea.value.substring(textArea.selectionEnd);
    textArea.value = updatedText;
}

function validateForm() {
    var cpfInput = document.getElementById("Cpf");
    var celularInput = document.getElementById("Celular");

    if (cpfInput.value.length !== 14) {
        alert("Por favor, digite um CPF válido.");
        return false; // Impede o envio do formulário
    }

    if (celularInput.value.length !== 15) {
        alert("Por favor, digite um número de celular válido.");
        return false; // Impede o envio do formulário
    }

    return true; // Permite o envio do formulário
}

$(document).ready(function () {
    $('.texto-rico').each(function () {
        let html = $(this).html();
        let formattedHtml = html.replace(/<strong>(.*?)<\/strong>/g, '<strong style="font-weight: bold;">$1</strong>');
        $(this).html(formattedHtml);
    });
});

function previewImage(input) {
    var preview = document.getElementById('preview');
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        preview.style.display = 'none';
    }
}
