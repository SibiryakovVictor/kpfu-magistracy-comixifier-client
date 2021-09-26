using System.IO;
using TopRightMenu.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace TopRightMenu
{
    public class ResultPreview : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private RawImage carousel;

        private bool _isPreviewOpened;

        private string _curCarouselImgPath;

        private string _previewImgPath;

        private EventBus _eventBus;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        private void Awake()
        {
            var preview = gameObject.GetComponent<RawImage>();
            
            _eventBus.SubscribeComixifierInitializedEvent(delegate
            {
                var texture = new Texture2D(80, 45);
                preview.texture = texture;
            });
            
            _eventBus.SubscribeGotComixifiedImageEvent(delegate(string imgPath)
            {
                _previewImgPath = imgPath;
                
                var texture = new Texture2D(80, 45);
                texture.LoadImage(File.ReadAllBytes(_previewImgPath)); //TODO: maybe it could be done different
                preview.texture = texture;
            });

            carousel = gameObject.transform.FindDeepChild("Carousel").gameObject.GetComponent<RawImage>();
            
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Images/");
            _previewImgPath = files[^1];
            var texture = new Texture2D(80, 45);
            texture.LoadImage(File.ReadAllBytes(_previewImgPath)); //TODO: maybe it could be done different
            preview.texture = texture;
        }

        private void Update()
        {
            if (_isPreviewOpened)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Images/");

                    var nextImgPath = "";
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] == _curCarouselImgPath)
                        {
                            if (i != 0)
                            {
                                nextImgPath = files[i - 1];
                                break;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    if (nextImgPath == "")
                    {
                        return;
                    } 
                
                    var texture = new Texture2D(960, 540);
                    texture.LoadImage(File.ReadAllBytes(nextImgPath)); //TODO: maybe it could be done different
                    carousel.texture = texture;
                    carousel.gameObject.SetActive(true);

                    _curCarouselImgPath = nextImgPath;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Images/");
                    var nextImgPath = "";
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] == _curCarouselImgPath)
                        {
                            if ((i + 1) != files.Length)
                            {
                                nextImgPath = files[i + 1];
                                break;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    if (nextImgPath == "")
                    {
                        return;
                    } 
                
                    var texture = new Texture2D(960, 540);
                    texture.LoadImage(File.ReadAllBytes(nextImgPath)); //TODO: maybe it could be done different
                    carousel.texture = texture;
                    carousel.gameObject.SetActive(true);

                    _curCarouselImgPath = nextImgPath;
                } 
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _curCarouselImgPath = "";
                    carousel.gameObject.SetActive(false);
                    _isPreviewOpened = false;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var texture = new Texture2D(960, 540);
            texture.LoadImage(File.ReadAllBytes(_previewImgPath)); //TODO: maybe it could be done different
            carousel.texture = texture;
            carousel.gameObject.SetActive(true);

            _curCarouselImgPath = _previewImgPath;
            _isPreviewOpened = true;
        }
    }
}