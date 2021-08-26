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
        [SerializeField] private int _maxPoopCapacity;
        [SerializeField] private int _poopAvailable;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _health;
        [SerializeField] private float _autoHealthRecoverySpeed = 0.01f;
        [SerializeField] private float _touchDamage;
        [SerializeField] private GameObject _preparedPoop;

        private void Awake()
        {
            _preparedPoop.SetActive(false);
        }

        public bool RefillPoop(int amountToRefill)
        {
            if (_poopAvailable == _maxPoopCapacity)
                return false;
            
            _poopAvailable = Mathf.Min(_maxPoopCapacity, _poopAvailable + amountToRefill);
            return true;
        }

        public void DecreaseHealth(float damage)
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

            if (PlayerController.ShootDown1 || PlayerController.ShootDown2)
//            if (Input.GetButtonDown(LITTLE_POOP) || Input.GetButtonDown(BIG_POOP))
            {
                PreparePoop();
            }

            if (PlayerController.ShootUp1)
//            if (Input.GetButtonUp(LITTLE_POOP))
            {
                SpawnPoop(false);
            }
            else if (PlayerController.ShootUp2)          
//            else if (Input.GetButtonUp(BIG_POOP))
            {
                SpawnPoop(true);
            }
        }

        private void SpawnPoop(bool isBigPoop)
        {
            _preparedPoop.SetActive(false);
        
            // TODO: Replace by pool usage
            Instantiate(isBigPoop ? _bigPoopPrefab : _littlePoopPrefab);
        }

        private void OnTriggerEnter(Collider other)
        {
            DecreaseHealth(_touchDamage);
        }

        private void PreparePoop()
        {
            // Show some small brown sphere peeking out of the seagull's ass
            _preparedPoop.SetActive(true);
        }

        private void UpdateUI()
        {
//            throw new NotImplementedException();
        }

        private void Die()
        {
            throw new NotImplementedException();
        }
    }
}