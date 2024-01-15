using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;

namespace ProgramAR.Menu
{
    public class ToggleCollection : MonoBehaviour
    {
        public Interactable[] toggleList;
        /// <summary>
        /// Array of Interactables that will be managed by this controller
        /// </summary>
        public Interactable[] ToggleList
        {
            get => toggleList;
            set
            {
                if (value != null && toggleList != value)
                {
                    if (toggleList != null)
                    {
                        // Destroy all listeners on previous toggleList
                        RemoveSelectionListeners();
                    }

                    // Set new list
                    toggleList = value;

                    // Add listeners to new list
                    AddSelectionListeners();
                }
            }
        }

        public bool useDefaultSelection = false;
        [Tooltip("Currently selected index in the ToggleList, default is -1")]
        [SerializeField]
        private int selectedIndex = -1;



        /// <summary>
        /// The current index in the array of interactable toggles
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set => SetSelection(value, true, true);
        }

        private int previousIndex = -1;
        public int PreviousIndex
        {
            get => previousIndex;
        }

        [Tooltip("This event is triggered when any of the toggles in the ToggleList are selected")]
        /// <summary>
        /// This event is triggered when any of the toggles in the ToggleList are selected
        /// </summary>
        public UnityEvent OnSelectionEvents = new UnityEvent();

        private List<UnityAction> toggleActions = new List<UnityAction>();

        private void OnValidate()
        {
            if (ToggleList == null)
            {
                ToggleList = GetComponentsInChildren<Interactable>();
            }
        }

        protected virtual void Start()
        {
            if (ToggleList != null)
            {
                // If the ToggleList is set before start, then it already has listeners
                // If the ToggleList is populated through the inspector, then it needs listeners
                if (toggleActions.Count == 0)
                {
                    // Add listeners to each toggle in ToggleList
                    AddSelectionListeners();

                    if (useDefaultSelection)
                    {
                        SetSelection(SelectedIndex, true, true);
                    }
                }
            }
        }

        /// <summary>
        /// Set the selection of a an element in the toggle collection based on index.
        /// <param name="index">Index of an element in ToggleList</param>
        /// <param name="force">Force selection set</param>
        /// <param name="fireOnClick">The manual trigger of the OnClick event. OnClick event is manually triggered 
        /// when the CurrentIndex is updated via script or inspector</param>
        /// </summary>
        public void SetSelection(int index, bool force = false, bool fireOnClick = false)
        {
            if (index < 0 || ToggleList.Length <= index || ToggleList == null || !isActiveAndEnabled)
            {
                Debug.LogWarning("Index out of range of ToggleList: " + index);
                return;
            }

            if (SelectedIndex != index || force)
            {
                selectedIndex = index;

                OnSelection(index);

                previousIndex = index;

                if (fireOnClick)
                {
                    // Trigger the OnClick event without checking CanInteract as we did not check when setting the index earlier
                    ToggleList[index].TriggerOnClick(true);
                }
            }
        }


        // Update the visual appearance and set the states of the selected and unselected toggles within 
        // Interactable
        protected virtual void OnSelection(int index)
        {
            if (PreviousIndex != -1)
            {
                ToggleList[PreviousIndex].IsToggled = false;
            }

            ToggleList[index].IsToggled = true;

            OnSelectionEvents?.Invoke();
        }

        private void AddSelectionListeners()
        {
            // Add listeners to new list
            for (int i = 0; i < ToggleList.Length; ++i)
            {
                int itemIndex = i;

                UnityAction setSelectionAction = () =>
                {
                    SetSelection(itemIndex, true, false);
                };

                toggleActions.Add(setSelectionAction);

                ToggleList[i].OnClick.AddListener(setSelectionAction);
                ToggleList[i].CanDeselect = false;
            }
        }

        public void Reset()
        {
            if (selectedIndex != -1)
            {
                toggleList[selectedIndex].IsToggled = false;
            }

            selectedIndex = -1;
        }

        private void RemoveSelectionListeners()
        {
            for (int i = 0; i < toggleActions.Count; ++i)
            {
                Interactable toggle = ToggleList[i];
                if (toggle != null)
                {
                    toggle.OnClick.RemoveListener(toggleActions[i]);
                }
            }

            toggleActions.Clear();
        }

        private void OnDestroy()
        {
            RemoveSelectionListeners();
        }
    }
}