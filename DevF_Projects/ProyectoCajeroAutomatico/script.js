const cuentas = {
    "cuenta1": {
        "saldo": 100,
        "password": "abc123"
    },
    "cuenta2": {
        "saldo": 500,
        "password": "def456"
    },
    "cuenta3": {
        "saldo": 800,
        "password": "ghi789"
    }
};

// Función para verificar si el password ingresado es correcto
function verificarPassword(cuentaSeleccionada) {
    const passwordIngresado = document.getElementById("password").value;
    if (passwordIngresado === cuentas[cuentaSeleccionada]["password"]) {
        return true;
    } else {
        alert("Password incorrecto. Intenta nuevamente.");
        return false;
    }
}

// Función para mostrar las opciones de la cuenta
function mostrarOpciones() {
    document.getElementById("opciones").style.display = "block";
}

// Función para ocultar las opciones de la cuenta
function ocultarOpciones() {
    document.getElementById("opciones").style.display = "none";
}

// Función para consultar el saldo de la cuenta
function consultarSaldo() {
    const cuentaSeleccionada = document.getElementById("cuenta").value;
    const saldoActual = cuentas[cuentaSeleccionada]["saldo"];
    document.getElementById("resultado").style.display = "block";
    document.getElementById("resultado").innerHTML = "El saldo actual de la cuenta es $" + saldoActual;
}

// Función para ingresar un monto en la cuenta
function ingresarMonto() {
    const cuentaSeleccionada = document.getElementById("cuenta").value;
    const montoIngresado = parseFloat(prompt("Ingresa el monto a ingresar:"));
    if (isNaN(montoIngresado)) {
        alert("Ingresa un valor numérico válido.");
        return;
    }
    const saldoActual = cuentas[cuentaSeleccionada]["saldo"];
    const nuevoSaldo = saldoActual + montoIngresado;
    if (nuevoSaldo > 990) {
        alert("No puedes tener más de $990 en la cuenta.");
        return;
    }
    cuentas[cuentaSeleccionada]["saldo"] = nuevoSaldo;
    document.getElementById("resultado").innerHTML = "Se agregaron $" + montoIngresado + " a la cuenta. El nuevo saldo es $" + nuevoSaldo;
}

function retirarMonto() {
    const cuentaSeleccionada = document.getElementById("cuenta").value;
    const montoRetirado = parseFloat(prompt("Ingresa el monto a retirar:"));
    if (isNaN(montoRetirado)) {
        alert("Ingresa un valor numérico válido.");
        return;
    }
    const saldoActual = cuentas[cuentaSeleccionada]["saldo"];
    const nuevoSaldo = saldoActual - montoRetirado;
    if (nuevoSaldo < 10 || nuevoSaldo > 990) {
        alert("El saldo de la cuenta no puede ser menor a $10 o mayor a $990.");
        return;
    }
    cuentas[cuentaSeleccionada]["saldo"] = nuevoSaldo;
    document.getElementById("resultado").innerHTML = "Se retiraron $" + montoRetirado + " de la cuenta. El nuevo saldo es $" + nuevoSaldo;
}

// Función para procesar el ingreso de password
function ingresar() {
    const cuentaSeleccionada = document.getElementById("cuenta").value;
    if (verificarPassword(cuentaSeleccionada)) {
        mostrarOpciones();
    } else {
        ocultarOpciones();
    }
}