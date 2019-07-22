using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Buildings;
using TMPro;

namespace UI
{
    public class UIRegularModeBuildingInfoItem : MonoBehaviour, IUIItem<BuildingObject>
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

        private ProductionBuilding productionBuilding;

        [SerializeField]
        private TMP_Text idText;
        [SerializeField]
        private Button startProductionButton;
        [SerializeField]
        private UIProgressBar productionProgressBar;

        public void Show()
        {
            idText.text = Source.Source.Id;
            if(Source.Source is ProductionBuilding)
            {
                productionBuilding = Source.Source as ProductionBuilding;
                switch (productionBuilding.ProductionState)
                {
                    case ProductionState.Construct:
                        startProductionButton.gameObject.SetActive(false);
                        productionProgressBar.gameObject.SetActive(false);
                        break;
                    case ProductionState.Idle:
                        startProductionButton.gameObject.SetActive(true);
                        productionProgressBar.gameObject.SetActive(false);
                        break;
                    case ProductionState.Production:
                        if (productionBuilding.ProductionStartTime != 0)
                        {
                            productionProgressBar.gameObject.SetActive(true);
                            productionProgressBar.SetData(DateTime.UtcNow.ToUnixTime() -
                                                          productionBuilding.ProductionStartTime,
                                                          productionBuilding.IncomeInterval);
                        }
                        startProductionButton.gameObject.SetActive(false);
                        break;
                }
                gameObject.SetActive(true);
            }
            RectTransform.anchoredPosition = Source.GetScreenPosition();
        }

        public void OnStartProductionButtonClick()
        {
            if(Source.Source is ProductionBuilding)
            {
                var productionBuilding = Source.Source as ProductionBuilding;
                productionBuilding.StartProduction();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnUpdate(int unixTime)
        {
            if (!productionBuilding)
                return;

            if(productionBuilding.ProductionState == ProductionState.Production)
            {
                if (productionBuilding.ProductionStartTime != 0)
                {
                    if (!productionProgressBar.gameObject.activeSelf)
                    {
                        productionProgressBar.gameObject.SetActive(true);
                    }
                    productionProgressBar.SetData(unixTime -
                                                  productionBuilding.ProductionStartTime,
                                                  productionBuilding.IncomeInterval);
                }
            }
            else if(productionBuilding.ProductionState != ProductionState.Construct)
            {
                if (!startProductionButton.gameObject.activeSelf)
                {
                    startProductionButton.gameObject.SetActive(true);
                }
                productionProgressBar.gameObject.SetActive(false);
            }
            RectTransform.anchoredPosition = Source.GetScreenPosition();
        }
    }
}