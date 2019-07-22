using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Currency;
namespace Buildings
{
    public enum ProductionBuildingType
    {
        Auto,
        Manual
    }

    public enum ProductionState
    {
        None,
        Construct,
        Idle,
        Production
    }

    [CreateAssetMenu(menuName ="ScriptableObjects/Buildings/ProductionBuilding")]
    public class ProductionBuilding : Building
    {
        [SerializeField]
        protected int constructionDuration;
        [SerializeField]
        protected ProductionBuildingType type;
        [SerializeField]
        protected ProductionState initialProductionState;
        [SerializeField]
        protected int incomeInterval;
        [SerializeField]
        protected List<CurrencyItem> income = new List<CurrencyItem>();

        public event Action<ProductionState> OnStateChanged;

        [NonSerialized]
        protected int constructionStartTime;
        [NonSerialized]
        protected int productionStartTime;
        [NonSerialized]
        protected ProductionState productionState;

        public int ConstructionDuration
        {
            get { return constructionDuration; }
        }

        public ProductionBuildingType Type
        {
            get { return type; }
        }

        public int IncomeInterval
        {
            get { return incomeInterval; }
        }

        public List<CurrencyItem> Income
        {
            get { return income; }
        }

        public int ConstructionStartTime
        {
            get { return constructionStartTime; }
            set { constructionStartTime = value; }
        }

        public int ProductionStartTime
        {
            get { return productionStartTime; }
            set { productionStartTime = value; }
        }

        public ProductionState ProductionState
        {
            get { return productionState; }
            set
            {
                if (productionState != value)
                {
                    var prev = productionState;
                    productionState = value;
                    OnStateChanged?.Invoke(productionState);
                    Debug.Log($"{id}:{prev} -> {productionState}");
                }
            }
        }

        private void OnEnable()
        {
            productionState = initialProductionState;
        }

        public virtual void Collect()
        {
            for(int i = 0; i < income.Count; i++)
            {
                var incomeItem = income[i];
                GameController.Instance.CurrencyController.AddCurrency(incomeItem.currencyType, incomeItem.currencyAmount);
            }
        }

        public override void Build()
        {
            constructionStartTime = DateTime.UtcNow.ToUnixTime();
            ProductionState = ProductionState.Construct;
        }

        public virtual void StartProduction()
        {
            productionStartTime = DateTime.UtcNow.ToUnixTime();
            ProductionState = ProductionState.Production;
        }

        public override void OnUpdate(int unixTime)
        {
            if (unixTime < constructionStartTime + constructionDuration)
            {
                ProductionState = ProductionState.Construct;
                return;
            }

            if (unixTime < productionStartTime + incomeInterval)
            {
                ProductionState = ProductionState.Production;
            }
            else
            {
                if (type != ProductionBuildingType.Auto)
                {
                    if (ProductionState != ProductionState.Idle)
                    {
                        if (productionStartTime != 0)
                        {
                            Collect();
                        }
                        ProductionState = ProductionState.Idle;
                    }
                }
                else
                {
                    if (productionStartTime != 0)
                    {
                        Collect();
                    }
                    productionStartTime = unixTime;
                }
            }
        }
    }
}