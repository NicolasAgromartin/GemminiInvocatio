using UnityEngine;

public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour
{
    public static Type Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            gameObject.transform.SetParent(null);
            Instance = this as Type;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
