using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PedalsInANonDescriptivePlace
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private Image _gameOverImage;

        public static GameOverManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        public void GameOver()
        {
            Time.timeScale = 0;
            _gameOverImage.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}