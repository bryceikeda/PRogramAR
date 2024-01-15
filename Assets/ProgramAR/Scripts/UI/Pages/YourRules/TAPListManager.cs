using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;
using ProgramAR.NumberPad; 
using ProgramAR.Menu;
using Microsoft.MixedReality.Toolkit.UI;
using ProgramAR.Variables;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class TAPListManager : MonoBehaviour, IDeleteTAPResponse, IIndicateIsRunningResponse, IHideTAPListResponse, IEditTAPPriorityResponse, IEditTAPResponse, ISaveTAPResponse, IUpdatePriorityResponse, IDuplicateTAPResponse, IHighlightRuleResponse
    {
        [SerializeField] DeleteTAPEvent deleteTAPEvent;
        [SerializeField] HideTAPListEvent hideTAPListEvent;
        [SerializeField] EditTAPEvent editTAPEvent;
        [SerializeField] EditTAPPriorityEvent editTAPPriorityEvent;
        [SerializeField] SaveTAPEvent saveTAPEvent;
        [SerializeField] UpdatePriorityEvent updatePriorityEvent;
        [SerializeField] DuplicateTAPEvent duplicateTAPEvent;
        [SerializeField] SelectRuleTypeEvent selectRuleType;
        [SerializeField] HighlightRuleEvent highlightRuleEvent;
        [SerializeField] IndicateIsRunningEvent indicateIsRunningEvent; 
        [SerializeField] TriggerActionRules triggerActionRules;
        [SerializeField] ClauseRuntimeSet triggerRuntimeSet;
        [SerializeField] ClauseRuntimeSet actionRuntimeSet;

        [SerializeField] GameObject numberPad; 
        [SerializeField] GameObject createTAPButton;
        [SerializeField] TAPButtonGroupFactory TAPButtonGroupFactory;
        [SerializeField] Interactable createRulesTabButton;
        [SerializeField] GameObject triggerList;
        [SerializeField] GameObject actionList;
        [SerializeField] GameObject ruleTypeBack;
        [SerializeField] GameObject ruleType;

        [SerializeField] BoolVariable triggerDefaultClauseAdded;
        [SerializeField] BoolVariable actionDefaultClauseAdded;
        [SerializeField] IsPointInsideBox isPointInsideBox;
        [SerializeField] Transform deleteParent; 
        private readonly int maxTapCount = 10;

        int scrollPosition = 1; 

        private int initialPriority; 
        public List<TAPButtonGroup> tapButtons = new List<TAPButtonGroup>();
        private GridObjectCollection gridObjectCollection;

        private int tapRunning = -1; 

        int loadedTAP = -1; 

        private void OnValidate()
        {
            if (gridObjectCollection == null)
            {
                gridObjectCollection = GetComponent<GridObjectCollection>();
            }
        }

        public void OnHideTAPListEvent()
        {
            foreach (TAPButtonGroup button in tapButtons)
            {
                button.gameObject.SetActive(false);
            }
            createTAPButton.SetActive(false);
        }

        public void OnAddTAP()
        {
            loadedTAP = -1; 
            triggerRuntimeSet.Items.Clear(); 
            actionRuntimeSet.Items.Clear();
            actionDefaultClauseAdded.Value = false;
            triggerDefaultClauseAdded.Value = false;

            ruleType.SetActive(true);
            triggerList.SetActive(false);
            actionList.SetActive(false); 

            createRulesTabButton.TriggerOnClick(true);
        }

        public void OnSaveTAPEvent()
        {
            if (loadedTAP != -1)
            {
                triggerActionRules.EditPair(loadedTAP, triggerRuntimeSet, actionRuntimeSet);
            }
            else
            {
                triggerActionRules.AddPair(triggerRuntimeSet, actionRuntimeSet);
            }
            OnShowTAPListEvent(); 
        }


        public void OnDuplicateTAPEvent(int selectedTAP)
        {
            if (tapButtons.Count < maxTapCount)
            {
                triggerActionRules.Copy(selectedTAP);
                OnShowTAPListEvent(); 
            }
        }
        public void OnShowTAPListEvent()
        {
            // Create a button if we have extra rule
            while (tapButtons.Count < triggerActionRules.pairs.Count)
            {
                TAPButtonGroup button = TAPButtonGroupFactory.GetInstance();
                button.gameObject.transform.parent = transform;
                button.gameObject.transform.localRotation = Quaternion.identity;
                tapButtons.Add(button);
            }

            //  Give buttons their descriptions
            for (int tapNum = 0; tapNum < triggerActionRules.pairs.Count; tapNum++)
            {
                tapButtons[tapNum].gameObject.SetActive(true);
                tapButtons[tapNum].SetProperties(triggerActionRules.GetTriggerText(tapNum), triggerActionRules.GetActionText(tapNum), tapNum);
            }

            createTAPButton.SetActive(tapButtons.Count != maxTapCount);
            createTAPButton.transform.SetAsLastSibling();

            var tabCount = tapButtons.Count - triggerActionRules.pairs.Count;

            if(tabCount > 0 )
            {
                for (int i = tabCount - 1; i >= 0; i--)
                {
                    GameObject obj = tapButtons[i].gameObject;
                    obj.transform.SetAsLastSibling();
                    tapButtons.Remove(tapButtons[i]);
                    Destroy(obj);
                }
                createTAPButton.transform.SetAsFirstSibling();
            }

            gridObjectCollection.UpdateCollection();
            ShowButtonWindow();
        }

        public void ScrollUp()
        {
            int maxScroll = (tapButtons.Count + 1) / 5;

            if (((tapButtons.Count + 1) % 5) > 0)
            {
                maxScroll += 1; 
            }

            if (scrollPosition < maxScroll)
            {
                var pos = transform.localPosition;
                transform.localPosition = new Vector3(pos.x, pos.y + .40f, pos.z);
                ShowButtonWindow();
                scrollPosition += 1;
            }
        }

        public void ScrollDown()
        {
            if (scrollPosition > 1)
            {
                var pos = transform.localPosition;
                transform.localPosition = new Vector3(pos.x, pos.y -.40f, pos.z);
                scrollPosition -= 1;
                ShowButtonWindow();
            }
        }

        public void ShowButtonWindow()
        {   
            foreach (TAPButtonGroup group in tapButtons)
            {
                if (isPointInsideBox.IsInside(group.transform.position))
                {
                    group.gameObject.SetActive(true);
                }
                else
                {
                    group.gameObject.SetActive(false);
                }
               
            }
            if (isPointInsideBox.IsInside(createTAPButton.transform.position))
            {
                createTAPButton.SetActive(true);
            }
            else
            {
                createTAPButton.SetActive(false);
            }
        }

        public void OnDeleteTAPEvent(int selectedTAP)
        {
            triggerActionRules.DeletePair(selectedTAP);

            var buttonGroup = transform.GetChild(selectedTAP);

            tapButtons.RemoveAt(selectedTAP);

            buttonGroup.transform.parent = deleteParent; 
            Destroy(buttonGroup.gameObject);

            createTAPButton.SetActive(true);
            gridObjectCollection.UpdateCollection();

            //  Give buttons their descriptions
            for (int tapNum = 0; tapNum < triggerActionRules.pairs.Count; tapNum++)
            {
                tapButtons[tapNum].gameObject.SetActive(true);
                tapButtons[tapNum].SetProperties(triggerActionRules.GetTriggerText(tapNum), triggerActionRules.GetActionText(tapNum), tapNum);
            }
            ShowButtonWindow();
        }

        public void OnIndicateIsRunningEvent(int index, bool isRunning)
        {
            if (isRunning)
            {
                if (tapRunning != -1 && tapRunning != index)
                {
                    tapButtons[tapRunning].IsRunning(false);
                }
                tapRunning = index;
                tapButtons[index].IsRunning(true);
            }
            else
            {
                tapButtons[index].IsRunning(false); 
                if (tapRunning != -1)
                {
                    tapButtons[tapRunning].IsRunning(false);
                    tapRunning = -1;
                }
            }
        }

        public void OnHighlightRuleEvent(int index, bool triggerIsValid, bool actionIsValid)
        {
            if (tapButtons.Count > index)
            {
                tapButtons[index].SetValidity(triggerIsValid, actionIsValid);
            }
        }

        public void OnEditTAPPriorityEvent(int initialPriority)
        {
            if (tapButtons.Count != 1)
            {
                this.initialPriority = initialPriority;
                numberPad.SetActive(true);
                OnHideTAPListEvent();
            }
        }
        public void OnUpdatePriorityEvent(int newPriority)
        {
            if (newPriority != -1)
            {
                triggerActionRules.ChangePriority(initialPriority, newPriority);
                if (tapRunning != -1)
                {
                    tapButtons[tapRunning].IsRunning(false);
                    tapRunning = -1; 
                }
            }
            numberPad.SetActive(false);
            OnShowTAPListEvent(); 
        }

        public void OnEditTAPEvent(int selectedTAP)
        {
            loadedTAP = selectedTAP;

            triggerRuntimeSet.Items.Clear();
            actionRuntimeSet.Items.Clear();

            if (triggerActionRules.pairs[selectedTAP].trigger[0].expressions[0].expressionName.Equals("If"))
            {
                selectRuleType.Raise("IfThen");
            }
            else if (triggerActionRules.pairs[selectedTAP].trigger[0].expressions[0].expressionName.Equals("While"))
            {
                selectRuleType.Raise("WhileDo");
            }

            foreach (Clause clause in triggerActionRules.pairs[selectedTAP].trigger)
            {
                triggerRuntimeSet.Add(clause);
            }

            foreach (Clause clause in triggerActionRules.pairs[selectedTAP].action)
            {
                actionRuntimeSet.Add(clause);
            }

            actionDefaultClauseAdded.Value = true;
            triggerDefaultClauseAdded.Value = true;


            triggerList.SetActive(true);
            actionList.SetActive(true);
            ruleTypeBack.SetActive(true);

            createRulesTabButton.TriggerOnClick(true);
        }

        private void OnEnable()
        {
            OnShowTAPListEvent();
            saveTAPEvent.RegisterListener(this);
            deleteTAPEvent.RegisterListener(this);
            hideTAPListEvent.RegisterListener(this);
            editTAPEvent.RegisterListener(this);
            editTAPPriorityEvent.RegisterListener(this);
            updatePriorityEvent.RegisterListener(this);
            duplicateTAPEvent.RegisterListener(this);
            createTAPButton.GetComponent<Interactable>().OnClick.AddListener(OnAddTAP);
            highlightRuleEvent.RegisterListener(this);
            indicateIsRunningEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            saveTAPEvent.UnregisterListener(this);
            deleteTAPEvent.UnregisterListener(this);
            hideTAPListEvent.UnregisterListener(this);
            editTAPEvent.UnregisterListener(this);
            editTAPPriorityEvent.UnregisterListener(this);
            updatePriorityEvent.UnregisterListener(this);
            duplicateTAPEvent.UnregisterListener(this);
            highlightRuleEvent.UnregisterListener(this);
            indicateIsRunningEvent.UnregisterListener(this);
            createTAPButton.GetComponent<Interactable>().OnClick.RemoveListener(OnAddTAP);
        }


    }
}
