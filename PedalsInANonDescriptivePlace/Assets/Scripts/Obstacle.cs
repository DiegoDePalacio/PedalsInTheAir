using System;
using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float _collisionDamage;
        [SerializeField] private float _frictionDamage;

        private Seagull _collidingSeagull;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Seagull"))
            {
                _collidingSeagull = other.gameObject.GetComponent<Seagull>();
                _collidingSeagull.DecreaseHealth(_collisionDamage);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == _collidingSeagull.gameObject)
            {
                _collidingSeagull = null;
            }
        }

        private void FixedUpdate()
        {
            _collidingSeagull?.DecreaseHealth(_frictionDamage);
        }
    }
}