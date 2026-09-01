using System.Collections.Generic;
using UnityEngine;
namespace ConquerTheStars.Pattern.Object_Pooling
{
    public class TestPooling : MonoBehaviour
    {
        private PooledObject object1;
        private PooledObject object2;
        private PooledObject object3;

        private Stack<PooledObject> pooledObject_I = new();
        private Stack<PooledObject> pooledObject_II = new();
        private Stack<PooledObject> pooledObject_III = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                object1 = ObjectPoolingManagers.Instance.GetPooledObject(
                    "Enemy_I",
                    new Vector3(0, 0, 0)
                );
                pooledObject_I.Push(object1);
                Debug.Log($"Spawn: {object1.name} | ID: {object1.GetInstanceID()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                object2 = ObjectPoolingManagers.Instance.GetPooledObject(
                    "Enemy_II",
                    new Vector3(2, 0, 0)
                );
                pooledObject_II.Push(object2);
                Debug.Log($"Spawn: {object2.name} | ID: {object1.GetInstanceID()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                object3 = ObjectPoolingManagers.Instance.GetPooledObject(
                    "Enemy_III",
                    new Vector3(4, 0, 0)
                );
                pooledObject_III.Push(object3);
                Debug.Log($"Spawn: {object3.name} | ID: {object1.GetInstanceID()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && object1 != null)
            {
                pooledObject_I.Pop().Release();
                Debug.Log("Release Enemy 1");
                Debug.Log($"Release: {object1.name} | ID: {object1.GetInstanceID()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) && object2 != null)
            {
                pooledObject_II.Pop().Release();
                Debug.Log("Release Enemy 2");
                Debug.Log($"Release: {object2.name} | ID: {object1.GetInstanceID()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha6) && object3 != null)
            {
                pooledObject_III.Pop().Release();
                Debug.Log("Release Enemy 3");
                Debug.Log($"Release: {object3.name} | ID: {object1.GetInstanceID()}");
            }
        }

    }
}
