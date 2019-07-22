using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [Serializable]
    public struct LinesModeData
    {
        public string modeId;
        public float lineSize;
    }

    public class BuildingsGridRenderer : MonoBehaviour
    {
        [SerializeField]
        private BuildingsGrid grid;
        [SerializeField]
        private Renderer gridRenderer;
        [SerializeField]
        private List<LinesModeData> linesData = new List<LinesModeData>();
        private List<Vector4> busyCells = new List<Vector4>();

        private void Awake()
        {
            grid.OnGridUpdated += OnGridUpdated;
            grid.OnGridCellSelected += OnGridCellSelected;

            gridRenderer.material.SetFloat("_BusyCellsLength", 0);
            gridRenderer.material.SetVectorArray("_BusyCells", new Vector4[2048]);
            gridRenderer.material.SetFloat("_LineSize", 0);
            BuildingsController.Instance.OnCurrentModeChanged += OnCurrentModeChanged;
        }

        private void OnCurrentModeChanged(string prevMode, string newMode)
        {
            var modeIndex = linesData.FindIndex(x => x.modeId == newMode);
            if(modeIndex != -1)
            {
                gridRenderer.material.SetFloat("_LineSize",linesData[modeIndex].lineSize);
            }
            else
            {
                gridRenderer.material.SetFloat("_LineSize", 0);
            }
        }

        public void OnGridUpdated(GridCell[,] gridCells)
        {
            busyCells.Clear();
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    var gridCell = gridCells[y, x];
                    if (gridCell.state != GridCellState.Busy)
                        continue;
                    busyCells.Add(new Vector4(x, y));
                }
            }

            gridRenderer.material.SetFloat("_BusyCellsLength",busyCells.Count);
            gridRenderer.material.SetVectorArray("_BusyCells", busyCells);
        }

        public void OnGridCellSelected(GridCell gridCell)
        {
            gridRenderer.material.SetFloat("_SelectedCellX", gridCell.x);
            gridRenderer.material.SetFloat("_SelectedCellY", gridCell.y);
        }

    }
}