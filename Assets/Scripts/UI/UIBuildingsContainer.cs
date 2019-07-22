using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buildings;

namespace UI
{
    public class UIBuildingsContainer : MonoBehaviour, IUIItem<BuildingsStorage.BuildingData>
    {
        public BuildingsStorage.BuildingData Source { get; set; }
        [SerializeField]
        private RectTransform uiBuildingsRoot;
        [SerializeField]
        private UIBuilding template;
        [SerializeField]
        private List<UIBuilding> uiBuildings = new List<UIBuilding>();
        public List<UIBuilding> UIBuildings
        {
            get { return uiBuildings; }
        }

        public void Init(BuildingsStorage.BuildingData buildingData)
        {
            Source = buildingData;
            for(int i = 0; i < Source.Buildings.Count; i++)
            {
                var building = Source.Buildings[i];
                var clone = Instantiate(template, uiBuildingsRoot);
                clone.Init(building);
                clone.gameObject.SetActive(true);
                uiBuildings.Add(clone);
            }
        }

        public void UpdateContainer()
        {

        }
    }
}