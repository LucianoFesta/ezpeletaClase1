document.addEventListener('DOMContentLoaded', function() {
    listadoEjerciciosFisicos();
});

function listadoEjerciciosFisicos(){

    $('#tbody-ejerciciosFisicos').empty();

    limpiarModal();

    $('#modalEjercicioFisico').modal("hide");

    $.ajax({
        url: '../../EjercicioFisico/ListadoEjerciciosFisicos',
        data: { },
        type: 'GET',
        dataType: 'json',
        success: function(ejerciciosFisicos){
            $.each(ejerciciosFisicos, function(index, ejercicio){

                // Formatear el objeto Date al formato deseado
                var fechaInicioFormateada = new Date(ejercicio.inicio).toLocaleDateString('es-ES', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });

                var fechaFinFormateada = new Date(ejercicio.fin).toLocaleDateString('es-ES', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });

                $('#tbody-ejerciciosFisicos').append(`
                    <tr>
                        <td class="tbody">${ejercicio.nombreTipoEjercicio}</td>
                        <td class="tbody">${fechaInicioFormateada} hs.</td>
                        <td class="tbody">${fechaFinFormateada} hs.</td>
                        <td class="tbody">${ejercicio.estadoEmocionalInicio}</td>
                        <td class="tbody">${ejercicio.estadoEmocionalFin}</td>
                        <td class="text-center">
                            <button type="button" class="btn btn-success mb-2" onclick="abrirModalEditar(${ejercicio.ejercicioFisicoID})">
                                Editar
                            </button>
                        </td>
                        <td class="text-center">
                            <button type="button" class="btn btn-danger mb-2" onclick="eliminarRegistro(${ejercicio.ejercicioFisicoID})">
                                Eliminar
                            </button>
                        </td>
                    </tr>
                `)
            })
        },
        error: function(hxr, status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un problema a la hora de mostrar el listado.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}


function limpiarModal(){
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("FechaInicio").value = "";
    document.getElementById("EstadoEmocionalInicio").value = 0;
    document.getElementById("FechaFin").value = "";
    document.getElementById("EstadoEmocionalFin").value = 0;
    document.getElementById("Observaciones").value = "";   
}

function nuevoRegistro(){
    $('#modalTitulo').text('Crear nuevo ejercicio físico');
}

function guardarRegistro(){

    var ejercicioFisicoID = document.getElementById("ejercicioFisicoID").value;
    var tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    var inicio = document.getElementById("FechaInicio").value;
    var estadoEmocionalInicio = document.getElementById("EstadoEmocionalInicio").value;
    var estadoEmocionalFin = document.getElementById("EstadoEmocionalFin").value;
    var fin = document.getElementById("FechaFin").value;
    var observaciones = document.getElementById("Observaciones").value;
    
    $.ajax({
        url: "../../EjercicioFisico/SaveEjercicio",
        data: { ejercicioFisicoID, tipoEjercicioID, inicio, estadoEmocionalInicio, estadoEmocionalFin, fin, observaciones },
        type: 'POST',
        dataType: 'json',
        success: function(result){
            listadoEjerciciosFisicos();
        },
        error: function(hxr,status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un error al almacear el nuevo ejercicio físico.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

function eliminarRegistro(ejercicioFisicoID){
    Swal.fire({
        title: "¿Desea eliminar el ejercicio físico?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar"

    }).then((result) =>{
        if (result.isConfirmed) {
            $.ajax({
                url: '../../EjercicioFisico/EliminarEjercicioFisico',
                data: { ejercicioFisicoID : ejercicioFisicoID },
                type: 'DELETE',
                dataType: 'json',
                success: function(result){
                    listadoEjerciciosFisicos();
                },
                error: function(kxr,status){
                    Swal.fire({
                        title: 'Ups, existe un inconveniente:',
                        text: 'Disculpe, ocurrió un error al intentar eliminar el ejercicio físico.',
                        icon: 'warning',
                        confirmButtonText: 'Volver a intentarlo'
                    });
                }
            })
        }
    })
}


function abrirModalEditar(ejercicioFisicoID){
    $.ajax({
        url: '../../EjercicioFisico/ListadoEjerciciosFisicos',
        data: { id : ejercicioFisicoID },
        type: 'POST',
        dataType: 'json',
        success: function(listadoEjerciciosFisicos){
            let ejercicioAEditar = listadoEjerciciosFisicos[0];
            
            document.getElementById('ejercicioFisicoID').value = ejercicioFisicoID;
            $('#modalTitulo').text('Editar ejercicio físico');

            document.getElementById("TipoEjercicioID").value = ejercicioAEditar.ejercicioFisicoID;
            document.getElementById("FechaInicio").value = ejercicioAEditar.inicio;
            document.getElementById("FechaFin").value = ejercicioAEditar.fin;
            document.getElementById("Observaciones").value = ejercicioAEditar.observaciones;
            document.getElementById("EstadoEmocionalInicio").value = ejercicioAEditar.estadoEmocionalInicioInt;
            document.getElementById("EstadoEmocionalFin").value = ejercicioAEditar.estadoEmocionalFinInt;
            $('#modalEjercicioFisico').modal("show");

        }
    })
}