using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ticTacToe
{
    public class Game // control the game: draw the board, swithch turns, end the game, etc.
    {

        bool gameOver = false;
        bool engineIsActive;


        BoardVisuals boardVisuals;
        BoardLogic boardLogic;
        Cursor cursor;

        ReturnTypes gameType;
        public Game(ReturnTypes returnType, bool engine)
        {
            boardLogic = new BoardLogic(); 
            boardLogic.NewBoard(0, 0, true); // centered for now. will be changed to player choice later. edit: probably will not be changed, because I dont want to make 2 games simultaneously.
            boardVisuals = new BoardVisuals(boardLogic);
            cursor = new Cursor(boardLogic);
            gameType = returnType;
            engineIsActive = engine;

        }
        public void RunPVP()
        {
            Player xPlayer = new Player(PlayerType.X_player, Difficulty.RealPlayer); 
            Player Oplayer = new Player(PlayerType.O_Player, Difficulty.RealPlayer); 
            cursor.Draw(); 

            // game loop for pvp
            
            while (!gameOver)
            {
                RealPlayerMove(xPlayer);

                if (!gameOver) // check if the game is over after player 1's turn
                {
                    RealPlayerMove(Oplayer);
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
                RealPlayerMove(player);

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
                    HandleEndOfTHeGame(gameState, bot);


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

        public void RealPlayerMove(Player player)
        {
            bool validAction = false; // check if the action is valid
            bool XTurn = player.playerType == PlayerType.X_player; 

            while (!validAction)
            {
                int actionPos = cursor.MoveUntilAction(engineIsActive); 
                if (actionPos == -1)
                {
                    EngineVisualiser engine = new EngineVisualiser(boardLogic, cursor);
                    engine.DrawEngineResults(XTurn); // draw the engine results for the current player turn
                    Utilities.GetValidInput(); // wait for the player to press a key to continue
                    engine.EraseEngineResults(); // erase the engine results after the player pressed a key
                    continue; // continue the loop to get a valid action
                }

                validAction = ExamineAction(player, actionPos);
            }

            GameState gameState = GameLogic.CheckGameState(boardLogic);
            HandleEndOfTHeGame(gameState, player);

        }


        bool ExamineAction(Player player, int cursorPos) // cursorPos is to support AI moves too
        {
 
            cursor.CursorPos = cursorPos;
            switch (player.playerType)
            {
                case PlayerType.X_player:
                    if (boardLogic.boardCells[cursorPos].state == CellState.Empty) 
                    {
                        cursor.Erase();
                        boardLogic.ChangeCellstate(CellState.X, cursorPos);
                        boardVisuals.DrawCell(CellState.X, cursorPos); 
                        cursor.Draw(); // draw the cursor again because DrawCell erases it
                        return true;
                    }
                    break;
                case PlayerType.O_Player:
                    if (boardLogic.boardCells[cursorPos].state == CellState.Empty) 
                    {
                        cursor.Erase();
                        boardLogic.ChangeCellstate(CellState.O, cursorPos);
                        boardVisuals.DrawCell(CellState.O, cursorPos); 
                        cursor.Draw(); // draw the cursor again because DrawCell erases it
                        return true;
                    }
                    break;
                default:
                    Utilities.Error("unexpected error. player not recognized");
                    return false;
            }
            return false;

        }
        void HandleEndOfTHeGame(GameState gameState, Player player)
        {
            switch (gameState)
            {
                case GameState.WinX:
                case GameState.WinO:
                    boardVisuals.DrawWinMessage(player);
                    gameOver = true; 
                    break;
                case GameState.Draw:
                    boardVisuals.DrawDrawMessage(); 
                    gameOver = true; 
                    break;
                case GameState.InProgress:
                    gameOver = false; 
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
            boardCells = new Cell[9];
            {
                boardCells[0] = new Cell(X, Y);
                boardCells[1] = new Cell(X + ((boardWidth - 2) / 3), Y);
                boardCells[2] = new Cell(X + 2 * ((boardWidth - 2) / 3), Y);
                boardCells[3] = new Cell(X, Y + ((boardHeight - 1) / 3));
                boardCells[4] = new Cell(X + ((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3));
                boardCells[5] = new Cell(X + 2 * ((boardWidth - 2) / 3), Y + ((boardHeight - 1) / 3));
                boardCells[6] = new Cell(X, Y + 2 * ((boardHeight - 1) / 3));
                boardCells[7] = new Cell(X + ((boardWidth - 2) / 3), Y + 2 * ((boardHeight - 1) / 3));
                boardCells[8] = new Cell(X + 2 * ((boardWidth - 2) / 3), Y + 2 * ((boardHeight - 1) / 3));
            }

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
        public void DrawWinMessage(Player player) // draw the win message on the screen
        {
            switch (player.difficulty)
            {
                case Difficulty.RealPlayer:

                    switch (player.playerType)
                    {
                        case PlayerType.X_player:
                            Art.Draw(Art.WinMessageX, boardLogic.X + ((Art.NewBoard[0].Length - Art.WinMessageX[0].Length) / 2), boardLogic.Y - Art.WinMessageX.Length - 2);
                            break;
                        case PlayerType.O_Player:
                            Art.Draw(Art.WinMessageO, boardLogic.X + ((Art.NewBoard[0].Length - Art.WinMessageO[0].Length) / 2), boardLogic.Y - Art.WinMessageO.Length - 2);
                            break;
                    }
                    break;
                default:
                    Art.Draw(Art.WinMessageBot, boardLogic.X + ((Art.NewBoard[0].Length - Art.WinMessageBot[0].Length) / 2), boardLogic.Y - Art.WinMessageBot.Length - 2);
                    break;
            }
        }

        public void DrawDrawMessage()
        {
            Art.Draw(Art.WinMessageDraw, boardLogic.X + ((Art.NewBoard[0].Length - Art.WinMessageDraw[0].Length) / 2), boardLogic.Y - Art.WinMessageDraw.Length - 2);
        }
    }
    public class EngineVisualiser
    {
        BoardLogic boardLogic; 
        Cursor winCursor;
        Cursor drawCursor;
        Cursor lossCursor;
        Cursor playingCursor;


        public EngineVisualiser(BoardLogic boardLogic, Cursor playingCursor)
        {
            this.boardLogic = boardLogic; 
            winCursor = new Cursor(boardLogic);
            drawCursor = new Cursor(boardLogic);
            lossCursor = new Cursor(boardLogic);
            this.playingCursor = playingCursor; // the cursor that is used to play the game. it will be erased after the engine results are drawn
        }
        List<int>[] GetEngineResults(bool forXTurn)
        {
            return AI.Engine(boardLogic, forXTurn);
        }

        public void DrawEngineResults(bool forXTurn)
        {
            playingCursor.Erase(); 
            List<int>[] analyzedMoves = GetEngineResults(forXTurn);

            Console.ForegroundColor = ConsoleColor.Green; // winning moves are green
            foreach (int move in analyzedMoves[0]) 
            {

                winCursor.CursorPos = move;
                winCursor.Draw();
            }

            Console.ForegroundColor = ConsoleColor.Yellow; // drawing moves are yellow
            foreach (int move in analyzedMoves[1]) 
            {
                Console.ForegroundColor = ConsoleColor.Yellow; 
                drawCursor.CursorPos = move;
                drawCursor.Draw();
            }

            Console.ForegroundColor = ConsoleColor.Red; // losing moves are red
            foreach (int move in analyzedMoves[2]) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                lossCursor.CursorPos = move;
                lossCursor.Draw();
            }
            Console.ResetColor(); // reset the color to default
        }
        public void EraseEngineResults()
        {
            int currentCursorPos = playingCursor.CursorPos; // save the current cursor position to return it after erasing the engine results

            for (int i = 0; i <9; i++)
            {
                playingCursor.CursorPos = i;
                if (i != currentCursorPos) // if the cursor is not in the current position, erase it
                {
                    playingCursor.Erase();
                }

            }
            playingCursor.CursorPos = currentCursorPos; // return the cursor to the previous position
            playingCursor.Draw(); // draw the cursor again in the previous position
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
        public int MoveUntilAction(bool engineIsActive)
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
                    case ConsoleKey.E:
                        if (engineIsActive)
                        {
                            return -1; // engine button
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Engine is not active right now. Please turn it on in options.");
                            continue; 
                        }
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
