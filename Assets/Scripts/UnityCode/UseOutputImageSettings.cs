using UnityEngine;
using UnityEngine.UI;

namespace UnityCode
{
    public class UseOutputImageSettings : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        private void Start() => this.toggle.isOn = Options.Default.UseOutputImage;

        public void OnToggled(bool value) => 
            Options.Default.UseOutputImage = value;
    }
}