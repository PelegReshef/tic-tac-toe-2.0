using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticTacToe
{
    public static class Utilities
    {
        public static int Width = Console.WindowWidth;
        public static int Height = Console.WindowHeight;
        public static GraphicsMode graphicsMode = GraphicsMode.Normal; 

        public static void RefreshWindow()
        {
            string request = ("to avoid visual glitches, please set your window size to fullscreen. press spacebar to continue");
            Console.SetCursorPosition(Width / 2 - request.Length / 2, Height / 2); //write the request in the middle of the screen
            Console.WriteLine(request);
            GetValidInput();
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;

            if (newWidth <= Art.TicTacToe[0].Length || newHeight <= Art.TicTacToe.Length * 1.5 + MenuLogic.SpaceBetweenButtons * 3 + Art.Play.Length)
            {
                Console.WriteLine("looks like your window size is too small. switching to smaller graphics mode. press spacebar to continue");
                GetValidInput();
                graphicsMode = GraphicsMode.Small; 
            }
            Width = newWidth;
            Height = newHeight;
            Console.BufferHeight = newHeight; // set the buffer height to the new height
            Console.BufferWidth = newWidth; // set the buffer width to the new width



            Console.Clear(); // clear the console window
            Console.SetCursorPosition(0, 0); // set the cursor position to the top left corner

        }
        public static void Setup()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;


        }
        public static void UpdateFrameVariables()
        {
            Art.Frame_Top = Height / 2 - Height / 4;
            Art.Frame_Left = Width / 2 - Width / 4;
            Art.Frame_Bottom = Height / 2 + Height / 4;
            Art.Frame_Right = Width / 2 + Width / 4;
            Art.Frame_CenterY = Height / 2;
            Art.Frame_CenterX = Width / 2;
            Art.FrameLenth_X = Width / 2;
            Art.FrameLength_Y = Height / 2;
        }
        public static ConsoleKeyInfo GetValidInput()
        {
            while (true)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                switch (keyPressed.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.A:
                    case ConsoleKey.S:
                    case ConsoleKey.D:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Spacebar:
                        return keyPressed;
                    default:

                        continue; // ignore any other key

                }
            }

        }
        public static void Error(string message)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(message);
            Console.ReadLine(); 
        }

    }
    public enum GraphicsMode
    {
        Normal,
        Small
    }
    public static class Art
    {
        public static int Frame_Left;
        public static int Frame_Right;
        public static int Frame_Top;
        public static int Frame_Bottom;
        public static int Frame_CenterY;
        public static int Frame_CenterX;
        public static int FrameLenth_X;
        public static int FrameLength_Y;

        public static void Draw(string[] pixelArt, int x, int y)
        {
            for (int i = 0; i < pixelArt.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(pixelArt[i]);
            }
        }
        public static void Erase(string[] pixelArt, int x, int y)
        {
            for (int i = 0; i < pixelArt.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                for (int j = 0; j < pixelArt[i].Length; j++)
                {
                    Console.SetCursorPosition(x + j, y + i);
                    Console.Write(' '); // erase the pixel art by writing spaces
                }
            }
        }
        public static int CenterX(string[] pixelArt)
        {
            return (Utilities.Width / 2) - (pixelArt[0].Length / 2);

        }
        public static int CenterY(string[] pixelArt)
        {
            return (Utilities.Height / 2) - (pixelArt.Length / 2);

        }
        public static void DrawTextFrame() 
        {
            // draw the frame
            Console.SetCursorPosition(Frame_Left, Frame_Top); // set the cursor position to the top left corner of the frame
            for (int i = 0; i < FrameLenth_X; i++)
            {
                Console.Write("#");
            }
            for (int i = 0; i < FrameLength_Y; i++)
            {
                Console.SetCursorPosition(Frame_Left, Frame_Top + i);
                Console.Write("#");
            }
            Console.SetCursorPosition(Frame_Left, Frame_Bottom); // set the cursor position to the bottom left corner of the frame
            for (int i = 0; i < FrameLenth_X; i++)
            {
                Console.Write("#");
            }
            Console.SetCursorPosition(Frame_Right, Frame_Top); // set the cursor position to the bottom left corner of the frame
            for (int i = 0; i < FrameLength_Y; i++)
            {
                Console.SetCursorPosition(Frame_Right, Frame_Top + i);
                Console.Write("#");
            }
            // erase the inside of the frame
            for (int i = 1; i < FrameLength_Y -1; i++)
            {
                Console.SetCursorPosition(Frame_Left + 1, Frame_Top + i);
                for (int j = 1; j < FrameLenth_X ; j++)
                {
                    Console.Write(" ");
                }
            }

        }
        public static string[] O =
        {

            "██████████████████",
            "██              ██",
            "██              ██",
            "██              ██",
            "██    ██████    ██",
            "██  ██      ██  ██",
            "██  ██      ██  ██",
            "██  ██      ██  ██",
            "██    ██████    ██",
            "██              ██",
            "██████████████████",

        };
        public static string[] X =
        {

            "██████████████████",
            "██              ██",
            "██              ██",
            "██              ██",
            "██  ██      ██  ██",
            "██    ██  ██    ██",
            "██      ██      ██",
            "██    ██  ██    ██",
            "██  ██      ██  ██",
            "██              ██",
            "██████████████████",
        };

        public static string[] Empty =
        {
            "██████████████████",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██              ██",
            "██████████████████",
        };
        public static string[] NewBoard =
        {
            "██████████████████████████████████████████████████",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██", //cell 1, cell 2, cell 3
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██████████████████████████████████████████████████",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██", //cell 4, cell 5, cell 6
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██████████████████████████████████████████████████",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██", //cell 7, cell 8, cell 9
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██              ██              ██              ██",
            "██████████████████████████████████████████████████",


        };
        public static string[] Play =
        {
            @" _____  _           __     __",
            @"|  __ \| |        /\\ \   / /",
            @"| |__) | |       /  \\ \_/ / ",
            @"|  ___/| |      / /\ \\   /  ",
            @"| |    | |____ / ____ \| |   ",
            @"|_|    |______/_/    \_\_|   "

        };
        public static string[] TicTacToe =
        {
            "TTTTTTTTTTTTTTTTTTTTTTTIIIIIIIIII      CCCCCCCCCCCCC         TTTTTTTTTTTTTTTTTTTTTTT         AAA                  CCCCCCCCCCCCC         TTTTTTTTTTTTTTTTTTTTTTT     OOOOOOOOO     EEEEEEEEEEEEEEEEEEEEEE",
            "T:::::::::::::::::::::TI::::::::I   CCC::::::::::::C         T:::::::::::::::::::::T        A:::A              CCC::::::::::::C         T:::::::::::::::::::::T   OO:::::::::OO   E::::::::::::::::::::E",
            "T:::::::::::::::::::::TI::::::::I CC:::::::::::::::C         T:::::::::::::::::::::T       A:::::A           CC:::::::::::::::C         T:::::::::::::::::::::T OO:::::::::::::OO E::::::::::::::::::::E",
            "T:::::TT:::::::TT:::::TII::::::IIC:::::CCCCCCCC::::C         T:::::TT:::::::TT:::::T      A:::::::A         C:::::CCCCCCCC::::C         T:::::TT:::::::TT:::::TO:::::::OOO:::::::OEE::::::EEEEEEEEE::::E",
            "TTTTTT  T:::::T  TTTTTT  I::::I C:::::C       CCCCCC         TTTTTT  T:::::T  TTTTTT     A:::::::::A       C:::::C       CCCCCC         TTTTTT  T:::::T  TTTTTTO::::::O   O::::::O  E:::::E       EEEEEE",
            "        T:::::T          I::::IC:::::C                               T:::::T            A:::::A:::::A     C:::::C                               T:::::T        O:::::O     O:::::O  E:::::E             ",
            "        T:::::T          I::::IC:::::C                               T:::::T           A:::::A A:::::A    C:::::C                               T:::::T        O:::::O     O:::::O  E::::::EEEEEEEEEE   ",
            "        T:::::T          I::::IC:::::C                               T:::::T          A:::::A   A:::::A   C:::::C                               T:::::T        O:::::O     O:::::O  E:::::::::::::::E   ",
            "        T:::::T          I::::IC:::::C                               T:::::T         A:::::A     A:::::A  C:::::C                               T:::::T        O:::::O     O:::::O  E:::::::::::::::E   ",
            "        T:::::T          I::::IC:::::C                               T:::::T        A:::::AAAAAAAAA:::::A C:::::C                               T:::::T        O:::::O     O:::::O  E::::::EEEEEEEEEE   ",
            "        T:::::T          I::::IC:::::C                               T:::::T       A:::::::::::::::::::::AC:::::C                               T:::::T        O:::::O     O:::::O  E:::::E             ",
            "        T:::::T          I::::I C:::::C       CCCCCC                 T:::::T      A:::::AAAAAAAAAAAAA:::::AC:::::C       CCCCCC                 T:::::T        O::::::O   O::::::O  E:::::E       EEEEEE",
            "      TT:::::::TT      II::::::IIC:::::CCCCCCCC::::C               TT:::::::TT   A:::::A             A:::::AC:::::CCCCCCCC::::C               TT:::::::TT      O:::::::OOO:::::::OEE::::::EEEEEEEE:::::E",
            "      T:::::::::T      I::::::::I CC:::::::::::::::C               T:::::::::T  A:::::A               A:::::ACC:::::::::::::::C               T:::::::::T       OO:::::::::::::OO E::::::::::::::::::::E",
            "      T:::::::::T      I::::::::I   CCC::::::::::::C               T:::::::::T A:::::A                 A:::::A CCC::::::::::::C               T:::::::::T         OO:::::::::OO   E::::::::::::::::::::E",
            "      TTTTTTTTTTT      IIIIIIIIII      CCCCCCCCCCCCC               TTTTTTTTTTTAAAAAAA                   AAAAAAA   CCCCCCCCCCCCC               TTTTTTTTTTT           OOOOOOOOO     EEEEEEEEEEEEEEEEEEEEEE"
        };
        public static string[] Options =
        {
            @"  ____   _____  _______  _____  ____   _   _   _____ ",
            @" / __ \ |  __ \|__   __||_   _|/ __ \ | \ | | / ____|",
            @"| |  | || |__) |  | |     | | | |  | ||  \| || (___  ",
            @"| |  | ||  ___/   | |     | | | |  | || . ` | \___ \ ",
            @"| |__| || |       | |    _| |_| |__| || |\  | ____) |",
            @" \____/ |_|       |_|   |_____|\____/ |_| \_||_____/ "
        };
        public static string[] Cursor =
        {
            @"__   ",
            @"\ \  ",
            @" \ \ ",
            @"  > >",
            @" / / ",
            @"/_/  ",

        };
        public static string[] HowToPlay =
        {
            @"  _    _  ______          __    _______ ____      _____  _           __     __",
            @" | |  | |/ __ \ \        / /   |__   __/ __ \    |  __ \| |        /\\ \   / /",
            @" | |__| | |  | \ \  /\  / /       | | | |  | |   | |__) | |       /  \\ \_/ / ",
            @" |  __  | |  | |\ \/  \/ /        | | | |  | |   |  ___/| |      / /\ \\   /  ",
            @" | |  | | |__| | \  /\  /         | | | |__| |   | |    | |____ / ____ \| |   ",
            @" |_|  |_|\____/   \/  \/          |_|  \____/    |_|    |______/_/    \_\_|   "
        };
        public static string[] ExitGame =
        {
            @"  ________   _______ _______      _____          __  __ ______ ",
            @" |  ____\ \ / /_   _|__   __|    / ____|   /\   |  \/  |  ____|",
            @" | |__   \ V /  | |    | |      | |  __   /  \  | \  / | |__   ",
            @" |  __|   > <   | |    | |      | | |_ | / /\ \ | |\/| |  __|  ",
            @" | |____ / . \ _| |_   | |      | |__| |/ ____ \| |  | | |____ ",
            @" |______/_/ \_\_____|  |_|       \_____/_/    \_\_|  |_|______|"
        };
        public static string[] PVP =
        {
            @"  _____  _           __     ________ _____     __      _______     _____  _           __     ________ _____  ",
            @" |  __ \| |        /\\ \   / /  ____|  __ \    \ \    / / ____|   |  __ \| |        /\\ \   / /  ____|  __ \ ",
            @" | |__) | |       /  \\ \_/ /| |__  | |__) |    \ \  / / (___     | |__) | |       /  \\ \_/ /| |__  | |__) |",
            @" |  ___/| |      / /\ \\   / |  __| |  _  /      \ \/ / \___ \    |  ___/| |      / /\ \\   / |  __| |  _  / ",
            @" | |    | |____ / ____ \| |  | |____| | \ \       \  /  ____) |   | |    | |____ / ____ \| |  | |____| | \ \ ",
            @" |_|    |______/_/    \_\_|  |______|_|  \_\       \/  |_____/    |_|    |______/_/    \_\_|  |______|_|  \_\"

        };
        public static string[] PVsAI =
        {
            @"  _____  _           __     ________ _____     __      _______      ____   ____ _______ ",
            @" |  __ \| |        /\\ \   / /  ____|  __ \    \ \    / / ____|    |  _ \ / __ \__   __|",
            @" | |__) | |       /  \\ \_/ /| |__  | |__) |    \ \  / / (___      | |_) | |  | | | |   ",
            @" |  ___/| |      / /\ \\   / |  __| |  _  /      \ \/ / \___ \     |  _ <| |  | | | |   ",
            @" | |    | |____ / ____ \| |  | |____| | \ \       \  /  ____) |    | |_) | |__| | | |   ",
            @" |_|    |______/_/    \_\_|  |______|_|  \_\       \/  |_____/     |____/ \____/  |_|   "
        };
        public static string[] ComingSoon =
        {
            @"   _____ ____  __  __ _____ _   _  _____      _____  ____   ____  _   _ ",
            @"  / ____/ __ \|  \/  |_   _| \ | |/ ____|    / ____|/ __ \ / __ \| \ | |",
            @" | |   | |  | | \  / | | | |  \| | |  __    | (___ | |  | | |  | |  \| |",
            @" | |   | |  | | |\/| | | | | . ` | | |_ |    \___ \| |  | | |  | | . ` |",
            @" | |___| |__| | |  | |_| |_| |\  | |__| |    ____) | |__| | |__| | |\  |",
            @"  \_____\____/|_|  |_|_____|_| \_|\_____|   |_____/ \____/ \____/|_| \_|"
        };
        public static string[] Back =
        {
            @"  ____          _____ _  __",
            @" |  _ \   /\   / ____| |/ /",
            @" | |_) | /  \ | |    | ' / ",
            @" |  _ < / /\ \| |    |  <  ",
            @" | |_) / ____ \ |____| . \ ",
            @" |____/_/    \_\_____|_|\_\"
        };
        public static string[] Engine =
        {
            @"  ______ _   _  _____ _____ _   _ ______ ",
            @" |  ____| \ | |/ ____|_   _| \ | |  ____|",
            @" | |__  |  \| | |  __  | | |  \| | |__   ",
            @" |  __| | . ` | | |_ | | | | . ` |  __|  ",
            @" | |____| |\  | |__| |_| |_| |\  | |____ ",
            @" |______|_| \_|\_____|_____|_| \_|______|"
        };
        public static string[] Controls =
        {
            @"   _____ ____  _   _ _______ _____   ____  _       _____ ",
            @"  / ____/ __ \| \ | |__   __|  __ \ / __ \| |     / ____|",
            @" | |   | |  | |  \| |  | |  | |__) | |  | | |    | (___  ",
            @" | |   | |  | | . ` |  | |  |  _  /| |  | | |     \___ \ ",
            @" | |___| |__| | |\  |  | |  | | \ \| |__| | |____ ____) |",
            @"  \_____\____/|_| \_|  |_|  |_|  \_\\____/|______|_____/ "
        };
        public static string[] Credits =
        {
            @"   _____ _____  ______ _____ _____ _______ _____ ",
            @"  / ____|  __ \|  ____|  __ \_   _|__   __/ ____|",
            @" | |    | |__) | |__  | |  | || |    | | | (___  ",
            @" | |    |  _  /|  __| | |  | || |    | |  \___ \ ",
            @" | |____| | \ \| |____| |__| || |_   | |  ____) |",
            @"  \_____|_|  \_\______|_____/_____|  |_| |_____/ "
        };
        public static string[] Easy =
        {
            @"  ______           _______     __",
            @" |  ____|   /\    / ____\ \   / /",
            @" | |__     /  \  | (___  \ \_/ / ",
            @" |  __|   / /\ \  \___ \  \   /  ",
            @" | |____ / ____ \ ____) |  | |   ",
            @" |______/_/    \_\_____/   |_|   "
        };
        public static string[] Medium =
        {
            @"  __  __ ______ _____ _____ _    _ __  __ ",
            @" |  \/  |  ____|  __ \_   _| |  | |  \/  |",
            @" | \  / | |__  | |  | || | | |  | | \  / |",
            @" | |\/| |  __| | |  | || | | |  | | |\/| |",
            @" | |  | | |____| |__| || |_| |__| | |  | |",
            @" |_|  |_|______|_____/_____|\____/|_|  |_|",
        };
        public static string[] Hard =
        {
            @"  _    _          _____  _____  ",
            @" | |  | |   /\   |  __ \|  __ \ ",
            @" | |__| |  /  \  | |__) | |  | |",
            @" |  __  | / /\ \ |  _  /| |  | |",
            @" | |  | |/ ____ \| | \ \| |__| |",
            @" |_|  |_/_/    \_\_|  \_\_____/ "
        };
        public static string[] WinMessageX = 
        {
            @"  __   __   __          _______ _   _  _____ _ ",
            @"  \ \ / /   \ \        / /_   _| \ | |/ ____| |",
            @"   \ V /     \ \  /\  / /  | | |  \| | (___ | |",
            @"    > <       \ \/  \/ /   | | | . ` |\___ \| |",
            @"   / . \       \  /\  /   _| |_| |\  |____) |_|",
            @"  /_/ \_\       \/  \/   |_____|_| \_|_____/(_)"
        };
        public static string[] WinMessageO =
        {
            @"    ____     __          _______ _   _  _____ _ ",
            @"   / __ \    \ \        / /_   _| \ | |/ ____| |",
            @"  | |  | |    \ \  /\  / /  | | |  \| | (___ | |",
            @"  | |  | |     \ \/  \/ /   | | | . ` |\___ \| |",
            @"  | |__| |      \  /\  /   _| |_| |\  |____) |_|",
            @"   \____/        \/  \/   |_____|_| \_|_____/(_)"
        };
        public static string[] WinMessageBot =
        {
            @"   ____   ____ _______    __          _______ _   _  _____ _ ",
            @"  |  _ \ / __ \__   __|   \ \        / /_   _| \ | |/ ____| |",
            @"  | |_) | |  | | | |       \ \  /\  / /  | | |  \| | (___ | |",
            @"  |  _ <| |  | | | |        \ \/  \/ /   | | | . ` |\___ \| |",
            @"  | |_) | |__| | | |         \  /\  /   _| |_| |\  |____) |_|",
            @"  |____/ \____/  |_|          \/  \/   |_____|_| \_|_____/(_)"
        };
        public static string[] WinMessageDraw =
        {
            @"  _____  _____       __          ___ ",
            @" |  __ \|  __ \     /\ \        / / |",
            @" | |  | | |__) |   /  \ \  /\  / /| |",
            @" | |  | |  _  /   / /\ \ \/  \/ / | |",
            @" | |__| | | \ \  / ____ \  /\  /  |_|",
            @" |_____/|_|  \_\/_/    \_\/  \/   (_)"
        };
        public static string[] HowToPlayContent =
        {
            "Tic Tac Toe is a two-player game played on a 3×3 grid.        ",
            "",
            "The players take turns marking a square with either X or O.   ",
            "",
            "The first player to get three of their marks in a             ",
            "",
            "row - horizontally, vertically, or diagonally wins the game.  ",
            "",
            "If all squares are filled and no one wins, its a draw.        "
            
        };
        public static string[] CreditsContent = new string[]
        {
            "Cretaed by: Peleg Reshef                           ",
            "",
            "Thanks to:",
            "",
            "* patorjk.com — for the ASCII Art used in the game",
            "",
            "* GitHub - for backups and sharing my code        ",
            "",
            "* My mom - for design reccomendations and ",
            " emotional support",
            "",
            "",
            "Project repository: github.com/PelegReshef/tic-tac-toe-2.0",
        };
        public static string[] ExitGameContent = new string[]
        {
            "Exiting game...   ",
            "",
            "Hope you liked it!",
            ""
        };
    }
}
