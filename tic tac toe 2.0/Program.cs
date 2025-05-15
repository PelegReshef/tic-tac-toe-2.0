


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
            gameManager.MenuSetup(); // setup the start screen
            gameManager.StartNewGame(); // start a new game
            Console.ReadKey();
        }
    }


    class GameManager // manage start screen, and creating new games 
    {
        MenuLogic menuLogic; // logic for the start screen
        MenuVisuals menuVisuals; // visuals for the start screen
        MenuCursor menuCursor; // cursor for the start screen

        public GameManager()
        {
            menuLogic = new MenuLogic(); // create the start screen logic
            menuVisuals = new MenuVisuals(menuLogic); // create the start screen visuals
            menuCursor = new MenuCursor(menuLogic); // create the start screen cursor

        }
        public void StartNewGame()
        {
            Utilities.Setup(); // setup the console for the game
            Game game;
            game = new Game(true); // create a new game
            game.Run(); // run the game
        }
        public void MenuSetup()
        {
            menuVisuals.DrawMainMenu(); // draw the main menu
            menuCursor.Reset(); // reset the cursor position
            menuCursor.MoveUntilAction(); // move the cursor until an action is made
            Console.ReadLine(); // wait for the user to press enter

        }
        public void ExamineCursorAction() // check what button the player pressed
        {
            switch (menuCursor.cursorPos)
            {
                case 0: 
                    switch (menuLogic.CurrentMenuStage)
                    {
                        case MenuStages.MainMenu:
                            menuLogic.CurrentMenuStage = MenuStages.MainMenu_Play;
                            break;
                        case MenuStages.MainMenu_Play:
                            menuLogic.CurrentMenuStage = MenuStages.Play_PlayerVsPlayer;
                            break;
                        case MenuStages.MainMenu_Options:
                            menuLogic.CurrentMenuStage= MenuStages.Options_engine;
                            break;
                        case MenuStages.Play_PlayerVsAI:
                            menuLogic.CurrentMenuStage = MenuStages.PlayerVsAI_Easy;
                            break;
                            
                    }
                    break;
                case 1: 
                    switch (menuLogic.CurrentMenuStage)
                    {
                        case MenuStages.MainMenu:
                            menuLogic.CurrentMenuStage = MenuStages.MainMenu_HowToPlay;
                            break;
                        case MenuStages.MainMenu_Play:
                            menuLogic.CurrentMenuStage = MenuStages.Play_PlayerVsAI;
                            break;
                        case MenuStages.MainMenu_Options:
                            menuLogic.CurrentMenuStage= MenuStages.Options_controls;
                            break;
                        case MenuStages.PlayerVsAI_Easy:
                            menuLogic.CurrentMenuStage = MenuStages.PlayerVsAI_Medium;
                            break;
                            
                    }
                    break;
                case 2: 
                    switch (menuLogic.CurrentMenuStage)
                    {
                        case MenuStages.MainMenu:
                            menuLogic.CurrentMenuStage = MenuStages.MainMenu_Options;
                            break;
                        case MenuStages.MainMenu_Play:
                            menuLogic.CurrentMenuStage = MenuStages.Play_ComingSoon;
                            break;
                        case MenuStages.MainMenu_Options:
                            menuLogic.CurrentMenuStage= MenuStages.Options_credits;
                            break;
                        case MenuStages.PlayerVsAI_Easy:
                            menuLogic.CurrentMenuStage = MenuStages.PlayerVsAI_Hard;
                            break;
                            
                    }
                    break;
                case 3: 
                    switch (menuLogic.CurrentMenuStage)
                    {
                        case MenuStages.MainMenu:
                            menuLogic.CurrentMenuStage = MenuStages.MainMenu_Exit;
                            break;
                        case MenuStages.MainMenu_Play:
                            menuLogic.CurrentMenuStage = MenuStages.Play_Back;
                            break;
                        case MenuStages.MainMenu_Options:
                            menuLogic.CurrentMenuStage= MenuStages.Options_Back;
                            break;
                        case MenuStages.PlayerVsAI_Easy:
                            menuLogic.CurrentMenuStage = MenuStages.PlayerVsAI_Back;
                            break;
                            
                    }
                    break;
                
            }

        }
        public void MenuAction() //draw menu stage according to the current menu stage
        {
            menuLogic.DetermineMenuStage(); 
            switch (menuLogic.CurrentMenuStage)
            {
                case MenuStages.MainMenu:
                    menuVisuals.DrawMainMenu(); 
                    break;
                case MenuStages.MainMenu_Play:
                    menuVisuals.DrawPlayMenu(); 
                    break;
                case MenuStages.MainMenu_Options:
                    menuVisuals.DrawOptionsMenu(); 
                    break;
                case MenuStages.Play_PlayerVsAI:
                    menuVisuals.DrawPlayVsAIMenu(); 
                    break;

            }
        }

    }
    
    
    
}
