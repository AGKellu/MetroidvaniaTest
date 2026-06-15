using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Persistence : MonoBehaviour
{
    private static Persistence instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
