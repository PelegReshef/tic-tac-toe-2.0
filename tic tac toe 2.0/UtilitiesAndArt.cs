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

        public static void RefreshWindow()
        {
            string request = ("to avoid visual glitches, please set your window size to fullscreen. press enter to continue");
            Console.SetCursorPosition(Width / 2 - request.Length / 2, Height / 2); //write the request in the middle of the screen
            Console.WriteLine(request);
            Console.ReadLine();
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;
            Console.BufferHeight = newHeight; // set the buffer height to the new height
            Console.BufferWidth = newWidth; // set the buffer width to the new width

            if (newWidth <= Width || newHeight <= Height)
            {
                Console.WriteLine("looks like you havnt changed your window size to fullscreen. if this is a mistake and you are in full screen press enter. otherwise, set your window size to fullscreen and press enter");
                Console.ReadLine();
                Console.Clear();
                Width = Console.WindowWidth;
                Height = Console.WindowHeight;
            }
            else
            {
                Width = Console.WindowWidth;
                Height = Console.WindowHeight;

            }


            Console.Clear(); // clear the console window
            Console.SetCursorPosition(0, 0); // set the cursor position to the top left corner

        }
        public static void Setup()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;


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
        }

    }
    public static class Art
    {
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
            Console.SetCursorPosition(0, 0);
        }
        public static string[] O = new string[]
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
        public static string[] X = new string[]
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

        public static string[] Empty = new string[]
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
        public static string[] NewBoard = new string[]
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
        public static string[] Play = new string[]
        {
            @" _____  _           __     __",
            @"|  __ \| |        /\\ \   / /",
            @"| |__) | |       /  \\ \_/ / ",
            @"|  ___/| |      / /\ \\   /  ",
            @"| |    | |____ / ____ \| |   ",
            @"|_|    |______/_/    \_\_|   "

        };
        public static string[] TicTacToe = new string[]{
            "TTTTTTTTTTTTTTTTTTTTTTTIIIIIIIIII      CCCCCCCCCCCCC               TTTTTTTTTTTTTTTTTTTTTTT         AAA                  CCCCCCCCCCCCC               TTTTTTTTTTTTTTTTTTTTTTT     OOOOOOOOO     EEEEEEEEEEEEEEEEEEEEEE",
            "T:::::::::::::::::::::TI::::::::I   CCC::::::::::::C               T:::::::::::::::::::::T        A:::A              CCC::::::::::::C               T:::::::::::::::::::::T   OO:::::::::OO   E::::::::::::::::::::E",
            "T:::::::::::::::::::::TI::::::::I CC:::::::::::::::C               T:::::::::::::::::::::T       A:::::A           CC:::::::::::::::C               T:::::::::::::::::::::T OO:::::::::::::OO E::::::::::::::::::::E",
            "T:::::TT:::::::TT:::::TII::::::IIC:::::CCCCCCCC::::C               T:::::TT:::::::TT:::::T      A:::::::A         C:::::CCCCCCCC::::C               T:::::TT:::::::TT:::::TO:::::::OOO:::::::OEE::::::EEEEEEEEE::::E",
            "TTTTTT  T:::::T  TTTTTT  I::::I C:::::C       CCCCCC               TTTTTT  T:::::T  TTTTTT     A:::::::::A       C:::::C       CCCCCC               TTTTTT  T:::::T  TTTTTTO::::::O   O::::::O  E:::::E       EEEEEE",
            "        T:::::T          I::::IC:::::C                                     T:::::T            A:::::A:::::A     C:::::C                                     T:::::T        O:::::O     O:::::O  E:::::E             ",
            "        T:::::T          I::::IC:::::C                                     T:::::T           A:::::A A:::::A    C:::::C                                     T:::::T        O:::::O     O:::::O  E::::::EEEEEEEEEE   ",
            "        T:::::T          I::::IC:::::C                                     T:::::T          A:::::A   A:::::A   C:::::C                                     T:::::T        O:::::O     O:::::O  E:::::::::::::::E   ",
            "        T:::::T          I::::IC:::::C                                     T:::::T         A:::::A     A:::::A  C:::::C                                     T:::::T        O:::::O     O:::::O  E:::::::::::::::E   ",
            "        T:::::T          I::::IC:::::C                                     T:::::T        A:::::AAAAAAAAA:::::A C:::::C                                     T:::::T        O:::::O     O:::::O  E::::::EEEEEEEEEE   ",
            "        T:::::T          I::::IC:::::C                                     T:::::T       A:::::::::::::::::::::AC:::::C                                     T:::::T        O:::::O     O:::::O  E:::::E             ",
            "        T:::::T          I::::I C:::::C       CCCCCC                       T:::::T      A:::::AAAAAAAAAAAAA:::::AC:::::C       CCCCCC                       T:::::T        O::::::O   O::::::O  E:::::E       EEEEEE",
            "      TT:::::::TT      II::::::IIC:::::CCCCCCCC::::C                     TT:::::::TT   A:::::A             A:::::AC:::::CCCCCCCC::::C                     TT:::::::TT      O:::::::OOO:::::::OEE::::::EEEEEEEE:::::E",
            "      T:::::::::T      I::::::::I CC:::::::::::::::C                     T:::::::::T  A:::::A               A:::::ACC:::::::::::::::C                     T:::::::::T       OO:::::::::::::OO E::::::::::::::::::::E",
            "      T:::::::::T      I::::::::I   CCC::::::::::::C                     T:::::::::T A:::::A                 A:::::A CCC::::::::::::C                     T:::::::::T         OO:::::::::OO   E::::::::::::::::::::E",
            "      TTTTTTTTTTT      IIIIIIIIII      CCCCCCCCCCCCC                     TTTTTTTTTTTAAAAAAA                   AAAAAAA   CCCCCCCCCCCCC                     TTTTTTTTTTT           OOOOOOOOO     EEEEEEEEEEEEEEEEEEEEEE"
    };
        public static string[] Options = new string[]
        {
            @"  ____   _____  _______  _____  ____   _   _   _____ ",
            @" / __ \ |  __ \|__   __||_   _|/ __ \ | \ | | / ____|",
            @"| |  | || |__) |  | |     | | | |  | ||  \| || (___  ",
            @"| |  | ||  ___/   | |     | | | |  | || . ` | \___ \ ",
            @"| |__| || |       | |    _| |_| |__| || |\  | ____) |",
            @" \____/ |_|       |_|   |_____|\____/ |_| \_||_____/ "
        };
        public static string[] Cursor = new string[]
        {
            @"__   ",
            @"\ \  ",
            @" \ \ ",
            @"  > >",
            @" / / ",
            @"/_/  ",

        };
        public static string[] HowToPlay = new string[]
        {
            @"  _    _  ______          __    _______ ____      _____  _           __     __",
            @" | |  | |/ __ \ \        / /   |__   __/ __ \    |  __ \| |        /\\ \   / /",
            @" | |__| | |  | \ \  /\  / /       | | | |  | |   | |__) | |       /  \\ \_/ / ",
            @" |  __  | |  | |\ \/  \/ /        | | | |  | |   |  ___/| |      / /\ \\   /  ",
            @" | |  | | |__| | \  /\  /         | | | |__| |   | |    | |____ / ____ \| |   ",
            @" |_|  |_|\____/   \/  \/          |_|  \____/    |_|    |______/_/    \_\_|   "
        };
        public static string[] ExitGame = new string[]
        {
            @"  ________   _______ _______      _____          __  __ ______ ",
            @" |  ____\ \ / /_   _|__   __|    / ____|   /\   |  \/  |  ____|",
            @" | |__   \ V /  | |    | |      | |  __   /  \  | \  / | |__   ",
            @" |  __|   > <   | |    | |      | | |_ | / /\ \ | |\/| |  __|  ",
            @" | |____ / . \ _| |_   | |      | |__| |/ ____ \| |  | | |____ ",
            @" |______/_/ \_\_____|  |_|       \_____/_/    \_\_|  |_|______|"
        };
        public static string[] PVP = new string[]
        {
            @"  _____  _           __     ________ _____     __      _______     _____  _           __     ________ _____  ",
            @" |  __ \| |        /\\ \   / /  ____|  __ \    \ \    / / ____|   |  __ \| |        /\\ \   / /  ____|  __ \ ",
            @" | |__) | |       /  \\ \_/ /| |__  | |__) |    \ \  / / (___     | |__) | |       /  \\ \_/ /| |__  | |__) |",
            @" |  ___/| |      / /\ \\   / |  __| |  _  /      \ \/ / \___ \    |  ___/| |      / /\ \\   / |  __| |  _  / ",
            @" | |    | |____ / ____ \| |  | |____| | \ \       \  /  ____) |   | |    | |____ / ____ \| |  | |____| | \ \ ",
            @" |_|    |______/_/    \_\_|  |______|_|  \_\       \/  |_____/    |_|    |______/_/    \_\_|  |______|_|  \_\"

        };
        public static string[] PVsAI = new string[]
        {
            @"  _____  _           __     ________ _____     __      _______              _____ ",
            @" |  __ \| |        /\\ \   / /  ____|  __ \    \ \    / / ____|       /\   |_   _|",
            @" | |__) | |       /  \\ \_/ /| |__  | |__) |    \ \  / / (___        /  \    | |  ",
            @" |  ___/| |      / /\ \\   / |  __| |  _  /      \ \/ / \___ \      / /\ \   | |  ",
            @" | |    | |____ / ____ \| |  | |____| | \ \       \  /  ____) |    / ____ \ _| |_ ",
            @" |_|    |______/_/    \_\_|  |______|_|  \_\       \/  |_____/    /_/    \_\_____|"
        };
        public static string[] ComingSoon = new string[]
        {
            @"   _____ ____  __  __ _____ _   _  _____      _____  ____   ____  _   _ ",
            @"  / ____/ __ \|  \/  |_   _| \ | |/ ____|    / ____|/ __ \ / __ \| \ | |",
            @" | |   | |  | | \  / | | | |  \| | |  __    | (___ | |  | | |  | |  \| |",
            @" | |   | |  | | |\/| | | | | . ` | | |_ |    \___ \| |  | | |  | | . ` |",
            @" | |___| |__| | |  | |_| |_| |\  | |__| |    ____) | |__| | |__| | |\  |",
            @"  \_____\____/|_|  |_|_____|_| \_|\_____|   |_____/ \____/ \____/|_| \_|"
        };
        public static string[] Back = new string[]
        {
            @"  ____          _____ _  __",
            @" |  _ \   /\   / ____| |/ /",
            @" | |_) | /  \ | |    | ' / ",
            @" |  _ < / /\ \| |    |  <  ",
            @" | |_) / ____ \ |____| . \ ",
            @" |____/_/    \_\_____|_|\_\"
        };
        public static string[] Engine = new string[]
        {
            @"  ______ _   _  _____ _____ _   _ ______ ",
            @" |  ____| \ | |/ ____|_   _| \ | |  ____|",
            @" | |__  |  \| | |  __  | | |  \| | |__   ",
            @" |  __| | . ` | | |_ | | | | . ` |  __|  ",
            @" | |____| |\  | |__| |_| |_| |\  | |____ ",
            @" |______|_| \_|\_____|_____|_| \_|______|"
        };
        public static string[] Controls = new string[]
        {
            @"   _____ ____  _   _ _______ _____   ____  _       _____ ",
            @"  / ____/ __ \| \ | |__   __|  __ \ / __ \| |     / ____|",
            @" | |   | |  | |  \| |  | |  | |__) | |  | | |    | (___  ",
            @" | |   | |  | | . ` |  | |  |  _  /| |  | | |     \___ \ ",
            @" | |___| |__| | |\  |  | |  | | \ \| |__| | |____ ____) |",
            @"  \_____\____/|_| \_|  |_|  |_|  \_\\____/|______|_____/ "
        };
        public static string[] Credits = new string[]
        {
            @"   _____ _____  ______ _____ _____ _______ _____ ",
            @"  / ____|  __ \|  ____|  __ \_   _|__   __/ ____|",
            @" | |    | |__) | |__  | |  | || |    | | | (___  ",
            @" | |    |  _  /|  __| | |  | || |    | |  \___ \ ",
            @" | |____| | \ \| |____| |__| || |_   | |  ____) |",
            @"  \_____|_|  \_\______|_____/_____|  |_| |_____/ "
        };
        public static string[] Easy = new string[]
        {
            @"  ______           _______     __",
            @" |  ____|   /\    / ____\ \   / /",
            @" | |__     /  \  | (___  \ \_/ / ",
            @" |  __|   / /\ \  \___ \  \   /  ",
            @" | |____ / ____ \ ____) |  | |   ",
            @" |______/_/    \_\_____/   |_|   "
        };
        public static string[] Medium = new string[]
        {
            @"  __  __ ______ _____ _____ _    _ __  __ ",
            @" |  \/  |  ____|  __ \_   _| |  | |  \/  |",
            @" | \  / | |__  | |  | || | | |  | | \  / |",
            @" | |\/| |  __| | |  | || | | |  | | |\/| |",
            @" | |  | | |____| |__| || |_| |__| | |  | |",
            @" |_|  |_|______|_____/_____|\____/|_|  |_|",
        };
        public static string[] Hard = new string[]
        {
            @"  _    _          _____  _____  ",
            @" | |  | |   /\   |  __ \|  __ \ ",
            @" | |__| |  /  \  | |__) | |  | |",
            @" |  __  | / /\ \ |  _  /| |  | |",
            @" | |  | |/ ____ \| | \ \| |__| |",
            @" |_|  |_/_/    \_\_|  \_\_____/ "
        };

    }
}
