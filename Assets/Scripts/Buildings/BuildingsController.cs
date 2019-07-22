using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Buildings
{
    public class BuildingsController : MonoSingleton<BuildingsController>
    {
        private BuildingsStorage storage;
        public BuildingsStorage Storage
        {
            get
            {
                if (!storage)
                {
                    storage = ResourceManager.GetResource<BuildingsStorage>(GameConstants.PATH_BUILDINGS_STORAGE);
                }
                return storage;
            }
        }

        private BuildingsModesStorage modesStorage;
        public BuildingsModesStorage ModesStorage
        {
            get
            {
                if (!modesStorage)
                {
                    modesStorage = ResourceManager.GetResource<BuildingsModesStorage>(GameConstants.PATH_BUILDINGS_MODES_STORAGE);
                }
                return modesStorage;
            }
        }

        [SerializeField]
        private BuildingsGrid buildingsGrid;
        public BuildingsGrid BuildingsGrid
        {
            get { return buildingsGrid; }
        }
        [SerializeField]
        private BuildingsPool buildingsPool;
        public BuildingsPool BuildingsPool
        {
            get { return buildingsPool; }
        }

        private List<BuildingObject> buildings = new List<BuildingObject>();
        public List<BuildingObject> Buildings
        {
            get { return buildings; }
        }

        private BuildingsMode currentMode;
        public BuildingsMode CurrentMode
        {
            get { return currentMode; }
            set
            {
                var prevMode = currentMode ? currentMode.Id : string.Empty;
                currentMode = value;
                OnCurrentModeChanged?.Invoke(prevMode, currentMode.Id);
            }
        }
        public event Action<string, string> OnCurrentModeChanged;

        public event Action<BuildingObject> OnBuildingClick;
        public event Action<BuildingObject> OnBuildingAdded;
        public event Action<Building> OnBuildingRemoved;

        [SerializeField]
        private float updateInterval = 1.0f;
        private float time;

        public override void Init()
        {
            buildingsGrid.Init();
            for (int i = 0; i < ModesStorage.Modes.Count; i++)
            {
                var mode = ModesStorage.Modes[i];
                mode.Init(buildingsGrid);

                if (mode.Id == GameConstants.DEFAULT_BUILDINGS_MODE_ID)
                    CurrentMode = mode;
            }
        }

        public void ActivateMode(string id)
        {
            var mode = ModesStorage.Modes.Find(x => x.Id == id);
            if (mode)
                CurrentMode = mode;
        }

        public void PlaceBuilding(Building building)
        {
            buildingsGrid.PlaceBuilding(building.Size.x, building.Size.y, out GridCell cell);
            if (!cell.Equals(GridCell.Default))
            {
                var buildingObject = buildingsPool.GetOrInstantiate(building.Id);
                buildingObject.Release();
                buildingObject.Init(building.Clone(), cell);
                buildingObject.Source.Build();
                buildings.Add(buildingObject);
                OnBuildingAdded?.Invoke(buildingObject);
            }
        }

        public void RemoveBuilding(BuildingObject buildingObject)
        {
            buildingsGrid.RemoveBuilding(buildingObject.MainGridCell.x, buildingObject.MainGridCell.y, buildingObject.Source.Size.x, buildingObject.Source.Size.y);
            buildings.Remove(buildingObject);
            OnBuildingRemoved?.Invoke(buildingObject.Source);
            buildingObject.Release();
        }

        public void OnPointerClick(BuildingObject target, PointerEventData eventData)
        {
            CurrentMode.OnPointerClick(target, eventData);
            OnBuildingClick?.Invoke(target);
        }

        public void OnBeginDrag(BuildingObject target, PointerEventData eventData)
        {
            CurrentMode.OnBeginDrag(target, eventData);
        }

        public void OnDrag(BuildingObject target, PointerEventData eventData)
        {
            CurrentMode.OnDrag(target, eventData);
        }

        public void OnEndDrag(BuildingObject target, PointerEventData eventData)
        {
            CurrentMode.OnEndDrag(target, eventData);
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time < updateInterval)
                return;

            time = 0.0f;

            var unixTime = DateTime.UtcNow.ToUnixTime();
            for(int i = buildings.Count -1 ; i >=0; i--)
            {
                var building = buildings[i];
                building.Source.OnUpdate(unixTime);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Resources.UnloadAsset(storage);
            storage = null;
        }
    }
}