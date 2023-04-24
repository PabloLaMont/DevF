class Student {
    constructor(nombre, apellidos, edad) {
        this.nombre = nombre;
        this.apellidos = apellidos;
        this.edad = edad;
        this.materiasInscritas = [];
        this.calificaciones = {};
    }

    inscribirMateria(materia) {
        this.materiasInscritas.push(materia);
    }

    asignarCalificacion(materia, calificacion) {
        this.calificaciones[materia] = calificacion;
    }
}

const students = [];

function crearAlumno(nombre, apellidos, edad) {
    const student = new Student(nombre, apellidos, edad);
    students.push(student);
    return student;
}

function mostrarAlumno(student) {
    const studentCard = document.createElement('div');
    studentCard.className = 'student-card';
    studentCard.dataset.nombre = student.nombre;
    studentCard.dataset.apellidos = student.apellidos;
    studentCard.innerHTML = `
        <h3>${student.nombre} ${student.apellidos}</h3>
        <p>Edad: ${student.edad}</p>
        <div class="materias-container"></div>
    `;
    document.getElementById('students-container').appendChild(studentCard);
}

function inscribirMateriaAlumno(nombre, apellidos, materia) {
    const student = students.find(student => student.nombre === nombre && student.apellidos === apellidos);

    if (student) {
        student.inscribirMateria(materia);
        displayMaterias(student);
    } else {
        alert('No se encontró al alumno con el nombre y apellidos proporcionados');
    }
}

function asignarCalificacionAlumno(nombre, apellidos, materia, calificacion) {
    const student = students.find(student => student.nombre === nombre && student.apellidos === apellidos);

    if (student) {
        student.asignarCalificacion(materia, calificacion);
    } else {
        alert('No se encontró al alumno con el nombre y apellidos proporcionados');
    }
}

function displayMaterias(student) {
    const studentCard = document.querySelector(`.student-card[data-nombre="${student.nombre}"][data-apellidos="${student.apellidos}"]`);
    const materiasContainer = studentCard.querySelector('.materias-container');
    materiasContainer.innerHTML = '';

    for (const materia of student.materiasInscritas) {
        const materiaElement = document.createElement('p');
        materiaElement.innerText = materia;
        materiasContainer.appendChild(materiaElement);
    }
}

document.getElementById('student-form').addEventListener('submit', (e) => {
    e.preventDefault();

    const nombre = document.getElementById('nombre').value;
    const apellidos = document.getElementById('apellidos').value;
    const edad = parseInt(document.getElementById('edad').value);

    const student = crearAlumno(nombre, apellidos, edad);
    mostrarAlumno(student);

    e.target.reset();
});

document.getElementById('search-form').addEventListener('submit', (e) => {
    e.preventDefault();

    const searchValue = document.getElementById('search').value.toLowerCase();
    const searchResults = students.filter(student => {
        return student.nombre.toLowerCase().includes(searchValue) || student.apellidos.toLowerCase().includes(searchValue);
    });

    displaySearchResults(searchResults);

    e.target.reset();
});

document.getElementById('inscripcion-form').addEventListener('submit', (e) => {
    e.preventDefault();

    const nombre = document.getElementById('student-nombre-inscripcion').value;
    const apellidos = document.getElementById('student-apellidos-inscripcion').value;
    const materia = document.getElementById('materia').value;

    inscribirMateriaAlumno(nombre, apellidos, materia);

    e.target.reset();
});

document.getElementById('calificacion-form').addEventListener('submit', (e) => {
    e.preventDefault();

    const nombre = document.getElementById('student-nombre-calificacion').value;
    const apellidos = document.getElementById('student-apellidos-calificacion').value;
    const materia = document.getElementById('materia-calificacion').value;
    const calificacion = parseFloat(document.getElementById('calificacion').value);

    asignarCalificacionAlumno(nombre, apellidos, materia, calificacion);

    e.target.reset();
});

function displaySearchResults(searchResults) {
    const searchResultsContainer = document.getElementById('search-results-container');
    searchResultsContainer.innerHTML = '';

    for (const student of searchResults) {
        const studentCard = document.createElement('div');
        studentCard.className = 'student-card search-result';
        studentCard.dataset.nombre = student.nombre;
        studentCard.dataset.apellidos = student.apellidos;
        studentCard.innerHTML = `
            <h3>${student.nombre} ${student.apellidos}</h3>
            <p>Edad: ${student.edad}</p>
            <div class="materias-container"></div>
        `;
        searchResultsContainer.appendChild(studentCard);
        displayMaterias(student);
        displayCalificaciones(student);
    }
}

function displayCalificaciones(student) {
    const studentCard = document.querySelector(`.student-card[data-nombre="${student.nombre}"][data-apellidos="${student.apellidos}"]`);
    const materiasContainer = studentCard.querySelector('.materias-container');

    for (const materia in student.calificaciones) {
        const calificacionElement = document.createElement('p');
        calificacionElement.innerText = `Calificación en ${materia}: ${student.calificaciones[materia]}`;
        materiasContainer.appendChild(calificacionElement);
    }
}
