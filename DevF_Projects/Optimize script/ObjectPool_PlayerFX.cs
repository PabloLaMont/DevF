using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

[ExecuteInEditMode]
public class ObjectPool_PlayerFX : ObjectPoolBase{

	public static ObjectPool_PlayerFX instance { get; private set; }
	
	#region member
	/// <summary>
	/// Member class for a prefab entered into the object pool
	/// </summary>
	[Serializable]
	public class ObjectPoolEntry {
		/// <summary>
		/// the object to pre instantiate
		/// </summary>
		[SerializeField]
		public GameObject Prefab;
		
		/// <summary>
		/// quantity of object to pre-instantiate
		/// </summary>
		[SerializeField]
		public int Count;

		[HideInInspector]
		public List<GameObject> pool;

		[HideInInspector]
		public int objectsInPool = 0;
	}
	#endregion
	
	/// <summary>
	/// The object prefabs which the pool can handle
	/// by The amount of objects of each type to buffer.
	/// </summary>
	public ObjectPoolEntry[] Entries;


	protected GameObject ContainerObject;
    private float numFrames = 1;

    [ContextMenu("SetCerosCount")]
	public void SetCerosCount()
    {
		int maxEntries = Entries.Length;

		for (int i = 0; i < maxEntries; i++)
        {
			Entries[i].Count = 0;

		}

    }
	void OnEnable()
	{
		instance = this;
        
	}
	/*
	// Use this for initialization
	void Awake()
	{

        #if UNITY_EDITOR

        if (!UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }

        #endif

        ContainerObject = this.gameObject;//new GameObject("ObjectPool");

		//Loop through the object prefabs and make a new list for each one.
		//We do this because the pool can only support prefabs set to it in the editor,
		//so we can assume the lists of pooled objects are in the same order as object prefabs in the array

		int max = Entries.Length;

		for (int i = 0; i < max; i++)
		{
			ObjectPoolEntry objectPrefab = Entries[i];

			//create the repository
			//objectPrefab.pool = new GameObject[objectPrefab.Count];

			#if UNITY_EDITOR
			objectPrefab.Count = 1;
			#endif

			
			objectPrefab.pool = new List<GameObject>(new GameObject[objectPrefab.Count]);
			objectPrefab.objectsInPool = 0;
			

			TestObje(objectPrefab);
			//fill it               

		}
		ObjectPool_Dynamic.instance.AvisaSceneControllerTerminoPools(this.name);
	}


	*/

	
	 void Awake()
	 {
		ContainerObject = this.gameObject;

		
		foreach (ObjectPoolEntry objectPrefab in Entries)
		{
			objectPrefab.pool = new List<GameObject>(new GameObject[objectPrefab.Count]);
			objectPrefab.objectsInPool = 0;


			StartLoading(objectPrefab);
		}

		ObjectPool_Dynamic.instance.AvisaSceneControllerTerminoPools(this.name);
	 }
	 
	  
	 private void StartLoading(ObjectPoolEntry objectPrefab) 
	 {
		int max = objectPrefab.Count;
		GameObject prefab = objectPrefab.Prefab;
		int objectsPerFrame = Mathf.CeilToInt((float)max / (float)numFrames);

		int startIndex = 0;
		while (startIndex < max) {
			int endIndex = Mathf.Min(startIndex + objectsPerFrame, max);
			ThreadPool.QueueUserWorkItem((state) => {
				for (int i = startIndex; i < endIndex; i++) {
					GameObject newObj = Instantiate(prefab);
					newObj.transform.rotation = prefab.transform.rotation;
					newObj.name = prefab.name;
					PoolObject(newObj, true);
				}
			});
			startIndex = endIndex;
		}
	 }	 
	 
	/*

	void Awake()
	{
		ContainerObject = this.gameObject;
		int max = Entries.Length;

		for (int i = 0; i < max; i++)
		{
			ObjectPoolEntry objectPrefab = Entries[i];
        
			objectPrefab.pool = new List<GameObject>(new GameObject[objectPrefab.Count]);
			objectPrefab.objectsInPool = 0;
        
			StartCoroutine(RunListRutine(objectPrefab));
		}

		ObjectPool_Dynamic.instance.AvisaSceneControllerTerminoPools(this.name);
	}

	private IEnumerator RunListRutine(ObjectPoolEntry objectPrefab) 
	{
		int max = objectPrefab.Count;
		int objectsPerFrame = 10;
    
		GameObject prefab = objectPrefab.Prefab;
    

		for (int n = 0; n < max; n += objectsPerFrame)
		{
			for (int j = n; j < n + objectsPerFrame && j < max; j++)
			{
				GameObject newObj = Instantiate(prefab);
				newObj.transform.rotation = prefab.transform.rotation;
				newObj.name = prefab.name;
				PoolObject(newObj, true);
				yield return null;
			}
		}

		//ObjectPool_Dynamic.instance.AvisaSceneControllerTerminoPools(this.name);
	} 

	

	private void TestObje(ObjectPoolEntry objectPrefab) 
	{
		int max = objectPrefab.Count;
		GameObject prefab = objectPrefab.Prefab;
	

		for (int n = 0; n < max; n++)
		{
			GameObject newObj = Instantiate(prefab);
			newObj.transform.rotation = prefab.transform.rotation;
	        newObj.name = prefab.name;
			PoolObject(newObj, true);
		}
	}

	*/

	public void RegeneraObjectPool()
	{
		int maxEntries = Entries.Length;

		for (int i = 0; i < maxEntries; i++)
		{
			ObjectPoolEntry objectPrefab = Entries[i];
			
			if (objectPrefab.Count > 0)
			{
				int cuantoFaltan = 0;
				for (int ikk = 0; ikk < objectPrefab.pool.Count; ikk++)
				{
					if (objectPrefab.pool[ikk] == null)
					{
						cuantoFaltan++;
					}
				}

				objectPrefab.objectsInPool = objectPrefab.pool.Count;

				if (cuantoFaltan > 0)
				{
					Debug.Log("<color=yellow>  :: : : : : : : :  :  cuantoFaltan   </color>" + cuantoFaltan + "   " );

					Entries[i].pool.RemoveAll(item => item == null);

					for (int jj = 0; jj < cuantoFaltan; jj++)
					{

						GameObject newObj = (GameObject)Instantiate(objectPrefab.Prefab);
						newObj.transform.rotation = objectPrefab.Prefab.transform.rotation;
						newObj.name = objectPrefab.Prefab.name;
						newObj.transform.parent = ContainerObject.transform;
						newObj.transform.rotation = Quaternion.identity;
						newObj.SetActive(false);
						objectPrefab.pool.Add(newObj);
					}
				}				
			}
		}


		Debug.Log("<color=yellow>  :: : : : : : : :  :  aviso   </color>" );

		ObjectPool_Dynamic.instance.AvisaSceneControllerTerminoPools(this.name);

	}


	public GameObject GetObjectForType(string objectType, bool onlyPooled)
	{
		
		for (int i = 0; i < Entries.Length; i++)
		{
			GameObject prefab = Entries[i].Prefab;
			
			if (prefab.name != objectType){
				continue;
			}

			if (prefab == null)
			{
				return null;
			}
			#if UNITY_EDITOR
			onlyPooled = false;
			#endif
			
			if (Entries[i].objectsInPool > 0)
			{
				
				GameObject pooledObject = Entries[i].pool[--Entries[i].objectsInPool];
				if (pooledObject!= null)
				{
					pooledObject.transform.parent = ObjectPool_Dynamic.instance.transform;
				}
				return pooledObject;

			} else if (!onlyPooled || Entries[i].objectsInPool == 0)
			{
				GameObject obj = (GameObject)Instantiate(Entries[i].Prefab);
				obj.name = Entries[i].Prefab.name;// obj.name;// +"_not_pooled";

				if (obj!= null)
				{
					obj.transform.parent = ObjectPool_Dynamic.instance.transform;
				}
				
				Entries[i].pool.Add(obj);
				return obj;
			}
		}

		return null;
	}
	

	public void PoolObject(GameObject obj, bool firstTime = false)
	{  

		//Debug.Log ("Pool " + Entries.Length + " / " + obj.name);
		for (int i = 0; i < Entries.Length; i++)
		{
            if (Entries[i].Prefab.name != obj.name)
            {
                continue;
            }

			
			obj.transform.parent = ContainerObject.transform;
			obj.transform.rotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
            if (firstTime)
            {
                //Debug.Log("rotation:  " + obj.transform.rotation);
                obj.transform.rotation = obj.transform.rotation;
            }
			obj.SetActive(false);
			
			//Debug.Log(obj.name);
			//Debug.Log(Entries[i].objectsInPool + " / " + Entries[i].Count + " / " + i) ;
			if (Entries[i].objectsInPool == Entries[i].Count)
			{
				//Debug.Log("Destroy " + obj.name);
				//Destroy(obj);
				return;
			}

            Entries[i].pool[Entries[i].objectsInPool] = obj;
			Entries[i].objectsInPool ++;
			return;
		}
		
		Destroy(obj);
		//GetObjectForType ("Chispitas", true);
	}


    public List<string> ReturnNamesList()
    {
	   
        return new List<ObjectPoolEntry>(Entries).Select(a => a.Prefab.name).ToList();
    }
	public override void FinalizaEscena()
	{

	}


	public override void RecargaEscena()
	{
		Debug.Log("recarga player fx");

		RegeneraObjectPool();

	}

}
