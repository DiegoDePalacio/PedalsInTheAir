using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    public class HealthRecoverer : MonoBehaviour
    {
        [SerializeField] private float _healthAmountToRecover;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Seagull"))
            {
                Seagull seagull = other.gameObject.GetComponent<Seagull>();
                seagull.IncreaseHealth(_healthAmountToRecover);
            }
            
            SpawnManager.Instance.AddSpawningPlace(transform.position);
            
            // TODO: Use a pool
            Destroy(gameObject);
        }
    }
}