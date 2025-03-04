window.onload = function () {
    ListarFacturacion();
}

let objFacturacion;

async function ListarFacturacion() {
    const roles = userRoles.split(',');

    if (!roles.includes('Admin')) {
        let btnNuevo = document.getElementById("btnNuevoFacturacion");
        btnNuevo.style.display = "none";
    }

    objFacturacion = {
        url: "Facturacion/ListarFacturacion",
        cabeceras: ["Id Facturacion", "Id Paciente", "Monto", "Metodo de Pago", "Fecha de Pago"],
        propiedades: ["idFacturacion", "idPaciente", "monto", "metodoPago", "fechaPago"],
        editar: roles.includes('Admin'),
        eliminar: roles.includes('Admin'),
        propiedadID: "idFacturacion"
    }

    pintar(objFacturacion);
}

function LimpiarFacturacion() {
    LimpiarDatos("frmFacturacion");
    ListarFacturacion();
}

function ValidarFormulario() {
    let form = document.getElementById("frmFacturacion");
    let isValid = true;
    // Validar campos de texto
    let camposTexto = ['monto', 'metodoPago', 'fechaPago'];
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


function GuardarFacturacion() {
    if (!ValidarFormulario()) {
        return;
    }
    let form = document.getElementById("frmFacturacion");
    let frm = new FormData(form);
    fetchPost("Facturacion/GuardarFacturacion", "text", frm, function (res) {
        // Perform other operations first
        Exito("Registro Guardado con Exito");
        ListarFacturacion();

        var myModal = bootstrap.Modal.getInstance(document.getElementById('modalFacturacion'));
        myModal.hide();
    });
}

function MostrarModal() {
    LimpiarDatos("frmFacturacion");
    var myModal = new bootstrap.Modal(document.getElementById('modalFacturacion'));
    myModal.show();
}

function Editar(id) {
    fetchget("facturacion/recuperarfacturacion/?idfacturacion=" + id, "json", function (data) {
        setn("idfacturacion", data.idfacturacion);
        setn("idpaciente", data.idpaciente);
        setn("monto", data.monto);
        setn("metodopago", data.metodopago);
        setn("fechapago", data.fechapago);l
        var mymodal = new bootstrap.modal(document.getelementbyid('modalfacturacion'));
        mymodal.show();
    });
}

function Eliminar(id) {
    fetchGet("Facturacion/RecuperarFacturacion/?idFacturacion=" + id, "json", function (data) {
        Confirmar(undefined, "¿Desea eliminar la factura ", function () {
            fetchGet("Facturacion/EliminarFacturacion/?idFacturacion=" + id, "text", function (r) {
                Exito("Registro Eliminado con Exito");
                ListarFacturacion();
            });
        });
    });
}