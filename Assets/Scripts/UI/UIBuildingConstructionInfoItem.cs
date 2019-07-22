using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Buildings;
using TMPro;
namespace UI
{
    public class UIBuildingConstructionInfoItem : MonoBehaviour, IUIItem<BuildingObject>, IPoolObject
    {
        public BuildingObject Source { get; set; }

        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (!rectTransform)
                {
                    rectTransform = (RectTransform)transform;
                }
                return rectTransform;
            }
        }

        [SerializeField]
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [SerializeField]
        private TMP_Text idText;
        [SerializeField]
        private UIProgressBar productionProgressBar;
        private ProductionBuilding productionBuilding;

        public void Show()
        {
            idText.text = Source.Source.Id;
            if(Source.Source is ProductionBuilding)
            {
                productionBuilding = Source.Source as ProductionBuilding;
                productionProgressBar.SetData(DateTime.UtcNow.ToUnixTime() -
                                              productionBuilding.ConstructionStartTime,
                                              productionBuilding.ConstructionDuration);
            }
            RectTransform.anchoredPosition = Source.GetScreenPosition();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnUpdate(int unixTime)
        {
            RectTransform.anchoredPosition = Source.GetScreenPosition();
            productionProgressBar.SetData(unixTime -
                                          productionBuilding.ConstructionStartTime,
                                          productionBuilding.ConstructionDuration);

            if (unixTime > productionBuilding.ConstructionStartTime + productionBuilding.ConstructionDuration)
            {
                Hide();
                Release();
            }
        }

        public void Release()
        {
            idText.text = string.Empty;
            productionBuilding = null;
        }
    }
}