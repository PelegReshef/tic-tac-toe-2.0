


/* whats left to do:
4. finish runPVSAI() method - Game class - heaven'ts started
6. AI class - haven't started:
    - easy - random move
    - medium - block and win if possible
    - hard - create a minimax algorithm that chooses the best move
7. create a start menu - in progress. should include: 
   - start game - **complete**
   - choose player vs AI or player vs player - **complete**
   - how to play - **complete**
   - exit game - **haven't started**
   - maybe add an option for multiple games simultaneously - **haven't started**

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
                if (menuManager.MenuLoop() != ReturnTypes.MenuButton) // if the game is started
                {

                    return; // exit the loop
                }
            }
        }

    }
    
    
    
}
