using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Modes/BuildMode")]
    public class BuildMode : BuildingsMode
    {
        public override void OnPointerClick(BuildingObject target, PointerEventData eventData)
        {
            target.Dragable = true;
        }

        public override void OnBeginDrag(BuildingObject target, PointerEventData eventData)
        {
            if (!target.Dragable)
                return;
            target.PrevPosition = target.transform.position;
        }

        public override void OnDrag(BuildingObject target, PointerEventData eventData)
        {
            if (!target.Dragable)
                return;

            Ray ray = CameraController.Instance.MainCamera.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target.transform.position = new Vector3(hit.point.x + Grid.CellSize.x * target.Source.Size.x / 2,
                                                        target.transform.position.y,
                                                        hit.point.z - Grid.CellSize.y * target.Source.Size.y / 2);
            }

        }

        public override void OnEndDrag(BuildingObject target, PointerEventData eventData)
        {
            if (!target.Dragable)
                return;

            Ray ray = CameraController.Instance.MainCamera.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var gridCell = Grid.PointToCell(hit.point);
                if (target.MainGridCell.state != GridCellState.None)
                {
                    Grid.RemoveBuilding(target.MainGridCell.x, target.MainGridCell.y, target.Source.Size.x, target.Source.Size.y);
                }

                if (Grid.CanPlaceBuilding(gridCell.x, gridCell.y, target.Source.Size.x, target.Source.Size.y))
                {
                    Grid.PlaceBuilding(gridCell.x, gridCell.y, target.Source.Size.x, target.Source.Size.y);
                    target.MainGridCell = Grid.GetGridCell(gridCell.x, gridCell.y);

                    var center = Grid.GetBuildingPosition(target.MainGridCell, target.Source.Size.x, target.Source.Size.y);
                    target.transform.position = new Vector3(center.x, target.transform.position.y, center.z);
                }
                else
                {
                    target.transform.position = target.PrevPosition;
                    if (target.MainGridCell.state != GridCellState.None)
                    {
                        Grid.PlaceBuilding(target.MainGridCell.x, target.MainGridCell.y, target.Source.Size.x, target.Source.Size.y);
                    }
                }
            }
            target.Dragable = false;
        }
    }
}