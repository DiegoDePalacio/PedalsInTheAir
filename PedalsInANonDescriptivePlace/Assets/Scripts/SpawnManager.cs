using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PedalsInANonDescriptivePlace
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private Food _foodPrefab;
        [SerializeField] private float _foodSpawningRate;
        private float _timeUntilNextFoodSpawn;

        [SerializeField] private HealthRecoverer _healthRecovererPrefab;
        [SerializeField] private float _healthRecovererSpawningRate;
        private float _timeUntilNextHealthRecovererSpawn;
        
        [SerializeField] private List<Vector3> _spawningPlaces;

        private static SpawnManager _instance;
        public static SpawnManager Instance => _instance;

        private void Awake()
        {
            _instance = this;
            _timeUntilNextFoodSpawn = _foodSpawningRate;
            _timeUntilNextHealthRecovererSpawn = _healthRecovererSpawningRate;
        }

        public Vector3 GetRandomSpawningPlace()
        {
            if (_spawningPlaces.Count == 0)
            {
                return Vector3.zero;
            }
        
            int selectedPlaceIndex = Random.Range(0, _spawningPlaces.Count);
            Vector3 selectedPlace = _spawningPlaces[selectedPlaceIndex];
            _spawningPlaces.Remove(selectedPlace);
            return selectedPlace;
        }

        public void AddSpawningPlace(Vector3 spawningPlace)
        {
            _spawningPlaces.Add(spawningPlace);
        }

        private void FixedUpdate()
        {
            TrySpawnFood();
            TrySpawnHealthRecoverer();
        }

        private void TrySpawnHealthRecoverer()
        {
            _timeUntilNextHealthRecovererSpawn -= Time.deltaTime;

            if (_timeUntilNextHealthRecovererSpawn <= 0f)
            {
                Vector3 spawningPlace = GetRandomSpawningPlace();
            
                if (spawningPlace != Vector3.zero)
                {
                    // TODO: Use pool
                    Instantiate(_healthRecovererPrefab, spawningPlace, Quaternion.identity);
                }

                _timeUntilNextHealthRecovererSpawn = _healthRecovererSpawningRate;
            }
        }

        private void TrySpawnFood()
        {
            _timeUntilNextFoodSpawn -= Time.deltaTime;

            if (_timeUntilNextFoodSpawn <= 0f)
            {
                Vector3 spawningPlace = GetRandomSpawningPlace();
            
                if (spawningPlace != Vector3.zero)
                {
                    // TODO: Use pool
                    Instantiate(_foodPrefab, spawningPlace, Quaternion.identity);
                }

                _timeUntilNextFoodSpawn = _foodSpawningRate;
            }
        }
    }
}