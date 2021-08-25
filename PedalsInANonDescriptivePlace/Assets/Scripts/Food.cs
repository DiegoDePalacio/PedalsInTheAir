using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private float _poopRefillAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Seagull"))
            {
                Seagull seagull = other.gameObject.GetComponent<Seagull>();
                seagull.RefillPoop(_poopRefillAmount);
            }
            
            SpawnManager.Instance.AddSpawningPlace(transform.position);
            
            // TODO: Use a pool
            Destroy(gameObject);
        }
    }
}