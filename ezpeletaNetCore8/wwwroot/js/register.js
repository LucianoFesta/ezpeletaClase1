document.addEventListener('DOMContentLoaded', (event) => {

})

document.getElementById('btnRegister').addEventListener('click', (e) => {
    e.preventDefault();

    var nombre = document.getElementById('nombreCompleto').value;
    var peso = parseFloat(document.getElementById('peso').value);
    var altura = parseFloat(document.getElementById('altura').value);
    var fechaNacimiento = document.getElementById('fechaNacimiento').value;
    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;
    var confirmPassword = document.getElementById('confirmPassword').value;
    var genero = parseInt(document.getElementById('Genero').value);  // Convertir a entero
    
    console.log(nombre, peso, altura, email, genero, password, fechaNacimiento);

    $.ajax({
        url: '../../Register/GuardarPersona',
        data: { nombreCompleto: nombre, peso, altura, fechaNacimiento, email, genero, password, confirmPassword },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            console.log(resultado);
            if (resultado.result) {
                Swal.fire({
                    title: 'Registro de Usuario',
                    text: 'Usuario registrado exitosamente!',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/Identity/Account/Login';
                    }
                });
            } else {
                Swal.fire({
                    title: 'Ups, existe un inconveniente:',
                    text: resultado.message,
                    icon: 'warning',
                    confirmButtonText: 'Volver a intentarlo'
                });
            }
        },
        error: function (xhr, status) {
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un error para proceder a registrar el usuario.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    });
});
