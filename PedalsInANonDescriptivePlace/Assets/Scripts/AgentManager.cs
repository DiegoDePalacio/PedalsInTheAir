using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PedalsInANonDescriptivePlace
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypointHolders;
        [SerializeField] private bool[] _isRandom;
        [SerializeField] private Person[] _characters;
        void Start()
        {
            for (int i = 0; i < _waypointHolders.Length; i++)
            {
                Person person = Instantiate(_characters[i], _waypointHolders[i].GetChild(0));
                person.SetWaypointHolder(_waypointHolders[i], _isRandom[i]);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }
    }
}