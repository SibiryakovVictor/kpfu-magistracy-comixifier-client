using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopRightMenu
{
    public class ButtonRunTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Dropdown _dropdownComixifiers;

        private Button _btnRunComixifier;
        
        private GameObject _tooltip;
        
        private void Awake()
        {
            _tooltip = GameObject.Find("ToggleComixifier").GetComponent<ToggleComixifier>().buttonTooltip;
            if (_tooltip == null)
            {
                throw new Exception("can't get component Tooltip: result is null");
            }
            
            _dropdownComixifiers = GameObject.Find("Dropdown Comixifiers").GetComponent<DropdownComixifiers>().dropdown;
            if (_dropdownComixifiers == null)
            {
                throw new Exception("can't get component Dropdown: result is null");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData != null)
            {
                Debug.Log("enter");
                if (_dropdownComixifiers.value == 0)
                {
                    _tooltip.SetActive(true);
                }
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData != null)
            {
                Debug.Log("exit");
                _tooltip.SetActive(false);   
            }
        }
    }
}