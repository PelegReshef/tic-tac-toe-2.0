


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
        static public readonly Dictionary<int, Dictionary<MenuStages, MenuStages>> MenuStageTransitions = new()
        {
            [0] = new Dictionary<MenuStages, MenuStages>
            {
                [MenuStages.MainMenu] = MenuStages.MainMenu_Play,
                [MenuStages.MainMenu_Play] = MenuStages.Play_PlayerVsPlayer,
                [MenuStages.MainMenu_Options] = MenuStages.Options_engine,
                [MenuStages.Play_PlayerVsAI] = MenuStages.PlayerVsAI_Easy,
            },
            [1] = new Dictionary<MenuStages, MenuStages>
            {
                [MenuStages.MainMenu] = MenuStages.MainMenu_HowToPlay,
                [MenuStages.MainMenu_Play] = MenuStages.Play_PlayerVsAI,
                [MenuStages.MainMenu_Options] = MenuStages.Options_controls,
                [MenuStages.Play_PlayerVsAI] = MenuStages.PlayerVsAI_Medium,
            },
            [2] = new Dictionary<MenuStages, MenuStages>
            {
                [MenuStages.MainMenu] = MenuStages.MainMenu_Options,
                [MenuStages.MainMenu_Play] = MenuStages.Play_ComingSoon,
                [MenuStages.MainMenu_Options] = MenuStages.Options_credits,
                [MenuStages.Play_PlayerVsAI] = MenuStages.PlayerVsAI_Hard,
            },
            [3] = new Dictionary<MenuStages, MenuStages>
            {
                [MenuStages.MainMenu] = MenuStages.MainMenu_Exit,
                [MenuStages.MainMenu_Play] = MenuStages.Play_Back,
                [MenuStages.MainMenu_Options] = MenuStages.Options_Back,
                [MenuStages.Play_PlayerVsAI] = MenuStages.PlayerVsAI_Back,
            },


        };

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
        public void ExamineCursorAction() // check what button the player pressed and change the menu stage accordingly
        {
            int cursorPos = menuCursor.cursorPos; // get the cursor position
            if (MenuStageTransitions.TryGetValue(cursorPos, out var changesForCertainPosition))
            {
                if (changesForCertainPosition.TryGetValue(menuLogic.CurrentMenuStage, out var newMenuStage))
                {
                    menuLogic.CurrentMenuStage = newMenuStage; // change the menu stage
                }
                else
                {
                    Utilities.Error("no menu stage found for the current menu stage and cursor position"); 
                }
            }
            else
            {
                Utilities.Error("invalid cursor position"); 
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
