using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace kitti
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadNextScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }

        public void LoadPreviousScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex - 1);
        }

        public void JumpToStartScene()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

