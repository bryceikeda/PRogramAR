 

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class AddClauseButton : MonoBehaviour
    {
        Interactable interactable;
        [SerializeField] AddClauseEvent addClauseEvent; 
        [SerializeField] HideClauseListEvent hideClauseListEvent; 

        private void OnValidate()
        {
            interactable = GetComponent<Interactable>();
        }

        private void OnEnable()
        {
            interactable.OnClick.AddListener(hideClauseListEvent.Raise);
        }

        private void OnDisable()
        {
            interactable.OnClick.RemoveListener(hideClauseListEvent.Raise);
        }
    }
}
