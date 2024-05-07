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
                $('#tbody-ejerciciosFisicos').append(`
                    <tr>
                        <td class="tbody">${ejercicio.nombreTipoEjercicio}</td>
                        <td class="tbody">${ejercicio.inicio}</td>
                        <td class="tbody">${ejercicio.fin}</td>
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
            alert('Ocurrió un error a la hora de mostrar el listado.')
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
            alert('Ocurrió un error al almacear el nuevo ejercicio físico.');
        }
    })
}

function eliminarRegistro(ejercicioFisicoID){
    $.ajax({
        url: '../../EjercicioFisico/EliminarEjercicioFisico',
        data: { ejercicioFisicoID : ejercicioFisicoID },
        type: 'DELETE',
        dataType: 'json',
        success: function(result){
            listadoEjerciciosFisicos();
        },
        error: function(kxr,status){
            alert('Disculpe, ocurrió un error al intentar eliminar el ejercicio físico.');
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
            document.getElementById("EstadoEmocionalInicio").value = ejercicioAEditar.estadoEmocionalInicio;
            document.getElementById("EstadoEmocionalFin").value = ejercicioAEditar.estadoEmocionalFin;
            $('#modalEjercicioFisico').modal("show");

        }
    })
}