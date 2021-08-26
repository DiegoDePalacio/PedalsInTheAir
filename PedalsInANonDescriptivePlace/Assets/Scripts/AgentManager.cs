using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace PedalsInANonDescriptivePlace
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypointHolders;
        [SerializeField] private bool[] _isRandom;
        [SerializeField] private Person[] _characters;
        [SerializeField] private Text _peopleLeft;
        [SerializeField] private Text _timeText;

        private int _numberOfPeople;
        private int _maxNumberOfPeople;
        private float _timer;
        void Start()
        {
            for (int i = 0; i < _waypointHolders.Length; i++)
            {
                Person person = Instantiate(_characters[i], _waypointHolders[i].GetChild(0));
                person.SetWaypointHolder(_waypointHolders[i], _isRandom[i]);
                person._manager = this;
                _numberOfPeople++;
            }
            _maxNumberOfPeople = _numberOfPeople;
            _peopleLeft.text = _numberOfPeople + "/" + _maxNumberOfPeople;
        }

        private void FixedUpdate()
        {
            _timer += Time.deltaTime;
            _timeText.text = _timer.ToString("0:00");
        }

        public void ReduceNumberOfPeople()
        {
            _numberOfPeople--;
            _peopleLeft.text = _numberOfPeople + "/" + _maxNumberOfPeople;
        }
    }
}