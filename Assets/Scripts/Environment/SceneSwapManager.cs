using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
