using System;
using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    public class Seagull : MonoBehaviour
    {
        [SerializeField] private Poop _littlePoopPrefab;
        [SerializeField] private int _littlePoopSize = 1;
        [SerializeField] private Poop _bigPoopPrefab;
        [SerializeField] private int _bigPoopSize = 2;
        [SerializeField] private int _maxPoopCapacity;
        [SerializeField] private int _poopAvailable;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _health;
        [SerializeField] private float _autoHealthRecoverySpeed = 0.01f;
        [SerializeField] private float _touchDamage = 0.5f;
        [SerializeField] private float _crashDamage = 2f;
        [SerializeField] private GameObject _preparedPoopGameObject;
        [SerializeField] private Transform _poopHoleLocation;

        private bool _isPoopPrepared = false;

        private void Awake()
        {
            _preparedPoopGameObject.SetActive(false);
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

            if (PlayerController.ShootDown1)
            {
                if (_poopAvailable < _littlePoopSize)
                {
                    // TODO: Make a sound for empty stomach
                    return;
                }
                else
                {
                    _poopAvailable -= _littlePoopSize;
                    PreparePoop();
                }
            }

            if (PlayerController.ShootDown2)
            {
                if (_poopAvailable < _bigPoopSize)
                {
                    // TODO: Make a sound for empty stomach
                    return;
                }
                else
                {
                    _poopAvailable -= _bigPoopSize;
                    PreparePoop();
                }
            }

            if (PlayerController.ShootUp1)
            {
                SpawnPoop(false);
            }
            else if (PlayerController.ShootUp2)          
            {
                SpawnPoop(true);
            }
        }

        private void SpawnPoop(bool isBigPoop)
        {
            if (!_isPoopPrepared)
                return;
        
            _preparedPoopGameObject.SetActive(false);
        
            // TODO: Replace by pool usage
            Instantiate(isBigPoop ? _bigPoopPrefab : _littlePoopPrefab, _poopHoleLocation.position, Quaternion.identity);

            _isPoopPrepared = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            DecreaseHealth(_touchDamage);
        }

        private void OnCollisionEnter(Collision other)
        {
            DecreaseHealth(_crashDamage);
        }

        private void PreparePoop()
        {
            // Show some small brown sphere peeking out of the seagull's ass
            _preparedPoopGameObject.SetActive(true);
            _isPoopPrepared = true;
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