using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buildings;

namespace UI
{
    public class UIBuildingConstructionInfoWindow : UIBaseWindow
    {
        private const string UI_BUILDING_CONSTRUCTION_INFO_ITEM_ID = "default";
        [SerializeField]
        private float updateInterval = 1.0f;
        private float time;

        [SerializeField]
        private UIBuildingConstructionInfoItemsPool uiItemsPool;
        private List<UIBuildingConstructionInfoItem> uiItems = new List<UIBuildingConstructionInfoItem>();
        
        public override void Init()
        {
            base.Init();
            BuildingsController.Instance.OnBuildingAdded += OnBuildingsAdded;
        }

        private void OnBuildingsAdded(BuildingObject buildingObject)
        {
            if (buildingObject.Source is ProductionBuilding == false)
                return;

            var item = uiItemsPool.GetOrInstantiate(UI_BUILDING_CONSTRUCTION_INFO_ITEM_ID);
            item.Source = buildingObject;
            item.Show();
            uiItems.Add(item);
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time < updateInterval)
                return;

            time = 0.0f;

            var unixTime = DateTime.UtcNow.ToUnixTime();
            for(int i = uiItems.Count - 1; i >= 0; i--)
            {
                var uiItem = uiItems[i];
                if (uiItem.gameObject.activeSelf)
                {
                    uiItem.OnUpdate(unixTime);
                }
                else
                {
                    uiItems.Remove(uiItem);
                }
            }
        }
    }
}