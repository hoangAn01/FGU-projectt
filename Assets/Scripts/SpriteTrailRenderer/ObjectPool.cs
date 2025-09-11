using System.Collections.Generic;
using UnityEngine;

namespace SpriteTrailRenderer
{
    public delegate void ReturnObjectToPool(GameObject objectToReturn);

    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private Stack<T> _objectsNotInUse;
        private int _numberOfObjectsToSpawnWhenEmpty;
        private GameObject _objectPrototype;

        public ObjectPool(GameObject objectProtoType, int numberOfObjectsToSpawn, int numberOfObjectsToAddWhenEmpty)
        {
            _objectPrototype = objectProtoType;

            _numberOfObjectsToSpawnWhenEmpty = numberOfObjectsToAddWhenEmpty;
            _objectsNotInUse = new Stack<T>(numberOfObjectsToSpawn);

            AddObjectsToPool(numberOfObjectsToSpawn);
        }

        public void HandleReturnToPool(GameObject objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            _objectsNotInUse.Push(objectToReturn.GetComponent<T>());
        }

        public T GetObjectFromPool()
        {
            if (_objectsNotInUse.Count <= 0)
            {
                AddObjectsToPool(_numberOfObjectsToSpawnWhenEmpty);
            }

            T projectileToReturn = _objectsNotInUse.Pop();
            return projectileToReturn;
        }

        private void AddObjectsToPool(int amountToAdd)
        {
            for (int i = 0; i < amountToAdd; i++)
            {
                GameObject newGameObject = Object.Instantiate(_objectPrototype, Vector3.zero, Quaternion.identity);
                newGameObject.SetActive(true);
                T poolObject = newGameObject.GetComponent<T>();
                poolObject.SetReturnToPool(HandleReturnToPool);

                // turn game object off
                newGameObject.SetActive(false);
                _objectsNotInUse.Push(poolObject);
            }
        }
    }
}