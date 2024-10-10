document.addEventListener('DOMContentLoaded', function() {
    listadoEjerciciosFisicos();
    crearGraficoEstatico();
});

let graficoEjerciciosRealizados;
let graficoNuevoCircular;

function crearGraficoEstatico(){
    var tipoEjercicioId = document.getElementById('TipoEjercicioIDGrafico').value;
    var mesEjercicioBuscado = document.getElementById('mesEjercicio').value;
    var yearEjercicioBuscado = document.getElementById('yearEjercicio').value;

    $.ajax({
        url: '../../EjercicioFisico/CrearGraficoEjercicios',
        data: { tipoEjercicioId : tipoEjercicioId, mes : mesEjercicioBuscado, year : yearEjercicioBuscado },
        type: 'GET',
        dataType: 'json',
        success: function(ejerciciosXdia){
            
            let labels = [];
            let data = [];
            let diasConEjercicioRealizado = 0;
            let totalidadMinitosEjercitados = 0;

            $.each(ejerciciosXdia, function(i, ejercicio){
                labels.push(ejercicio.dia + " " + ejercicio.mes);
                data.push(ejercicio.cantidadMinutos)

                totalidadMinitosEjercitados += ejercicio.cantidadMinutos;

                if(ejercicio.cantidadMinutos > 0){
                    diasConEjercicioRealizado += 1;
                }
            })

            //Obtengo el tipo de ejercicio seleccionado en nombre
            var tipoEjercicio = document.getElementById('TipoEjercicioIDGrafico');
            var nombreTipoEjercicio = tipoEjercicio.options[tipoEjercicio.selectedIndex].text;
            let cantDiasSinEjercicio = ejerciciosXdia.length - diasConEjercicioRealizado;

            $("#cardTotalEjercicios").text(totalidadMinitosEjercitados + " Minutos en " + diasConEjercicioRealizado + " días.");
            $("#cardSinEjercicios").text(cantDiasSinEjercicio + " días sin " + nombreTipoEjercicio + ".");

            const grafico = document.getElementById('miPrimerGrafico');

            graficoEjerciciosRealizados = new Chart(grafico, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Minutos de Actividad',
                        data: data,
                        borderColor: 'red',
                        backgroundColor: 'red',
                        borderWidth: 2,
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            labels: {
                                font: {
                                    size: 15
                                },
                                boxWidth: 20
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        },
                    }
                }
            })
            GraficoCircular();
        },
        error: function(e, status){
            console.log('Error al crear un gráfico de ejercicios.');
        }
    })
}


function GraficoCircular(){
    var mesEjercicioBuscado = document.getElementById('mesEjercicio').value;
    var yearEjercicioBuscado = document.getElementById('yearEjercicio').value;

    $.ajax({
        url: '../../EjercicioFisico/GraficoTipoEjercicios',
        data: { mes : mesEjercicioBuscado, year : yearEjercicioBuscado },
        type: 'GET',
        dataType: 'json',
        success: function(tiposEjerciciosView){

            var labels = [];
            var data = [];
            var colorFondo = [];

            $.each(tiposEjerciciosView, function(i, tipoEjercicioView){
                labels.push(tipoEjercicioView.descripcion);
                data.push(tipoEjercicioView.cantidadMinutos);

                var color = generarColorVerde();
                colorFondo.push(color);
            })

            var graficoTorta = document.getElementById("graficoTipoEjercicios");
            graficoNuevoCircular = new Chart(graficoTorta, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: colorFondo,
                    }],
                },
            });

        },
        error: function(e, status){
            console.log('Ocurrió un error al crear el gráfico circular de Tipo Ejercicios.');
        }
    })
}


$("#TipoEjercicioIDGrafico").change(function() {
    graficoEjerciciosRealizados.destroy();
    graficoNuevoCircular.destroy();
    crearGraficoEstatico();
});

$("#mesEjercicio, #yearEjercicio").change(function() {
    graficoEjerciciosRealizados.destroy();
    graficoNuevoCircular.destroy();
    crearGraficoEstatico();
});


function listadoEjerciciosFisicos(){

    $('#tbody-ejerciciosFisicos').empty();

    limpiarModal();

    $('#modalEjercicioFisico').modal("hide");

    var fechaDesdeBuscar = $('#FechaDesdeBuscar').val();
    var fechaHastaBuscar = $('#FechaHastaBuscar').val();
    var tipoEjercicioFisicoID = $('#TipoEjercicioIDBuscar').val();

    $.ajax({
        url: '../../EjercicioFisico/ListadoEjerciciosFisicos',
        data: { FechaDesdeBuscar: fechaDesdeBuscar, FechaHastaBuscar: fechaHastaBuscar, TipoEjercicioFisicoID: tipoEjercicioFisicoID },
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
                        <td class="tbody">${ejercicio.lugar}</td>
                        <td class="tbody">${ejercicio.observaciones}</td>
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
    document.getElementById("Lugar").value = 0;   
}

function nuevoRegistro(){
    $("#modalTitulo").text("Nuevo tipo de ejercicio");
}

function guardarRegistro(){

    var ejercicioFisicoID = document.getElementById("ejercicioFisicoID").value;
    var tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    var inicio = document.getElementById("FechaInicio").value;
    var estadoEmocionalInicio = document.getElementById("EstadoEmocionalInicio").value;
    var estadoEmocionalFin = document.getElementById("EstadoEmocionalFin").value;
    var fin = document.getElementById("FechaFin").value;
    var observaciones = document.getElementById("Observaciones").value;
    var lugar = document.getElementById("Lugar").value;
    
    $.ajax({
        url: "../../EjercicioFisico/SaveEjercicio",
        data: { ejercicioFisicoID, tipoEjercicioID, inicio, estadoEmocionalInicio, estadoEmocionalFin, fin, observaciones, lugar },
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
                    listadoEjerciciosFisicos();
    
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

function generarColorVerde() {
    // El valor de GG será alto (de 128 a 255) para garantizar que predomine el verde.
    // Los valores de RR y BB serán bajos (de 0 a 127).

    let rr = Math.floor(Math.random() * 128) + 128; // 128 a 255 
    let gg = Math.floor(Math.random() * 128); // 0 a 127
    let bb = Math.floor(Math.random() * 128); // 0 a 127

    // Convertimos a hexadecimal y formateamos para que tenga siempre dos dígitos.
    let colorHex = `#${rr.toString(16).padStart(2, '0')}${gg.toString(16).padStart(2, '0')}${bb.toString(16).padStart(2, '0')}`;
    return colorHex;
}