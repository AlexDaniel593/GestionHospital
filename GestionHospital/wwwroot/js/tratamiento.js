window.onload = function () {
    ListarTratamiento();
}

let objTratamiento;

async function ListarTratamiento() {
    const roles = userRoles.split(',');

    if (!roles.includes('Admin')) {
        let btnNuevo = document.getElementById("btnNuevoTratamiento");
        btnNuevo.style.display = "none";
    }

    objTratamiento = {
        url: "Tratamiento/ListarTratamiento",
        cabeceras: ["Id Tratamiento", "Id Paciente", "Descripcion", "Fecha", "Costo"],
        propiedades: ["idTratamiento", "idPaciente","descripcion", "fecha", "costo"],
        editar: roles.includes('Admin'),
        eliminar: roles.includes('Admin'),
        propiedadID: "idTratamiento"
    }

    pintar(objTratamiento);
}

function LimpiarTratamiento() {
    LimpiarDatos("frmTratamiento");
    ListarTratamiento();
}

function ValidarFormulario() {
    let form = document.getElementById("frmTratamiento");
    let isValid = true;

    // Validar campos de texto
    let camposTexto = ['descripcion', 'fecha', 'costo'];
    camposTexto.forEach(function (campo) {
        let input = form.querySelector('#' + campo);
        if (input.value.trim() === '') {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    return isValid;
}

function GuardarTratamiento() {
    if (!ValidarFormulario()) {
        return;
    }

    let form = document.getElementById("frmTratamiento");
    let frm = new FormData(form);
    fetchPost("Tratamiento/GuardarTratamiento", "text", frm, function (res) {
        // Perform other operations first
        Exito("Registro Guardado con Exito");
        ListarTratamiento();

        var myModal = bootstrap.Modal.getInstance(document.getElementById('modalTratamiento'));
        myModal.hide();
    });
}

function MostrarModal() {
    LimpiarDatos("frmTratamiento");
    var myModal = new bootstrap.Modal(document.getElementById('modalTratamiento'));
    myModal.show();
}

function Editar(id) {
    fetchGet("Tratamiento/RecuperarTratamiento/?idTratamiento=" + id, "json", function (data) {
        setN("idTratamiento", data.idTratamiento);
        setN("idPaciente", data.idPaciente);
        setN("descripcion", data.descripcion);
        setN("fecha", data.fecha);
        setN("costo", data.costo);

        // Show the modal
        var myModal = new bootstrap.Modal(document.getElementById('modalTratamiento'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Tratamiento/RecuperarTratamiento/?idTratamiento=" + id, "json", function (data) {
        Confirmar(undefined, "¿Desea eliminar el tratamiento ", function () {
            fetchGet("Tratamiento/EliminarTratamiento/?idTratamiento=" + id, "text", function (r) {
                Exito("Registro Eliminado con Exito");
                ListarTratamiento();
            });
        });
    });
}