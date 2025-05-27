using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tic_tac_toe_2._0
{
    public class Game // control the game: draw the board, swithch turns, end the game, etc.
    {

        bool gameOver = false; //true = game over, false = game not over


        BoardVisuals boardVisuals;
        BoardLogic boardLogic;
        Cursor cursor; // cursor for the player to move around the board

        ReturnTypes gameType;
        /*
         * make a method for proccesing the return type to a new enum
         * check the new enum in Run() method
         * create players and bots in Run() method instead of constructor
         */
        public Game(ReturnTypes returnType)
        {
            boardLogic = new BoardLogic(); // create the board logic
            boardLogic.NewBoard(0, 0, true); // centered for now. will be changed to player choice later
            boardVisuals = new BoardVisuals(boardLogic); // create the board visuals
            cursor = new Cursor(boardLogic); // create the cursor
            gameType = returnType;

            // create the players and cursor



        }
        public void RunPVP()
        {
            Player xPlayer = new Player(PlayerType.X_player); // player 1
            Player playerO = new Player(PlayerType.O_Player); // player 2


            Turns turn = Turns.Player1; // start with player 1
            cursor.Draw(); // draw the cursor

            // game loop for pvp
            
            while (!gameOver)
            {
                if (turn == Turns.Player1)
                {

                    bool validAction = false; // check if the action is valid

                    do
                    {
                        int actionPos = cursor.MoveUntilAction(); // move the player icon
                        validAction = ExamineActionPVP(xPlayer, cursor); // examine the action

                    } while (!validAction); // repeat until a valid action is made

                    GameState gameState = GameLogic.CheckGameState(boardLogic);
                    HandleGameState(gameState, "Player 1");
                    if (!gameOver)
                    {
                        turn = Turns.Player2; // game continues + switch turns
                    }
                }
                else if (turn == Turns.Player2)
                {

                    bool validAction = false; // check if the action is valid

                    do
                    {
                        int actionPos = cursor.MoveUntilAction(); // move the player icon
                        validAction = ExamineActionPVP(playerO, cursor); // examine the action

                    } while (!validAction); // repeat until a valid action is made

                    GameState gameState = GameLogic.CheckGameState(boardLogic);
                    HandleGameState(gameState, "Player 2");
                    if (!gameOver)
                    {
                        turn = Turns.Player1; // game continues + switch turns
                    }

                }
                else
                {
                    Utilities.Error("unexpected error. turn not recognized(RunPVP())"); // if the turn is not recognized
                    gameOver = true; // end the game

                }
            }
        }
        public void RunVSAI(BotDifficulty difficulty)
        {
            AIBot bot = new AIBot(difficulty); 
        }
        public void Run()
        {
            boardVisuals.DrawNewBoard(); // draw the board

            switch (gameType) // set the game type
            {
                case ReturnTypes.PlayerVsPlayer:
                    RunPVP();
                    break;
                case ReturnTypes.PlayerVsAI_Easy:
                    RunVSAI(BotDifficulty.Easy); 
                    break;
                case ReturnTypes.PlayerVsAI_Medium:
                    RunVSAI(BotDifficulty.Medium); 
                    break;
                case ReturnTypes.PlayerVsAI_Hard:
                    RunVSAI(BotDifficulty.Hard); 
                    break;
                default:
                    Utilities.Error("unexpected error. game type is not recognized (Run())"); // if the game type is not recognized
                    gameOver = true; // end the game
                    return;
            }
        }



        bool ExamineActionPVP(Player player, Cursor cursor) // 
        {
            switch (player.playerType)
            {
                case PlayerType.X_player:
                    if (boardLogic.boardCells[cursor.CursorPos].state == CellState.Empty) // check if the cell is empty
                    {
                        boardLogic.ChangeCellstate(CellState.X, cursor.CursorPos); // change the cell state to X
                        boardVisuals.DrawCell(CellState.X, cursor.CursorPos); // draw the cell on screen
                        cursor.Draw(); // draw the cursor again because DrawCell erases it
                        return true;
                    }
                    break;
                case PlayerType.O_Player:
                    if (boardLogic.boardCells[cursor.CursorPos].state == CellState.Empty) // check if the cell is empty
                    {
                        boardLogic.ChangeCellstate(CellState.O, cursor.CursorPos); // change the cell state to O
                        boardVisuals.DrawCell(CellState.O, cursor.CursorPos); // draw the cell on screen
                        cursor.Draw(); // draw the cursor again because DrawCell erases it
                        return true;
                    }
                    break;
                default:
                    Utilities.Error("unexpected error. player not recognized"); // if the player type is not recognized
                    return false;
            }
            return false; // return false if the cell is not empty

        }
        void HandleGameState(GameState gameState, string playerName)
        {
            switch (gameState)
            {
                case GameState.Win:
                    Console.SetCursorPosition(0, 0); // reset the cursor position
                    Console.WriteLine(playerName + " wins!"); // print the winner
                    gameOver = true; // end the game
                    break;
                case GameState.Draw:
                    Console.SetCursorPosition(0, 0); // reset the cursor position
                    Console.WriteLine("Draw!"); // print the draw
                    gameOver = true; // end the game
                    break;
                case GameState.InProgress:
                    gameOver = false; // game is still in progress
                    break;
            }
        }


    }
    public enum Turns
    {
        Player1,
        Player2,
        AI,
    }

    public class BoardLogic
    {
        public Cell[] boardCells = new Cell[9]; // array of cells. 3x3 grid

        int boardWidth = Art.NewBoard[0].Length; // OldWidth of the board
        int boardHeight = Art.NewBoard.Length; // OldHeight of the board

        public int X; // X position of the board. top left corner
        public int Y; // Y position of the board. top left corner

        public void NewBoard(int x, int y, bool centered) // create a new board
        {
            X = x; // set the X position of the board
            Y = y; // set the Y position of the board
            if (centered)
            {
                X = (Utilities.Width / 2) - (boardWidth / 2);
                Y = (Utilities.Height / 2) - (boardHeight / 2);
            }
            CreateCells(); // recalculate the cells
        }

        void CreateCells()
        {
            boardCells = new Cell[9] // create the cells
           {
            new (X , Y), // cell 1
            new (X + ((boardWidth - 2) / 3), Y), // cell 2
            new (X + 2*((boardWidth - 2) / 3), Y), // cell 3
            new (X , Y + ((boardHeight - 1) / 3)), // cell 4
            new (X + ((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3)), // cell 5
            new (X + 2*((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3)), // cell 6
            new (X , Y + 2*((boardHeight - 1) / 3)), // cell 7
            new (X + ((boardWidth - 2) / 3), Y + 2*((boardHeight - 1) / 3)), // cell 8
            new (X + 2*((boardWidth - 2) / 3), Y + 2 *((boardHeight - 1) / 3)), // cell 9
           };
        }

        public void ChangeCellstate(CellState state, int i) // i = index of the cell, state = state of the cell
        {
            boardCells[i].state = state; // change the state of the cell

        }

    }

    public class BoardVisuals // every visual aspect of the board
    {

        BoardLogic boardLogic;

        public BoardVisuals(BoardLogic boardLogic)
        {
            this.boardLogic = boardLogic; // match the board to the board logic
        }

        public void DrawNewBoard()
        {

            for (int i = 0; i < Art.NewBoard.Length; i++)
            {
                Console.SetCursorPosition(boardLogic.X, boardLogic.Y + i);
                Console.Write(Art.NewBoard[i]);
            }
        }
        public void DrawCell(CellState state, int cellIndex) // draw the cell
        {
            switch (state)
            {
                case CellState.Empty:
                    for (int i = 0; i < Art.Empty.Length; i++)
                    {
                        Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                        Console.Write(Art.Empty[i]);
                    }
                    break;
                case CellState.X:
                    for (int i = 0; i < Art.X.Length; i++)
                    {
                        Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                        Console.Write(Art.X[i]);
                    }
                    break;
                case CellState.O:
                    for (int i = 0; i < Art.O.Length; i++)
                    {
                        Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                        Console.Write(Art.O[i]);
                    }
                    break;
            }
        }
    }
    public class AIBot
    {
        public BotDifficulty difficulty; // difficulty of the bot. easy, medium, hard
        public AIBot(BotDifficulty difficulty)
        {
            this.difficulty = difficulty; 
        }
    }

    public enum CellState
    {
        Empty,
        X,
        O
    }
    public enum PlayerType
    {
        X_player,
        O_Player
        
    }
    public enum BotDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    public class Cell
    {
        public int X; // X position of the cell. top left corner
        public int Y; // Y position of the cell. top left corner

        int cellHeight;
        int cellWidth;
        public int PlayerIconPosX;
        public int PlayerIconPosY;

        public CellState state = CellState.Empty; // state of the cell. empty, X, O

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;

            cellHeight = Art.Empty[0].Length / 2;
            cellWidth = Art.Empty.Length / 2;

            PlayerIconPosX = X + cellWidth / 2 + 6; // position of the player icon in the cell
            PlayerIconPosY = Y + 2; // position of the player icon in the cell


        }


    }
    public enum GameState
    {
        Win,
        Draw,
        Loss,
        InProgress
    }

    
    /* in charge of:
     - player type
     - player icon
      */
    public struct Player // move the player that let you choose where to go
    {
        public const string playerIcon = @"\/"; // player character
        public PlayerType playerType; // type of player. player 1, player 2, O_AI

        public Player(PlayerType playerType)
        {
            this.playerType = playerType; // set the player type
        }


    }
    public class Cursor // move the player icon around the board
    {
        BoardLogic boardLogic;

        int[] cellsPlayerIconPosX = new int[9];
        int[] cellsPlayerIconPosY = new int[9];

        public int CursorPos = 4; // position of the cursor. cell index. 0-8


        public Cursor(BoardLogic boardLogic)
        {
            this.boardLogic = boardLogic;

            for (int i = 0; i < 9; i++)
            {
                cellsPlayerIconPosX[i] = boardLogic.boardCells[i].PlayerIconPosX; // get the X position of the player icon in the cell
                cellsPlayerIconPosY[i] = boardLogic.boardCells[i].PlayerIconPosY; // get the Y position of the player icon in the cell
            }

        }
        public int MoveUntilAction()
        {
            while (true)
            {
                ConsoleKeyInfo keyPressed = Utilities.GetValidInput(); // get the key pressed

                switch (keyPressed.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (CursorPos >= 3)
                        {
                            Erase();
                            CursorPos -= 3; // move up
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (CursorPos <= 5)
                        {
                            Erase();
                            CursorPos += 3; // move down
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (CursorPos % 3 != 2)
                        {
                            Erase();
                            CursorPos++; // move right
                        }
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if (CursorPos % 3 != 0)
                        {
                            Erase();
                            CursorPos--; // move left
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        return CursorPos;



                }
                Draw();
            }

        }
        public void Draw()
        {
            Console.SetCursorPosition(boardLogic.boardCells[CursorPos].PlayerIconPosX, boardLogic.boardCells[CursorPos].PlayerIconPosY);
            Console.Write(Player.playerIcon);
        }
        public void Erase()
        {
            Console.SetCursorPosition(boardLogic.boardCells[CursorPos].PlayerIconPosX, boardLogic.boardCells[CursorPos].PlayerIconPosY);
            Console.Write(' ');
            Console.SetCursorPosition(boardLogic.boardCells[CursorPos].PlayerIconPosX + 1, boardLogic.boardCells[CursorPos].PlayerIconPosY);
            Console.Write(' ');
        }

    }
}
