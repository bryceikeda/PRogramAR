using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace ProgramAR
{
    namespace Menu
    {
        [RequireComponent(typeof(Interactable))]
        public class TabSwitcher : MonoBehaviour
        {
            PageController pageController;
            Interactable interactable;

            [SerializeField]
            private PageType connectedPage;

            void Awake()
            {
                interactable = GetComponent<Interactable>();
                interactable.OnClick.AddListener(OnClick);
                pageController = PageController.Instance;
            }

            void OnClick()
            {
                pageController.TurnPageOnSingle(connectedPage);
            }
        }
    }
}
