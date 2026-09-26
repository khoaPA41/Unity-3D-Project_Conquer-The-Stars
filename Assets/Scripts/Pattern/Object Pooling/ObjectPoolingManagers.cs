using System;
using System.Collections.Generic;
using UnityEngine;

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
        private Dictionary<string, Transform> parentByNameDict;
        private Dictionary<string, PooledObject> objectByNameDict;

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


            SetupParrentObject(); // Save Parrent
            SetupPooledObject(); // Save Object
            Setup();
            IsSetupFinished = true;
        }

        private void SetupParrentObject()
        {
            parentByNameDict = new();
            foreach (var pooledObject in poolObjectList)
            {
                var parent = new GameObject(pooledObject.ObjectName + "_Pool");
                parent.transform.SetParent(transform);

                parentByNameDict[pooledObject.ObjectName] = parent.transform; // Save parrent Transform as soon as created
            }
        }

        private void SetupPooledObject()
        {
            objectByNameDict = new();
            foreach (var pooledObject in poolObjectList)
            {
                objectByNameDict[pooledObject.ObjectName] = pooledObject.PooledObject; // Save Pooled Object as soon as created
            }
        }

        private void Setup()
        {
            if (poolObjectList.Count == 0) return;

            pooledObjectDict = new Dictionary<string, Stack<PooledObject>>();
            foreach (var pooledObject in poolObjectList)
            {
                var poolStack = new Stack<PooledObject>();
                var parent = parentByNameDict[pooledObject.ObjectName];

                for (int i = 0; i < pooledObject.Quantity; i++)
                {
                    var newObject = Instantiate(pooledObject.PooledObject);
                    newObject.name = pooledObject.ObjectName;
                    newObject.gameObject.transform.SetParent(parent);
                    newObject.gameObject.SetActive(false);
                    poolStack.Push(newObject);
                }
                pooledObjectDict.Add(pooledObject.ObjectName, poolStack);
            }
        }

        public PooledObject GetPooledObject(string objectName, Vector3 pos)
        {
            if (String.IsNullOrEmpty(objectName) || !pooledObjectDict.ContainsKey(objectName))
            {
                Debug.LogWarning($"Don't have object {objectName}");
                return null;
            }

            if (pooledObjectDict[objectName].Count == 0)
            {
                var newObject = Instantiate(objectByNameDict[objectName]);
                newObject.name = objectName;
                newObject.transform.position = pos;
                newObject.transform.SetParent(parentByNameDict[objectName]);
                newObject.gameObject.SetActive(true);
                return newObject;
            }

            var existedObject = pooledObjectDict[objectName].Pop();

            existedObject.transform.position = pos;
            existedObject.transform.rotation = Quaternion.identity;
            existedObject.gameObject.SetActive(true);
            return existedObject;
        }

        public void Release(PooledObject pooledObject)
        {
            if (String.IsNullOrEmpty(pooledObject.name) || !pooledObjectDict.ContainsKey(pooledObject.name))
            {
                Debug.LogWarning("Don't have object");
                return;
            }
            pooledObject.gameObject.SetActive(false);
            pooledObject.transform.SetParent(parentByNameDict[pooledObject.name]);
            pooledObjectDict[pooledObject.name].Push(pooledObject);
        }
    }
}
