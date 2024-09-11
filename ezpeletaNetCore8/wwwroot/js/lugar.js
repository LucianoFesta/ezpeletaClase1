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

function listadoLugares(){
    $('#modalLugar').modal("hide");
}

function guardarLugar(){

    var lugarID = document.getElementById("lugarID").value;
    var lugar = document.getElementById("lugar").value;

    console.log(lugar, lugarID)
    
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