using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Modes/RegularMode")]
    public class RegularMode : BuildingsMode
    {
        public override void OnBeginDrag(BuildingObject target, PointerEventData eventData)
        {
        }

        public override void OnDrag(BuildingObject target, PointerEventData eventData)
        {
        }

        public override void OnEndDrag(BuildingObject target, PointerEventData eventData)
        {
        }

        public override void OnPointerClick(BuildingObject target, PointerEventData eventData)
        {

        }
    }
}