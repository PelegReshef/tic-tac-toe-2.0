
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
                StartNewGame(type, menuManager.EngineIsActive);
                Utilities.GetValidInput();
            }

        }
        public void StartNewGame(ReturnTypes type, bool engineIsActive)
        {
            Utilities.Setup(); 
            Game game = new Game(type, menuManager.EngineIsActive); 
            
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
