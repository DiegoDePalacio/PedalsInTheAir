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
        [SerializeField] private Image[] _introImages;
        [SerializeField] private GameObject _intro;
        [SerializeField] private GameObject _game;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;

        private int _currentSlide;
        private int _numberOfPeople;
        private int _maxNumberOfPeople;
        private float _timer;

        void Awake()
        {
            Time.timeScale = 0;
            _prevButton.gameObject.SetActive(false);
        }

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

        public void SkipIntro()
        {
            _intro.SetActive(false);
            _game.SetActive(true);
            Time.timeScale = 1;
        }

        public void NextIntro()
        {
            _currentSlide++;
            UpdateIntro();
        }

        public void PrevIntro()
        {
            _currentSlide--;
            UpdateIntro();
        }

        private void UpdateIntro()
        {
                _prevButton.gameObject.SetActive(_currentSlide != 0);

            if (_currentSlide > _introImages.Length - 1)
            {
                _intro.SetActive(false);
                _game.SetActive(true);
                Time.timeScale = 1;
            }

            for (int i = 0; i < _introImages.Length; i++)
                _introImages[i].enabled = (i == _currentSlide);
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