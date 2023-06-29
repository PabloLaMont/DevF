using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public enum PlayerInfoCircuitos { 

    Empty,
    P1,
    P2
}
[System.Serializable]
public struct PlayerInCircuito {

    public PlayerInfoCircuitos player;
}
//lista circuitos
[System.Serializable]
public class Circuitos
{

    [FoldoutGroup("Circuito")]
    public int circuitoId;
    [FoldoutGroup("Circuito")]
    public Color color;
    [FoldoutGroup("Circuito")]
    public List<NodoPlaca> nodos;
    [FoldoutGroup("Circuito")]
    public List<NodoPlaca> generadores;
    [FoldoutGroup("Circuito")]
    public Transform p1;
    [FoldoutGroup("Circuito")]
    public Transform p2;
    [FoldoutGroup("Circuito")]
    public PlayerInfoCircuitos player;
    [FoldoutGroup("Circuito")]
    public bool statusElectrificada;

}



[System.Serializable]
public struct NodosConectados
{
    public int circuidoId;
    public NodoPlaca nodo;
    public List<NodoPlaca> conexiones;
}



public class ManagerCircuitos : MonoBehaviour
{

    public List<Color> colores = new List<Color>();
    public List<Color> coloresOriginal = new List<Color>();
    //variables publicas
    public string playerActual = "p1";
    public PlayerInfoCircuitos playerActualEnum;



    protected static ManagerCircuitos instance;
    public static ManagerCircuitos Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<ManagerCircuitos>();
            if (instance != null)
            {
                return instance;
            }
            Create();
            return instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        coloresOriginal.Clear();
        coloresOriginal.AddRange( colores);
    }


    public static ManagerCircuitos Create()
    {       
        GameObject msd = new GameObject("ManagerCircuitos");
        instance = msd.AddComponent<ManagerCircuitos>();
        return instance;
    }





   



    public List<Circuitos> circuitos = new List<Circuitos>();
    public List<NodosConectados> nodosConexiones = new List<NodosConectados>();

    //public HashSet<Circuitos> circuitos = new HashSet<Circuitos>();
    //public HashSet<NodosConectados> nodosConexiones = new HashSet<NodosConectados>();


    public NodoPlaca nodoPlacaCurrent = null;
    public Circuitos circuitoCurrent = new Circuitos();







    //cuando una estaca chocoa con un nodo este sera asignado como el ultimo
    public void SetNodoCurrent(Object nodo )
    {
       
        //Debug.Log("<color=yellow>set nodo current</color>");
    }

    public void SetCircuitoCurrent (int nodoIndex)
    {
       
        //Debug.Log("<color=yellow>SetCircuitoCurrent</color>");
    }


    private int incre = 0;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodo"></param>
    /// <param name="np"></param>
    /// 

    bool existeEnCircuito = false;
    int circuitoIndexConectado;
    Circuitos circuitoTemp;


    /// <summary>
    /// Cuando una estaca llega a un nodo, este avisa a esta funcion para revisar si se debe crear un circuito o si se debe agregar a un circuito existente
    /// 
    /// </summary>
    /// <param name="nodo"></param>
    /// <param name="np"></param>
    public void OcurrioConexionNodo(Transform nodo , NodoPlaca np, PlayerInfoCircuitos playerInfo)
    //public void OcurrioConexionNodo(Transform nodo , NodoPlaca np)
    {
        //una estaca fue conectada a un nodo
        //Debug.Log("OcurrioConexionNodo " + playerActual + " / " + np.p1 + " / " + np.p2 + " / " + Time.time);
        //seteando el player que fue conectado en la placa conectada

        if (playerActual == "p1")
        {
            np.p1 = true;
        }
        else
        {
            np.p2 = true;
        }

        playerActualEnum = playerInfo;

        np.playerCircuitos.player = playerInfo;
        //Debug.Log(playerInfo + " / " + np.playerCircuitos.player);
        //donde se conecto el player actualmente,
        //checar si el nodo placa existe en un circuito o no
        existeEnCircuito = false;
        circuitoIndexConectado = circuitos.Count;
        


        for (int i = 0; i < circuitos.Count; i++)
        {
            //alamacenamos el circuito del nodo
            circuitoTemp = circuitos[i];


            existeEnCircuito = circuitoTemp.nodos.Find(x => x == np);
           // Debug.Log(existeEnCircuito);
            //el nodo pertenecia a un circuito
            if (existeEnCircuito)
            {
                //Debug.Log(nodoPlacaCurrent);
                //el player no estaba conectado a nada antes
                if (nodoPlacaCurrent == null)
                {
                    //Debug.Log("camino 1");
                    circuitoCurrent = circuitoTemp;
                    //Debug.Log(circuitoTemp.statusElectrificada);
                    circuitoIndexConectado = i;
                    //player me meto en ese circuito

                    if (playerActual == "p1")
                    {
                        circuitoTemp.p1 = PlayerRefSceneController.Instance.baseCollection.elPlayer;
                    }
                    else
                    {
                        circuitoTemp.p2 = PlayerRefSceneController.Instance.baseCollection.elPlayer;
                    }

                    circuitoTemp.player = playerActualEnum;
                    //falta apagar los elementos del circuito 

                    for (int j = 0; j < circuitoTemp.nodos.Count; j++)
                    {

                        if (playerActual == "p1")
                        {
                            circuitoTemp.nodos[j].p1 = true;
                        }
                        else
                        {
                            circuitoTemp.nodos[j].p2 = true;
                        }

                        circuitoTemp.nodos[j].playerCircuitos.player = playerActualEnum;
                        //np.playerCircuitos.player = playerInfo;
                    }


                    if (circuitoTemp.statusElectrificada)
                    {
                        np.SetEstacasElectrificado(true);

                        
                    }
                  
                    circuitos[i] = circuitoTemp;
                    //np.SetColorEstacas(circuitoTemp.color);

                }


                //el player SI  estaba conectado a un circuito antes
                else
                {

                    //Debug.Log("camino 2");
                    

                    if (circuitoTemp.circuitoId != circuitoCurrent.circuitoId)
                    {
                    
                        //te traes la info del nodo placa donde esta y la metes donde el player esta conectado
                        //despues se elimina ese circuito
                        for (int j = 0; j < circuitoTemp.nodos.Count; j++)
                        {                           
                            AddNodosCircuitoX(ref circuitoCurrent, circuitoTemp.nodos[j]);
                        }

                        for (int k = 0;  k < circuitoCurrent.generadores.Count; k++)
                        {
                            if (circuitoCurrent.generadores[k].electrifica)
                            {
                                Debug.Log("Se encontro un generador en el circuito");
                                ElectrificoCiruitoX(ref circuitoCurrent);
                                circuitoCurrent.statusElectrificada = true;
                                break;
                            }
                        }

                        //if (circuitoCurrent.statusElectrificada)
                        //{
                        //    Debug.Log("Prende los que acaban de llegar en caso de estar electrificado");
                        //    for (int k = 0; k < circuitoCurrent.nodos.Count; i++)
                        //    {
                        //        circuitoCurrent.nodos[k].SetEstacasELectrificado(circuitoCurrent.statusElectrificada);
                        //        circuitoCurrent.nodos[k].SetColorEstacas(circuitoCurrent.color);
                        //    }
                        //}
                        
                        //np.SetColorEstacas(circuitoCurrent.color);
                        //borrar al circuito del nodo
                        //colores.Insert(0, circuitoTemp.color);
                        circuitos.Remove(circuitoTemp);
                        //ResetColores();
                    }

                }


                break;
            }
        }


        //si no existe el nodo en un circuito hago un nuevo
        if (!existeEnCircuito)
        {

            //Debug.Log(!existeEnCircuito + " / " +nodoPlacaCurrent);
            //el player no estaba conectado a nada antes
            if (nodoPlacaCurrent == null)
            {
                //Debug.Log("camino 3");
                CreaCircuitoNuevo( np);
            }


            //el player SI estaba conectado a  antes a un circuito
            else
            {
                //Debug.Log("camino 4");
                //integra al nodo al circuito
                AddNodosCircuitoX(ref circuitoCurrent, np);

                for (int i = 0; i < circuitos.Count; i++)
                {
                    if(circuitos[i].circuitoId == circuitoCurrent.circuitoId)
                    {
                        circuitos[i]= circuitoCurrent;
                    }
                }

                //np.SetColorEstacas(circuitoCurrent.color);

            }
        }

        //como separarlo por player?

        SetNodosConexiones(np);
       // Debug.Log("///////");
        nodoPlacaCurrent = np;

        //si no existe el nodo en un circuito

        


    }


    Circuitos circuitoTempCircuitoNuevo;
    Color _color;

    


    /// <summary>
    /// Crea curcuitos nuevos basados en placas, se utiliza cuando un nodo ya murio y debe crear nuevos nuevos circuitos
    /// o cuando llega un nodo y no existia en ningun circuito
    /// al crearlo se agrega a la lista de circuitos y se asigna el player con el cual se conecto
    /// </summary>
    /// <param name="np"></param>
    public void CreaCircuitoNuevo(NodoPlaca np)
    {
        //Debug.Break();
        //Debug.Log("Crea Circuito nuevo " + np.transform + " / " + colores.Count + " / " + Time.time);
        circuitoTempCircuitoNuevo = new Circuitos();
        
        //_color = colores[0];
        //colores.RemoveAt(0);
        //circuitoTempCircuitoNuevo.color = _color;//new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        //circuitoTemp.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        circuitoTempCircuitoNuevo.circuitoId = incre;
        incre++;
        circuitoTempCircuitoNuevo.nodos = new List<NodoPlaca>();
        circuitoTempCircuitoNuevo.generadores = new List<NodoPlaca>();

        
        AddNodosCircuitoX(ref circuitoTempCircuitoNuevo, np);

        for (int i = 0; i < circuitoTempCircuitoNuevo.generadores.Count; i++)
        {
            if (circuitoTempCircuitoNuevo.generadores[i].electrifica)
            {
                Debug.Log("Generador activo encontrado");
                circuitoTempCircuitoNuevo.statusElectrificada = true;
            }
        }

        circuitoCurrent = circuitoTempCircuitoNuevo;
        //Debug.Log(circuitoTempCircuitoNuevo.statusElectrificada + " / " + circuitoCurrent.statusElectrificada);
        //np.colorCircuito = circuitoTempCircuitoNuevo.color;
        np.SetEstacasElectrificado(circuitoCurrent.statusElectrificada);
        //np.SetColorEstacas(circuitoCurrent.color);

        circuitos.Add(circuitoTempCircuitoNuevo);


    }

   /// <summary>
   /// se utiliza para crear o subdivididir circuitos
   /// la muerte de un nodo puede provocar que ahora se tengan que crear varios circuitos
   /// </summary>
   /// <param name="nps"></param>

    public void CreaCircuitoNuevo(List<NodosConectados> nps)
    {
        
        //Circuitos circuitoTempCircuitoNuevo;
        circuitoTempCircuitoNuevo = new Circuitos();
        //circuitoTemp.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //_color = colores[0];
        //colores.RemoveAt(0);
        //circuitoTempCircuitoNuevo.color = _color; //new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        circuitoTempCircuitoNuevo.circuitoId = incre;
        circuitoTempCircuitoNuevo.generadores = new List<NodoPlaca>();
        incre++;
        circuitoTempCircuitoNuevo.nodos = new List<NodoPlaca>();

       

        bool setPlayerIn = false;
        if (nodoPlacaCurrent != null)
        {
            if (nps.Exists(x => x.nodo == nodoPlacaCurrent))
            {
                setPlayerIn = true;
            }
        }

        for (int i = 0; i < nps.Count; i++)
        {
            //nps[i].nodo.colorCircuito = circuitoTempCircuitoNuevo.color;
            AddNodosCircuitoX(ref circuitoTempCircuitoNuevo, nps[i].nodo, setPlayerIn); 
        }       
       
        circuitoCurrent = circuitoTempCircuitoNuevo;
      
        for (int i = 0; i < nps.Count; i++)
        {
            nps[i].nodo.SetEstacasElectrificado(circuitoCurrent.statusElectrificada);
            nps[i].nodo.SetColorEstacas(circuitoCurrent.color);
        }
        
        circuitos.Add(circuitoTempCircuitoNuevo);
        Debug.Log("Creo Circuito nuevo");
    }

    /// <summary>
    /// agrega nodos a un circuito y les asigna el estado correspondiente, por ejemplo si es un nodo tipo generador o si esta elestrificado o no
    /// </summary>
    /// <param name="circuito"></param>
    /// <param name="np"></param>
    /// <param name="asignaPlayer"></param>
    public void AddNodosCircuitoX(ref Circuitos circuito, NodoPlaca np, bool asignaPlayer = true)
    {
        circuito.nodos.Add(np);
        //Debug.Log("np " + np + " : "  + np.IsGenerador + " : " + np.electrifica + " : " + asignaPlayer, np.gameObject);
        if (np.IsGenerador)
        {
            circuito.generadores.Add(np);

            if (np.electrifica)
            {
                if (!circuito.statusElectrificada)
                {
                    //Debug.Log("electrifico : " + circuito.circuitoId);
                    circuito.statusElectrificada = true;
                    ElectrificoCiruitoX(ref circuito);
                   
                }

            }
        }

        np.SetColorEstacas(circuito.color);

        if (asignaPlayer)
        {
            np.playerCircuitos.player = playerActualEnum;
                
            if (playerActual == "p1")
            {
                //Debug.Log("p1 ");
                np.p1 = true;
                circuito.p1 = PlayerRefSceneController.Instance.baseCollection.elPlayer;
            }
            else
            {
                np.p2 = true;
                circuito.p2 = PlayerRefSceneController.Instance.baseCollection.elPlayer;
            }

            //// 
            /// revisa si el player esta electrificando para que le cambie el status al circuito
            /// 
            if (playerElectrificado)
            {
                Debug.Log("Circuito nuevo debe electrificarse " + playerElectrificado + " / " + circuito.statusElectrificada);
                circuito.statusElectrificada = true;
            }

        }
        else
        {
            np.p1 = false;
            circuito.p1 = null;
            np.p2 = false;
            circuito.p2 = null;
        }


        if (circuito.statusElectrificada)
        {
            np.SetEstacasElectrificado(true);
            np.SetColorEstacas(circuito.color);
        }
    }

    public void UpdatePlacaCurrentOld() {
        /////
        ///falta actulizar la informacion de placa current con placaold cuando se reintegra la cuerda y se asigna al nodo anterior
        ///
        Debug.Log("UpdatePlacaCurrentOld");
    }
    /// <summary>
    /// se ejecuta cuando se corta la cuerda del player
    /// checa si se debe remover un circuito
    /// quita el generador del player del circuito
    /// </summary>
    public void CortonCuerda()
    {
        GeneradorOffPlayer();
        nodoPlacaCurrent = null;

        if (circuitoCurrent.nodos == null)
        {
            return;
        }

        circuitoCurrent.player = PlayerInfoCircuitos.Empty;
        if (playerActual == "p1")
        {
            circuitoCurrent.p1 = null;
        }
        else
        {
            circuitoCurrent.p2 = null;
        }



        for (int j = 0; j < circuitoCurrent.nodos.Count; j++)
        {
            circuitoCurrent.nodos[j].playerCircuitos.player = playerActualEnum;
            if (playerActual == "p1")
            {
                circuitoCurrent.nodos[j].p1 = false;
            }
            else
            {
                circuitoCurrent.nodos[j].p2 = false;
            }
        }


        if (circuitoCurrent.nodos.Count == 1)
        {

            //Debug.Log("solo un circuito");
          

            for (int i = 0; i < circuitos.Count; i++)
            {
                if (circuitos[i].circuitoId == circuitoCurrent.circuitoId)
                {
                    nodosConexiones.Remove(nodosConexiones.Find(x => x.nodo == circuitoCurrent.nodos[0]));

                    //colores.Insert(0, circuitos[i].color);
                    circuitos.RemoveAt(i);
                    //ResetColores();
                    break;
                }
            }

        }
        else if (circuitoCurrent.nodos.Count > 1)
        {
            for (int i = 0; i < circuitos.Count; i++)
            {
                if (circuitos[i].circuitoId == circuitoCurrent.circuitoId)
                {
                    circuitos[i] = circuitoCurrent;
                }
            }
        }







        circuitoCurrent = new Circuitos();



    }




    /// <summary>
    /// Setea las conexiones de los nodos
    /// </summary>
    NodosConectados nodosConectados;
    public void SetNodosConexiones(NodoPlaca npAdd)
    {
        //habia alguien los meto a los dos pero verifico que no existan
        //Debug.Log("SetNodosConexiones " + npAdd + " / " + nodoPlacaCurrent + " / " + Time.time);
        if (nodoPlacaCurrent != null)
        {


            //NodosConectados nodosConectados;
            //primero busco el nodo al que me conecte
            //si no existe lo agrego con conexion a current
            // si existe le agrego la conexion a current


            if (!nodosConexiones.Exists(x => x.nodo == npAdd))
            {
                //Debug.Log("NewConexion " + npAdd + " / " + nodoPlacaCurrent);
                NewConexion(npAdd, nodoPlacaCurrent);
            }
            else
            {
                //Debug.Log("SetConexion " + npAdd + " / " + nodoPlacaCurrent);
                nodosConectados = nodosConexiones.Find(x => x.nodo == npAdd);
                SetConexion(npAdd, nodoPlacaCurrent, nodosConectados);
            }



            //luego busco el current
            //si no existe lo agrego con conexion a nodo
            // si existe le agrego la conexion a nodo
            
            if (!nodosConexiones.Exists(x => x.nodo == nodoPlacaCurrent))
            {
                //Debug.Log("NewConexion/// " + npAdd + " / " + nodoPlacaCurrent);
                NewConexion(nodoPlacaCurrent, npAdd);
            }
            
            else
            {
                //Debug.Log("SetConexion/// " + npAdd + " / " + nodoPlacaCurrent);
                nodosConectados = nodosConexiones.Find(x => x.nodo == nodoPlacaCurrent);
                SetConexion(nodoPlacaCurrent, npAdd, nodosConectados);
            }

        }
    }


    NodosConectados nodoConectado;

    public void NewConexion(NodoPlaca nodo, NodoPlaca conexion)
    {
        //Debug.Log("New Conexion " + nodo + " / " + conexion);
        nodoConectado = new NodosConectados();
        nodoConectado.nodo = nodo;
        nodoConectado.conexiones = new List<NodoPlaca>();
        nodoConectado.conexiones.Add(conexion);
        nodosConexiones.Add(nodoConectado);
    }


    public void SetConexion(NodoPlaca nodo, NodoPlaca conexion, NodosConectados nodoConectado)    
    {
        //Debug.Log("Set Conexion " + nodo + " / " + conexion );
        nodoConectado.conexiones.Add(conexion);

    }











    public int indiceCircuitoTmp = -1;


    NodosConectados nodoMuertoTmp = new NodosConectados();

    List<NodoPlaca> conexionesNodoMuerto = new List<NodoPlaca>();

    //almacena las conexiones de un nodo i temporalmente
    List<NodoPlaca> conexionestmp;
    int indiceABorrar = -1;


    Circuitos circuitoTempMuerte;

    int indiceRevisionMuerte = 0;
    List<NodosConectados> nodosConexionesTmpMuerte = new List<NodosConectados>();
    NodosConectados nodoXMuerte = new NodosConectados();

    /// <summary>
    /// si el nodo muere debe buscarse en que circuito estaba para saber que hacer con el circuito, esto puede ser remover el circuito, o dividirlo
    /// tambien se busca en todas las relaciones que tenia y se quita de ellas
    /// en caso de ser generador tambien se debe descontar de ellas y si era el unico generador debe apagarlas, en caso de no ser el unico el status de electrificado se mantiene igual
    /// </summary>
    /// <param name="nodoMuerto"></param>
    public void MuerteNodo(NodoPlaca nodoMuerto)
    {
        Debug.Log("-----MuerteNodo ----/ " + nodoMuerto);

        indiceCircuitoTmp = -1;
        indiceCircuitoTmp = BuscaNodoEnCircuito(nodoMuerto);

        //Debug.Log("-----MuerteNodo ----/ " + indiceCircuitoTmp);

        if (indiceCircuitoTmp == -1)
        {

            Debug.Log("no esta en circuito");
           // //Debug.Break();
            return;
        }


        //Debug.Log("paso1");

        if (nodoMuerto.IsGenerador)
        {
            GeneradorOff(nodoMuerto);
        }

        nodoMuertoTmp = new NodosConectados();
        conexionesNodoMuerto = new List<NodoPlaca>();
        //almacena las conexiones de un nodo i temporalmente       
        indiceABorrar = -1;
        for (int i = 0; i < nodosConexiones.Count; i++)
        {
            conexionestmp = nodosConexiones[i].conexiones;
            //busca el nodo muerto en las conexiones del nodo i
            for (int j = 0; j < conexionestmp.Count; j++)
            {
                if (conexionestmp[j] == nodoMuerto)
                {
                    //si lo encuentra lo borra
                    conexionestmp.RemoveAt(j);
                    break;
                }
            }

            //si es el nodo muerto lo almacena nodoMuertoTmp de la list
            if (nodosConexiones[i].nodo == nodoMuerto)
            {
                nodoMuertoTmp = nodosConexiones[i];
                //nos traemos las conexiones
                conexionesNodoMuerto = nodosConexiones[i].conexiones;
                indiceABorrar = i;
                Debug.Log("Save Remover " + i);

            }
        }

        if (indiceABorrar >=0)
        {
            Debug.Log("Remover " + indiceABorrar);
            nodosConexiones.RemoveAt(indiceABorrar); 
        }


        //Debug.Log("paso2");

        //si hay mas de una conexion significa que se haran dos o mas circuitos
        if (conexionesNodoMuerto.Count > 1)
        {

            //Debug.Log("paso 3 - 1 ");
            //busca a que circuito pertenece
            circuitoTempMuerte =  new Circuitos();
            int indiceDelCircuito = 0;
            Debug.Log("circuitos " + circuitos.Count);
            for (int i = 0; i < circuitos.Count; i++)
            {
                //alamacenamos el circuito del nodo
                circuitoTempMuerte = circuitos[i];
                Debug.Log("circuitoTempMuerte " + i + " / "  + circuitoTempMuerte + " / Nodo Muerto " + nodoMuerto);
                if (circuitoTempMuerte.nodos.Find(x => x == nodoMuerto))
                {
                    Debug.Log("indiceDelCircuito donde murio " + i);
                    indiceDelCircuito = i;
                    indiceCircuitoTmp = indiceDelCircuito;
                    break;
                }

            }


            indiceRevisionMuerte = 0;
            nodosConexionesTmpMuerte = new List<NodosConectados>();
            nodoXMuerte = new NodosConectados();
            Debug.Log("conexionesNodoMuerto " + conexionesNodoMuerto.Count);
            for (int i = 0; i < conexionesNodoMuerto.Count; i++)
            {
                indiceRevisionMuerte = 0;
                nodosConexionesTmpMuerte.Clear();
                //siempre metemos en el primero el que se esta trabajando
                
                nodoXMuerte = BuscaNodoConexiones(conexionesNodoMuerto[i]);
                Debug.Log("conexionesNodoMuerto " + i  + " /Conexion " + conexionesNodoMuerto[i]+ " / indiceRevisionMuerte " + indiceRevisionMuerte + " / " + "nodoXMuerte " + nodoXMuerte.conexiones.Count);
                for (int j = 0; j < nodoXMuerte.conexiones.Count; j++)
                {
                    Debug.Log("Conexion muerto " + j + " / " + nodoXMuerte.conexiones[j]);
                }
                nodosConexionesTmpMuerte.Add(nodoXMuerte);

                CheckNewCircuitos(indiceRevisionMuerte,  ref nodosConexionesTmpMuerte);

                //crea un circuito tmp
                if (nodosConexionesTmpMuerte.Count > 1)
                {
                    //crea circuito tmp
                    Debug.Log("CreaCircuitoNuevo");
                    CreaCircuitoNuevo(nodosConexionesTmpMuerte);
                }
                else
                {
                    //Debug.Log("no hizo nada");

                    //si ahi esta el player

                    if (nodoPlacaCurrent != null)
                    {
                        if (nodoPlacaCurrent == conexionesNodoMuerto[i])
                        {
                            Debug.Log("///");
                            CreaCircuitoNuevo(conexionesNodoMuerto[i]);
                        }
                        else
                        {
                            conexionesNodoMuerto[i].playerCircuitos.player = PlayerInfoCircuitos.Empty;
                            if (playerActual == "p1")
                            {
                                conexionesNodoMuerto[i].p1 = false;
                            }
                            else
                            {
                                conexionesNodoMuerto[i].p2 = false;
                            }
                            Debug.Log("///---");
                            conexionesNodoMuerto[i].SetEstacasElectrificado(false);
                        }



                    }
                    else
                    {
                        conexionesNodoMuerto[i].playerCircuitos.player = PlayerInfoCircuitos.Empty;

                        if (playerActual == "p1")
                        {
                            conexionesNodoMuerto[i].p1 = false;
                        }
                        else
                        {
                            conexionesNodoMuerto[i].p2 = false;
                        }
                        Debug.Log("///---////");
                        conexionesNodoMuerto[i].SetEstacasElectrificado(false);
                    }

                }
            }

            

            //borrar circuito original
            //colores.Insert(0, circuitoTempMuerte.color);
            circuitos.Remove(circuitoTempMuerte);
            //ResetColores();



        }

        //si no no me acuerdo
        else{

            //Debug.Log("paso 3 - 2 " + indiceCircuitoTmp);
            Debug.Log("una sola conexion");

            //que se borre si el esta como generador           
            int indiceDelCircuito = indiceCircuitoTmp;


            bool onPlacaCurrent = false;


            if (nodoMuerto == nodoPlacaCurrent)
            {
                onPlacaCurrent = true;
                if (indiceDelCircuito != -1)
                {
                    circuitos[indiceDelCircuito].player = PlayerInfoCircuitos.Empty;

                    if (playerActual == "p1")
                    {
                        Debug.Log("nodoPlacaCurrent");
                        circuitos[indiceDelCircuito].p1 = null;
                    }
                    else
                    {
                        circuitos[indiceDelCircuito].p2 = null;
                    }


                    for (int j = 0; j < circuitos[indiceDelCircuito].nodos.Count; j++)
                    {

                        circuitos[indiceDelCircuito].player = PlayerInfoCircuitos.Empty;

                        if (playerActual == "p1")
                        {
                            Debug.Log("nodoPlacaCurrent into circuito");
                            circuitos[indiceDelCircuito].nodos[j].p1 = false;
                        }
                        else
                        {
                            circuitos[indiceDelCircuito].nodos[j].p2 = false;
                        }
                    }

                }

            }




            circuitos[indiceDelCircuito].nodos.Remove(nodoMuerto);
            if (circuitos[indiceDelCircuito].generadores.Contains(nodoMuerto))
            {
                Debug.Log("generadores remove");
                circuitos[indiceDelCircuito].generadores.Remove(nodoMuerto);
                CheckGeneradoresEnCircuito(indiceCircuitoTmp);
            }


         
           

            
            if (circuitos[indiceDelCircuito].nodos.Count == 1 && (circuitos[indiceDelCircuito].p1 == null) && (circuitos[indiceDelCircuito].p2 == null))
            {
                Debug.Log("longitud 1");
                //colores.Insert(0, circuitos[indiceDelCircuito].color);
                circuitos.RemoveAt(indiceDelCircuito);
                //ResetColores();

                indiceDelCircuito = -1;

                /*
                     After J
                     */

                indiceCircuitoTmp = -1;
            }

            if (indiceDelCircuito != -1)
                
            {
                if (circuitos[indiceDelCircuito].nodos.Count == 1 && (circuitos[indiceDelCircuito].nodos[0] != nodoPlacaCurrent))
                {
                    Debug.Log("longitud 01");
                    //colores.Insert(0, circuitos[indiceDelCircuito].color);
                    circuitos.RemoveAt(indiceDelCircuito);
                    //ResetColores();

                    indiceDelCircuito = -1;
                } 
            }

            if (indiceDelCircuito != -1)
            {
                if (circuitos[indiceDelCircuito].nodos.Count < 1)
                {

                    Debug.Log("longitud 0");
                    //colores.Insert(0, circuitos[indiceDelCircuito].color);
                    circuitos.RemoveAt(indiceDelCircuito);
                    //ResetColores();
                    //Debug.Log("longitud 00");



                    /*
                     After J
                     */

                    indiceCircuitoTmp = -1;
                } 
            }



          

        }


        ////Debug.Log("nodosConexiones.Count "+ nodosConexiones.Count);
        if (nodosConexiones.Count > 0)
        {
            for (int i = nodosConexiones.Count - 1; i >= 0; i--)
            {
                if (nodosConexiones[i].conexiones.Count == 0)
                {
                    // //Debug.Log("i:" + i);
                    Debug.Log("Remove COnexiones");
                    nodosConexiones.RemoveAt(i);
                }
            }
        }
      


        if (circuitos.Count==0)
        {

            circuitoCurrent = new Circuitos();
        }

        if (nodoMuerto == nodoPlacaCurrent)
        {
            //busca el circuios
            //int circuitoDndEstaba = BuscaNodoEnCircuito(nodoMuerto);
            Debug.Log("circuitoDndEstaba: ");

            nodoPlacaCurrent = null;
            circuitoCurrent = new Circuitos();
        }

        Debug.Log("Muerte Nodo " + indiceCircuitoTmp + " / " + circuitos.Count);

        if (indiceCircuitoTmp != -1 && circuitos.Count!=0)
        {
            IntentaApagarCircuito(indiceCircuitoTmp);

        }



        if (nodoPlacaCurrent == null)
        {
            //Debug.Log("El player no esta conectado a nada " + playerElectrificado);
            playerElectrificado = false;
        }
        //circuitos[indiceCircuitoTmp]


    }

    public NodosConectados BuscaNodoConexiones(NodoPlaca nodo)
    {
        return nodosConexiones.Find(x => x.nodo == nodo);
    }

    bool status = false;

    NodoPlaca nodoPlacaTmpGenerador;
    public void CheckGeneradoresEnCircuito(int indice)
    {

        Debug.Log("CheckGeneradoresEnCircuito");
        //Debug.Break();

        if(indice == -1)
        {
            return;
        }

        status = false;
        
        for (int i = 0; i < circuitos[indice].generadores.Count; i++)
        {
            nodoPlacaTmpGenerador = circuitos[indice].generadores[i];
            if (nodoPlacaTmpGenerador.electrifica)
            {
                status = true;
                break;
            }
        }


        if (!status)
        {

            if (circuitos[indice].p1 != null || circuitos[indice].p2 != null)
            {
                status = playerElectrificado;                
            }
            
        }

        //circuitos[indice].statusElectrificada = status;


    }


    NodosConectados nodoX;
    public void CheckNewCircuitos(int indiceRevision, ref List<NodosConectados> nodosConexionesTmp )
    {
        nodoX = new NodosConectados();
        nodoX = nodosConexionesTmp[indiceRevision];
       
        for (int j = 0; j < nodoX.conexiones.Count; j++)
        {
            if (!nodosConexionesTmp.Exists(x=> x.nodo == nodoX.conexiones[j]))
            {                
                nodosConexionesTmp.Add(BuscaNodoConexiones(nodoX.conexiones[j]));
            }
        }
               
        indiceRevision++;
        if (indiceRevision <= nodosConexionesTmp.Count - 1)
        {
            CheckNewCircuitos(indiceRevision, ref nodosConexionesTmp);
        }

    }

    Circuitos circuitoTempNodoEnCircuito = new Circuitos();
    public int BuscaNodoEnCircuito(NodoPlaca np)
    {
        circuitoTempNodoEnCircuito = new Circuitos();
        int indiceDelCircuito = -1;
        //Debug.Log("");
        //Debug.Log("Start Busqueda Nodos en Circuitos");
        //Debug.Log("NP find " + np, np.gameObject);
        for (int i = 0; i < circuitos.Count; i++)
        {
            //alamacenamos el circuito del nodo
            circuitoTempNodoEnCircuito = circuitos[i];
            //Debug.Log("i " + i + " / id: " + circuitoTempNodoEnCircuito.circuitoId);
            if (circuitoTempNodoEnCircuito.nodos.Find(x => x == np))
            {
                //Debug.Log("Finded in; " + i);
                indiceDelCircuito = i;
               
                break;
            }
        }

        return indiceDelCircuito;
    }



    Circuitos circuitoTempElectrificado = new Circuitos();
    public void BuscaNodoEnCircuitoStatusElectricado(NodoPlaca np)
    {
        circuitoTempElectrificado = new Circuitos();
        int indiceDelCircuito = -1;

        for (int i = 0; i < circuitos.Count; i++)
        {
            //alamacenamos el circuito del nodo
            circuitoTempElectrificado = circuitos[i];
            if (circuitoTempElectrificado.nodos.Find(x => x == np))
            {
                if (circuitoTempElectrificado.statusElectrificada)
                {
                    np.SetEstacasElectrificado(true);
                   
                }
                
                np.SetColorEstacas(circuitoTempElectrificado.color);
            }
            //for (int j = 0; j < circuitoTemp.nodos.Count; j++)
            //{
            //    circuitoTemp.nodos[j].colorCircuito = circuitoTemp.color;
            //}
        }
        
        
    }




    private Circuitos circuitoSetTmp = new Circuitos();
    private int indiceDelCircuito = 0;
    public void GeneradorOn(NodoPlaca np)
    {
        indiceDelCircuito = BuscaNodoEnCircuito(np);
        circuitoSetTmp = circuitos[indiceDelCircuito];
        circuitoSetTmp.statusElectrificada = true;
        circuitos[indiceDelCircuito] = circuitoSetTmp;
        ElectrificoCiruitoX(ref circuitoSetTmp);
        circuitoCurrent.statusElectrificada = true;

        

    }
    
    public void GeneradorOff(NodoPlaca np)
    {
        //Debug.Log("GeneradorOff");
        indiceDelCircuito = BuscaNodoEnCircuito(np);

        if (indiceDelCircuito >= 0)
        {
            IntentaApagarCircuito(indiceDelCircuito);
        }
            
    }



    public void IntentaApagarCircuito (int indice)
    {

        //if (indice >= circuitos.Count)
        //{
        //    Debug.Log("El indice que intenta apagar ya no existe en los circuitos");
        //    return;
        //}
        indiceDelCircuito = indice;
        circuitoSetTmp = circuitos[indiceDelCircuito];

        //Debug.Log("indiceDelCircuito: " + indiceDelCircuito);
        bool puedeApagar = true;
        for (int i = 0; i < circuitoSetTmp.generadores.Count; i++)
        {
            if (circuitoSetTmp.generadores[i].electrifica)
            {
                puedeApagar = false;
                break;
            }
        }

        // if (puedeApagar ) si el player esta aqui y esta electrificando  puedeApagar = false
        if (puedeApagar && playerElectrificado)
        {
            if (circuitoSetTmp.nodos.Exists(x => x == nodoPlacaCurrent))
            {
                puedeApagar = false;
            }
        }



       
        if (puedeApagar)
        {
            //Debug.Log("puedeApagar: " + puedeApagar + " / " + circuitoSetTmp.circuitoId + " / " + (circuitoSetTmp == circuitoCurrent));
            circuitoSetTmp.statusElectrificada = false;
            circuitos[indiceDelCircuito] = circuitoSetTmp;
            DeselectrificoCiruitoX(ref circuitoSetTmp);
           
            
            if (circuitoSetTmp == circuitoCurrent)
            {
                circuitoCurrent.statusElectrificada = false;
            }
        }


        /*
         se busca en todos los nodos y si alguno se queda con informacion perdida
         se remueve y apaga

         */


        //for (int i = 0; i < circuitos.Count; i++)
        //{
        //    //alamacenamos el circuito del nodo
        //    circuitoTempNodoEnCircuito = circuitos[i];
        //    Debug.Log("i " + i + " / id: " + circuitoTempNodoEnCircuito.circuitoId);
        //    if (circuitoTempNodoEnCircuito.nodos.Find(x => x == np))
        //    {
        //        Debug.Log("Finded in; " + i);
        //        indiceDelCircuito = i;

        //        break;
        //    }
        //}



        //for (int i = 0; i < nodosConexiones.Count; i++)
        //{
            
        //    if (nodosConexiones[i].nodo == null)
        //    {
                
        //    }
        //}
        
    }




    bool playerElectrificado = false;
    public void GeneradorOnPlayer()
    {
        
        if (nodoPlacaCurrent == null)
        {
            return;
        }
        Debug.Log("GeneradorOnPlayer");
        playerElectrificado = true;
        GeneradorOn(nodoPlacaCurrent);
    } 
    
    public void GeneradorOffPlayer()
    {
        if (nodoPlacaCurrent == null)
        {
            return;
        }
        playerElectrificado = false;
        //Debug.Log("GeneradorOffPlayer");
        GeneradorOff(nodoPlacaCurrent);
    }


    public void ElectrificoCiruitoX(ref Circuitos circuito)
    {
        for (int i = 0; i < circuito.nodos.Count; i++)
        {
            if (circuito.nodos[i].gameObject.activeInHierarchy)
            {
                circuito.nodos[i].SetEstacasElectrificado(true);
            }
        }
    }
    public void DeselectrificoCiruitoX(ref Circuitos circuito)
    {
        for (int i = 0; i < circuito.nodos.Count; i++)
        {
            if (circuito.nodos[i].gameObject.activeInHierarchy)
            {
                circuito.nodos[i].SetEstacasElectrificado(false);
            }
            
        }
    }



    private void ResetColores()
    {                
        if (circuitos.Count==0)
        {           
            colores = coloresOriginal;
        }
    }

}

