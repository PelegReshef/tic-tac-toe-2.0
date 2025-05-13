using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticTacToe;

namespace ticTacToe
{
    public class StartScreenLogic
    {
        MenuStages menuStage = MenuStages.MainMenu; // start at the first menu


        public StartScreenCursorPos[] CursorPositions;
        public int CursorPosX = Utilities.Width / 2 - Art.HowToPlay.Length / 2 - 40; // X position of the cursor

        public StartScreenLogic()
        {
            CursorPositions = new StartScreenCursorPos[4]
            {
        new StartScreenCursorPos(CursorPosX, Utilities.Height / 2 - 2), //first cursor slot
        new StartScreenCursorPos(CursorPosX, Utilities.Height / 2 + 5), //second cursor slot
        new StartScreenCursorPos(CursorPosX, Utilities.Height / 2 + 12), //third cursor slot
        new StartScreenCursorPos(CursorPosX, Utilities.Height / 2 + 19), //fourth cursor slot
            };
        }



        public MenuStages WhatScreenToDraw()
        {
            switch (menuStage)
            {
                case MenuStages.MainMenu: //main menu
                    return MenuStages.MainMenu;

                // second menu
                case MenuStages.MainMenu_Play:
                    return MenuStages.MainMenu_Play;
                case MenuStages.MainMenu_HowToPlay:
                    return MenuStages.MainMenu_HowToPlay;
                case MenuStages.MainMenu_Options:
                    return MenuStages.MainMenu_Options;
                case MenuStages.MainMenu_Exit:
                    return MenuStages.MainMenu_Exit;
                // third menu. play
                case MenuStages.Play_PlayerVsPlayer:
                    return MenuStages.Play_PlayerVsPlayer;
                case MenuStages.Play_PlayerVsAI:
                    return MenuStages.Play_PlayerVsAI;
                case MenuStages.Play_ComingSoon:
                    return MenuStages.Play_ComingSoon;
                case MenuStages.Play_Back:
                    menuStage = MenuStages.MainMenu; // go back to the first menu
                    return MenuStages.MainMenu;
                // third menu. options
                case MenuStages.Options_engine:
                    return MenuStages.Options_engine;
                case MenuStages.Options_controls:
                    return MenuStages.Options_controls;
                case MenuStages.Options_credits:
                    return MenuStages.Options_credits;
                case MenuStages.Options_Back:
                    menuStage = MenuStages.MainMenu; // go back to the first menu
                    return MenuStages.MainMenu;
                // fourth menu. player vs player
                case MenuStages.PlayerVsPlayer_Easy:
                    return MenuStages.PlayerVsPlayer_Easy;
                case MenuStages.PlayerVsPlayer_Medium:
                    return MenuStages.PlayerVsPlayer_Medium;
                case MenuStages.PlayerVsPlayer_Hard:
                    return MenuStages.PlayerVsPlayer_Hard;
                case MenuStages.PlayerVsPlayer_Back:
                    menuStage = MenuStages.MainMenu_Play; // go back to the play menu
                    return MenuStages.MainMenu_Play;
                // error screen
                default:
                    return MenuStages.error; // return error





            }
        }

    }
    public class StartScreenVisuals
    {
        StartScreenLogic startScreenLogic;

        public StartScreenVisuals(StartScreenLogic startScreenLogic)
        {
            this.startScreenLogic = startScreenLogic; // set the start screen logic
        }
        public void DrawMainMenu()
        {
            Utilities.Setup();
            Art.Draw(Art.TicTacToe, Art.CenterX(Art.TicTacToe), Art.CenterY(Art.TicTacToe) - Art.TicTacToe.Length); // draw the tictactoe title
            Art.Draw(Art.Play, startScreenLogic.CursorPositions[0].X + 10, startScreenLogic.CursorPositions[0].Y); // draw the play title
            Art.Draw(Art.HowToPlay, startScreenLogic.CursorPositions[1].X + 10, startScreenLogic.CursorPositions[1].Y); // draw the how to play title
            Art.Draw(Art.Options, startScreenLogic.CursorPositions[2].X + 10, startScreenLogic.CursorPositions[2].Y); // draw the options title
            Art.Draw(Art.ExitGame, startScreenLogic.CursorPositions[3].X + 10, startScreenLogic.CursorPositions[3].Y); // draw the exit title

        }
    }
    public class StartScreenCursor
    {
        StartScreenLogic startScreenLogic;
        public int cursorPos = 0; // position of the cursor. 0-3



        public StartScreenCursor(StartScreenLogic startScreenLogic)
        {
            this.startScreenLogic = startScreenLogic; // set the start screen logic
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
            Erase(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y); // erase the cursor
            cursorPos = 0; // reset the cursor position
            Draw(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y); // draw the cursor
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
                            Erase(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y);
                            cursorPos--;
                            Draw(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y);
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (cursorPos != 3) //not equal to the bottom position
                        {
                            Erase(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y);
                            cursorPos++;
                            Draw(startScreenLogic.CursorPositions[cursorPos].X, startScreenLogic.CursorPositions[cursorPos].Y);
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


        PlayerVsPlayer_Easy,
        PlayerVsPlayer_Medium,
        PlayerVsPlayer_Hard,
        PlayerVsPlayer_Back, // go back to the play menu

        error, // error screen
    }



    public class StartScreenCursorPos
    {
        public int X; // X position of the cursor
        public int Y; // Y position of the cursor
        public StartScreenCursorPos(int x, int y)
        {
            this.X = x; // set the X position of the cursor
            this.Y = y; // set the Y position of the cursor
        }
    }
}
