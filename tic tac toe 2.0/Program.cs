


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

using tic_tac_toe_2._0;

namespace ticTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.RefreshWindow(); // refresh the window size
            Utilities.UpdateFrameVariables(); // update the frame variables
            Utilities.Setup();
            GameManager gameManager = new GameManager(); // create the game manager
            gameManager.Run(); 
            Console.ReadKey();
        }
    }


    class GameManager // manage menu, and creating new games 
    {
        MenuManager menuManager;

        public GameManager()
        {
            menuManager = new MenuManager(); 
        }
        public void Run()
        {
            while (true)
            {
                MenuLoop();
                StartNewGame();
                Console.ReadLine();
            }

        }
        public void StartNewGame()
        {
            Utilities.Setup(); 
            Game game = new Game(true); 
            
            game.Run(); 
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
                bool gameStarted = menuManager.MenuLoop(); // loop through the menu
                if (gameStarted) // if the game is started
                {
                    return; // exit the loop
                }
            }
        }

    }
    
    
    
}
