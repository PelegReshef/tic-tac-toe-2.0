


/* whats left to do:


3. finish GameLogic class - check for win/draw/loss - haven't started
4. finish run() method - Game class - in progress
6. AI class - haven't started
7. create a start menu - haven't started. should include: havnt started
   - start game
   - choose player vs AI or player vs player
   - how to play
   - exit game
   - maybe add an option for multiple games simultaneously

*/

class Program
{
    static void Main(string[] args)
    {

        Utilities.Setup();

        Game game = new Game(true); // true for now. will be changed to player choice later


        game.Run(); // run the game


    }
}

/* game class would:
     1. create new board V
     2. create new boardLogc for the board created V
     3. create new players. one for first player, one for second player/AI V
     4. use class GameLogic to check for win/draw/loss
     5. use class Player to move the player
     */

class Game // control the game: draw the board, swithch turns, end the game, etc.
{


    bool yourTurn = true; //true = player, false = AI/ 2nd player
    bool gameOver = false; //true = game over, false = game not over


    Player p1; // player 1
    Player p2; // player 2
    Player AI; // AI player
    BoardVisuals boardVisuals; 
    BoardLogic boardLogic;
    Cursor cursor; // cursor for the player to move around the board

    bool playingAgainstRealPlayer; // true = playing against real player, false = playing against AI

    public Game(bool realPlayer)
    {
        boardLogic = new BoardLogic(); // create the board logic
        boardLogic.NewBoard(0, 0, true); // centered for now. will be changed to player choice later
        boardVisuals = new BoardVisuals(boardLogic); // create the board visuals


        // create the players and cursor

        cursor = new Cursor(boardLogic); // create the cursor
        p1 = new Player(PlayerType.Player1); // player 1

        playingAgainstRealPlayer = realPlayer;
        p2 = new Player(PlayerType.Player2); // player 2
        AI = new Player(PlayerType.AI); // AI


    }
    public void RunPVP()
    {
        Turns turn = Turns.Player1; // start with player 1
        cursor.Draw(); // draw the cursor

        // game loop for pvp
        if (playingAgainstRealPlayer)
            while (!gameOver)
            {
                if (turn == Turns.Player1)
                {

                    bool validAction = false; // check if the action is valid

                    do
                    {
                        int actionPos = cursor.MoveUntilAction(); // move the player icon
                        validAction = ExamineActionPVP(p1, cursor); // examine the action

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
                        validAction = ExamineActionPVP(p2, cursor); // examine the action

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
                    Utilities.Error("unexpected error. turn not recognized"); // if the turn is not recognized
                    gameOver = true; // end the game

                }
            }
    }
    public void RunVSAI()
    {
        // AI logic here
    }
    public void Run()
    {
        boardVisuals.DrawNewBoard(boardLogic.X, boardLogic.Y); // draw the board

        if (playingAgainstRealPlayer)
        {
            RunPVP(); // run the player vs player game
        }
        else
        {
            RunVSAI(); // run the player vs AI game
        }

    }



    bool ExamineActionPVP(Player player, Cursor cursor) // 
    {
        switch (player.playerType)
        {
            case PlayerType.Player1:
                if (boardLogic.boardCells[cursor.CursorPos].state == CellState.Empty) // check if the cell is empty
                {
                    boardLogic.ChangeCellstate(CellState.X, cursor.CursorPos); // change the cell state to X
                    boardVisuals.DrawCell(CellState.X, cursor.CursorPos); // draw the cell on screen
                    cursor.Draw(); // draw the cursor again because DrawCell erases it
                    return true;
                }
                break;
            case PlayerType.Player2:
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

class BoardLogic
{
    public Cell[] boardCells = new Cell[9]; // array of cells. 3x3 grid

    int boardWidth = PixelArt.newBoard[0].Length; // width of the board
    int boardHeight = PixelArt.newBoard.Length; // height of the board

    public int X; // X position of the board. top left corner
    public int Y; // Y position of the board. top left corner

    public void NewBoard(int x, int y, bool centered) // create a new board
    {
        X = x; // set the X position of the board
        Y = y; // set the Y position of the board
        if (centered)
        {
            X = (Console.BufferWidth / 2) - (boardWidth / 2);
            Y = (Console.BufferHeight / 2) - (boardHeight / 2);
        }
        RecalculateCells(); // recalculate the cells
    }

    void RecalculateCells()
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

class BoardVisuals // every visual aspect of the board
{

    BoardLogic boardLogic;

    public int X;// X position of the board. top left corner
    public int Y;// Y position of the board. top left corner

    public BoardVisuals(BoardLogic boardLogic)
    {
        this.boardLogic = boardLogic; // match the board to the board logic
    }

    public void DrawNewBoard(int x, int y)
    {
        X = x; // set the X position of the board
        Y = y; // set the Y position of the board


        for (int i = 0; i < PixelArt.newBoard.Length; i++)
        {
            Console.SetCursorPosition(X, Y + i);
            Console.Write(PixelArt.newBoard[i]);
        }
    }
    public void DrawCell(CellState state, int cellIndex) // draw the cell
    {
        switch (state)
        {
            case CellState.Empty:
                for (int i = 0; i < PixelArt.Empty.Length; i++)
                {
                    Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                    Console.Write(PixelArt.Empty[i]);
                }
                break;
            case CellState.X:
                for (int i = 0; i < PixelArt.X.Length; i++)
                {
                    Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                    Console.Write(PixelArt.X[i]);
                }
                break;
            case CellState.O:
                for (int i = 0; i < PixelArt.O.Length; i++)
                {
                    Console.SetCursorPosition(boardLogic.boardCells[cellIndex].X, boardLogic.boardCells[cellIndex].Y + i);
                    Console.Write(PixelArt.O[i]);
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
    Player1,
    Player2,
    AI
}
class Cell
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

        cellHeight = PixelArt.Empty[0].Length / 2;
        cellWidth = PixelArt.Empty.Length / 2;

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

static class GameLogic // check for win/draw/loss
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
            if (winCondition[0] ==  winCondition[1] && winCondition[1] == winCondition[2] && winCondition[0] != CellState.Empty) // check if all 3 cells are the same 
            {
                return GameState.Win; // return win
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
    /* in charge of:
     - player type
     - player icon
      */
    struct Player // move the player that let you choose where to go
{
    public const string playerIcon = @"\/"; // player character
    public PlayerType playerType; // type of player. player 1, player 2, AI

    public Player(PlayerType playerType)
    {
        this.playerType = playerType; // set the player type
    }


}
class Cursor // move the player icon around the board
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
                    return CursorPos; // return true to indicate that the player has made a move



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
class AI // AI that plays the game
{

}
static class Utilities
{
    public static void Setup()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.BufferWidth = 240;
        Console.BufferHeight = 63;
        Console.CursorVisible = false;

    }
    public static ConsoleKeyInfo GetValidInput()
    {
        while (true)
        {
            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            switch (keyPressed.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.A:
                case ConsoleKey.S:
                case ConsoleKey.D:
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.Spacebar:
                    return keyPressed;
                default:

                    continue; // ignore any other key

            }
        }

    }
    public static void Error(string message)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(message);
    }

}
static class PixelArt
{
    public static string[] O = new string[]
    {

        "██████████████████",
        "██              ██",
       @"██              ██",
        "██              ██",
        "██    ██████    ██",
        "██  ██      ██  ██",
        "██  ██      ██  ██",
        "██  ██      ██  ██",
        "██    ██████    ██",
        "██              ██",
        "██████████████████",

    };
    public static string[] X = new string[]
    {

        "██████████████████",
        "██              ██",
       @"██              ██",
        "██              ██",
        "██  ██      ██  ██",
        "██    ██  ██    ██",
        "██      ██      ██",
        "██    ██  ██    ██",
        "██  ██      ██  ██",
        "██              ██",
        "██████████████████",
    };

    public static string[] Empty = new string[]
    {
        "██████████████████",
        "██              ██",
       @"██              ██",
        "██              ██",
        "██              ██",
        "██              ██",
        "██              ██",
        "██              ██",
        "██              ██",
        "██              ██",
        "██████████████████",
    };
    public static string[] newBoard = new string[]
    {
        "██████████████████████████████████████████████████",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██", //cell 1, cell 2, cell 3
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██████████████████████████████████████████████████",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██", //cell 4, cell 5, cell 6
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██████████████████████████████████████████████████",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██", //cell 7, cell 8, cell 9
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██              ██              ██              ██",
        "██████████████████████████████████████████████████",


    };
}