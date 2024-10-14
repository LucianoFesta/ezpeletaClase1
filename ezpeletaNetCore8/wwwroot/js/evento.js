document.addEventListener('DOMContentLoaded', function() {
    listadoEventos();
});

function limpiarModalEvento(){
    document.getElementById('evento').value = '';
    document.getElementById('eventoID').value = 0;
}

function nuevoEvento(){
    $('#modalTituloEvento').text('Crear nuevo evento');
}

function guardarEvento(){

    var eventoID = document.getElementById("eventoID").value;
    var evento = document.getElementById("evento").value;

    $.ajax({
        url: "../../EventoDeportivo/SaveEvento",
        data: { eventoID, evento },
        type: 'POST',
        dataType: 'json',
        success: function(result){
            
            if(result.success == false){
                Swal.fire({
                    title: 'Ups, existe un inconveniente:',
                    text: result.message,
                    icon: 'warning',
                    confirmButtonText: 'Volver a intentarlo'
                });
            
            }else{
                if(result.success == true){
                    listadoEventos();

                    limpiarModalEvento();
                }
            }

        },
        error: function(hxr,status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un error al almacear el nuevo evento.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

function listadoEventos(){
    $('#tbody-eventos').empty();

    $('#modalEvento').modal("hide");

    $.ajax({
        url: "../../EventoDeportivo/ListadoEventos",
        data: { },
        type: 'GET',
        dataType: 'json',
        success: function(result){

            if(result.success){

                $.each(result.lista, function(index, evento){
                    $('#tbody-eventos').append(`
                        <tr>
                            <td class="tbody">${evento.descripcion}</td>
                            <td class="text-center">
                                <button type="button" class="btn btn-success mb-2" onclick="abrirModalEditar(${evento.eventoID})">
                                    Editar
                                </button>
                            </td>
                            <td class="text-center">
                                <button type="button" class="btn btn-danger mb-2" onclick="eliminarRegistro(${evento.eventoID})">
                                    Eliminar
                                </button>
                            </td>
                        </tr>
                    `)
                })
            }
        },
        error: function(xrs, status){
            console.log('Ocurrió un error a la hora de mostrar el listado');
        }
    })
}

function eliminarRegistro(id)
{
    Swal.fire({
        title: "¿Desea eliminar el evento?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar"

    }).then((result) =>{
        if (result.isConfirmed) {
            $.ajax({
                url: '../../EventoDeportivo/EliminarEvento',
                data: { id },
                type: 'DELETE',
                dataType: 'json',
                success: function(result){
                    if(result == true){
                        listadoEventos();
                    }else{
                        Swal.fire({
                            title: 'Ups, existe un inconveniente:',
                            text: result.message,
                            icon: 'warning',
                            confirmButtonText: 'Volver a intentarlo'
                        }); 
                    }
                },
                error: function(kxr,status){
                    Swal.fire({
                        title: 'Ups, existe un inconveniente:',
                        text: 'Disculpe, ocurrió un error al intentar eliminar el evento.',
                        icon: 'warning',
                        confirmButtonText: 'Volver a intentarlo'
                    });
                }
            })
        }
    })
}

function abrirModalEditar(idEvento){
    $.ajax({
        url: '../../EventoDeportivo/ListadoEventos',
        data: { idEvento },
        type: 'POST',
        dataType: 'json',
        success: function(result) {
            let evento = result.lista[0];

            document.getElementById("eventoID").value = idEvento;
            $("#modalTituloEvento").text("Editar evento");
            
            document.getElementById("evento").value = evento.descripcion;
            $("#modalEvento").modal("show");

        },

        error: function(xhr,status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: "Disculpe, ocurrio un problema en la edición del evento.",
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}