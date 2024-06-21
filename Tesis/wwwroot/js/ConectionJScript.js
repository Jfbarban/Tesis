$(document).ready(function () {
    $("#netProxy").change(function () {
        var selectedOption = $(this).val();
        if (selectedOption == "Si") {
            $("#proxySettings").show();
        } else {
            $("#proxySettings").hide();
        }
    });

    $("#formGroup").submit(function (event) {
        event.preventDefault(); // Prevent traditional form submission

        var formData = {
            url: $("#url").val(),
            user: $("#user").val(),
            password: $("#password").val(),
            netProxy: $("#netProxy").val() === "Si",
            proxyIP: $("#proxyIP").val(),
            proxyPort: $("#proxyPort").val(),
            userProxy: $("#userProxy").val(),
            passProxy: $("#passProxy").val()
        };

        axios.post('/Home/SetConnectionSettings', formData, {
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                if (response.data.success) {
                    alert('Configuración guardada satisfactoriamente!');
                    window.location.href = '/Home/Index?message=Configuración%20guardada%20satisfactoriamente!';
                } else {
                    alert('Fallo al guardar la configuración');
                }
            })
            .catch(function () {
                alert('Ocurrió un error. Intente otra vez.');
            });
    });
});