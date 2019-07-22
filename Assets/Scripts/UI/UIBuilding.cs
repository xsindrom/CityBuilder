using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Buildings;
namespace UI
{
    public class UIBuilding : MonoBehaviour, IUIItem<Building>
    {
        public Building Source { get; set; }
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private UICurrencyItem priceTemplate;
        [SerializeField]
        private RectTransform pricesRoot;
        private List<UICurrencyItem> prices = new List<UICurrencyItem>();

        public void Init(Building building)
        {
            Source = building;
            var icons = ResourceManager.GetResource<SpriteResources>(GameConstants.PATH_BUILDINGS_ICONS);
            icon.sprite = icons.Resources.Find(x => x.name == Source.Id);
            nameText.text = Source.Id;
            for(int i = 0; i < building.Price.Count; i++)
            {
                var price = building.Price[i];
                var clone = Instantiate(priceTemplate, pricesRoot);
                clone.Source = price;
                clone.gameObject.SetActive(true);
                prices.Add(clone);
            }
        }

        public void OnClick()
        {
            if (GameController.Instance.CurrencyController.TrySubstructCurrencies(Source.Price))
            {
                BuildingsController.Instance.PlaceBuilding(Source);
            }

        }
    }
}