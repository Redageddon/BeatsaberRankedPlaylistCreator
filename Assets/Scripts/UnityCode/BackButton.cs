using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCode
{
    public class BackButton : MonoBehaviour
    {
        public void OnClicked() => SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}