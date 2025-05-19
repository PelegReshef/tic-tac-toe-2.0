using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticTacToe;

namespace ticTacToe
{
    public class MenuManager
    {
        MenuLogic menuLogic; // logic for the start screen
        MenuVisuals menuVisuals; // visuals for the start screen
        public MenuCursor menuCursor; // cursor for the start screen
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
            menuCursor.MoveUntilAction(); // move the cursor until an action is made

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
                    Utilities.Error("no menu stage found for the current menu stage and cursor position");
                    menuLogic.CurrentMenuStage = MenuStages.MainMenu; 
                    Console.ReadLine();
                }
            }
            else
            {
                Utilities.Error("invalid cursor position");
                menuCursor.cursorPos = 0; 
                Console.ReadLine();
            }
        }
        public void MenuAction() //draw menu stage according to the current menu stage
        {
            menuLogic.HandleBackButtons();
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
        public void MenuLoop() // main menu loop
        {
            
            int cursorPos = menuCursor.MoveUntilAction(); // move the cursor until an action is made
            ExamineCursorAction(cursorPos); // check what button the player pressed and change the menu stage accordinglyג
            MenuAction(); // draw the menu stage according to the current menu stage
            menuCursor.Reset(); // reset the cursor position

        }




    }
    public class MenuLogic
    {
        public MenuStages CurrentMenuStage = MenuStages.MainMenu; // start at the first menu
        public int Buttons = 3; // number of Buttons in the menu. with 0 being the first one


        public MenuCursorPosition[] CursorPositions;
        public int CursorPosX = Utilities.Width / 2 - Art.HowToPlay.Length / 2 - 40; // X position of the cursor

        public MenuLogic()
        {
            CursorPositions = new MenuCursorPosition[4]
            {
                new MenuCursorPosition(CursorPosX, Utilities.Height / 2 - 2), //first cursor slot
                new MenuCursorPosition(CursorPosX, Utilities.Height / 2 + 5), //second cursor slot
                new MenuCursorPosition(CursorPosX, Utilities.Height / 2 + 12), //third cursor slot
                new MenuCursorPosition(CursorPosX, Utilities.Height / 2 + 19), //fourth cursor slot

            };
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
            this.menuLogic = startScreenLogic; // set the start screen logic
        }
        void DrawMenu(string[] art1, string[] art2, string[] art3, string[] art4)
        {
            Utilities.Setup();
            Art.Draw(Art.TicTacToe, Art.CenterX(Art.TicTacToe), Art.CenterY(Art.TicTacToe) - Art.TicTacToe.Length);
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
            DrawMenu(Art.Easy, Art.Medium, Art.Hard, Art.Back); // draw the play vs AI menu
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
    {
        MainMenu,

        MainMenu_Play, // open enum SecondMenu_Play
        MainMenu_HowToPlay, // dont need to open another enum. get you to the how to play screen immediately
        MainMenu_Options, // open enum SecondMenu_Options
        MainMenu_Exit, // exit the game

        Play_PlayerVsPlayer, // start a new game with 2 players.no enum
        Play_PlayerVsAI, // open enum ThirdMenu_PlayerVsAI
        Play_ComingSoon, // will make it Custom Game later
        Play_Back, // go back to the first menu

        Options_engine,
        Options_controls,
        Options_credits,
        Options_Back, // go back to the first menu


        PlayerVsAI_Easy,
        PlayerVsAI_Medium,
        PlayerVsAI_Hard,
        PlayerVsAI_Back // go back to the play menu

        
    }



    public class MenuCursorPosition
    {
        public int X; // X position of the cursor
        public int Y; // Y position of the cursor
        public MenuCursorPosition(int x, int y)
        {
            this.X = x; // set the X position of the cursor
            this.Y = y; // set the Y position of the cursor
        }
    }
}
