using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Buildings
{
    public class BuildingObject : MonoBehaviour, IPoolObject, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField]
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public BuildingsGrid Grid
        {
            get { return BuildingsController.Instance.BuildingsGrid; }
        }

        private Building source;
        public Building Source
        {
            get { return source; }
        }

        [SerializeField]
        private Transform infoAttachPosition;
        public Transform InfoAttachPosition
        {
            get { return infoAttachPosition; }
        }

        public GridCell MainGridCell { get; set; }
        public bool Dragable { get; set; }
        public Vector3 PrevPosition { get; set; }
        

        public void Init(Building building, GridCell gridCell)
        {
            source = building;
            MainGridCell = gridCell;
            Dragable = false;

            var center = Grid.GetBuildingPosition(MainGridCell, source.Size.x, source.Size.y);
            transform.position = new Vector3(center.x, transform.position.y, center.z);
        }

        public void Release()
        {
            source = null;
            transform.position = Vector3.zero;
            MainGridCell = GridCell.Default;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BuildingsController.Instance.OnPointerClick(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BuildingsController.Instance.OnBeginDrag(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            BuildingsController.Instance.OnDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            BuildingsController.Instance.OnEndDrag(this, eventData);
        }

        public Vector2 GetScreenPosition()
        {
            var camera = CameraController.Instance.MainCamera;
            var screenPoint = camera.WorldToScreenPoint(infoAttachPosition.position);
            return screenPoint - new Vector3(Screen.width / 2, Screen.height / 2);
        }
    }
}