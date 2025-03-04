using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static readonly object _lock = new object();
	public static T Instance
	{
		get
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = FindFirstObjectByType<T>();

					if (_instance == null)
					{
						GameObject singletonObj = new GameObject(typeof(T).Name);
						_instance = singletonObj.AddComponent<T>();
						DontDestroyOnLoad(singletonObj);
					}
				}
				return _instance;
			}
		}
	}

	protected virtual void Awake()
	{
		if (transform.parent != null)
			transform.SetParent(null);

		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this) 
		{
			Destroy(gameObject);
		}
		

	}

}
