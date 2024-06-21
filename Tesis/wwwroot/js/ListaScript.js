$(document).ready(function () {
    $("#btnBorrar").click(function (event) {
        axios.get('/Home/BorrarLista')
            .then(function (response) {
                if (response.data.success) {
                    alert("Archivo de registro limpiado exitosamente.");
                    // Redirigir a la vista Lista
                    window.location.href = '/Home/Listar';
                } else {
                    alert("No se borro el fichero.");
                }
            })
            .catch(function (error) {
                console.error('Error al borrar la lista:', error);
            });
    });
});