using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Currency
{
    public enum CurrencyType
    {
        Gold,
        Steel,
        Wood,
        Crystals
    }
    [Serializable]
    public struct CurrencyItem
    {
        public CurrencyType currencyType;
        public int currencyAmount;

        public override string ToString()
        {
            return $"{currencyType}:{currencyAmount}";
        }
    }

    [CreateAssetMenu(menuName ="ScriptableObjects/Currencies/Settings", order = 0)]
    public class CurrencySettings : ScriptableObject
    {
        [SerializeField]
        private List<CurrencyItem> initialCurrencies = new List<CurrencyItem>();
        public List<CurrencyItem> InitialCurrencies
        {
            get { return initialCurrencies; }
        }
    }
}