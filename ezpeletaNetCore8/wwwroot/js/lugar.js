document.addEventListener('DOMContentLoaded', function() {
    listadoLugares();
});

function limpiarModalLugar(){
    document.getElementById('lugar').value = '';
    document.getElementById('lugarID').value = 0;
}

function nuevoLugar(){
    $('#modalTituloLugar').text('Crear nuevo lugar');
}

function guardarLugar(){

    var lugarID = document.getElementById("lugarID").value;
    var lugar = document.getElementById("lugar").value;
    
    $.ajax({
        url: "../../Lugar/SaveLugar",
        data: { lugarID, lugar },
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
                if(result == true){
                    listadoLugares();

                    limpiarModalLugar();
    
                }else{
                    Swal.fire({
                        title: 'Ups, existe un inconveniente:',
                        text: 'Por favor, completa todos los campos para poder crear un ejercicio.',
                        icon: 'warning',
                        confirmButtonText: 'Volver a intentarlo'
                    });
                }
            }

        },
        error: function(hxr,status){
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: 'Ocurrió un error al almacear el nuevo lugar.',
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })
}

function listadoLugares(){
    $('#tbody-lugares').empty();

    $('#modalLugar').modal("hide");

    var idLugar = document.getElementById('lugarID').value;

    $.ajax({
        url: "../../Lugar/ListadoLugares",
        data: { idLugar },
        type: 'GET',
        dataType: 'json',
        success: function(result){

            if(result.success){

                $.each(result.lista, function(index, lugar){
                    $('#tbody-lugares').append(`
                        <tr>
                            <td class="tbody">${lugar.nombre}</td>
                            <td class="text-center">
                                <button type="button" class="btn btn-success mb-2" onclick="abrirModalEditar(${lugar.lugarID})">
                                    Editar
                                </button>
                            </td>
                            <td class="text-center">
                                <button type="button" class="btn btn-danger mb-2" onclick="eliminarRegistro(${lugar.lugarID})">
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
        title: "¿Desea eliminar el lugar?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar"

    }).then((result) =>{
        if (result.isConfirmed) {
            $.ajax({
                url: '../../Lugar/EliminarLugar',
                data: { id },
                type: 'DELETE',
                dataType: 'json',
                success: function(result){
                    if(result == true){
                        listadoLugares();
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
                        text: 'Disculpe, ocurrió un error al intentar eliminar el lugar.',
                        icon: 'warning',
                        confirmButtonText: 'Volver a intentarlo'
                    });
                }
            })
        }
    })
}