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
        static bool IsValidMove(BoardLogic boardLogic, int cellPosition) // check if the cellPosition is valid
        {
            if (boardLogic.boardCells[cellPosition].state == CellState.Empty) // check if the cell is empty
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
        static int Easy(BoardLogic boardLogic) //returnes cursor pos of the selected cellPosition. 
        {
            while (true)
            {
                int move = random.Next(0, 9);
                if (IsValidMove(boardLogic, move))
                {
                    return move; // return the cellPosition
                }

            }

        }
        static int Medium(BoardLogic boardLogic)
        {
            // create a list of possible moves 
            List<int> availableMoves = new List<int>();
            for (int i = 0; i < boardLogic.boardCells.Length; i++)
            {
                if (IsValidMove(boardLogic, i)) 
                {
                    availableMoves.Add(i);
                }
            }

            // check for winning moves
            foreach (int move in availableMoves)
            {
                if (IsWinningMove(boardLogic, move, CellState.O)) return move;
            }

            //check for moves that block the opponent
            foreach(int move in availableMoves)
            {
                if (IsWinningMove(boardLogic, move, CellState.X)) return move;
            }

            // else choose random move
             return ChooseRandomMove(availableMoves);

            throw new Exception("this error should not be possible. (AI.Medium() method)");

        }
        static int Hard(BoardLogic boardLogic) // uses the minimax algorithm to find the best move
        {
            int bestMove = Minimax(boardLogic, true);
            return bestMove;
        }

        static int Minimax(BoardLogic boardLogic, bool isMaximizing)
        {
            // create a list of possible moves 
            List<int> availableMoves = new List<int>();
            for (int i = 0; i < boardLogic.boardCells.Length; i++)
            {
                if (IsValidMove(boardLogic, i))
                {
                    availableMoves.Add(i);
                }
            }


            int bestMovePos;
            int bestMove = -1;
            List<int> winningMoves = new List<int>();
            List<int> inProgression = new List<int>();

            foreach (var move in availableMoves)
            {
                BoardLogic boardLogicClone = boardLogic.Clone();
                boardLogicClone.ChangeCellstate(CellState.O, move);
                int score = EvaluateBoard(boardLogicClone);
                if (isMaximizing)
                {
                    if (score > bestMove)
                    {
                        bestMove = score;
                        bestMovePos = move;
                    }
                }
                else
                {
                    if (score < bestMove)
                    {
                        bestMove = score;
                        bestMovePos = move;
                    }

                }
            }

            if (isMaximizing)
            {

            }
        }




        static bool IsWinningMove(BoardLogic boardLogic, int position, CellState moveCellState) 
        {
            BoardLogic boardCopy = boardLogic.Clone(); // create a copy of the board logic to not change the original board
            boardCopy.ChangeCellstate(moveCellState, position);
            GameState state = GameLogic.CheckGameState(boardCopy);
            if (state == GameState.Win || state == GameState.Draw) // if draw there's only one pos avalible so you have to return it.
            {
                return true;
            }
            return false;
        }
        static int EvaluateBoard(BoardLogic boardLogic)
        {
            if (GameLogic.CheckGameState(boardLogic) == GameState.Win) 
            {
                return 1;
            }
            else if (GameLogic.CheckGameState(boardLogic) == GameState.Draw) 
            {
                return 0;
            }
            else 
            {
                return -1; 
            }
        }

        static int ChooseRandomMove(List<int> avalibleMoves)
        {
            int chosenMove = random.Next(0, avalibleMoves.Count); 
            return avalibleMoves[chosenMove];

        }
    }

}
