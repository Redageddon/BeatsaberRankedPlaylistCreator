using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCode
{
    public class CreateSpecificSceneLoader : MonoBehaviour
    {
        public void OpenScene() => SceneManager.LoadScene("CreateCustom", LoadSceneMode.Single);
    }
}