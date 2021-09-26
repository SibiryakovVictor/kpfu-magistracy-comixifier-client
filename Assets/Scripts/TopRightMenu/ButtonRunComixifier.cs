using System;
using OpenNLP.Tools.Trees.TRegex.Tsurgeon;
using TopRightMenu.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TopRightMenu
{
    public class ButtonRunComixifier : MonoBehaviour
    {
        public Button button;
        
        private EventBus _eventBus;
        
        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
            Debug.Log("ButtonRunComixifier: wow, I've got EventManager!");
        }
        
        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
            if (button == null)
            {
                throw new Exception("can't get component Dropdown: result is null");
            }

            button.onClick.AddListener(delegate
            {
                button.interactable = false;
                _eventBus.InvokeComixifierInitializedEvent();
            });
            
            _eventBus.SubscribeGotComixifiedImageEvent(delegate(string path)
            {
                button.interactable = true;
            });
        }
    }
}
