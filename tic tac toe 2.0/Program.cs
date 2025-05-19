


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
            gameManager.MenuLoop(); 
            gameManager.StartNewGame(); // start a new game
            Console.ReadKey();
        }
    }


    class GameManager // manage menu, and creating new games 
    {
        MenuManager menuManager;
        bool gameStarted = false; // flag to check if the game has started

        public GameManager()
        {
            menuManager = new MenuManager(); 
        }
        public void StartNewGame()
        {
            Utilities.Setup(); // setup the console for the game
            Game game = new Game(true); // create a new game
            game.Run(); // run the game
        }
        public void MenuSetup()
        {
            menuManager.MenuSetup(); // setup the menu
        }
        public void MenuLoop()
        {
            menuManager.MenuSetup(); // go to the main menu
            while (true)
            {
                menuManager.MenuLoop(); // loop through the menu
                if (gameStarted) // if the game is started
                {
                    break; // exit the loop
                }
            }
        }

    }
    
    
    
}
