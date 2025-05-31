using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ticTacToe
{
    public static class GameLogic // check for win/draw/loss
    {
        public static GameState CheckGameState(BoardLogic boardLogic)
        {
            // check for win

            CellState[][] winConditions = new CellState[][]
            {
            new CellState[3] { boardLogic.boardCells[0].state, boardLogic.boardCells[1].state, boardLogic.boardCells[2].state }, // top line
            new CellState[3] { boardLogic.boardCells[3].state, boardLogic.boardCells[4].state, boardLogic.boardCells[5].state }, // mid line
            new CellState[3] { boardLogic.boardCells[6].state, boardLogic.boardCells[7].state, boardLogic.boardCells[8].state }, // bottom line
            new CellState[3] { boardLogic.boardCells[0].state, boardLogic.boardCells[3].state, boardLogic.boardCells[6].state }, // left column
            new CellState[3] { boardLogic.boardCells[1].state, boardLogic.boardCells[4].state, boardLogic.boardCells[7].state }, // mid column
            new CellState[3] { boardLogic.boardCells[2].state, boardLogic.boardCells[5].state, boardLogic.boardCells[8].state }, // right column
            new CellState[3] { boardLogic.boardCells[0].state, boardLogic.boardCells[4].state, boardLogic.boardCells[8].state }, // diagonal left to right
            new CellState[3] { boardLogic.boardCells[2].state, boardLogic.boardCells[4].state, boardLogic.boardCells[6].state }  // diagonal right to left
            };
            foreach (CellState[] winCondition in winConditions)
            {
                if (winCondition[0] == winCondition[1] && winCondition[1] == winCondition[2] && winCondition[0] != CellState.Empty) // check if all 3 cells are the same 
                {
                    return GameState.Win; 
                }
            }

            // check for draw
            foreach (Cell cell in boardLogic.boardCells)
            {
                if (cell.state == CellState.Empty) // check if there is an empty cell
                {
                    return GameState.InProgress; // return in progress
                }
            }
            return GameState.Draw; // return draw if there are no empty cells

        }
    }
    public static class AI // O_AI that plays the game
    {
        private static Random random;
        static AI() 
        {
            random = new Random(); 
        }
        static bool IsValidMove(BoardLogic boardLogic, int move) // check if the move is valid
        {
            if (boardLogic.boardCells[move].state == CellState.Empty) // check if the cell is empty
            {
                return true; 
            }
            return false; 
        }
        public static int AIMove(BoardLogic boardLogic, Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return Easy(boardLogic);
                case Difficulty.Medium:
                    return Medium(boardLogic);
                case Difficulty.Hard:
                    return Hard(boardLogic);
                default:
                    Utilities.Error("Invalid difficulty level(AIMove())");
                    return -1;
            }
        }
        public static int Easy(BoardLogic boardLogic) //returnes cursor pos of the selected move. 
        {
            while (true)
            {
                int move = random.Next(0, 9);
                if (IsValidMove(boardLogic, move))
                {
                    return move; // return the move
                }

            }

        }
        public static int Medium(BoardLogic boardLogic)
        {
            BoardLogic boardLogicCopy = boardLogic;
            List<MoveWithValue> availableMoves = new List<MoveWithValue>();
            for (int i = 0; i < boardLogicCopy.boardCells.Length; i++)
            {
                MoveWithValue move = new MoveWithValue(i);
                if (IsValidMove(boardLogicCopy, move.Position)) 
                {
                    availableMoves.Add(move);
                }
            }
            List<MoveWithValue> notWinningMoves = new List<MoveWithValue>();
            foreach (MoveWithValue move in availableMoves)
            {
                boardLogicCopy.ChangeCellstate(CellState.O, move.Position);
                GameState state = GameLogic.CheckGameState(boardLogicCopy);
                if (state == GameState.Win || state == GameState.Draw) // if draw there's only one pos avalible so you have to return it.
                {
                    return move.Position;
                }
                else
                {
                    notWinningMoves.Add(move);
                }
                
                
            }
            throw new ArgumentOutOfRangeException("this error should not be possible. (AI.Medium() method)");

        }
        public static int Hard(BoardLogic boardLogic)
        {
            return -1; // not implemented yet, but should use a minimax algorithm to choose the best move
        }
        public struct MoveWithValue
        {
            public int Position;
            public int Value;

            public MoveWithValue(int position)
            {
                Position = position;
            }

        }
    }

}
