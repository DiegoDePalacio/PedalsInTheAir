using System;
using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Poop : MonoBehaviour
    {
        [SerializeField] private float _damage;

        private void Awake()
        {
            Debug.Log("Poop " + _damage);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Person"))
            {
                Person person = other.gameObject.GetComponent<Person>();
                person.OnBeingPooped(_damage);
            }
            
            if (!other.gameObject.CompareTag("Seagull"))
                Destroy(gameObject);
        }
    }
}