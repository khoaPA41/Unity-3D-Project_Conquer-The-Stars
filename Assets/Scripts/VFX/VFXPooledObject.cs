using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

namespace ConquerTheStars.Vfx
{
    public class VFXPooledObject : MonoBehaviour
    {
        [Header("Particle System")]
        [SerializeField]
        private ParticleSystem vfx;

        [Header("PooledObject")]
        [SerializeField]
        private PooledObject pooledObject;

        private void OnEnable()
        {
            vfx.Play();
        }


        private void OnParticleSystemStopped()
        {
            vfx.Stop();
            pooledObject.Release();
        }
    }
}
