using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Buildings;

namespace UI
{
    public class UIBuildingsWindow : UIBaseWindow
    {
        [SerializeField]
        private string modeId;
        [SerializeField]
        private RectTransform containersButtonsRoot;
        [SerializeField]
        private AdvancedToggle containerButtonTemplate;
        private List<AdvancedToggle> containersButtons = new List<AdvancedToggle>();
        [SerializeField]
        private RectTransform containersRoot;
        [SerializeField]
        private UIBuildingsContainer containerTemplate;
        private List<UIBuildingsContainer> containers = new List<UIBuildingsContainer>();

        public override void Init()
        {
            base.Init();
            var storage = BuildingsController.Instance.Storage;
            for (int i = 0; i < storage.BuildingsData.Count; i++)
            {
                var buildingData = storage.BuildingsData[i];
                var clone = Instantiate(containerTemplate, containersRoot);
                clone.Init(buildingData);
                containers.Add(clone);

                var index = i;
                var cloneButton = Instantiate(containerButtonTemplate, containersButtonsRoot);
                cloneButton.Toggle.onValueChanged.AddListener((value) => OnContainerButtonClick(index, value));
                cloneButton.Text.text = buildingData.Id;

                cloneButton.gameObject.SetActive(true);
                if (index == 0)
                    cloneButton.Toggle.isOn = true;
                
                containersButtons.Add(cloneButton);
            }
            BuildingsController.Instance.OnCurrentModeChanged += OnBuildingsModeChanged;
        }

        private void OnBuildingsModeChanged(string prevMode, string newMode)
        {
            if(modeId == prevMode)
            {
                CloseWindow();
            }
            if(modeId == newMode)
            {
                OpenWindow();
            }
        }

        private void OnContainerButtonClick(int index, bool value)
        {
            containers[index].gameObject.SetActive(value);
            containers[index].UpdateContainer();
        }
    }
}