


/* whats left to do:
4. finish runPVSAI() method - Game class - heaven'ts started
6. AI class - haven't started
7. create a start menu - in progress. should include: 
   - start game
   - choose player vs AI or player vs player
   - how to play
   - exit game
   - maybe add an option for multiple games simultaneously

*/

namespace ticTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.RefreshWindow(); // refresh the window size
            Utilities.Setup();

            GameManager gameManager = new GameManager(); // create the game manager
            gameManager.StartScreenSetup(); // setup the start screen
            gameManager.StartNewGame(); // start a new game
            Console.ReadKey();
        }
    }


    class GameManager // manage start screen, and creating new games 
    {
        StartScreenLogic startScreenLogic; // logic for the start screen
        StartScreenVisuals startScreenVisuals; // visuals for the start screen
        StartScreenCursor startScreenCursor; // cursor for the start screen

        public GameManager()
        {
            startScreenLogic = new StartScreenLogic(); // create the start screen logic
            startScreenVisuals = new StartScreenVisuals(startScreenLogic); // create the start screen visuals
            startScreenCursor = new StartScreenCursor(startScreenLogic); // create the start screen cursor

        }
        public void StartNewGame()
        {
            Utilities.Setup(); // setup the console for the game
            Game game;
            game = new Game(true); // create a new game
            game.Run(); // run the game
        }
        public void StartScreenSetup()
        {
            startScreenVisuals.DrawMainMenu(); // draw the main menu
            startScreenCursor.Reset(); // reset the cursor position
            startScreenCursor.MoveUntilAction(); // move the cursor until an action is made
            Console.ReadLine(); // wait for the user to press enter

        }

    }
    
    
    
}
