using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Currency;
using TMPro;
namespace UI
{
    public class UICurrencyItem : MonoBehaviour, IUIItem<CurrencyItem>
    {
        [SerializeField]
        private CurrencyItem source;
        public CurrencyItem Source
        {
            get { return source; }
            set
            {
                source = value;
                UpdateItem();
            }
        }

        [SerializeField]
        private Image currencyIcon;
        [SerializeField]
        private string currencyAmountFormat;
        [SerializeField]
        private TMP_Text currencyAmountText;

        public void UpdateItem()
        {
            currencyAmountText.text = string.Format(currencyAmountFormat, source.currencyAmount);
            if (!currencyIcon.sprite)
            {
                var currencyIcons = ResourceManager.GetResource<SpriteResources>(GameConstants.PATH_CURRENCY_ICONS);
                currencyIcon.sprite = currencyIcons.Resources.Find(x => x.name == source.currencyType.ToString());
            }
        }
    }
}