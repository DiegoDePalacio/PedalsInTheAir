using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private int _poopRefillAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Seagull"))
            {
                Seagull seagull = other.gameObject.GetComponent<Seagull>();
                
                if (!seagull.RefillPoop(_poopRefillAmount))
                    return;
            
                SpawnManager.Instance.AddSpawningPlace(transform.position);
            
                // TODO: Use a pool
                Destroy(gameObject);
            }
        }
    }
}