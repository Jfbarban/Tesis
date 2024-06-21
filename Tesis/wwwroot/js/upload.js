document.addEventListener("DOMContentLoaded", function () {
    
    document.getElementById("uploadForm").addEventListener("submit", function (event) {
        event.preventDefault();

        var folderPath = document.getElementById("folderInput").value;
        if (!folderPath) {
            alert("Por favor, proporciona una ruta de carpeta.");
            return;
        }

        var formData = new FormData();
        formData.append("folderPath", folderPath); // Asegurarse de que el nombre del campo coincide

        axios.post('/Home/UploadFile', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
            .then(function (response) {
                if (response.data.success) {

                    const messageContainer = document.getElementById('messageContainer');

                    if (response.data.message === "Existe") {
                        alert('El archivo ya se encuentra subido');
                    } else {

                        alert('¡Carpeta subida exitosamente!');

                        messageContainer.innerHTML = `
                                <div class="alert alert-success" role="alert">
                                    <a href="${response.data.message}" target="_blank">${response.data.message}</a>
                                </div>
                            `;

                    }
                    
                } else {
                    alert('Ocurrió un error al subir la carpeta. Por favor, intenta de nuevo.');
                }
            })
            .catch(function (error) {
                alert('Ocurrió un error al subir la carpeta. Por favor, intenta de nuevo.');
            });
    });
});