using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EX ) GameManager 가 MonoSingleton 을 참조하게 되면 MonoSingleton 과 MonoSingleton 이 참조하고 있는
// MonoBehaviour 까지 참조해서 사용할 수 있다.
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	static bool bShutdown = false;
	private static T _instance = null;
	public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				if(bShutdown == false)
				{
					T instance = GameObject.FindObjectOfType<T>() as T;
					if (instance == null)
					{
						instance = new GameObject(
							typeof(T).ToString(),
							typeof(T)
							).GetComponent<T>();
					}

					InstanceInit(instance);

					Debug.Assert(_instance != null,
						typeof(T).ToString() + " 싱글턴 생성 실패");
				}
			}

			return _instance;
		}
	}

	private static void InstanceInit(Object instance)
	{
		_instance = instance as T;
		_instance.Init();
	}

	public virtual void Init()
	{
		DontDestroyOnLoad(_instance);
	}

	public virtual void OnDestroy()
	{
		_instance = null;
	}

	private void OnApplicationQuit()
	{
		_instance = null;
		bShutdown = true;
	}


}
