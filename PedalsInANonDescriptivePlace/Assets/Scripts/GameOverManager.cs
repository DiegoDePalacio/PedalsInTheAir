using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PedalsInANonDescriptivePlace
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private Image _gameOverImage;

        public void GameOver()
        {
            _gameOverImage.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}