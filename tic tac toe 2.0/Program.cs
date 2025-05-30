


/* whats left to do:
4. finish runPVSAI() method - Game class - heaven'ts started
6. O_AI class - haven't started:
    - easy - random move
    - medium - block and win if possible
    - hard - create a minimax algorithm that chooses the best move
7. create a start menu - in progress. should include: 
   - start game - **complete**
   - choose player vs O_AI or player vs player - **complete**
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
                ReturnTypes type = MenuLoop();
                StartNewGame(type);
                Utilities.GetValidInput();
            }

        }
        public void StartNewGame(ReturnTypes type)
        {
            Utilities.Setup(); 
            Game game = new Game(type); 
            
            game.Run(); 
        }
        public void MenuSetup()
        {
            menuManager.MenuSetup(); // setup the menu
        }
        public ReturnTypes MenuLoop()
        {
            menuManager.MenuSetup(); // go to the main menu
            while (true)
            {
                ReturnTypes returnType = menuManager.MenuLoop(); 
                if (returnType != ReturnTypes.MenuButton) 
                {

                    return returnType; 
                }
            }
        }

    }
    
    
    
}
