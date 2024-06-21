$(document).ready(function () {
    $("#SetConnectionSetings").click(function (event) {
        axios.get('/Home/GetConnectionView')
            .then(function (response) {
                if (response.data.success) {
                    window.location.href = '/Home/Connection';
                } else {
                    alert("No se puede encontrar la pagina.");
                }
            })
            .catch(function () {
                alert("Ocurrio un error, intente otra vez.");
            });
    });

    $("#subirArchivo").click(function (event) {
        axios.get('/Home/GetConnectionSettings')
            .then(function (response) {
                if (response.data.success) {
                    window.location.href = '/Home/SubirArchivo?';
                } else {
                    alert("No hay configuracion guardada.");
                    window.location.href = '/Home/Connection';
                }
            })
            .catch(function () {
                alert("Ocurrio un error, intentelo otra vez.");
            });
    });
    $("#btnMostrarSuvidos").click(function (event) {
        axios.get('/Home/Listar')
            .then(function (response) {
                console.log(response);
                // Redirigir a la vista Lista
                window.location.href = '/Home/Listar';
            })
            .catch(function (error) {
                console.error('Error al cargar la lista:', error);
            });
    });
});