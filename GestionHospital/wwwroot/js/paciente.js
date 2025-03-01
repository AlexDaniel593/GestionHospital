window.onload = function () {
    ListarPaciente();
}

let objPaciente;

async function ListarPaciente() {

    objPaciente = {
        url: "Paciente/ListarPaciente",
        cabeceras: ["Id Paciente", "Nombre", "Apellido", "Fecha de Nacimiento", "Telefono", "Email", "Direccion"],
        propiedades: ["idPaciente", "nombre", "apellido", "fechaNacimiento", "telefono", "email", "direccion"],
        editar: true,
        eliminar: true,
        propiedadID: "idPaciente"
    }

    pintar(objPaciente);
}


function LimpiarTipoMedicamento() {
    LimpiarDatos("frmTipoMedicamento");
}
