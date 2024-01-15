 

using Microsoft.MixedReality.Toolkit.UI;
using ProgramAR.Events;
using ProgramAR.Pages;
using ProgramAR.Menu; 
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.ButtonHelpers
{
    public class SaveCoordinator : MonoBehaviour, IToggleSaveVisibilityResponse
    {
        [SerializeField] ToggleSaveVisibilityEvent toggleSaveVisibilityEvent;
        [SerializeField] GameObject saveButton;

        [SerializeField] SaveClauseEvent saveClauseEvent;
        [SerializeField] SaveTAPEvent saveTAPEvent;

        [SerializeField] Interactable yourRulesPageButton;

        private string currentSaveOrigin; 



        private void OnValidate()
        {
            saveButton = transform.GetChild(0).gameObject; 
        }

        public void OnToggleSaveVisibilityEvent(bool visibility, string saveOrigin)
        {
            currentSaveOrigin = saveOrigin;
            saveButton.SetActive(visibility);
        }

        public void OnToggleSaveVisibilityEvent(bool visibility)
        {
            saveButton.SetActive(visibility);
        }

        public void OnSaveEvent()
        {
            if (currentSaveOrigin.Equals("ExpressionPage"))
            {
                saveClauseEvent.Raise(); 
            }
            else if (currentSaveOrigin.Equals("ClauseListManager"))
            {
                yourRulesPageButton.TriggerOnClick(true);
                saveTAPEvent.Raise(); 
            }
        }

        private void OnEnable()
        {
            toggleSaveVisibilityEvent.RegisterListener(this);
            saveButton.GetComponent<Interactable>().OnClick.AddListener(OnSaveEvent);
        }

        private void OnDisable()
        {
            toggleSaveVisibilityEvent.UnregisterListener(this);
            saveButton.GetComponent<Interactable>().OnClick.RemoveListener(OnSaveEvent);
        }


    }
}
