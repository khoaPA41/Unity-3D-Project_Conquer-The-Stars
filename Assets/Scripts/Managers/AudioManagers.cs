using System.Collections;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagers : MonoBehaviour
{
    public static AudioManagers Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(Transform pos, AudioResource resource)
    {
        var audioPool = ObjectPoolingManagers.Instance.GetPooledObject("Audio", pos.position);
        var audioSource = audioPool.GetComponent<AudioSource>();
        audioSource.resource = resource;
        audioSource.Play();
        StartCoroutine(WaitToReturnSound(audioPool, audioSource));
    }

    private IEnumerator WaitToReturnSound(PooledObject pooledObject, AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        pooledObject.Release();
    }
}
