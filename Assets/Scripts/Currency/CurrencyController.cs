using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Currency
{
    public class CurrencyController : IDisposable
    {
        private CurrencySettings currencySettings;
        public CurrencySettings CurrencySettings
        {
            get
            {
                if (!currencySettings)
                {
                    currencySettings = ResourceManager.GetResource<CurrencySettings>(GameConstants.PATH_CURRENCY_SETTINGS);
                }
                return currencySettings;
            }
        }

        private List<CurrencyItem> currencies = new List<CurrencyItem>();
        public List<CurrencyItem> Currencies
        {
            get { return currencies; }
        }
        
        /// <summary>
        /// 1 - type of currency that was changed
        /// 2 - prevAmount
        /// 3 - newAmount
        /// </summary>
        public event Action<CurrencyType, int, int> OnCurrencyChanged;

        public CurrencyController()
        {
            for(int i = 0; i < CurrencySettings.InitialCurrencies.Count; i++)
            {
                var currencyItem = CurrencySettings.InitialCurrencies[i];
                currencies.Add(currencyItem);
            }
        }

        public int GetCurrency(CurrencyType currencyType)
        {
            var currencyIndex = currencies.FindIndex(x => x.currencyType == currencyType);
            return currencyIndex == -1 ? -1 : currencies[currencyIndex].currencyAmount;
        }

        public void AddCurrency(CurrencyType currencyType, int currencyAmount)
        {
            var currencyIndex = currencies.FindIndex(x => x.currencyType == currencyType);
            if (currencyIndex == -1)
                return;

            var currency = currencies[currencyIndex];
            var prevCurrencyAmount = currency.currencyAmount;
            currency.currencyAmount = currency.currencyAmount + currencyAmount < 0? 0 : currency.currencyAmount + currencyAmount;
            currencies[currencyIndex] = currency;
            OnCurrencyChanged?.Invoke(currencyType, prevCurrencyAmount, currency.currencyAmount);
            Debug.Log(currency);
        }

        public bool TrySubstractCurrency(CurrencyType currencyType, int amount)
        {
            if(GetCurrency(currencyType) > amount)
            {
                AddCurrency(currencyType, -amount);
                return true;
            }
            return false;
        }
        
        public bool TrySubstructCurrencies(List<CurrencyItem> currencies)
        {
            if (currencies.TrueForAll(x => GetCurrency(x.currencyType) >= x.currencyAmount))
            {
                for(int i = 0; i < currencies.Count; i++)
                {
                    var currency = currencies[i];
                    AddCurrency(currency.currencyType, -currency.currencyAmount);
                }
                return true;
            }
            return false;
        }

        public bool TrySubstructCurrencies(params CurrencyItem[] currencies)
        {
            if (Array.TrueForAll(currencies,x => GetCurrency(x.currencyType) >= x.currencyAmount))
            {
                for (int i = 0; i < currencies.Length; i++)
                {
                    var currency = currencies[i];
                    AddCurrency(currency.currencyType, -currency.currencyAmount);
                }
                return true;
            }
            return false;
        }


        public void Dispose()
        {
            Resources.UnloadAsset(currencySettings);
            OnCurrencyChanged = null;
            currencySettings = null;
        }
    }
}