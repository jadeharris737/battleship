using System;
using System.Threading; //To allow use of sleep/wait

namespace Battleship_Assignent_11SDD_Jade_Harris
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare (and initialise appropriate variables)
            //Boolean variable to check if the entire game is running
            bool gameRunning = true;
            //Only run while loop of the entire game when game should be running (contained in bool)
            while (gameRunning == true)
            {
                //Generate 20 x 20 2D player board array (which is visible to player)
                string[,] playerBoard = new string[20, 20];
                //Generate 20 x 20 2D computer board array (shown to player) - so user can't view ship location
                string[,] computerBoard = new string[20, 20];
                //Generate 20 x 20 2D computer board array (hidden to player)
                string[,] hiddenComputerBoard = new string[20, 20];
                //Generate 20 x 20 2D player board array (hidden to player)
                string[,] hiddenPlayerBoard = new string[20, 20];
                //Contains player or computer's turn
                string turn = "player";
                //Number of computer's ships still unsunk
                int computerShips = 0;
                //Number of player's ships still unsunk
                int playerShips = 0;
                //Contains if the player chooses to play again
                string playAgain = " ";
                //Bool which contains if the round is running
                bool roundRunning = true;
                //Clears the console completely (specifically for formatting if user choses to replay but otherwise ensures no formatting errors)
                Console.Clear();
                //Reset all board arrays by setting their contents to " "
                ResetBoards(playerBoard, hiddenPlayerBoard, computerBoard, hiddenComputerBoard);
                //Set up computer's and player's ships randomized initial board positions. Pass a reference of each ship number to change the ship number and halt generating at 4
                SetBattleships(hiddenComputerBoard, ref computerShips, ref playerShips, hiddenPlayerBoard);
                //Run loop while the round is running (no one has 0 ships)
                while (roundRunning == true)
                {
                    //Function which prints ASCII text title of AIBATTLESHIP
                    PrintTitle();
                    //Print PLAYER and AI headings which includes a label above whose board it is and a white background colour depending who's turn it currently is
                    PrintHeadings(turn);
                    //Display player and computer boards
                    DisplayBoards(playerBoard, computerBoard);
                    /*//Uncomment to see hidden boards (where the ships were randomly placed)
                    //Add a formatting line
                    Console.WriteLine();
                    //Display the hidden boards
                    DisplayHiddenBoards(hiddenPlayerBoard, hiddenComputerBoard);*/
                    //Alternate between whose turn it is with the TakeTurn function according to the turn string variable
                    TakeTurn(playerBoard, hiddenPlayerBoard, computerBoard, hiddenComputerBoard, ref turn);
                    //Function to check if any ships were sunk and change number of ships unsunk accordingly
                    CheckForShipSink(hiddenComputerBoard, hiddenPlayerBoard, ref computerShips, ref playerShips);
                    //Check if there was a win by checking if playerShips or computerShips is 0
                    CheckForWin(ref turn, ref computerShips, ref playerShips, ref roundRunning);
                }
                if (roundRunning == false) //If roundRunning is false (changed by the check for win function when someone has won)
                {
                    //Ask if the player chooses to play/begin again by repeating the gameRunning while loop or if they choose no, to terminate the program.
                    CheckForGameEnd(playAgain, ref gameRunning);
                }
            }
        }

        private static void ResetBoards(string[,] playerBoard, string[,] hiddenPlayerBoard, string[,] computerBoard, string[,] hiddenComputerBoard)
        {
            //Reset each tile of array (x-coordinate)
            for (int i = 0; i < 20; i++) 
            {
                //Reset each tile of array (y-coordinate)
                for (int j = 0; j < 20; j++) 
                {
                    //Set (x,y) element of array blank
                    playerBoard[i, j] = " ";
                    //Set (x,y) element of array blank
                    computerBoard[i, j] = " ";
                    //Set (x,y) element of array blank
                    hiddenComputerBoard[i, j] = " ";
                    //Set (x,y) element of array blank
                    hiddenPlayerBoard[i, j] = " "; 
                }
            }
        }

        private static void SetBattleships(string[,] hiddenComputerBoard, ref int computerShips, ref int playerShips, string[,] hiddenPlayerBoard)
        {
            //Decalare integers for row and column of random boat and upAndDown (to randomly select whether boat is horizontal or vertical)
            int row, column, upOrDown;
            //Initialize randomNumber to a new random class
            Random randomNumber = new Random();
            //Continue generating with the while loop to get total of 4 COMPUTER ships on the board
            while (computerShips < 4)
            {
                //Random number is a minimum of 1 so that the boat can +- 1 and remain in boundaries (0 is limit for the first box) and max 18 so it can +- 1 (19 is limit for end box)
                row = randomNumber.Next(1, 19);
                //Random number is a minimum of 1 so that the boat can +- 1 and remain in boundaries (0 is limit for the first box) and max 18 so it can +- 1 (19 is limit for end box)
                column = randomNumber.Next(1, 19);
                //If the row and column is empty (no ship is already in that spot) and the grid horizontally 1 before and 1 after or vertically 1 above and below is equal to the empty square therefore empty too
                if (hiddenComputerBoard[row, column] == " " && hiddenComputerBoard[row, column] == hiddenComputerBoard[row + 1, column] && hiddenComputerBoard[row + 1, column] == hiddenComputerBoard[row - 1, column] && hiddenComputerBoard[row, column] == hiddenComputerBoard[row, column + 1] && hiddenComputerBoard[row, column + 1] == hiddenComputerBoard[row, column - 1])
                {
                    //Then make the original row, column = O (represents ship)
                    hiddenComputerBoard[row, column] = "O";
                    //Then generate another random number (o or 1) to randomly generate a horizontal or vertical ship around that point
                    upOrDown = randomNumber.Next(0, 2);
                    //If random number generated of upOrDown is 0
                    if (upOrDown == 0)
                    {
                        //Make ship horizontal (+1 left -1 right from row) and add 1 to the computerShip count so at 4 it will stop
                        hiddenComputerBoard[row + 1, column] = "O";
                        hiddenComputerBoard[row - 1, column] = "O";
                        computerShips += 1;
                    }
                    //If number isn't 0 then the random number generated must be 1
                    else
                    {
                        //Make ship horizontal (+1 up -1 down to the column) and add 1 computerShip count so at 4 it will stop
                        hiddenComputerBoard[row, column + 1] = "O";
                        hiddenComputerBoard[row, column - 1] = "O";
                        computerShips += 1;
                    }
                }
            }
            //Continue generating with the while loop to get total of 4 PLAYER ships on the board
            while (playerShips < 4)
            {
                //Random number is a minimum of 1 so that the boat can +- 1 and remain in boundaries (0 is limit for the first box) and max 18 so it can +- 1 (19 is limit for end box)
                row = randomNumber.Next(1, 19);
                //Random number is a minimum of 1 so that the boat can +- 1 and remain in boundaries (0 is limit for the first box) and max 18 so it can +- 1 (19 is limit for end box)
                column = randomNumber.Next(1, 19);
                //If the row and column is empty (no ship is already in that spot) and the grid horizontally 1 before and 1 after or vertically 1 above and below is equal to the empty square therefore empty too
                if (hiddenPlayerBoard[row, column] == " " && hiddenPlayerBoard[row, column] == hiddenPlayerBoard[row + 1, column] && hiddenPlayerBoard[row + 1, column] == hiddenPlayerBoard[row - 1, column] && hiddenPlayerBoard[row, column] == hiddenPlayerBoard[row, column + 1] && hiddenPlayerBoard[row, column + 1] == hiddenPlayerBoard[row, column - 1])
                {
                    //Then make the original row, column = O (represents ship)
                    hiddenPlayerBoard[row, column] = "O";
                    //Then generate another random number (o or 1) to randomly generate a horizontal or vertical ship around that point
                    upOrDown = randomNumber.Next(0, 2);
                    //If random number generated of upOrDown is 0
                    if (upOrDown == 0)
                    {
                        //Make ship horizontal (+1 left -1 right from row) and add 1 to the computerShip count so at 4 it will stop
                        hiddenPlayerBoard[row + 1, column] = "O";
                        hiddenPlayerBoard[row - 1, column] = "O";
                        playerShips += 1;
                    }
                    //If number isn't 0 then the random number generated must be 1
                    else
                    {
                        //Make ship horizontal (+1 up -1 down to the column) and add 1 computerShip count so at 4 it will stop
                        hiddenPlayerBoard[row, column + 1] = "O";
                        hiddenPlayerBoard[row, column - 1] = "O";
                        playerShips += 1;
                    }
                }
            }
        }

        private static void PrintTitle()
        {
            //Define each separate line of the ASCII text art (as it relies on multiple lines)
            string Line1 = "████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████";
            string Line2 = "█░░░░░░░░░░░░░░█░░░░░░░░░░██████░░░░░░░░░░░░░░███░░░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░█████████░░░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░██░░░░░░█░░░░░░░░░░█░░░░░░░░░░░░░░█";
            string Line3 = "█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀░░██████░░▄▀▄▀▄▀▄▀▄▀░░███░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░█████████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
            string Line4 = "█░░▄▀░░░░░░▄▀░░█░░░░▄▀░░░░██████░░▄▀░░░░░░▄▀░░███░░▄▀░░░░░░▄▀░░█░░░░░░▄▀░░░░░░█░░░░░░▄▀░░░░░░█░░▄▀░░█████████░░▄▀░░░░░░░░░░█░░▄▀░░░░░░░░░░█░░▄▀░░██░░▄▀░░█░░░░▄▀░░░░█░░▄▀░░░░░░▄▀░░█";
            string Line5 = "█░░▄▀░░██░░▄▀░░███░░▄▀░░████████░░▄▀░░██░░▄▀░░███░░▄▀░░██░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████████░░▄▀░░█████████░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░█";
            string Line6 = "█░░▄▀░░░░░░▄▀░░███░░▄▀░░████████░░▄▀░░░░░░▄▀░░░░█░░▄▀░░░░░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░█████████░░▄▀░░░░░░░░░░█░░▄▀░░░░░░░░░░█░░▄▀░░░░░░▄▀░░███░░▄▀░░███░░▄▀░░░░░░▄▀░░█";
            string Line7 = "█░░▄▀▄▀▄▀▄▀▄▀░░███░░▄▀░░████████░░▄▀▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░█████████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░███░░▄▀░░███░░▄▀▄▀▄▀▄▀▄▀░░█";
            string Line8 = "█░░▄▀░░░░░░▄▀░░███░░▄▀░░████████░░▄▀░░░░░░░░▄▀░░█░░▄▀░░░░░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░█████████░░▄▀░░░░░░░░░░█░░░░░░░░░░▄▀░░█░░▄▀░░░░░░▄▀░░███░░▄▀░░███░░▄▀░░░░░░░░░░█";
            string Line9 = "█░░▄▀░░██░░▄▀░░███░░▄▀░░████████░░▄▀░░████░░▄▀░░█░░▄▀░░██░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████████████████░░▄▀░░█░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░█████████";
            string Line10 = "█░░▄▀░░██░░▄▀░░█░░░░▄▀░░░░██████░░▄▀░░░░░░░░▄▀░░█░░▄▀░░██░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀░░░░░░░░░░█░░▄▀░░░░░░░░░░█░░░░░░░░░░▄▀░░█░░▄▀░░██░░▄▀░░█░░░░▄▀░░░░█░░▄▀░░█████████";
            string Line11 = "█░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀░░██████░░▄▀▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░██░░▄▀░░█████░░▄▀░░█████████░░▄▀░░█████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀░░█████████";
            string Line12 = "█░░░░░░██░░░░░░█░░░░░░░░░░██████░░░░░░░░░░░░░░░░█░░░░░░██░░░░░░█████░░░░░░█████████░░░░░░█████░░░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░██░░░░░░█░░░░░░░░░░█░░░░░░█████████";
            string Line13 = "████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████";
            /*Center title and have a 1 row border from top
            Set the ASCII string to begin at 30 columns across (measurement of spaces across from the left) to center horizontally when expanded as using Console.WindowWidth.Length - Line.Length / 2 produces an error because
            the ASCII text is long and print it 1 row down (measurement of spaces across from the top) then each row number ascending by 2 (to skip to next row)
            Write each title line from that cursor point.*/
            //Position text. By setting the second number at 1, start it at row 1 down
            Console.SetCursorPosition(30, 1);
            //Print Line1
            Console.Write(Line1);
            //Changing it to CursorTop + 1 sets cursor to the current line then adds another line on that. Print on next line
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            //Print Line2
            Console.Write(Line2);
            //Continue
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line3);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line4);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line5);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line6);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line7);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line8);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line9);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line10);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line11);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line12);
            Console.SetCursorPosition(30, Console.CursorTop + 1);
            Console.Write(Line13);
            //Add 2 lines for formatting to print the labels/headings one line down
            Console.WriteLine("\n");
        }

        private static void PrintHeadings(string turn)
        {
            //If it's the players turn
            if (turn == "player")
            {
                //Print headings above the boards with player label background white to highlight it's the player's turn
                PrintPlayerTurnHeading();
            }
            //Else (must be AI's turn):
            else
            {
                //Print headings above the boards with AI label background white to highlight it's the AI's turn
                PrintAITurnHeading();
            }
        }

        private static void PrintPlayerTurnHeading()
        {
            //Print player label with black text colour and a white background (indicate player's turn)
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" PLAYER ");
            //Reset label back to default (gray text and black background)
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            //Add 145 spaces to place AI label above AI board/ocean
            for (int i = 0; i < 145; i++)
            {
                Console.Write(" ");
            }
            //Print AI label normally above the AI board (because player's turn)
            Console.Write("AI");
            Console.WriteLine();
        }

        private static void PrintAITurnHeading()
        {
            //Print player label above the player board (because computer's turn)
            Console.Write("PLAYER");
            //Add 145 spaces to place the AI label above the AI board/ocean
            for (int i = 0; i < 145; i++)
            {
                Console.Write(" ");
            }
            //Print AI label with black text colour and a white background (indicate computer's turn)
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" AI ");
            //Reset label back to default (gray text and black background)
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            //Add line to print boards below
            Console.WriteLine();
        }

        private static void DisplayBoards(string[,] playerBoard, string[,] computerBoard)
        {
            //Create top border
            //Print corner
            Console.Write("+");
            //Print top of player (left) board - 1 less ---+ for formatting
            for (int i = 0; i < 20; i++)
            {
                Console.Write("---");
                Console.Write("+");
            } //Print 71 spaces to print AI to right side of screen
            for (int k = 0; k < 71; k++)
            {
                Console.Write(" ");
            }
            //Print corner
            Console.Write("+");
            //Print top of ai's board (right)
            for (int i = 0; i < 20; i++)
            {
                Console.Write("---");
                Console.Write("+");
            }
            //Move to next line to create grid sides of each grid box
            Console.WriteLine();
            //Print alphabet coordinates for player
            Console.Write("| A | B | C | D | E | F | G | H | I | J | K | L | M | N | O | P | Q | R | S | T |");
            //Print 71 spaces to print AI to right side of screen
            for (int k = 0; k < 71; k++)
            {
                Console.Write(" ");
            }
            //Print alphabet coordinates for AI
            Console.Write("| A | B | C | D | E | F | G | H | I | J | K | L | M | N | O | P | Q | R | S | T |");
            Console.WriteLine();
            //Print corner
            Console.Write("+");
            //Print bottom of player (left) bottom side of alphabet coordinates
            for (int i = 0; i < 21; i++)
            {
                Console.Write("---");
                Console.Write("+");
            }
            //Print 67 spaces to print AI's bottom side of alphabet coordinates to the right of screen
            for (int k = 0; k < 67; k++)
            {
                Console.Write(" ");
            }
            //Print corner
            Console.Write("+");
            //Print bottom of AI's (right) bottom side of alphabet coordinates
            for (int i = 0; i < 21; i++)
            {
                Console.Write("---");
                Console.Write("+");
            } 
            //Move to next line to create grid sides of each grid box
            for (int i = 0; i < 20; i++) //For each row
            {
                for (int j = 0; j < 20; j++)
                {
                    {
                        //Print each row of player board (with no right side so it doesn't double pipe symbol for each box)
                        Console.Write("| " + playerBoard[i, j] + " ");
                    }
                }
                //Still in for loop for each row, but if the end of the row is less than row 9, print the full last box with a number for the number coordinates
                if (i < 9)
                {
                    //Print number coordinates with the entire box (two | symbols)
                    Console.Write("| " + (i + 1) + " |");
                }
                //Thus row is 10 and above
                else
                {
                    //Print entire number coordinate box (two | symbols) but with no space before last pipe symbol 
                    Console.Write("| " + (i + 1) + "|");
                }
                //Add 67 spaces to print the AI board on the right
                for (int k = 0; k < 67; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < 20; j++)
                {
                    {
                        //Print each row of computer board (with no right side so it doesn't double pipe symbol for each box)
                        Console.Write("| " + computerBoard[i, j] + " ");
                    }
                }
                //Still printing for each row but if the row is less than row 9, print the full last box with a number for the number coordinates
                if (i < 9)
                {
                    //Print number coordinates with the entire box (two | symbols)
                    Console.Write("| " + (i + 1) + " |");
                }
                else //Thus row is 10 and above
                {
                    //Print entire number coordinate box (two | symbols) but with no space before last pipe symbol 
                    Console.Write("| " + (i + 1) + "|");
                }
                //Print corner
                Console.Write("+");
                //Print bottom of player board
                for (int j = 0; j < 21; j++)
                {
                    Console.Write("---");
                    Console.Write("+");
                }
                //Print 67 spaces to place AI board on left side of screen
                for (int j = 0; j < 67; j++)
                {
                    Console.Write(" ");
                }
                //Print bottom of computer board
                Console.Write("+");
                for (int j = 0; j < 21; j++)
                {
                    Console.Write("---");
                    Console.Write("+");
                }
            }
        }

        private static void TakeTurn(string[,] playerBoard, string[,] hiddenPlayerBoard, string[,] computerBoard, string[,] hiddenComputerBoard, ref string turn)
        {
            //If it's the player's turn from the reference to turn variable
            if (turn == "player")
            {
                //Direct to player's turn function
                TakeTurnPlayer(computerBoard, hiddenComputerBoard, ref turn);
            }
            //Else thus the computer's turn
            else
            {
                //Direct to computer's automated turn function
                TakeTurnComputer(playerBoard, hiddenPlayerBoard, ref turn);
            }
        }

        private static void TakeTurnPlayer(string[,] computerBoard, string[,] hiddenAIBoard, ref string turn)
        {
            //Declare variables
            string xAxisLetter;
            int xAxis, yAxis;
            //States whose turn from the turn variable (player's turn)
            Console.WriteLine("\n{0}'s TURN", turn);
            Console.WriteLine();
            //Asks for input of letter (x coorindate)
            Console.Write("Enter Letter of Your Grid Move [Column]: ");
            //The player's input is case insensitive (by converting letter to uppercase) for program usability then sets xAxis as the letter
            xAxisLetter = Console.ReadLine().ToUpper();
            //Call alphabet converter switch then set xAxis to the number coordinate returned
            xAxis = ConvertLetterToNumber(xAxisLetter);
            //Asks for input of number (y coorindate)
            Console.Write("Enter Number of Your Grid Move [Row]: ");
            //Convert string input to number and as the numbers start at 1 not 0 for the y-coordinate for program usability, - 1 from the input (so 1 = 0 thus first row)
            yAxis = Convert.ToInt32(Console.ReadLine()) - 1;
            //Formatting
            Console.WriteLine();
            //If xAxis isn't between 0-19 and yAxis isn't 0-19 then print RETRY tag and rerun function (player must choose new coordiantes)
            if (xAxis >= 0 && xAxis <= 19 && yAxis >= 0 && yAxis <= 19)
            {
                //Checks if the coordinates are avaliable/empty on the hidden computer's board (thus no hit)
                if (hiddenAIBoard[yAxis, xAxis] == " ")
                {
                    //Add "-" to the hidden computerboard (not the shown one) as the player cannot see where the computer's boats are and this is the board the program will check. Also, so they are unable to take a turn there again
                    hiddenAIBoard[yAxis, xAxis] = "-";
                    //Add "-" to the shown (normal) computer board as well to indicate for the player that they have previously gone there 
                    computerBoard[yAxis, xAxis] = "-";
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    //Create a noticable tag/message with a yellow background (secondary notation) to alert the player they did not hit
                    Console.WriteLine(" MISS ");
                    //Reset the text background to black (default) for rest of program
                    Console.BackgroundColor = ConsoleColor.Black;
                    //Wait/leave message for 5 milliseconds before continuing out of the if statement and clearing the console for the AI's turn
                    Thread.Sleep(500);
                }
                //If the coordinates aren't empty, then the hidden computer's board must have a battleship on them (indicated by O)
                else if (hiddenAIBoard[yAxis, xAxis] == "O") 
                {
                    //Add "X" to the hidden computerboard (not the shown one) as this is the board which has the battleships so the program must check it and check for win with it.
                    hiddenAIBoard[yAxis, xAxis] = "X";
                    //Add "X" to the shown (normal) computer board as well to represent that their missile has made a direct hit
                    computerBoard[yAxis, xAxis] = "X";
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    //Create a noticable tag/message with a green background (rewarding secondary notation) to alert the player they hit a segment
                    Console.WriteLine(" MISSILE DIRECT HIT ");
                    //Reset the text background for rest of program
                    Console.BackgroundColor = ConsoleColor.Black;
                    //Wait/leave message for 5 milliseconds before continuing out of the if statement and clearing the console for the AI's turn
                    Thread.Sleep(500);
                }
                //If the coordinates are neither empty (" ") nor contain a missed battleship ("O"), then the player must have already hit a segment ("X") or been there ("-")
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    //Create a noticable tag/message with a red background (warning secondary notation) to alert the player they need to retry
                    Console.WriteLine(" RETRY ");
                    //Reset the text background to black (default) for rest of program
                    Console.BackgroundColor = ConsoleColor.Black;
                    //Rerun function (player must choose new coordinates)
                    TakeTurnPlayer(computerBoard, hiddenAIBoard, ref turn);
                }
                /*If the player's turn was successful (if statement was not rerouted by TakeTurnPlayer function rerun), then the player must have had their turn so change turn to 
                 computer so the while lopp in main function (while (roundRunning == true)) gives computer turn instead*/
                //Change global string of turn as it was passed as a reference
                turn = "computer";
            }
            //Else to initial if statement (so xAxis isn't between 0-19 and yAxis isn't 0-19) then print RETRY
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                //Create a noticable tag/message with a red background (warning secondary notation) to alert the player they need to retry
                Console.WriteLine(" RETRY ");
                //Reset the text background for rest of program
                Console.BackgroundColor = ConsoleColor.Black;
                //Rerun the function (player must choose new coordinates/re-take turn)
                TakeTurnPlayer(computerBoard, hiddenAIBoard, ref turn);
            }
        }

        private static int ConvertLetterToNumber(string xAxisLetter)
        {
            int xAxis;
            //Switchcase to convert the uppercase letter into an x coordinate
            switch (xAxisLetter)
            {
                //If the user inputs A meaning the xAxisLetter string would be "A"
                case "A":
                    //Then they would be referring to the first column by xAxis - a separate variable 
                    xAxis = 0;
                    //Then break is necessary to exit switch statement
                    break;
                //Continue with the first 20 letters of the alphabet and their according number
                case "B":
                    xAxis = 1;
                    break;
                case "C":
                    xAxis = 2;
                    break;
                case "D":
                    xAxis = 3;
                    break;
                case "E":
                    xAxis = 4;
                    break;
                case "F":
                    xAxis = 5;
                    break;
                case "G":
                    xAxis = 6;
                    break;
                case "H":
                    xAxis = 7;
                    break;
                case "I":
                    xAxis = 8;
                    break;
                case "J":
                    xAxis = 9;
                    break;
                case "K":
                    xAxis = 10;
                    break;
                case "L":
                    xAxis = 11;
                    break;
                case "M":
                    xAxis = 12;
                    break;
                case "N":
                    xAxis = 13;
                    break;
                case "O":
                    xAxis = 14;
                    break;
                case "P":
                    xAxis = 15;
                    break;
                case "Q":
                    xAxis = 16;
                    break;
                case "R":
                    xAxis = 17;
                    break;
                case "S":
                    xAxis = 18;
                    break;
                case "T":
                    xAxis = 19;
                    break;
                //Default case (if the user doesn't input a letter between A-T
                default:
                    //Set xAxis to -1 thus the first if statement in the TakeTurnPlayer function will force the player to retry with new values
                    xAxis = -1;
                    break;
            }
            //Return value of switch statement to set the TakeTurnPlayer value of xAxis to the xAxis in this converter's variable value
            return xAxis;
        }

        private static void TakeTurnComputer(string[,] playerBoard, string[,] hiddenPlayerBoard, ref string turn)
        {
            //Declare variables
            int xAxis, yAxis;
            //Initialize randomNumber to a new random class
            Random randomNumber = new Random();
            //State's whose turn from turn variable (computer's)
            Console.WriteLine("\n{0}'s TURN", turn);
            //Formatting
            Console.WriteLine(); 
            //Create 3 dot's to indicate to user that computer is processing/taking turn for usability
            for (int i = 0; i < 3; i++)
            {
                //Print a dot and space
                Console.Write(". ");
                //Wait 4 milliseconds
                Thread.Sleep(400);
            }
            //Formatting
            Console.WriteLine("\n");
            //For AI's turn, generate a random number between (including) 0-19
            xAxis = randomNumber.Next(0, 19);
            //For AI's turn, generate a random number between (including) 0-19
            yAxis = randomNumber.Next(0, 19);
            //Checks if the coordinates are avaliable/empty on the playerr's board (thus no hit)
            if (hiddenPlayerBoard[xAxis, yAxis] == " ")
            {
                //Add "-" to the hidden playerboard (not the shown one) as this is the one the program will check so that the computer is unable to take a turn there again
                hiddenPlayerBoard[yAxis, xAxis] = "-";
                //Add "-" to the playerboard so the AI is unable to take a turn there again
                playerBoard[xAxis, yAxis] = "-";
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                //Create a noticable tag/message with a yellow background (secondary notation) to alert the player they did not hit
                Console.WriteLine(" MISS ");
                //Reset the text background to black (default) for rest of program
                Console.BackgroundColor = ConsoleColor.Black;
                //Wait/leave message for 5 milliseconds before continuing out of the if statement and clearing the console for the AI's turn
                Thread.Sleep(500);
            }
            //If the coordinates aren't empty, then the hidden computer's board must have a battleship on them (represented by O)
            else if (hiddenPlayerBoard[xAxis, yAxis] == "O")
            {
                //Add "X" to the hidden playerboard (not the shown one) as this is the board which the program must check for win with
                hiddenPlayerBoard[xAxis, yAxis] = "X";
                //Add "X" to the shown playerbaord to represent that the missile has made a direct hit to the player
                playerBoard[xAxis, yAxis] = "X";
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                //Create a noticable tag/message with a green background (rewarding secondary notation) to alert the player computer has hit a part of thier boat
                Console.WriteLine(" MISSILE DIRECT HIT ");
                //Reset the text background to black (default) for rest of program
                Console.BackgroundColor = ConsoleColor.Black;
                //Wait/leave message for 5 milliseconds before continuing out of the if statement and clearing the console for the AI's turn
                Thread.Sleep(500);
            }
            //If the coordinates are not empty or a missed battleship then computer must retry/regenerate numbers
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                //Create a noticable tag/message with a red background (warning secondary notation) to alert the player that the computer/AI is retrying
                Console.WriteLine(" RETRY ");
                //Reset the text background for rest of program
                Console.BackgroundColor = ConsoleColor.Black;
                //Rerun the function (computer must generate new coordinates/re-take turn)
                TakeTurnComputer(playerBoard, hiddenPlayerBoard, ref turn);
            }
            /*If the computer's turn was successful (if statement was not rerouted by TakeTurnComputer function rerun), then the computer must have had their turn so change turn to 
             player so the while lopp in main function (while (roundRunning == true)) gives player turn*/
            turn = "player";
        }

        private static void CheckForShipSink(string[,] hiddenComputerBoard, string[,] hiddenPlayerBoard, ref int computerShips, ref int playerShips)
        {
            //For each row between 0-19
            for (int i = 0; i < 20; i++)
            {
                //For each column (j starts at 1 and goes until 19 as the check runs from j - 1 to j + 1 therefore minimum within boundaries of array is 0 and maximum is 19)
                for (int j = 1; j < 19; j++)
                {
                    //If the for loop of [i and j - 1] is equal to "X" (direct hit on battleship) and [i, j - 1] == i, j then [i, j] must be "X" (hit) and if [i, j] == [i, j + 1] then [i, j + 1] must be "X" therefore 3 in a row must be hit and a computer battleship sunk
                    if (hiddenComputerBoard[i, j - 1] == "X" && hiddenComputerBoard[i, j - 1] == hiddenComputerBoard[i, j] && hiddenComputerBoard[i, j] == hiddenComputerBoard[i, j + 1])
                    {
                        //Set the 3 coordinates which were "X" to ~ so that the check for win event doesn't register multiple times
                        hiddenComputerBoard[i, j - 1] = "~";
                        hiddenComputerBoard[i, j] = "~";
                        hiddenComputerBoard[i, j + 1] = "~";
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        //Create a noticable tag/message with a blue background (secondary notation) to alert the player they sunk a computer ship
                        Console.WriteLine(" AI SHIP SUNK ");
                        //Reset the text background for rest of program
                        Console.BackgroundColor = ConsoleColor.Black;
                        //Wait/leave message for 1 second
                        Thread.Sleep(1000);
                        //Reduce computer ships variable reference as one was sunk
                        computerShips -= 1;
                    }
                    //If the for loop of i and j - 1 is equal to "X" (direct hit on battleship) and i, j - 1 == i, j then i, j must be "X" (hit) and if i, j == i, j + 1 then i, j + 1 must be "X" therefore 3 in a row must be hit and a player battleship sunk
                    if (hiddenPlayerBoard[i, j - 1] == "X" && hiddenPlayerBoard[i, j - 1] == hiddenPlayerBoard[i, j] && hiddenPlayerBoard[i, j] == hiddenPlayerBoard[i, j + 1])
                    {
                        hiddenPlayerBoard[i, j - 1] = "~";
                        hiddenPlayerBoard[i, j] = "~";
                        hiddenPlayerBoard[i, j + 1] = "~";
                        //Set the 3 coordinates which were "X" to ~ so that the check for win event doesn't register multiple times
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        //Create a noticable tag/message with a blue background (secondary notation) to alert the player a ship was sunk
                        Console.WriteLine(" PLAYER SHIP SUNK ");
                        //Reset the text background for rest of program
                        Console.BackgroundColor = ConsoleColor.Black;
                        //Wait/leave message for 1 second
                        Thread.Sleep(1000);
                        //Reduce player ships variable reference as one was sunk
                        playerShips -= 1; 
                    }
                }
            }
            //For each column between 0-19
            for (int i = 0; i < 20; i++)
            {
                //For each row (j starts at 1 and goes until 19 as the check runs from j - 1 to j + 1 therefore minimum within boundaries of array is 0 and maximum is 19)
                for (int j = 1; j < 19; j++)
                {
                    //If the for loop of j - 1 and i is equal to "X" (direct hit on battleship) and j - 1, i == j, i then j, i must be "X" (hit) and if j, i == j + 1, i then j + 1, i must be "X" therefore 3 in a row must be hit and a computer battleship sunk
                    if (hiddenComputerBoard[j - 1, i] == "X" && hiddenComputerBoard[j - 1, i] == hiddenComputerBoard[j, i] && hiddenComputerBoard[j, i] == hiddenComputerBoard[j + 1, i])
                    {
                        //Set the 3 coordinates which were "X" to ~ so that the check for win event doesn't register multiple times
                        hiddenComputerBoard[j - 1, i] = "~";
                        hiddenComputerBoard[j, i] = "~";
                        hiddenComputerBoard[j + 1, i] = "~";
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine(" AI SHIP SUNK "); //Create a noticable tag/message with a blue background (secondary notation) to alert the player they sunk a computer ship
                        Console.BackgroundColor = ConsoleColor.Black; //Reset the text background for rest of program
                        Thread.Sleep(1000); //Wait/leave message for 1 second
                        computerShips -= 1; //Reduce computer ships variable reference as one was sunk then continue program
                    }
                    //If the for loop of j - 1 and i is equal to "X" (direct hit on battleship) and j - 1, i == j, i then j, i must be "X" (hit) and if j, i == j + 1, i then j + 1, i must be "X" therefore 3 in a row must be hit and a player battleship sunk
                    if (hiddenPlayerBoard[j - 1, i] == "X" && hiddenPlayerBoard[j - 1, i] == hiddenPlayerBoard[j, i] && hiddenPlayerBoard[j, i] == hiddenPlayerBoard[j + 1, i])
                    {
                        hiddenPlayerBoard[j - 1, i] = "~";
                        hiddenPlayerBoard[j, i] = "~";
                        hiddenPlayerBoard[j + 1, i] = "~";
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        //Create a noticable tag/message with a blue background (secondary notation) to alert the player that their ship was sunk
                        Console.WriteLine(" PLAYER SHIP SUNK ");
                        //Reset the text background for rest of program
                        Console.BackgroundColor = ConsoleColor.Black;
                        //Wait/leave message for 1 second
                        Thread.Sleep(1000);
                        //Reduce player ships variable reference as one was sunk then continue program
                        playerShips -= 1;
                    }
                }
            }
        }

        private static void CheckForWin(ref string player, ref int computerShips, ref int playerShips, ref bool roundRunning)
        {
            //Clear boards from console (formatting)
            Console.Clear();
           //If playerships == 0 (variable passed through) then computer wins and message to congratulate winner
            if (playerShips == 0)
            {
                //Define each separate line of the ASCII text art (as it relies on multiple lines)
                string Line1 = "█████████████████████████████████████████████████████████████████████████████████████████████████████████ ";
                string Line2 = "█░░░░░░░░░░░░░░█░░░░░░░░░░███████░░░░░░██████████░░░░░░█░░░░░░░░░░█░░░░░░██████████░░░░░░█░░░░░░░░░░░░░░█ ";
                string Line3 = "█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀░░███████░░▄▀░░██████████░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀░░░░░░░░░░██░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
                string Line4 = "█░░▄▀░░░░░░▄▀░░█░░░░▄▀░░░░███████░░▄▀░░██████████░░▄▀░░█░░░░▄▀░░░░█░░▄▀▄▀▄▀▄▀▄▀░░██░░▄▀░░█░░▄▀░░░░░░░░░░█";
                string Line5 = "█░░▄▀░░██░░▄▀░░███░░▄▀░░█████████░░▄▀░░██████████░░▄▀░░███░░▄▀░░███░░▄▀░░░░░░▄▀░░██░░▄▀░░█░░▄▀░░█████████";
                string Line6 = "█░░▄▀░░░░░░▄▀░░███░░▄▀░░█████████░░▄▀░░██░░░░░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░▄▀░░░░░░░░░░█";
                string Line7 = "█░░▄▀▄▀▄▀▄▀▄▀░░███░░▄▀░░█████████░░▄▀░░██░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
                string Line8 = "█░░▄▀░░░░░░▄▀░░███░░▄▀░░█████████░░▄▀░░██░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░░░░░░░░░▄▀░░█";
                string Line9 = "█░░▄▀░░██░░▄▀░░███░░▄▀░░█████████░░▄▀░░░░░░▄▀░░░░░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░░░░░▄▀░░█████████░░▄▀░░█";
                string Line10 = "█░░▄▀░░██░░▄▀░░█░░░░▄▀░░░░███████░░▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀░░█░░░░▄▀░░░░█░░▄▀░░██░░▄▀▄▀▄▀▄▀▄▀░░█░░░░░░░░░░▄▀░░█ ";
                string Line11 = "█░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀░░███████░░▄▀░░░░░░▄▀░░░░░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀░░██░░░░░░░░░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█ ";
                string Line12 = "█░░░░░░██░░░░░░█░░░░░░░░░░███████░░░░░░██░░░░░░██░░░░░░█░░░░░░░░░░█░░░░░░██████████░░░░░░█░░░░░░░░░░░░░░█ ";
                string Line13 = "█████████████████████████████████████████████████████████████████████████████████████████████████████████";
                /*Center title and have a 1 row border from top
                Set the ASCII string to begin at 60 columns across (measurement of spaces across from the left) to center horiztonally
                and 1 row down (measurement of spaces across from the top) then each row number ascending by 2 (to skip to next row)
                then write title from that cursor point.*/
                //Position text. By setting the second number at 1, start at row 1 down
                Console.SetCursorPosition(60, 1);
                //Print Line1
                Console.Write(Line1);
                //Changing it to CursorTop + 1 sets cursor to the current line then adds another line on that. Print on next line
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                //Print Line2
                Console.Write(Line2);
                //Continue
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line3);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line4);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line5);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line6);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line7);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line8);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line9);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line10);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line11);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line12);
                Console.SetCursorPosition(60, Console.CursorTop + 1);
                Console.Write(Line13);
                //Set roundrunning to false to exit roundRunning while loop in main function
                roundRunning = false;
            }
            //If computerships == 0 (variable passed through) then player wins and gets message to congratulate winner
            if (computerShips == 0)
            {
                //Define each separate line of the ASCII text art (as it relies on multiple lines)
                string Line1 = "█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████";
                string Line2 = "█░░░░░░░░░░░░░░█░░░░░░█████████░░░░░░░░░░░░░░█░░░░░░░░██░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░░░░░░░░░░░█████████░░░░░░██████████░░░░░░█░░░░░░░░░░█░░░░░░██████████░░░░░░█░░░░░░░░░░░░░░█";
                string Line3 = "█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░█████████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀░░██░░▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀▄▀░░█████████░░▄▀░░██████████░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀░░░░░░░░░░██░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
                string Line4 = "█░░▄▀░░░░░░▄▀░░█░░▄▀░░█████████░░▄▀░░░░░░▄▀░░█░░░░▄▀░░██░░▄▀░░░░█░░▄▀░░░░░░░░░░█░░▄▀░░░░░░░░▄▀░░█████████░░▄▀░░██████████░░▄▀░░█░░░░▄▀░░░░█░░▄▀▄▀▄▀▄▀▄▀░░██░░▄▀░░█░░▄▀░░░░░░░░░░█";
                string Line5 = "█░░▄▀░░██░░▄▀░░█░░▄▀░░█████████░░▄▀░░██░░▄▀░░███░░▄▀▄▀░░▄▀▄▀░░███░░▄▀░░█████████░░▄▀░░████░░▄▀░░█████████░░▄▀░░██████████░░▄▀░░███░░▄▀░░███░░▄▀░░░░░░▄▀░░██░░▄▀░░█░░▄▀░░█████████";
                string Line6 = "█░░▄▀░░░░░░▄▀░░█░░▄▀░░█████████░░▄▀░░░░░░▄▀░░███░░░░▄▀▄▀▄▀░░░░███░░▄▀░░░░░░░░░░█░░▄▀░░░░░░░░▄▀░░█████████░░▄▀░░██░░░░░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░▄▀░░░░░░░░░░█";
                string Line7 = "█░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░█████████░░▄▀▄▀▄▀▄▀▄▀░░█████░░░░▄▀░░░░█████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀▄▀▄▀▄▀▄▀▄▀░░█████████░░▄▀░░██░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
                string Line8 = "█░░▄▀░░░░░░░░░░█░░▄▀░░█████████░░▄▀░░░░░░▄▀░░███████░░▄▀░░███████░░▄▀░░░░░░░░░░█░░▄▀░░░░░░▄▀░░░░█████████░░▄▀░░██░░▄▀░░██░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░██░░▄▀░░█░░░░░░░░░░▄▀░░█";
                string Line9 = "█░░▄▀░░█████████░░▄▀░░█████████░░▄▀░░██░░▄▀░░███████░░▄▀░░███████░░▄▀░░█████████░░▄▀░░██░░▄▀░░███████████░░▄▀░░░░░░▄▀░░░░░░▄▀░░███░░▄▀░░███░░▄▀░░██░░▄▀░░░░░░▄▀░░█████████░░▄▀░░█";
                string Line10 = "█░░▄▀░░█████████░░▄▀░░░░░░░░░░█░░▄▀░░██░░▄▀░░███████░░▄▀░░███████░░▄▀░░░░░░░░░░█░░▄▀░░██░░▄▀░░░░░░███████░░▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀░░█░░░░▄▀░░░░█░░▄▀░░██░░▄▀▄▀▄▀▄▀▄▀░░█░░░░░░░░░░▄▀░░█";
                string Line11 = "█░░▄▀░░█████████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░██░░▄▀░░███████░░▄▀░░███████░░▄▀▄▀▄▀▄▀▄▀░░█░░▄▀░░██░░▄▀▄▀▄▀░░███████░░▄▀░░░░░░▄▀░░░░░░▄▀░░█░░▄▀▄▀▄▀░░█░░▄▀░░██░░░░░░░░░░▄▀░░█░░▄▀▄▀▄▀▄▀▄▀░░█";
                string Line12 = "█░░░░░░█████████░░░░░░░░░░░░░░█░░░░░░██░░░░░░███████░░░░░░███████░░░░░░░░░░░░░░█░░░░░░██░░░░░░░░░░███████░░░░░░██░░░░░░██░░░░░░█░░░░░░░░░░█░░░░░░██████████░░░░░░█░░░░░░░░░░░░░░█";
                string Line13 = "█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████";
                /*Center title and have a 1 row border from top
                Set the ASCII string to begin at 30 columns across (measurement of spaces across from the left) to center horizontally
                and 1 row down (measurement of spaces across from the top) then each row number ascending by 2 (to skip to next row)
                then write title from that cursor point.*/
                //Position text. By setting the second number at 1, start at row 1 down
                Console.SetCursorPosition(30, 1);
                //Print Line1
                Console.Write(Line1);
                //Changing it to CursorTop + 1 sets cursor to the current line then adds another line on that. Print on next line
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                //Print Line2
                Console.Write(Line2);
                //Continue
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line3);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line4);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line5);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line6);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line7);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line8);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line9);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line10);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line11);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line12);
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                Console.Write(Line13);
                //Set roundrunning to false to exit roundRunning while loop in main function
                roundRunning = false;
            }
        }

        private static void CheckForGameEnd(string playAgain, ref bool gameRunning)
        {
            //While playAgain is not Y or N, repeat asking question until response
            while (playAgain != "Y" && playAgain != "N")
            {
                //Give option to play again
                Console.WriteLine("\nGAME OVER! Play Again? (Y/N) ");
                //For usability, make case insensitive by converting y or n to uppercase
                playAgain = Console.ReadLine().ToUpper();
                //If they do want to play again ("Y")
                if (playAgain == "Y")
                {
                    //Then set gameRunning boolean to true to repeat the entire while gameRunning statement (game begins again)
                    gameRunning = true;
                }
                //Therefore they must have inputted "N" - they do not wish to play again
                else
                {
                    //Formatting
                    Console.WriteLine("\n");
                    //Print farewell with black text colour and a white background to grab attention
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    //Give a departing message
                    Console.Write(" FAREWELL :( ");
                    //Reset label back to default (gray text and black background)
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                    //Exit while gameRunning loop (so game terminates) then console terminal closes when key is pressed
                    gameRunning = false;
                }
            }
        }

        private static void DisplayHiddenBoards(string[,] hiddenPlayerBoard, string[,] hiddenComputerBoard)
        {
            //Create top border
            //Print corner
            Console.Write("+");
            //Print top of player (left) board - 1 less ---+ for formatting
            for (int i = 0; i < 20; i++)
            {
                Console.Write("---");
                Console.Write("+");
            } //Print 71 spaces to print AI to right side of screen
            for (int k = 0; k < 71; k++)
            {
                Console.Write(" ");
            }
            //Print corner
            Console.Write("+");
            //Print top of ai's board (right)
            for (int i = 0; i < 20; i++)
            {
                Console.Write("---");
                Console.Write("+");
            }
            //Move to next line to create grid sides of each grid box
            Console.WriteLine();
            //Print alphabet coordinates for player
            Console.Write("| A | B | C | D | E | F | G | H | I | J | K | L | M | N | O | P | Q | R | S | T |");
            //Print 71 spaces to print AI to right side of screen
            for (int k = 0; k < 71; k++)
            {
                Console.Write(" ");
            }
            //Print alphabet coordinates for AI
            Console.Write("| A | B | C | D | E | F | G | H | I | J | K | L | M | N | O | P | Q | R | S | T |");
            Console.WriteLine();
            //Print corner
            Console.Write("+");
            //Print bottom of player (left) bottom side of alphabet coordinates
            for (int i = 0; i < 21; i++)
            {
                Console.Write("---");
                Console.Write("+");
            }
            //Print 67 spaces to print AI's bottom side of alphabet coordinates to the right of screen
            for (int k = 0; k < 67; k++)
            {
                Console.Write(" ");
            }
            //Print corner
            Console.Write("+");
            //Print bottom of AI's (right) bottom side of alphabet coordinates
            for (int i = 0; i < 21; i++)
            {
                Console.Write("---");
                Console.Write("+");
            }
            //Move to next line to create grid sides of each grid box
            for (int i = 0; i < 20; i++) //For each row
            {
                for (int j = 0; j < 20; j++)
                {
                    {
                        //Print each row of player board (with no right side so it doesn't double pipe symbol for each box)
                        Console.Write("| " + hiddenPlayerBoard[i, j] + " ");
                    }
                }
                //Still in for loop for each row, but if the end of the row is less than row 9, print the full last box with a number for the number coordinates
                if (i < 9)
                {
                    //Print number coordinates with the entire box (two | symbols)
                    Console.Write("| " + (i + 1) + " |");
                }
                //Thus row is 10 and above
                else
                {
                    //Print entire number coordinate box (two | symbols) but with no space before last pipe symbol 
                    Console.Write("| " + (i + 1) + "|");
                }
                //Add 67 spaces to print the AI board on the right
                for (int k = 0; k < 67; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < 20; j++)
                {
                    {
                        //Print each row of computer board (with no right side so it doesn't double pipe symbol for each box)
                        Console.Write("| " + hiddenComputerBoard[i, j] + " ");
                    }
                }
                //Still printing for each row but if the row is less than row 9, print the full last box with a number for the number coordinates
                if (i < 9)
                {
                    //Print number coordinates with the entire box (two | symbols)
                    Console.Write("| " + (i + 1) + " |");
                }
                else //Thus row is 10 and above
                {
                    //Print entire number coordinate box (two | symbols) but with no space before last pipe symbol 
                    Console.Write("| " + (i + 1) + "|");
                }
                //Print corner
                Console.Write("+");
                //Print bottom of player board
                for (int j = 0; j < 21; j++)
                {
                    Console.Write("---");
                    Console.Write("+");
                }
                //Print 67 spaces to place AI board on left side of screen
                for (int j = 0; j < 67; j++)
                {
                    Console.Write(" ");
                }
                //Print bottom of computer board
                Console.Write("+");
                for (int j = 0; j < 21; j++)
                {
                    Console.Write("---");
                    Console.Write("+");
                }
            }
        } //I know it's bad practice to include an unused function, however, I thought if you wish to win/test quickly then activate this by uncommenting in the main function
    }
}
