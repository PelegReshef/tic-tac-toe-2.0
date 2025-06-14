using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ticTacToe
{
    public class MenuManager
    {
        MenuLogic menuLogic; // logic for the start screen
        MenuVisuals menuVisuals; // visuals for the start screen
        public MenuCursor menuCursor; // cursor for the start screen

        public bool EngineIsActive = false; 
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
        public MenuManager()
        {
            menuLogic = new MenuLogic(); // create the start screen logic
            menuVisuals = new MenuVisuals(menuLogic); // create the start screen visuals
            menuCursor = new MenuCursor(menuLogic); // create the start screen cursor
        }
        public void MenuSetup()
        {
            menuVisuals.DrawMainMenu(); // draw the main menu
            menuCursor.Reset(); // reset the cursor position

        }
        public void ExamineCursorAction(int cursorPos) // check what button the player pressed and change the menu stage accordingly
        {
            
            if (MenuStageTransitions.TryGetValue(cursorPos, out var changesForCertainPosition))
            {
                if (changesForCertainPosition.TryGetValue(menuLogic.CurrentMenuStage, out var newMenuStage))
                {
                    menuLogic.CurrentMenuStage = newMenuStage; // change the menu stage
                }
                else
                {
                    Utilities.Error("this buttons was not implemented yet. press enter to go back to home screen");
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu; 
                    
                }
            }
            else
            {
                Utilities.Error("invalid cursor position");
                menuCursor.cursorPos = 0; 
            }
        }
        public ButtonTypes SortToButtonTypes() // sort the button to the correct type
        {
            menuLogic.HandleBackButtons();
            switch (menuLogic.CurrentMenuStage)
            {
                // menu buttons
                case MenuStages.MainMenu:
                case MenuStages.MainMenu_Play:
                case MenuStages.MainMenu_Options:
                case MenuStages.Play_PlayerVsAI:
                    return ButtonTypes.MenuButton;

                // action menu buttons
                case MenuStages.MainMenu_HowToPlay:
                case MenuStages.MainMenu_Exit:
                case MenuStages.Options_engine:
                case MenuStages.Options_credits:
                    return ButtonTypes.Action_MenuButton;

                // action game buttons
                case MenuStages.Play_PlayerVsPlayer:
                case MenuStages.PlayerVsAI_Easy:
                case MenuStages.PlayerVsAI_Medium:
                case MenuStages.PlayerVsAI_Hard:
                    return ButtonTypes.Action_GameButton;

                // useless buttons
                case MenuStages.Play_ComingSoon: 
                case MenuStages.Options_controls:
                    return ButtonTypes.UselessButton;
                default:
                    Utilities.Error("invalid menu stage (SortToButtonTypes)");
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu; 
                    return ButtonTypes.UselessButton; 


            }
        }
        public void HandleMenuButtons() // draw according to the menu stage
        {
            switch (menuLogic.CurrentMenuStage)
            {
                case MenuStages.MainMenu:
                    menuVisuals.DrawMainMenu(); // draw the main menu
                    break;
                case MenuStages.MainMenu_Play:
                    menuVisuals.DrawPlayMenu(); // draw the play menu
                    break;
                case MenuStages.MainMenu_Options:
                    menuVisuals.DrawOptionsMenu(); // draw the options menu
                    break;
                case MenuStages.Play_PlayerVsAI:
                    menuVisuals.DrawPlayVsAIMenu(); // draw the play vs O_AI menu
                    break;
                // action menu buttons
                case MenuStages.MainMenu_HowToPlay:
                    menuVisuals.DrawHowToPlayMenu(); 
                    Utilities.GetValidInput();
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu; 
                    menuVisuals.DrawMainMenu(); 
                    break;
                case MenuStages.Options_credits:
                    menuVisuals.DrawCreditsMenu(); 
                    Utilities.GetValidInput();
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu_Options; 
                    menuVisuals.DrawOptionsMenu(); 
                    break;
                case MenuStages.MainMenu_Exit:
                    menuVisuals.DrawExitGameMenu(); 
                    System.Threading.Thread.Sleep(3500);
                    Environment.Exit(0);
                    break;
                case MenuStages.Options_engine:
                    if (EngineIsActive)
                    {
                        menuVisuals.DrawEngineInactiveText();
                    }
                    else
                    {
                        menuVisuals.DrawEngineActiveText();
                    }
                    EngineIsActive = !EngineIsActive;
                    Utilities.GetValidInput();
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu_Options;
                    menuVisuals.DrawOptionsMenu();
                    break;

                default:
                    Utilities.Error("this buttons was not implemented yet. press enter then space then enter to go back to home screen (dont ask why).   ( method: HandleMenuButtons)");
                    break;
            }
        }

        public ReturnTypes MenuLoop() // main menu loop. 
        {
            /*
             * this method is looped in GameManager until a Action_GameButton is pressed and works like this: 
             * move the cursor until an action is made
             * check what button the player pressed and change the menu stage accordingly
             * sort the button to the correct type
             *  if the button is an action game button (which means a game need to be started) check what play button was pressed and return the correct type to GameManager
             *  else determine what to do in HandleMenuButtons() and act accordingly
             */


            int cursorPos = menuCursor.MoveUntilAction(); 
            ExamineCursorAction(cursorPos);
            ButtonTypes buttonType = SortToButtonTypes();
            if (buttonType == ButtonTypes.Action_GameButton)
            {
                switch (menuLogic.CurrentMenuStage)
                {
                    case MenuStages.Play_PlayerVsPlayer:
                        menuLogic.CurrentMenuStage = MenuStages.MainMenu; 
                        return ReturnTypes.PlayerVsPlayer; 
                    case MenuStages.PlayerVsAI_Easy:
                        menuLogic.CurrentMenuStage = MenuStages.MainMenu;
                        return ReturnTypes.PlayerVsAI_Easy; 
                    case MenuStages.PlayerVsAI_Medium:
                        menuLogic.CurrentMenuStage = MenuStages.MainMenu;
                        return ReturnTypes.PlayerVsAI_Medium; 
                    case MenuStages.PlayerVsAI_Hard:
                        menuLogic.CurrentMenuStage = MenuStages.MainMenu;
                        return ReturnTypes.PlayerVsAI_Hard; 
                    default:
                        Utilities.Error("invalid menu stage (MenuManager.MenuLoop)");
                        menuLogic.CurrentMenuStage = MenuStages.MainMenu;
                        return ReturnTypes.MenuButton;
                }
            }
            else
            {
                HandleMenuButtons(); 
                if (buttonType == ButtonTypes.MenuButton)
                    menuCursor.Reset();
                else
                    menuCursor.Draw(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y);

                return ReturnTypes.MenuButton; 

            }


        }




    }
    public class MenuLogic
    {
        public MenuStages CurrentMenuStage = MenuStages.MainMenu; 
        public ButtonTypes CurrentButtonType = ButtonTypes.MenuButton;
        public int Buttons = 3; // number of Buttons in the menu. with 0 being the first one
        public const int SpaceBetweenButtons = 7; // space between the buttons in the menu


        public MenuCursorPosition[] CursorPositions;
        public int CursorPosX = Utilities.Width / 2 - Art.PVsAI[0].Length / 2 -10; // X position of the cursor

        public MenuLogic()
        {
            if (Utilities.graphicsMode == GraphicsMode.Normal)
            {
                CursorPositions = new MenuCursorPosition[4]
                {
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - 2), //first cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - 2 + SpaceBetweenButtons), //second cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - 2 + SpaceBetweenButtons * 2), //third cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - 2 + SpaceBetweenButtons * 3), //fourth cursor slot

                };
            }
            else
            {
                CursorPositions = new MenuCursorPosition[4]
                {
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - ((SpaceBetweenButtons * 3) / 2 + Art.Play.Length/2)), //first cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - ((SpaceBetweenButtons * 3) / 2 + Art.Play.Length/2) + SpaceBetweenButtons), //second cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - ((SpaceBetweenButtons * 3) / 2 + Art.Play.Length/2) + SpaceBetweenButtons * 2), //third cursor slot
                    new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - ((SpaceBetweenButtons * 3) / 2 + Art.Play.Length/2) + SpaceBetweenButtons * 3), //fourth cursor slot

                };
            }
        }



        public void HandleBackButtons()
        {
            switch (CurrentMenuStage)
            {
                case MenuStages.Play_Back:
                case MenuStages.Options_Back:
                    CurrentMenuStage = MenuStages.MainMenu; 
                    return;
                case MenuStages.PlayerVsAI_Back:
                    CurrentMenuStage = MenuStages.MainMenu_Play; 
                    return;
                default:
                    return;
            }
        }

    }
    public class MenuVisuals
    {
        MenuLogic menuLogic;
        public const int SpaceBetwwenButtonAndCursor = 10; // space between the button and the cursor

        public MenuVisuals(MenuLogic startScreenLogic)
        {
            menuLogic = startScreenLogic; // set the start screen logic
        }
        void DrawMenu(string[] art1, string[] art2, string[] art3, string[] art4)
        {
            Utilities.Setup();
            if (Utilities.graphicsMode == GraphicsMode.Normal)
            {
                Art.Draw(Art.TicTacToe, Art.CenterX(Art.TicTacToe), Art.CenterY(Art.TicTacToe) - Art.TicTacToe.Length);
            }
            Art.Draw(art1, menuLogic.CursorPositions[0].X + SpaceBetwwenButtonAndCursor, menuLogic.CursorPositions[0].Y); // draw the first button
            Art.Draw(art2, menuLogic.CursorPositions[1].X + SpaceBetwwenButtonAndCursor, menuLogic.CursorPositions[1].Y); // draw the second button
            Art.Draw(art3, menuLogic.CursorPositions[2].X + SpaceBetwwenButtonAndCursor, menuLogic.CursorPositions[2].Y); // draw the third button
            Art.Draw(art4, menuLogic.CursorPositions[3].X + SpaceBetwwenButtonAndCursor, menuLogic.CursorPositions[3].Y); // draw the fourth button

        }
        public void DrawMainMenu()
        {
            DrawMenu(Art.Play, Art.HowToPlay, Art.Options, Art.ExitGame); // draw the main menu
        }
        public void DrawPlayMenu()
        {
            DrawMenu(Art.PVP, Art.PVsAI, Art.ComingSoon, Art.Back); // draw the play menu
        }
        public void DrawOptionsMenu()
        {
            DrawMenu(Art.Engine, Art.Controls, Art.Credits, Art.Back); // draw the options menu
        }
        public void DrawPlayVsAIMenu()
        {
            DrawMenu(Art.Easy, Art.Medium, Art.Hard, Art.Back); // draw the play vs O_AI menu
        }
        public void DrawHowToPlayMenu()
        {
            Art.DrawTextFrame();
            Art.Draw(Art.HowToPlayContent, Art.Frame_CenterX - Art.HowToPlayContent[0].Length/2, Art.Frame_CenterY - Art.HowToPlayContent.Length/2);

        }
        public void DrawCreditsMenu()
        {
            Art.DrawTextFrame();
            Art.Draw(Art.CreditsContent, Art.Frame_CenterX - Art.CreditsContent[0].Length / 2, Art.Frame_CenterY - Art.CreditsContent.Length / 2); 
        }
        public void DrawExitGameMenu()
        {
            Art.DrawTextFrame();
            Art.Draw(Art.ExitGameContent, Art.Frame_CenterX - Art.ExitGameContent[0].Length / 2, Art.Frame_CenterY - Art.ExitGameContent.Length / 2); 
        }
        public void DrawEngineActiveText()
        {
            Art.DrawTextFrame(); 
            Art.Draw(Art.EngineOnContent, Art.Frame_CenterX - Art.EngineOnContent[0].Length / 2, Art.Frame_CenterY - Art.EngineOnContent.Length / 2); 
        }
        public void DrawEngineInactiveText()
        {
            Art.DrawTextFrame();
            Art.Draw(Art.EngineOffContent, Art.Frame_CenterX - Art.EngineOffContent[0].Length / 2, Art.Frame_CenterY - Art.EngineOffContent.Length / 2);
        }
    }
    public class MenuCursor
    {
        MenuLogic menuLogic;
        public int cursorPos = 0; // position of the cursor. 0-3



        public MenuCursor(MenuLogic menuLogic)
        {
            this.menuLogic = menuLogic; // set the start screen logic
        }
        public void Draw(int x, int y)
        {
            Art.Draw(Art.Cursor, x, y); // draw the cursor
        }
        public void Erase(int x, int y)
        {
            Art.Erase(Art.Cursor, x, y); // erase the cursor
        }
        public void Reset()
        {
            Erase(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y); // erase the cursor
            cursorPos = 0; // reset the cursor position
            Draw(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y); // draw the cursor
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
                        if (cursorPos != 0) //not equal to the top position
                        {
                            Erase(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y);
                            cursorPos--;
                            Draw(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y);
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (cursorPos != menuLogic.Buttons) //not equal to the bottom position
                        {
                            Erase(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y);
                            cursorPos++;
                            Draw(menuLogic.CursorPositions[cursorPos].X, menuLogic.CursorPositions[cursorPos].Y);
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.Spacebar:
                        return cursorPos; // return the action position
                }
            }
        }

    }
    public enum MenuStages
    /*
     * menu buttons: butons that change the menu. dont start any action (play, back, etc)
     * action buttons: buttons that start an action (how to play, PvP, exit gmae, etc)
     * useless buttons: buttons that dont do anything (coming soon)
     */
    {
        MainMenu,

        MainMenu_Play, // menu button
        MainMenu_HowToPlay, // action menu button
        MainMenu_Options, // menu button
        MainMenu_Exit, // action menu button

        Play_PlayerVsPlayer, // game action button
        Play_PlayerVsAI, // menu button
        Play_ComingSoon, // useless button (for now)
        Play_Back, // menu button

        Options_engine, // action menu button
        Options_controls, // useless button (for now)
        Options_credits, // action menu button
        Options_Back, // menu button


        PlayerVsAI_Easy, // action game button
        PlayerVsAI_Medium, // action gaem button
        PlayerVsAI_Hard, // action game button
        PlayerVsAI_Back // menu button


    }
    public enum ButtonTypes
    {
        MenuButton, // button that changes the menu
        Action_MenuButton, // button that starts an action in the menu (how to play, credits, etc)
        Action_GameButton, // button that starts an action in the game  that reqires starting a new game (PvP, PvAI_Easy, etc)
        UselessButton // button that does nothing (coming soon, controls)
    }
    public enum ReturnTypes //what Action_GameButton button was pressed
    {
        MenuButton, // no game action was made, just a menu button
        PlayerVsPlayer, 
        PlayerVsAI_Easy, 
        PlayerVsAI_Medium, 
        PlayerVsAI_Hard, 
    }



    public class MenuCursorPosition
    {
        public int X; // X position of the cursor
        public int Y; // Y position of the cursor
        public MenuCursorPosition(int x, int y)
        {
            X = x; // set the X position of the cursor
            Y = y; // set the Y position of the cursor
        }
    }
}
