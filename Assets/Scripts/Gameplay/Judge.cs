using System.Linq;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Determines the outcome of the game
    /// </summary>
    public class Judge
    {
        private Cell[,] cells;
        private int fieldDimensions;

        public Judge(Cell[,] cells)
        {
            this.cells = cells;
            fieldDimensions = this.cells.GetLength(0);
        }

        public (CellState, bool) GetWinner()
        {
            var rowWinner = GetRowWinner();
            if (rowWinner != CellState.Empty)
            {
                return (rowWinner, true);
            }

            var columnWinner = GetColumnWinner();
            if (columnWinner != CellState.Empty)
            {
                return (columnWinner, true);
            }

            var diagonalWinner = GetDiagonalWinner();
            if (diagonalWinner != CellState.Empty)
            {
                return (diagonalWinner, true);
            }

            return (CellState.Empty, IsFieldFull());
        }

        /// <summary>
        /// Check if there are no more empty cells on playing field
        /// </summary>
        /// <returns>True if field is full of non-empty cells</returns>
        private bool IsFieldFull()
        {
            for (var i = 0; i < fieldDimensions; i++)
            {
                for (var j = 0; j < fieldDimensions; j++)
                {
                    if (cells[i, j].GetState() == CellState.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get current winner by diagonal rule
        /// </summary>
        /// <returns></returns>
        private CellState GetDiagonalWinner()
        {
            var firstDiagonal = new CellState[fieldDimensions];
            for (var i = 0; i < fieldDimensions; i++)
            {
                firstDiagonal[i] = cells[i, i].GetState();
            }

            var firstDiagonalResult = firstDiagonal.All(cell => cell == firstDiagonal[0]);
            if (firstDiagonalResult && firstDiagonal[0] != CellState.Empty)
            {
                Debug.Log("Win by diagonal");
                return firstDiagonal[0];
            }

            var secondDiagonal = new CellState[fieldDimensions];
            for (var i = 0; i < fieldDimensions; i++)
            {
                secondDiagonal[i] = cells[fieldDimensions - 1 - i, i].GetState();
            }

            var secondDiagonalResult = secondDiagonal.All(cell => cell == secondDiagonal[0]);
            if (secondDiagonalResult && secondDiagonal[0] != CellState.Empty)
            {
                Debug.Log("Win by diagonal");
                return secondDiagonal[0];
            }

            return CellState.Empty;
        }

        /// <summary>
        /// Get current winner by column rule
        /// </summary>
        /// <returns></returns>
        private CellState GetColumnWinner()
        {
            for (var i = 0; i < fieldDimensions; i++)
            {
                var firstState = cells[0, i].GetState();
                var result = Enumerable.Range(0, cells.GetLength(0)).Select(x => cells[x, i])
                    .All(cell => cell.GetState() == firstState);
                if (result && firstState != CellState.Empty)
                {
                    Debug.Log("Win by column!");
                    return firstState;
                }
            }

            return CellState.Empty;
        }

        /// <summary>
        /// Get current winner by row rule
        /// </summary>
        /// <returns></returns>
        private CellState GetRowWinner()
        {
            for (var i = 0; i < fieldDimensions; i++)
            {
                var firstState = cells[i, 0].GetState();
                var result = Enumerable.Range(0, cells.GetLength(1)).Select(x => cells[i, x])
                    .All(cell => cell.GetState() == firstState);
                if (result && firstState != CellState.Empty)
                {
                    Debug.Log("Win by row!");
                    return firstState;
                }
            }

            return CellState.Empty;
        }
    }
}