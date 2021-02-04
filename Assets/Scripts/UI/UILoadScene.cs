using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{

    public class UILoadScene : MonoBehaviour
    {

        public void LoadSceneByName(string name)
        {

            StartCoroutine(LoadSceneByNameCoroutine(name));

        }

        public IEnumerator LoadSceneByNameCoroutine(string name)
        {

            if (gameObject.TryGetComponent(out AudioSource audioSource))
            {

                yield return new WaitForSeconds(audioSource.clip.length);

            }

            SceneManager.LoadScene(name);

        }

    }

}
