using System;
using TopRightMenu.Comixifier;
using TopRightMenu.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TopRightMenu
{
    public class DropdownComixifiers : MonoBehaviour
    {
        public Dropdown dropdown;
        
        private EventBus _eventBus;

        private Button _buttonRun;
        
        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        private void Awake()
        {
            dropdown = gameObject.GetComponent<Dropdown>();
            if (dropdown == null)
            {
                throw new Exception("can't get component Dropdown: result is null");
            }
            
            var buttonRun = GameObject.Find("Button Run").GetComponent<ButtonRunComixifier>().gameObject.GetComponent<Button>();
            if (dropdown.value == 0)
            {
                buttonRun.interactable = false;
            }
            
            dropdown.onValueChanged.AddListener(delegate
            {
                var dropdownValue = dropdown.value;
                Debug.Log("new dropdown value: " + dropdownValue);

                if (dropdownValue == 0)
                {
                    buttonRun.interactable = false;
                }
                else
                {
                    buttonRun.interactable = true;
                    
                    var uiComixifier = MapToComixifierUI(dropdown.value);
                    
                    uiComixifier.Activate();
                    
                    var comixifier = uiComixifier.GetComixifier();
                    _eventBus.InvokeComixifierChosenEvent(comixifier);
                }
            });
        }

        private UIComixifier MapToComixifierUI(int dropdownValue)
        {
            return dropdownValue switch
            {
                1 => gameObject.GetComponent<UIVanceAI>(),
                2 => gameObject.GetComponent<UIFace2Comics>(),
                3 => gameObject.GetComponent<UICutOut>(),
                _ => throw new Exception("Comixifier not found")
            };
        }
    }
}
