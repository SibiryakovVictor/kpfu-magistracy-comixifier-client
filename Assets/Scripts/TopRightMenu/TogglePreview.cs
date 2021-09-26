using UnityEngine;
using UnityEngine.UI;

namespace TopRightMenu
{
    public class TogglePreview : MonoBehaviour
    {
        private void Awake()
        {
            var toggle = gameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate
            {
                var preview = gameObject.transform.FindDeepChild("ResultPreview");
                preview.gameObject.SetActive(!toggle.isOn);
            });
        }
    }
}