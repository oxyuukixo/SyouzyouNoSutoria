using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
<<<<<<< HEAD

    private static T instance;
    public static T Instance

=======
    private static T instance;
    public static T Instance
>>>>>>> origin/development
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }
<<<<<<< HEAD
=======

>>>>>>> origin/development
            return instance;
        }
    }
}
