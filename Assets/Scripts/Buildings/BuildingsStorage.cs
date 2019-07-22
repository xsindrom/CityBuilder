using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Buildings/Storage", order = 0)]
    public class BuildingsStorage : ScriptableObject
    {
        [Serializable]
        public class BuildingData
        {
            [SerializeField]
            private string id;
            [SerializeField]
            private List<Building> buildings = new List<Building>();

            public string Id
            {
                get { return id; }
            }

            public List<Building> Buildings
            {
                get { return buildings; }
            }
        }
        [SerializeField]
        private List<BuildingData> buildingsData = new List<BuildingData>();
        public List<BuildingData> BuildingsData
        {
            get { return buildingsData; }
        }
    }
}