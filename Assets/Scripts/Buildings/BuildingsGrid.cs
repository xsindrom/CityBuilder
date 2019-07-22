using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public enum GridCellState
    {
        None = 0,
        Free = 1,
        Busy = 2
    }

    [Serializable]
    public struct GridCell
    {
        public int x;
        public int y;
        public Vector3 position;
        public Vector3 min;
        public Vector3 max;
        public GridCellState state;

        public Vector3 center { get {return (min + max) / 2; } }
        public static GridCell Default = new GridCell()
        {
            x = -1,
            y = -1,
            position = Vector3.zero,
            min = Vector3.zero,
            max = Vector3.zero,
            state = GridCellState.None
        };

        public override string ToString()
        {
            return $"[{y},{x}] position:{position} state:{state}";
        }
    }

    public class BuildingsGrid : MonoBehaviour
    {
        [SerializeField]
        private Vector3 gridMin;
        [SerializeField]
        private Vector3 gridMax;
        [SerializeField]
        private Vector2 cellSize;
        [SerializeField]
        private Vector2Int gridSize;
        private GridCell[,] grid;

        public event Action<GridCell[,]> OnGridUpdated;
        public event Action<GridCell> OnGridCellSelected; 

        public Vector2Int GridSize
        {
            get { return gridSize; }
        }

        public Vector2 CellSize
        {
            get { return cellSize; }
        }

        public void Init()
        {
            grid = new GridCell[gridSize.y, gridSize.x];
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var gridCell = grid[y, x];
                    gridCell.state = GridCellState.Free;
                    gridCell.position = new Vector3(gridMin.x + (x + 0.5f) * cellSize.x,
                                                    gridMin.y,
                                                    gridMax.z - (y + 0.5f) * cellSize.y);
                    gridCell.min = new Vector3(gridCell.position.x - 0.5f * cellSize.x,
                                               gridCell.position.y,
                                               gridCell.position.z - 0.5f * cellSize.y);

                    gridCell.max = new Vector3(gridCell.position.x + 0.5f * cellSize.x,
                                               gridCell.position.y,
                                               gridCell.position.z + 0.5f * cellSize.y);
                    gridCell.x = x;
                    gridCell.y = y;
                    grid[y, x] = gridCell;
                }
            }
        }

        public bool CanPlaceBuilding(int xCenter, int yCenter, int xSize, int ySize)
        {
            for (int y = yCenter; y < yCenter + ySize; y++)
            {
                for (int x = xCenter; x < xCenter + xSize; x++)
                {
                    if (x >= gridSize.x || y >= gridSize.y || grid[y, x].state == GridCellState.Busy)
                        return false;
                }
            }
            return true;
        }

        public void PlaceBuilding(int xCenter, int yCenter, int xSize, int ySize)
        {
            for (int y = yCenter; y < yCenter + ySize; y++)
            {
                for (int x = xCenter; x < xCenter + xSize; x++)
                {
                    grid[y, x].state = GridCellState.Busy;
                }
            }
            OnGridUpdated?.Invoke(grid);
        }

        public void PlaceBuilding(int xSize, int ySize, out GridCell cell)
        {
            cell = GridCell.Default;
            for(int y = 0; y < gridSize.y; y++)
            {
                for(int x = 0; x < gridSize.x; x++)
                {
                    var gridCell = grid[y, x];
                    if(CanPlaceBuilding(x, y, xSize, ySize))
                    {
                        PlaceBuilding(x, y, xSize, ySize);
                        cell = gridCell;
                        cell.state = GridCellState.Busy;
                        return;
                    }
                }
            }
        }

        public void RemoveBuilding(int xCenter, int yCenter, int xSize, int ySize)
        {
            for (int y = yCenter; y < yCenter + ySize; y++)
            {
                for (int x = xCenter; x < xCenter + xSize; x++)
                {
                    if (gridSize.y > y && gridSize.x > x && y >= 0 && x >= 0)
                        grid[y, x].state = GridCellState.Free;
                }
            }
            OnGridUpdated?.Invoke(grid);
        }

        public GridCell PointToCell(Vector3 position)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {

                    var gridCell = grid[y, x];
                    if (position.x < gridCell.max.x && position.z < gridCell.max.z &&
                       position.x > gridCell.min.x && position.z > gridCell.min.z)
                    {
                        return gridCell;
                    }
                }
            }
            return GridCell.Default;
        }

        public GridCell GetGridCell(int x, int y)
        {
            return x < gridSize.x && x >= 0 && y < gridSize.y && y >=0 ? grid[y, x] : GridCell.Default;
        }

        public Vector3 GetBuildingPosition(GridCell mainCell, int sizeX, int sizeY)
        {
            var botCell = GetGridCell(mainCell.x + sizeX - 1, mainCell.y + sizeY - 1);
            var topLeft = new Vector3(mainCell.min.x, mainCell.min.y, mainCell.max.z);
            var botRight = new Vector3(botCell.max.x, botCell.min.y, botCell.min.z);
            return (topLeft + botRight) / 2;
        }
    }
}