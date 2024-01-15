using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class ActionClauseButtonGroup : MonoBehaviour, IDeselectClauseSimButtonEvent
    {
        [SerializeField] Interactable simulateInteractable;
        [SerializeField] TextMeshPro simulateActionText;
        [SerializeField] DeselectClauseSimButtonEvent deselectClauseSimButtonEvent;
        [SerializeField] ClauseButtonGroup clauseButtongroup;
        [SerializeField] SimulateClauseEvent onSimulateClauseEvent;

        string defaultText = "Simulate Action";

        void Start()
        {
            deselectClauseSimButtonEvent.RegisterListener(this);
            simulateActionText.text = defaultText;
        }


        public void OnDeselectClauseSimButtonEvent(int selectedClause, string description)
        {
            if (simulateInteractable.IsToggled && selectedClause == clauseButtongroup.selectedClause)
            {
                simulateActionText.text = description;
                simulateInteractable.IsToggled = false;
            }
        }
        public void OnSimulateRuleEvent()
        {
            simulateActionText.text = defaultText;
            onSimulateClauseEvent.Raise(clauseButtongroup.selectedClause, simulateInteractable.IsToggled);
        }
        private void OnEnable()
        {
            simulateInteractable.OnClick.AddListener(OnSimulateRuleEvent);
            simulateActionText.text = defaultText;
        }

        private void OnDisable()
        {
            if (simulateInteractable != null)
            {
                simulateInteractable.OnClick.RemoveListener(OnSimulateRuleEvent);
            }
        }
    }
}
