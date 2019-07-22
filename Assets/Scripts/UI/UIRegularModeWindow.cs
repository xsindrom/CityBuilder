using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buildings;
namespace UI
{
    public class UIRegularModeWindow : UIBaseWindow
    {
        private const string UI_BUILDING_CONSTRUCTION_INFO_ITEM_ID = "default";
        [SerializeField]
        private float updateInterval = 1.0f;
        private float time;

        [SerializeField]
        private string modeId;
        [SerializeField]
        private UIRegularModeBuildingInfoItem uiInfoItem;

        public override void Init()
        {
            base.Init();
            BuildingsController.Instance.OnCurrentModeChanged += OnCurrentModeChanged;
            BuildingsController.Instance.OnBuildingClick += OnBuildingClick;
        }

        private void OnCurrentModeChanged(string prevMode, string newMode)
        {
            if(newMode == modeId)
            {
                OpenWindow();
            }
            else
            {
                CloseWindow();
            }
            
        }

        public void OnBuildingClick(BuildingObject buildingObject)
        {
            uiInfoItem.Source = buildingObject;
            uiInfoItem.Show();
        }

        public override void OpenWindow()
        {
            uiInfoItem.Hide();
            base.OpenWindow();
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time < updateInterval)
                return;

            time = 0.0f;

            var unixTime = DateTime.UtcNow.ToUnixTime();
            uiInfoItem.OnUpdate(unixTime);
        }
    }
}