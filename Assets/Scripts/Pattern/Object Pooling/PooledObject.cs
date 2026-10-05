using UnityEngine;

namespace ConquerTheStars.Pattern.Object_Pooling
{
    public class PooledObject : MonoBehaviour
    {
        private bool isReleased;
        private void OnEnable() => isReleased = false;

        public PooledObjectId ObjectId { get; set; }
        public void Release()
        {
            if (isReleased) return;
            isReleased = true;
            ObjectPoolingManagers.Instance.Release(this);
        }
    }
}

