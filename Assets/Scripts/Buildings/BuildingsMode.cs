using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Buildings
{
    public abstract class BuildingsMode : ScriptableObject
    {
        [SerializeField]
        protected string id;
        public string Id
        {
            get { return id; }
        }

        public BuildingsGrid Grid { get; set; }

        public virtual void Init(BuildingsGrid grid)
        {
            Grid = grid;
        }

        public abstract void OnPointerClick(BuildingObject target, PointerEventData eventData);

        public abstract void OnBeginDrag(BuildingObject target, PointerEventData eventData);

        public abstract void OnDrag(BuildingObject target, PointerEventData eventData);

        public abstract void OnEndDrag(BuildingObject target, PointerEventData eventData);
    }
}