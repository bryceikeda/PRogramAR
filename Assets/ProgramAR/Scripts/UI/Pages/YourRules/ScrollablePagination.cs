using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class ScrollablePagination : MonoBehaviour
    {
        [SerializeField]
        private ScrollingObjectCollection scrollView;

        private void OnValidate()
        {
            scrollView = GetComponent<ScrollingObjectCollection>(); 
        }

        public void ScrollUp()
        {
            scrollView.MoveByTiers(-4);
        }

        public void ScrollDown()
        {
            scrollView.MoveByTiers(4);
        }
    }
}
