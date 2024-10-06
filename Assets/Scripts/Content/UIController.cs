using UnityEngine;
using UnityEngine.UIElements;

namespace Content
{
    public class UIController : MonoBehaviour
    {
        private VisualElement _centerDot;
        private VisualElement _popupWindow;

        private void OnEnable()
        {
            var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;


            _centerDot = rootVisualElement.Q<VisualElement>("center-dot");
            _popupWindow = rootVisualElement.Q<VisualElement>("popup-window");


            rootVisualElement.RegisterCallback<KeyDownEvent>(OnKeyDown);
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode != KeyCode.Space) return;
            
            var isPopupVisible = _popupWindow.style.display == DisplayStyle.Flex;

            if (isPopupVisible)
            {
                _popupWindow.style.display = DisplayStyle.None;
                _centerDot.style.display = DisplayStyle.Flex;
            }
            else
            {
                _popupWindow.style.display = DisplayStyle.Flex;
                _centerDot.style.display = DisplayStyle.None;
            }
        }
    }

}