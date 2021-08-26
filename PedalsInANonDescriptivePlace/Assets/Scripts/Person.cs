using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;
namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Collider))]
    public class Person : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private String _name;
        [SerializeField] private Text _nameText;
        [SerializeField] private Image _healthFill;
        [SerializeField] private List<SFX> _sfx = new List<SFX>();
        
        public AgentManager _manager;
        
        private List<Transform> _waypoints = new List<Transform>();
        private float _maxHealth;
        private bool _isRandom;
        private int _currentDestinationIndex;

        private void Start()
        {
            _nameText.text = _name;
            _maxHealth = _health;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _waypoints[_currentDestinationIndex].position) <= 1)
                UpdateDestination();
        }

        private void UpdateDestination()
        {
            if (!_isRandom)
                _currentDestinationIndex = UnityEngine.Random.Range(0, _waypoints.Count - 1);
            else
            {
                if (_currentDestinationIndex >= _waypoints.Count - 1)
                    _currentDestinationIndex = 0;
                else
                    _currentDestinationIndex++;
            }

            _agent.destination = _waypoints[_currentDestinationIndex].position;
        }

        public void SetWaypointHolder(Transform parent, bool isRandom)
        {
            for (int i = 0; i < parent.childCount; i++)
                _waypoints.Add(parent.GetChild(i));
            
            _agent.SetDestination(_waypoints[1].position);
            _currentDestinationIndex = 1;
            _isRandom = isRandom;
        }

        public void OnBeingPooped(float damage)
        {
            SoundManager.Instance.PlaySound(SFX.LittlePoopHit);
            _health -= damage;
            
            int rndClip = Random.Range(0, _sfx.Count);
            SoundManager.Instance.PlaySound(_sfx[rndClip]);

            if (_health <= 0f)
            {
                Die();
            }
            
            UpdateUI();
        }

        private void Die()
        {
            _manager.ReduceNumberOfPeople();
            SoundManager.Instance.PlaySound(SFX.BigPoopHit);
            Destroy(gameObject);
        }

        private void UpdateUI()
        {
            _healthFill.fillAmount = _health / _maxHealth;
        }
    }
}