using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


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
                    return winCondition[0] == CellState.X ? GameState.WinX : GameState.WinO; // return win if X, loss if O
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




        static int Easy(BoardLogic boardLogic) 
        {
            List<int> availableMoves = GetAvailableMoves(boardLogic); 
            return ChooseRandomMove(availableMoves); 
        }
        static int Medium(BoardLogic boardLogic)
        {
            List<int> availableMoves = GetAvailableMoves(boardLogic); 

            // check for winning moves
            List<int> winningMoves = new List<int>(); // list of winning moves for the bot to chooae randomly from
            foreach (int move in availableMoves)
            {
                if (IsWinningMove(boardLogic, move, CellState.O))
                {
                    winningMoves.Add(move); 
                }
            }
            if (winningMoves.Count > 0)
            {
                return winningMoves[random.Next(0, winningMoves.Count)];
            }

            List<int> blockingMoves = new List<int>(); 
            foreach (int move in availableMoves)
            {
                if (IsWinningMove(boardLogic, move, CellState.X)) // if the opponent has a winning move, block it
                {
                    blockingMoves.Add(move); 
                }
            }
            if (blockingMoves.Count > 0)
            {
                return blockingMoves[random.Next(0, blockingMoves.Count)];
            }

            // else choose random move
            return ChooseRandomMove(availableMoves);
        }
        static int Hard(BoardLogic boardLogic) 
        {
            const int chanceForBestMove = 70;
            int randomChance = random.Next(0, 100);
            if (randomChance < chanceForBestMove) // 70% chance to use the best move
            {
                return GetBestMove(boardLogic);
            }
            else // 30% chance to use the medium move
            {
                return Medium(boardLogic);
            }

        }


        static int Minimax(BoardLogic boardLogic, bool isMaximizing)
        {
            List<int> availableMoves = GetAvailableMoves(boardLogic); 

            // check if someone has won to stop recursion
            int score = EvaluateBoard(boardLogic);
            if (score != 99)
            {
                return score;
            }

            // if the game hasnt ended minimax every possible move on a copy of boardLogic
            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                foreach (var move in availableMoves)
                {
                    BoardLogic boardLogicClone = boardLogic.Clone();
                    boardLogicClone.ChangeCellstate(CellState.O, move);
                    int moveScore = Minimax(boardLogicClone, false);
                    bestScore = Math.Max(bestScore, moveScore);
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                foreach (var move in availableMoves)
                {
                    BoardLogic boardLogicClone = boardLogic.Clone();
                    boardLogicClone.ChangeCellstate(CellState.X, move);
                    int moveScore = Minimax(boardLogicClone, true);
                    bestScore = Math.Min(bestScore, moveScore);
                }
                return bestScore;
            }
        }

        static int GetBestMove(BoardLogic boardLogic)
        {
            List<int> availableMoves = GetAvailableMoves(boardLogic);

            int bestMovePos = -1;
            int bestScore = int.MinValue;
            List<int> bestMoves = new List<int>();
            foreach (var move in availableMoves)
            {
                var boardClone = boardLogic.Clone();
                boardClone.ChangeCellstate(CellState.O, move);
                int score = Minimax(boardClone, false); // false cuz im calling Minimax after making a move
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMovePos = move;
                    bestMoves.Clear(); // clear the list of previos best moves cuz they are not the best anymore
                    bestMoves.Add(move); 
                }
                else if (score == bestScore) // if the score is the same as the best score, add the move to the list of best moves
                {
                    bestMoves.Add(move);
                }
            }

            // if all moves are losing make the minimax to block the opponent instead of a rundom move cuz it looks less stupid
            if (bestMovePos < 0)
                throw new Exception(" unexpected error in GetBestMove()");
            else
            {
                if (bestScore == -1)
                {
                    List<int> blockingMoves = new List<int>();
                    foreach (int move in bestMoves)
                    {
                        if (IsWinningMove(boardLogic, move, CellState.X))
                        {
                            blockingMoves.Add(move);
                        }
                    }
                    if (blockingMoves.Count > 0)
                    {
                        return blockingMoves[random.Next(0, blockingMoves.Count)];
                    }

                }

                return bestMoves[random.Next(0, bestMoves.Count)];

            }
            
        }
        
        public static List<int>[] Engine(BoardLogic boardLogic, bool calculatesForX) // gives an array of lists of winning, drawing and losing moves.
        {
            List<int>[] moves = new List<int>[3]; // 0 - winning moves, 1 - drawing moves, 2 - losing moves
            moves[0] = new List<int>(); 
            moves[1] = new List<int>(); 
            moves[2] = new List<int>();

            List<int> availableMoves = GetAvailableMoves(boardLogic); 

            foreach (int move in availableMoves)
            {
                var boardLogicClone = boardLogic.Clone();
                boardLogicClone.ChangeCellstate(CellState.O, move); 
                int score = Minimax(boardLogicClone, calculatesForX);
                // if calculatesForX is true, then the AI is playing as X and we need to invert the score
                if (calculatesForX) // if the AI is playing as X, invert the score
                {
                    score = score * (-1);
                }
                switch (score)
                {
                    case 1: // win
                        moves[0].Add(move);
                        break;
                    case 0: // draw
                        moves[1].Add(move);
                        break;
                    case -1: // loss
                        moves[2].Add(move);
                        break;
                }
            }

            return moves;

        }

        static bool IsWinningMove(BoardLogic boardLogic, int position, CellState moveCellState) 
        {
            BoardLogic boardCopy = boardLogic.Clone(); // create a copy of the board logic to not change the original board
            boardCopy.ChangeCellstate(moveCellState, position);
            GameState state = GameLogic.CheckGameState(boardCopy);
            if (state == GameState.WinX || state == GameState.WinO || state == GameState.Draw) // if draw there's only one pos avalible so you have to return it.
            {
                return true;
            }
            return false;
        }
        static int EvaluateBoard(BoardLogic boardLogic)
        {
            GameState state = GameLogic.CheckGameState(boardLogic);
            switch (state)
            {
                case GameState.WinX:
                    return -1; 
                case GameState.WinO:
                    return 1; 
                case GameState.Draw:
                    return 0; 
                default:
                    return 99; // game in progress
            }
        }
        static List<int> GetAvailableMoves(BoardLogic boardLogic)
        {
            List<int> availableMoves = new List<int>();
            for (int i = 0; i < boardLogic.boardCells.Length; i++)
            {
                if (IsValidMove(boardLogic, i))
                {
                    availableMoves.Add(i);
                }
            }
            return availableMoves;
        }

        static int ChooseRandomMove(List<int> avalibleMoves)
        {
            int chosenMove = random.Next(0, avalibleMoves.Count); 
            return avalibleMoves[chosenMove];

        }
    }

}
