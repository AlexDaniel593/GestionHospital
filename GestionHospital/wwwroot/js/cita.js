window.onload = function () {
    ListarCitas();
    ListarPacientes();
    ListarMedicos();
};

let objCita;

async function ListarCitas() {
    objCita = {
        url: "Cita/ListarCitas",
        cabeceras: ["Id Cita", "Paciente", "Médico", "Fecha y Hora", "Estado"],
        propiedades: ["idCita", "nombreCompletoPaciente", "nombreCompletoMedico", "fechaHora", "estado"],
        editar: true,
        eliminar: true,
        propiedadID: "idCita"
    };
    pintar(objCita);
}

function ListarPacientes() {
    fetchGet("Paciente/ListarPaciente", "json", function (data) {
        let selectPaciente = document.getElementById("idPaciente");
        data.forEach((paciente) => {
            let option = document.createElement("option");
            option.value = paciente.idPaciente;
            option.text = paciente.nombre + " " + paciente.apellido;
            selectPaciente.appendChild(option);
        });
    });
}

function ListarMedicos() {
    fetchGet("Medico/ListarMedico", "json", function (data) {
        let selectMedico = document.getElementById("idMedico");
        data.forEach((medico) => {
            let option = document.createElement("option");
            option.value = medico.idMedico;
            option.text = medico.nombre + " " + medico.apellido;
            selectMedico.appendChild(option);
        });
    });
}

function LimpiarCita() {
    LimpiarDatos("frmCita");
    let form = document.getElementById("frmCita");
    let inputs = form.querySelectorAll('.form-control');
    inputs.forEach(input => input.classList.remove('is-invalid'));
}

function MostrarModal() {
    LimpiarCita();
    var myModal = new bootstrap.Modal(document.getElementById('modalCita'));
    myModal.show();
}

function ValidarFormularioCita() {
    let form = document.getElementById("frmCita");
    let isValid = true;

    let camposRequeridos = ['idPaciente', 'idMedico', 'fechaHora', 'estado'];
    camposRequeridos.forEach(campo => {
        let input = form.querySelector('#' + campo);
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    return isValid;
}

function GuardarCita() {
    if (!ValidarFormularioCita()) {
        console.log("Formulario inválido. Revise los campos.");
        return;
    }

    let form = document.getElementById("frmCita");
    let frm = new FormData(form);
    fetchPost("Cita/GuardarCita", "text", frm, function (res) {
        LimpiarCita();
        Exito("Cita guardada con éxito");
        ListarCitas();

        var modalElement = document.getElementById('modalCita');
        var myModal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        if (myModal) myModal.hide();
    });
}

function Editar(id) {
    fetchGet("Cita/RecuperarCita?idCita=" + id, "json", function (data) {
        setN("idCita", data.idCita);
        setN("idPaciente", data.idPaciente);
        setN("idMedico", data.idMedico);
        setN("fechaHora", data.fechaHora);
        setN("estado", data.estado);

        let form = document.getElementById("frmCita");
        let inputs = form.querySelectorAll('.form-control');
        inputs.forEach(input => input.classList.remove('is-invalid'));

        var myModal = new bootstrap.Modal(document.getElementById('modalCita'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Cita/RecuperarCita?idCita=" + id, "json", function (data) {
        Confirmar(undefined, "¿Desea eliminar la cita de " + data.nombrePaciente + "?", function () {
            fetchGet("Cita/EliminarCita?idCita=" + id, "text", function (r) {
                Exito("Cita eliminada con éxito");
                ListarCitas();
            });
        });
    });
}