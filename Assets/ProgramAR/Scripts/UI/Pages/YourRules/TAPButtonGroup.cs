using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class TAPButtonGroup : MonoBehaviour
    {
        [SerializeField] DeleteTAPEvent deleteTAPEvent;
        [SerializeField] EditTAPEvent editTAPEvent;
        [SerializeField] EditTAPPriorityEvent editTAPPriorityEvent;
        [SerializeField] DuplicateTAPEvent duplicateTAPEvent; 

        [SerializeField] TextMeshPro triggerTextBox;
        [SerializeField] TextMeshPro actionTextBox;

        [SerializeField] ButtonConfigHelper tapButtonConfig;
        [SerializeField] ButtonConfigHelper deleteButtonConfig;
        [SerializeField] ButtonConfigHelper cloneTAPButtonConfig;
        [SerializeField] ButtonConfigHelper priorityButtonConfig;
        [SerializeField] Renderer triggerBackPlate;
        [SerializeField] Renderer actionBackPlate;
        [SerializeField] GameObject isExecutingIndicator; 


        int selectedTAP;
        public Color32 invalid;
        public Color32 valid;

        public void SetProperties(string triggerText, string actionText, int selectedTAP)
        {
            this.selectedTAP = selectedTAP;
            priorityButtonConfig.MainLabelText = (selectedTAP + 1).ToString() + "."; 
            triggerTextBox.text = triggerText;
            actionTextBox.text = actionText;
        }

        public void IsRunning(bool isExecuting)
        {
            if (isExecuting)
            {
                isExecutingIndicator.SetActive(true);
            }
            else
            {
                isExecutingIndicator.SetActive(false); 
            }
        }

        public void SetValidity(bool triggerIsValid, bool actionIsValid)
        {
            if (triggerIsValid)
            {
                triggerBackPlate.material.color = valid;
            }
            else
            {
                triggerBackPlate.material.color = invalid;
            }
            if (actionIsValid)
            {
                actionBackPlate.material.color = valid;
            }
            else
            {
                actionBackPlate.material.color = invalid;
            }
        }

        public void OnDeleteTAPEvent()
        {
            deleteTAPEvent.Raise(selectedTAP);
        }

        public void OnEditTAPEvent()
        {
            editTAPEvent.Raise(selectedTAP);
        }

        public void OnEditTAPPriorityEvent()
        {
            editTAPPriorityEvent.Raise(selectedTAP);
        }

        public void OnCloneTAPEvent()
        {
            duplicateTAPEvent.Raise(selectedTAP);
        }

        private void OnEnable()
        {
            tapButtonConfig.OnClick.AddListener(OnEditTAPEvent);
            deleteButtonConfig.OnClick.AddListener(OnDeleteTAPEvent);
            priorityButtonConfig.OnClick.AddListener(OnEditTAPPriorityEvent);
            cloneTAPButtonConfig.OnClick.AddListener(OnCloneTAPEvent);
        }
        private void OnDisable()
        {
            if (tapButtonConfig != null)
            {
                tapButtonConfig.OnClick.RemoveListener(OnEditTAPEvent);
            }
            if (deleteButtonConfig != null)
            {
                deleteButtonConfig.OnClick.RemoveListener(OnDeleteTAPEvent);
            }
            if (priorityButtonConfig != null)
            {
                priorityButtonConfig.OnClick.RemoveListener(OnEditTAPPriorityEvent);
            }
            if (cloneTAPButtonConfig != null)
            {
                cloneTAPButtonConfig.OnClick.RemoveListener(OnCloneTAPEvent);
            }
        }
    }
}
