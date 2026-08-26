using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace ConquerTheStars.Pattern.Object_Pooling
{
    [Serializable]
    public class PoolObject
    {
        public PooledObject PooledObject;
        public string ObjectName;
        [Range(1, 20)] public uint Quantity;

    }
    public class ObjectPoolingManagers : MonoBehaviour
    {
        public static ObjectPoolingManagers Instance { get; private set; }

        [Header("Object Information")]
        [SerializeField] private List<PoolObject> poolObjectList;

        private Dictionary<string, Stack<PooledObject>> pooledObjectDict;
        private List<GameObject> parentsObject;

        public bool IsSetupFinished { get; private set; }

        void Awake()
        {
            IsSetupFinished = false;
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);


            parentsObject = new List<GameObject>();
            foreach (var pooledObject in poolObjectList)
            {
                var parent = new GameObject(pooledObject.ObjectName + "_Pool");
                parent.transform.SetParent(transform);
                parentsObject.Add(parent);
            }
            Setup();
            IsSetupFinished = true;
        }

        private void Setup()
        {
            if (poolObjectList.Count == 0) return;

            pooledObjectDict = new Dictionary<string, Stack<PooledObject>>();
            foreach (var pooledObect in poolObjectList)
            {
                var pooleds = new Stack<PooledObject>();
                var parent = parentsObject.Find(temp => temp.name.Substring(0, temp.name.Length - 5).Contains(pooledObect.ObjectName)).transform;

                for (int i = 0; i < pooledObect.Quantity; i++)
                {
                    var newObject = Instantiate(pooledObect.PooledObject);
                    newObject.name = pooledObect.ObjectName;
                    newObject.gameObject.transform.SetParent(parent);
                    newObject.gameObject.SetActive(false);
                    pooleds.Push(newObject);
                }
                pooledObjectDict.Add(pooledObect.ObjectName, pooleds);
            }
        }

        public PooledObject GetPooledObject(string objectName, Vector3 pos, Vector3 quaternion)
        {
            if (String.IsNullOrEmpty(objectName) || !pooledObjectDict.ContainsKey(objectName))
            {
                Debug.Log($"Đang tìm Pool có tên: '{objectName}'");
                Debug.Log("Don't have object");
                return null;
            }

            if (pooledObjectDict[objectName].Count == 0)
            {
                var newObject = Instantiate(poolObjectList.Find(itemPool => itemPool.ObjectName.Contains(objectName)).PooledObject);
                newObject.name = objectName;
                newObject.transform.position = pos;
                newObject.transform.SetParent(parentsObject.Find(temp => temp.name.Substring(0, temp.name.Length - 5).Contains(objectName)).transform);
                newObject.gameObject.SetActive(true);
                // pooledObjectDict[objectName].Push(newObject);
                return newObject;
            }

            var existedObject = pooledObjectDict[objectName].Pop();

            existedObject.transform.position = pos;
            existedObject.transform.Rotate(quaternion);
            existedObject.gameObject.SetActive(true);
            return existedObject;
        }



        public void Release(PooledObject pooledObject)
        {
            if (String.IsNullOrEmpty(pooledObject.name) || !pooledObjectDict.ContainsKey(pooledObject.name))
            {
                Debug.Log("Don't have object");
                return;
            }

            pooledObject.gameObject.SetActive(false);
            pooledObjectDict[pooledObject.name].Push(pooledObject);
        }


    }
}
