


/* whats left to do:
6. AI class - in progress:
    - medium - block and win if possible - **haven't started**
    - hard - create a minimax algorithm that chooses the best move - **haven't started**
7. create a start menu - in progress. should include: 
   - exit game - **haven't started**
   - maybe add an option for multiple games simultaneously - **haven't started** - probably not needed

*/


namespace ticTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.RefreshWindow(); 
            Utilities.UpdateFrameVariables(); 
            Utilities.Setup();
            GameManager gameManager = new GameManager(); 
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
