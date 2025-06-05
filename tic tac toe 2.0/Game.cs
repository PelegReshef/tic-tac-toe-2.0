using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticTacToe
{
    public class Game // control the game: draw the board, swithch turns, end the game, etc.
    {

        bool gameOver = false; 


        BoardVisuals boardVisuals;
        BoardLogic boardLogic;
        Cursor cursor;

        ReturnTypes gameType;
        public Game(ReturnTypes returnType)
        {
            boardLogic = new BoardLogic(); 
            boardLogic.NewBoard(0, 0, true); // centered for now. will be changed to player choice later. edit: probably will not be changed, because I dont want to make 2 games simultaneously.
            boardVisuals = new BoardVisuals(boardLogic);
            cursor = new Cursor(boardLogic);
            gameType = returnType;

        }
        public void RunPVP()
        {
            Player xPlayer = new Player(PlayerType.X_player, Difficulty.RealPlayer); 
            Player Oplayer = new Player(PlayerType.O_Player, Difficulty.RealPlayer); 
            cursor.Draw(); 

            // game loop for pvp
            
            while (!gameOver)
            {
                RealPlayerMove(xPlayer, "Player 1");

                if (!gameOver) // check if the game is over after player 1's turn
                {
                    RealPlayerMove(Oplayer, "Player 2");
                }
            }
        }
        public void RunVSAI(Difficulty difficulty)
        {
            Player player = new Player(PlayerType.X_player, Difficulty.RealPlayer);
            Player bot = new Player(PlayerType.O_Player, difficulty);
            cursor.Draw();

            while (!gameOver)
            {
                RealPlayerMove(player, "player 1");

                if (!gameOver) // check if the game is over after player 1's turn
                {
                    System.Threading.Thread.Sleep(750); // wait for a bit before the bot's turn
                    int botActionPos = AI.AIMove(boardLogic, bot.difficulty); 

                    int previousCursorPos = cursor.CursorPos; 
                    ExamineAction(bot, botActionPos);

                    // return the cursor to the previous position cuz apperently it feels annoying to have the cursor in the bot's position after the bot's turn
                    cursor.Erase();
                    cursor.CursorPos = previousCursorPos; 
                    cursor.Draw();

                    GameState gameState = GameLogic.CheckGameState(boardLogic);
                    HandleGameState(gameState, "bot");


                }
            }
        }
        public void Run()
        {
            boardVisuals.DrawNewBoard(); 

            switch (gameType) 
            {
                case ReturnTypes.PlayerVsPlayer:
                    RunPVP();
                    break;
                case ReturnTypes.PlayerVsAI_Easy:
                    RunVSAI(Difficulty.Easy); 
                    break;
                case ReturnTypes.PlayerVsAI_Medium:
                    RunVSAI(Difficulty.Medium); 
                    break;
                case ReturnTypes.PlayerVsAI_Hard:
                    RunVSAI(Difficulty.Hard); 
                    break;
                default:
                    Utilities.Error("unexpected error. game type is not recognized (Run())");
                    gameOver = true; 
                    return;
            }
        }

        public void RealPlayerMove(Player player, string playerWinMessage)
        {
            bool validAction = false; // check if the action is valid

            do
            {
                int actionPos = cursor.MoveUntilAction(); // move the player icon
                validAction = ExamineAction(player, actionPos); // examine the action

            } while (!validAction); // repeat until a valid action is made

            GameState gameState = GameLogic.CheckGameState(boardLogic);
            HandleGameState(gameState, playerWinMessage);

        }


        bool ExamineAction(Player player, int cursorPos) // cursorPos is to support AI moves too
        {
            cursor.Erase(); 
            cursor.CursorPos = cursorPos;
            switch (player.playerType)
            {
                case PlayerType.X_player:
                    if (boardLogic.boardCells[cursorPos].state == CellState.Empty) // check if the cell is empty
                    {
                        boardLogic.ChangeCellstate(CellState.X, cursorPos); // change the cell state to X
                        boardVisuals.DrawCell(CellState.X, cursorPos); // draw the cell on screen
                        cursor.Draw(); // draw the cursor again because DrawCell erases it
                        return true;
                    }
                    break;
                case PlayerType.O_Player:
                    if (boardLogic.boardCells[cursorPos].state == CellState.Empty) // check if the cell is empty
                    {
                        boardLogic.ChangeCellstate(CellState.O, cursorPos); // change the cell state to O
                        boardVisuals.DrawCell(CellState.O, cursorPos); // draw the cell on screen
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
                case GameState.WinX:
                case GameState.WinO:
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

    public class BoardLogic
    {
        public Cell[] boardCells = new Cell[9]; // array of cells. 3x3 grid

        int boardWidth = Art.NewBoard[0].Length; 
        int boardHeight = Art.NewBoard.Length; 

        // top left corner
        public int X; 
        public int Y; 

        public void NewBoard(int x, int y, bool centered) 
        {
            X = x; 
            Y = y; 
            if (centered)
            {
                X = (Utilities.Width / 2) - (boardWidth / 2);
                Y = (Utilities.Height / 2) - (boardHeight / 2);
            }
            CreateCells(); 
        }

        void CreateCells()
        {
            boardCells = new Cell[9] 
           {
            new (X , Y), 
            new (X + ((boardWidth - 2) / 3), Y), 
            new (X + 2*((boardWidth - 2) / 3), Y), 
            new (X , Y + ((boardHeight - 1) / 3)), 
            new (X + ((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3)), 
            new (X + 2*((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3)), 
            new (X , Y + 2*((boardHeight - 1) / 3)), 
            new (X + ((boardWidth - 2) / 3), Y + 2*((boardHeight - 1) / 3)), 
            new (X + 2*((boardWidth - 2) / 3), Y + 2 *((boardHeight - 1) / 3)), 
           };
        }

        public void ChangeCellstate(CellState state, int cellPosition) 
        {
            boardCells[cellPosition].state = state; 

        }
        public BoardLogic Clone() // for AI moves in GameLogic
        {
            BoardLogic clone = new BoardLogic();
            clone.X = this.X;
            clone.Y = this.Y;
            clone.boardCells = new Cell[9];
            for (int i = 0; i < 9; i++)
            {
                clone.boardCells[i] = new Cell(this.boardCells[i].X, this.boardCells[i].Y);
                clone.boardCells[i].state = this.boardCells[i].state; 
            }
            return clone;
        }

    }

    public class BoardVisuals // every visual aspect of the board. based on the BoardLogic class
    {

        BoardLogic boardLogic;

        public BoardVisuals(BoardLogic boardLogic)
        {
            this.boardLogic = boardLogic; 
        }

        public void DrawNewBoard()
        {

            for (int i = 0; i < Art.NewBoard.Length; i++)
            {
                Console.SetCursorPosition(boardLogic.X, boardLogic.Y + i);
                Console.Write(Art.NewBoard[i]);
            }
        }
        public void DrawCell(CellState state, int cellIndex) 
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
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        RealPlayer
    }
    public class Cell
    {
        // top left corner of the cell
        public int X; 
        public int Y;

        int cellWidth;

        // position of the player icon in the cell
        public int PlayerIconPosX;
        public int PlayerIconPosY;

        public CellState state = CellState.Empty; 

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;

            cellWidth = Art.Empty[0].Length / 2;

            PlayerIconPosX = X + cellWidth / 2 + 4;
            PlayerIconPosY = Y + 2; 


        }


    }
    public enum GameState
    {
        WinX,
        WinO,
        Draw,
        InProgress
    }
    
    public struct Player // move the player that let you choose where to go
    {
        public PlayerType playerType; // type of player. player 1, player 2
        public Difficulty difficulty;

        public Player(PlayerType playerType, Difficulty difficulty)
        {
            this.playerType = playerType; 
            this.difficulty = difficulty; 
        }


    }
    public class Cursor // move the player icon around the board
    {
        BoardLogic boardLogic;

        int[] cellsPlayerIconPosX = new int[9];
        int[] cellsPlayerIconPosY = new int[9];

        public int CursorPos = 4; // position of the cursor. cell index. 0-8
        const string Icon = @"\/";


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
            Console.Write(Icon);
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
