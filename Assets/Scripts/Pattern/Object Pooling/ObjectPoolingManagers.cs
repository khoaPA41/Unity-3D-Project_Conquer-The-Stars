using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConquerTheStars.Pattern.Object_Pooling
{
    [Serializable]
    public class PoolObject
    {
        public PooledObject PooledObject;
        public PooledObjectId ObjectId;
        [Range(1, 20)] public uint Quantity;

    }
    public class ObjectPoolingManagers : MonoBehaviour
    {
        public static ObjectPoolingManagers Instance { get; private set; }

        [Header("Object Information")]
        [SerializeField] private List<PoolObject> poolObjectList;

        private Dictionary<PooledObjectId, Stack<PooledObject>> pooledObjectDict;
        private Dictionary<PooledObjectId, Transform> parentByNameDict;
        private Dictionary<PooledObjectId, PooledObject> objectByNameDict;

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


            SetupParentObject(); // Save Parent
            SetupPooledObject(); // Save Object
            Setup();
            IsSetupFinished = true;
        }

        private void SetupParentObject()
        {
            parentByNameDict = new();
            foreach (var pooledObject in poolObjectList)
            {
                var parent = new GameObject(pooledObject.ObjectId + "_Pool");
                parent.transform.SetParent(transform);

                parentByNameDict[pooledObject.ObjectId] = parent.transform; // Save parent Transform as soon as created
            }
        }

        private void SetupPooledObject()
        {
            objectByNameDict = new();
            foreach (var pooledObject in poolObjectList)
            {
                objectByNameDict[pooledObject.ObjectId] = pooledObject.PooledObject; // Save Pooled Object as soon as created
            }
        }

        private void Setup()
        {
            if (poolObjectList.Count == 0) return;

            pooledObjectDict = new Dictionary<PooledObjectId, Stack<PooledObject>>();
            foreach (var pooledObject in poolObjectList)
            {
                var poolStack = new Stack<PooledObject>();
                var parent = parentByNameDict[pooledObject.ObjectId];

                for (int i = 0; i < pooledObject.Quantity; i++)
                {
                    var newObject = Instantiate(pooledObject.PooledObject);
                    newObject.ObjectId = pooledObject.ObjectId;
                    newObject.name = pooledObject.ObjectId.ToString();
                    newObject.gameObject.transform.SetParent(parent);
                    newObject.gameObject.SetActive(false);
                    poolStack.Push(newObject);
                }
                pooledObjectDict.Add(pooledObject.ObjectId, poolStack);
            }
        }

        public PooledObject GetPooledObject(PooledObjectId objectName, Vector3 pos)
        {
            if (objectName == PooledObjectId.None || !pooledObjectDict.ContainsKey(objectName))
            {
                Debug.LogWarning($"Don't have object {objectName}");
                return null;
            }

            if (pooledObjectDict[objectName].Count == 0)
            {
                var newObject = Instantiate(objectByNameDict[objectName]);
                newObject.gameObject.SetActive(false);
                newObject.name = objectName.ToString();
                newObject.ObjectId = objectName;
                newObject.transform.position = pos;
                newObject.transform.rotation = Quaternion.identity;
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
            if (pooledObject.ObjectId == PooledObjectId.None || !pooledObjectDict.ContainsKey(pooledObject.ObjectId))
            {
                Debug.LogWarning("Don't have object");
                return;
            }
            pooledObject.gameObject.SetActive(false);
            pooledObject.transform.SetParent(parentByNameDict[pooledObject.ObjectId]);
            pooledObjectDict[pooledObject.ObjectId].Push(pooledObject);
        }
    }
}
