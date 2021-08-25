using System;
using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    public class Seagull : MonoBehaviour
    {
        private const string LITTLE_POOP = "Fire1";
        private const string BIG_POOP = "showStats";
    
        [SerializeField] private Poop _littlePoopPrefab;
        [SerializeField] private Poop _bigPoopPrefab;
        [SerializeField] private float _poopAvailable;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _health;
        [SerializeField] private float _autoHealthRecoverySpeed = 0.01f;

        public void RefillPoop(float amountToRefill)
        {
            _poopAvailable += amountToRefill;
        }

        public void OnColliding(float damage)
        {
            _health -= damage;

            if (_health <= 0f)
            {
                Die();
            }

            UpdateUI();
        }

        public void IncreaseHealth(float _amountToIncrease)
        {
            _health = Mathf.Min(_maxHealth, _health + _amountToIncrease);
            UpdateUI();
        }

        private void FixedUpdate()
        {
            IncreaseHealth(_autoHealthRecoverySpeed);

            if (Input.GetButtonDown(LITTLE_POOP) || Input.GetButtonDown(BIG_POOP))
            {
                PreparePoop();
            }

            if (Input.GetButtonUp(LITTLE_POOP))
            {
                SpawnPoop(false);
            }
            else if (Input.GetButtonUp(BIG_POOP))
            {
                SpawnPoop(true);
            }
        }

        private void SpawnPoop(bool isBigPoop)
        {
            // TODO: Replace by pool usage
            Instantiate(isBigPoop ? _bigPoopPrefab : _littlePoopPrefab);
        }

        private void PreparePoop()
        {
            // Show some small brown sphere peeking out of the seagull's ass
            throw new NotImplementedException();
        }

        private void UpdateUI()
        {
            throw new NotImplementedException();
        }

        private void Die()
        {
            throw new NotImplementedException();
        }
    }
}