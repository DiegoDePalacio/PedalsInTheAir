using System;
using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Collider))]
    public class Person : MonoBehaviour
    {
        [SerializeField] private float _health;

        public void OnBeingPooped(float damage)
        {
            _health -= damage;

            if (_health <= 0f)
            {
                Die();
            }
            
            UpdateUI();
        }

        private void Die()
        {
            throw new NotImplementedException();
        }

        private void UpdateUI()
        {
            throw new NotImplementedException();
        }
    }
}