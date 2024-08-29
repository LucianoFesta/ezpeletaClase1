document.addEventListener('DOMContentLoaded', () => {
    ListadoEjercicios();
})

function ListadoEjercicios(){

    $('#tbody-reporte').empty();

    var fechaDesdeBuscar = $('#FechaDesdeBuscar').val();
    var fechaHastaBuscar = $('#FechaHastaBuscar').val();

    $.ajax({
        url: '../../EjercicioFisico/ListadoReporte',
        data: { Desde:fechaDesdeBuscar, Hasta:fechaHastaBuscar },
        type: 'GET',
        dataType: 'json',
        success: function(result) {
            if(result){
                console.log(result)
                $.each(result, function(index, ejercicios){
                    $('#tbody-reporte').append(`
                        <tr class="trEjercicio">
                            <td class="tbody">${ejercicios[0].nombreTipoEjercicio}</td>
                            <td class="tbody"></td>
                            <td class="tbody"></td>
                            <td class="tbody"></td>
                            <td class="tbody"></td>
                            <td class="tbody"></td>
                            <td class="tbody"></td>
                        </tr>
                    `)
                    $.each(ejercicios, function(i, ejercicio){
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

                        $('#tbody-reporte').append(`
                            <tr>
                                <td class="tbody"></td>
                                <td class="tbody">${fechaInicioFormateada} hs.</td>
                                <td class="tbody">${fechaFinFormateada} hs.</td>
                                <td class="tbody">${ejercicio.intervaloDeTiempoEjercicio} hs.</td>
                                <td class="tbody">${ejercicio.estadoEmocionalInicio}</td>
                                <td class="tbody">${ejercicio.estadoEmocionalFin}</td>
                                <td class="tbody">${ejercicio.observaciones}</td>
                            </tr>
                        `)
                    })
                })

            }else{
                Swal.fire({
                    title: 'Ups, existe un inconveniente:',
                    text: 'Error al mostrar el listado',
                    icon: 'warning',
                    confirmButtonText: 'Cerrar'
                });
            }
        },
        error: function(xhr, status) {
            Swal.fire({
                title: 'Ups, existe un inconveniente:',
                text: "Disculpe, ocurrio un problema en el borrado del tipo de ejercicio.",
                icon: 'warning',
                confirmButtonText: 'Volver a intentarlo'
            });
        }
    })

}