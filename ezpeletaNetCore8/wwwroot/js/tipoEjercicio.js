
document.addEventListener('DOMContentLoaded', function() {
    ListaTipoEjercicios();
});

//UTILIZANDO AJAX-JQUERY
function ListaTipoEjercicios(){

    $.ajax({
        url: '../../TipoEjercicio/ListadoTipoEjercicios',
        data: { },
        type: 'GET',
        dataType: 'json',
        success: function (tipoDeEjercicios){
            $('#tbody-tipoejercicios').empty();
            
            $("#modalTipoEjercicio").modal("hide");
            limpiarModal();

            document.getElementById('ejercicio').value = '';

            $.each(tipoDeEjercicios, function(index, tipoEjercicio) {
                $('#tbody-tipoejercicios').append(`
                    <tr>
                        <td class="tbody">${tipoEjercicio.nombre}</td>
                        <td class="text-center">
                            <button type="button" class="btn btn-success mb-2" onclick="abrirModalEditar(${tipoEjercicio.tipoEjercicioID})">
                                Editar
                            </button>
                        </td>
                        <td class="text-center">
                            <button type="button" class="btn btn-danger mb-2" onclick="eliminarRegistro(${tipoEjercicio.tipoEjercicioID})">
                                Eliminar
                            </button>
                        </td>
                    </tr>
                `)
            });
        },
        error: function(xhr, status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un problema a la hora de mostrar el listado.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

//UTILIZANDO JS
// function ListaTipoEjercicios(){
//     $.ajax({
//         url: '../../TipoEjercicio/ListadoTipoEjercicios',
//         data: { },
//         type: 'GET',
//         dataType: 'json',
//         success: function(tipoDeEjercicios){
//             let contenidoTabla = '';

//             $.each(tipoDeEjercicios, function(index, tipoEjercicio){
//                 contenidoTabla += `
//                     <tr>
//                         <td>${tipoEjercicio.nombre}</td>
//                         <td class="text-center"></td>
//                         <td class="text-center"></td>
//                     </tr>
//                 `;
//             })

//             document.getElementById('tbody-tipoejercicios').innerHTML = contenidoTabla;
//         }

//     })
// }

function guardarRegistro(){
    var nombre = document.getElementById('ejercicio').value;
    let tipoEjercicioID = document.getElementById("tipoEjercicioID").value;

    $.ajax({
        url: '../../TipoEjercicio/GuardarTipoEjercicio',
        data: { tipoEjercicioID: tipoEjercicioID, nombre : nombre },
        type: 'POST',
        dataType: 'json',
        success: function (result){
            if(result != ""){
                Swal.fire({
                    title: 'Ups, existe un inconveniente:',
                    text: result,
                    icon: 'warning',
                    confirmButtonText: 'Volver a intentarlo'
                });
            }

            ListaTipoEjercicios();
        },
        error: function(xhr, status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un error a la hora de guardar el tipo de actividad.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

function limpiarModal(){
    document.getElementById('ejercicio').value = '';
    document.getElementById('tipoEjercicioID').value = 0;
}

function nuevoRegistro(){
    $("#modalTitulo").text("Nuevo tipo de ejercicio");
}

function abrirModalEditar(tipoEjercicioID){
    $.ajax({
        url: '../../TipoEjercicio/ListadoTipoEjercicios',
        data: { id: tipoEjercicioID },
        type: 'POST',
        dataType: 'json',
        success: function(tipoDeEjercicios) {

            let tipoDeEjercicio = tipoDeEjercicios[0];

            document.getElementById("tipoEjercicioID").value = tipoEjercicioID;
            $("#modalTitulo").text("Editar tipo de ejercicio");
            
            document.getElementById("ejercicio").value = tipoDeEjercicio.nombre;
            $("#modalTipoEjercicio").modal("show");

        },

        error: function(xhr,status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: "Disculpe, ocurrio un problema en la edición del tipo de ejercicio.",
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

function eliminarRegistro(tipoEjercicioID) {

    Swal.fire({
        title: "¿Desea eliminar el tipo de ejercicio?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar"

    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../TipoEjercicio/EliminarTipoEjercicio',
                data: { tipoEjercicioID: tipoEjercicioID },
                type: 'POST',
                dataType: 'json',
                success: function(result) {
                    ListaTipoEjercicios(); // Actualiza la lista de tipos de ejercicio
                },
                error: function(xhr, status) {
                    Swal.fire({
                        title: 'Ups, existe un inconveniente:',
                        text: "Disculpe, ocurrio un problema en la edición del tipo de ejercicio.",
                        icon: 'warning',
                        confirmButtonText: 'Volver a intentarlo'
                    });
                }
            });
        }
    });
}