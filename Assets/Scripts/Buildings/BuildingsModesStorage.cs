using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Modes/BuildingsModesStorage")]
    public class BuildingsModesStorage : ScriptableObject
    {
        [SerializeField]
        private List<BuildingsMode> modes = new List<BuildingsMode>();
        public List<BuildingsMode> Modes
        {
            get { return modes; }
        }
    }
}