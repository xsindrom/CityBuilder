using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Currency;
using Buildings;
namespace UI
{
    public class UIMainWindow : UIBaseWindow
    {
        [SerializeField]
        private UICurrencyItem currencyTemplate;
        [SerializeField]
        private RectTransform currenciesRoot;
        [SerializeField]
        private List<UICurrencyItem> currencies = new List<UICurrencyItem>();

        public override void Init()
        {
            base.Init();
            var currencyController = GameController.Instance.CurrencyController;
            for(int i = 0; i < currencyController.Currencies.Count; i++)
            {
                var currency = currencyController.Currencies[i];
                var currencyIndex = currencies.FindIndex(x => x.Source.currencyType == currency.currencyType);
                if (currencyIndex == -1)
                {
                    var clone = Instantiate(currencyTemplate, currenciesRoot);
                    clone.Source = currency;
                    currencies.Add(clone);
                }
                else
                {
                    currencies[currencyIndex].Source = currency;
                }
            }
            currencyController.OnCurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(CurrencyType currencyType, int prevAmount, int newAmount)
        {
            var currency = currencies.Find(x => x.Source.currencyType == currencyType);
            if (currency)
            {
                currency.Source = new CurrencyItem() { currencyType = currencyType, currencyAmount = newAmount };
            }
        }

        public void OnModeButtonClick(string modeId)
        {
            BuildingsController.Instance.ActivateMode(modeId);
        }
    }
}