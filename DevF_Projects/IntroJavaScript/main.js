/*function Hello()
{
    let edad = 0;
    let name = "";

    console.log("Hola, ", name = prompt("Cual es tu nombre?"))
    console.log(" ",edad = prompt("Cual es tu edad?"))

    alert("Es Mayor de edad : " + CheckIfClub(edad));
    alert("Eres VIP : " + CheckIfVIP(name))

}

function Hello()
{
    let numero = 0;

    console.log("Hola escribe tu numero, ", numero = prompt("Escribe tu numero"))
    alert("Es perfecto : " + EsPerfecto(numero));
}
*/

function Hello()
{
    CheckImpares();
}

function CheckIfClub(edad)
{   
    if(edad >= 18)
    {
        return true;
    }
    else
    {
        return false
    }
}

function CheckIfVIP(name)
{
    let n = name.toUpperCase();
    let valid = ["Pablo","Carlos","Mario"];

    let isValid;

    for(var i = 0; i < valid.length;i++)
    {
        if(n == valid[i].toUpperCase() )
        {
            isValid =  true; 
        }
        else
        {
            isValid = false
        }
    }

    return isValid;

}

function EsPerfecto(num)
 {
    let divisores = 0;

    for (let i = 1; i < num; i++)
    {
        if (num % i == 0) 
        {
            divisores += i;
        }
    }

    return divisores == num;
}


function CheckImpares()
{
    let i = 0
    while (i <= 50)
    {
        if(i%2!=0)
        {
            console.log("este es un numero impar : " + i)
        }
        
        i++;
    }
}

