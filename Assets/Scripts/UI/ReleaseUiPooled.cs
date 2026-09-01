using System;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReleaseUiPooled : MonoBehaviour
{
    [SerializeField] private PooledObject pooledObject;

    // void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    // private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    // {
    //     pooledObject.Release();
    // }
}
