﻿window.onload = function () {
    ListarPaciente();
}

let objPaciente;

async function ListarPaciente() {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Receptionist');

    objPaciente = {
        url: "Paciente/ListarPaciente",
        cabeceras: ["Id Paciente", "Nombre", "Apellido", "Fecha de Nacimiento", "Telefono", "Email", "Direccion"],
        propiedades: ["idPaciente", "nombre", "apellido", "fechaNacimiento", "telefono", "email", "direccion"],
        editar: hasAccess,
        eliminar: hasAccess,
        propiedadID: "idPaciente"
    }

    pintar(objPaciente);

    if (!hasAccess) {
        let btnNuevo = document.getElementById("btnNuevoPaciente");
        btnNuevo.style.display = "none";
    }
}



function LimpiarPaciente() {
    LimpiarDatos("frmPaciente");
    ListarPaciente();
}

function ValidarFormulario() {
    let form = document.getElementById("frmPaciente");
    let isValid = true;

    // Validar campos de texto
    let camposTexto = ['nombre', 'apellido', 'telefono', 'email', 'direccion'];
    camposTexto.forEach(function (campo) {
        let input = form.querySelector('#' + campo);
        if (input.value.trim() === '') {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    // Validar fecha de nacimiento
    let fechaNacimiento = form.querySelector('#fechaNacimiento');
    if (fechaNacimiento.value === '') {
        fechaNacimiento.classList.add('is-invalid');
        isValid = false;
    } else {
        fechaNacimiento.classList.remove('is-invalid');
    }

    return isValid;
}


function GuardarPaciente() {
    if (!ValidarFormulario()) {
        return;
    }

    let form = document.getElementById("frmPaciente");
    let frm = new FormData(form);
    fetchPost("Paciente/GuardarPaciente", "text", frm, function (res) {
        // Perform other operations first
        Exito("Registro Guardado con Exito");
        ListarPaciente();

        var myModal = bootstrap.Modal.getInstance(document.getElementById('modalPaciente'));
        myModal.hide();
    });
}

function MostrarModal() {
    LimpiarDatos("frmPaciente");
    var myModal = new bootstrap.Modal(document.getElementById('modalPaciente'));
    myModal.show();
}

function Editar(id) {
    fetchGet("Paciente/RecuperarPaciente/?idPaciente=" + id, "json", function (data) {
        setN("idPaciente", data.idPaciente);
        setN("nombre", data.nombre);
        setN("apellido", data.apellido);
        setN("fechaNacimiento", data.fechaNacimiento ? new Date(data.fechaNacimiento).toISOString().split('T')[0] : '');
        setN("telefono", data.telefono);
        setN("email", data.email);
        setN("direccion", data.direccion);

        // Show the modal
        var myModal = new bootstrap.Modal(document.getElementById('modalPaciente'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Paciente/RecuperarPaciente/?idPaciente=" + id, "json", function (data) {
        Confirmar(undefined, "¿Desea eliminar el paciente " + data.nombre + " " + data.apellido + "?", function () {
            fetchGet("Paciente/EliminarPaciente/?idPaciente=" + id, "text", function (r) {
                Exito("Registro Eliminado con Exito");
                ListarPaciente();
            });
        });
    });
}
