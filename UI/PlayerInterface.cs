using System;
using System.Text;
using System.Collections.Generic;
using Ex02.ConsoleUtils;
using Logic;

namespace PlayerInterface
{
    public class PlayerInterface
    {
        private static GamePlay s_GamePlay;
        private static int s_TurnNumber;
        private static int s_MaxAmountOfTurns;

        public static void Main()
        {
            RunGame();
        }

        public static void RunGame()
        {
            string keepPlaying = "Y";
            s_GamePlay = new GamePlay();

            while (keepPlaying == "Y")
            {
                bool won = false;

                Screen.Clear();
                s_MaxAmountOfTurns = pickNumOfTurns();
                if (s_MaxAmountOfTurns == -1)  // user pressed Q.  
                {
                    userQuits();
                    break;
                }

                s_GamePlay.SetNewGame();
                s_TurnNumber = 0;
                updateVisualBoard();
                while (s_TurnNumber < s_MaxAmountOfTurns && !won)
                {
                    string userPickedLetters = getUserGuesses();

                    if(userPickedLetters == null) // user pressed Q.
                    {
                        userQuits();
                        keepPlaying = "N";
                        break;
                    }

                    won = isWonAndNextMove(userPickedLetters);
                    s_TurnNumber++;
                    updateVisualBoard();
                }

                if(keepPlaying == "N")
                {
                    break;
                }

                if (won)
                {
                    string winningMsg = string.Format("You guessed after {0} steps!", s_TurnNumber);

                    Console.WriteLine(winningMsg);
                }
                else
                {
                    Console.WriteLine("No more guesses allowed. You Lost.");
                }

                Console.WriteLine("Would you like to start a new game? <Y/N>");
                do
                {
                    keepPlaying = Console.ReadLine();
                }
                while (keepPlaying != "Y" && keepPlaying != "N");
            }
        }

        private static void userQuits()
        {
            Screen.Clear();
            Console.WriteLine("bye bye! :)");
            Console.WriteLine("press enter to quit");
            Console.ReadLine();
        }

        private static int pickNumOfTurns()
        {
            int numOfTurns = -1;
            bool validInput = false;
            const string k_TurnsPhrase = "Please enter how many turns would you like to play(4 - 10) or 'Q' to quit: ";

            Console.WriteLine(k_TurnsPhrase);
            while(!validInput)
            {
                string userInput = Console.ReadLine();

                validInput = int.TryParse(userInput, out numOfTurns);
                if(validInput)
                {
                    if(numOfTurns > 10 || numOfTurns < 4)
                    {
                        Console.WriteLine("Invalid Input! the given number is out of bounds");
                        Console.WriteLine(k_TurnsPhrase);
                        validInput = false;
                        continue;
                    }
                }
                else
                {
                    if(userInput == "Q") // user wants to quit
                    {
                        numOfTurns = -1;
                        break;
                    }

                    Console.WriteLine("Invalid Input! did not enter a number");
                    Console.WriteLine(k_TurnsPhrase);
                }
            }

            return numOfTurns;
        }

        private static string getUserGuesses()
        {
            StringBuilder userPickedLetters = new StringBuilder();
            List<char> availableOption = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            Console.WriteLine("Please type your next guess <A-H> or 'Q' to quit:");
            while (userPickedLetters.Length != 4)
            {
                string userInput = Console.ReadLine();
                bool invalidInput = false;

                userInput = userInput.Replace(" ", string.Empty);
                if(userInput[0] == 'Q')
                {
                    return null;
                }

                if(userInput.Length != 4)
                {
                    Console.WriteLine("Please enter exactly 4 letters. Try again: ");
                }
                else
                {
                    foreach(char letter in userInput)
                    {
                        if (availableOption.Contains(letter))
                        {
                            userPickedLetters.Append(letter);
                            availableOption.Remove(letter);
                        }
                        else if (userPickedLetters.ToString().Contains(letter.ToString()))
                        {
                            Console.WriteLine("The letter {0} has already been picked. Try again", letter);
                            invalidInput = true;
                        }
                        else if (char.IsLetter(letter))
                        {
                            Console.WriteLine("the letter {0} is not in the A-H range. Try again", letter);
                            invalidInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input: {0}. Please pick letters in A-H range. Try again", letter);
                            invalidInput = true;
                        }
                    }

                    if (invalidInput)
                    {
                        availableOption.AddRange(userPickedLetters.ToString().ToCharArray());
                        userPickedLetters.Clear();
                    }
                }

                /*for(int i = 0; i < userInput.Length; i++)
                {
                    //isChar = char.TryParse(userInput[i].ToString(), out letter);
                    isLetter = char.IsLetter(userInput, i);
                    if (isLetter)
                    {
                        if (availableOption.Contains(userInput[i])) ////letter in bounds A-H.
                        {
                            userPickedLetters.Append(userInput[i]);
                            lettersPicked.Add(userInput[i]);
                            availableOption.Remove(userInput[i]);
                            if(lettersPicked.Count > 4)
                            {
                                Console.WriteLine("More than 4 letters picked, try again.");
                                userPickedLetters.Clear();
                                availableOption.AddRange(lettersPicked);
                                lettersPicked.Clear();
                            }
                        }
                        else if(lettersPicked.Contains(userInput[i]))
                        {
                            Console.WriteLine("letter already been picked");
                        }
                        else if (userInput[i] == 'Q') // user wants to quit
                        {
                            return null;
                        }
                        else// is letter but out of A-H bounds.
                        {
                            Console.WriteLine("Invalid Input! char entered is not in range(A-H). Please try again");
                        }
                    }
                    else if(userInput[i] != ' ') // is not a letter
                    {
                        Console.WriteLine("did not enter a letter");
                    }
                }*/
            }

            return userPickedLetters.ToString();
        }

        private static void updateVisualBoard()
        {
            string headLine = string.Format("Current board status:{0}", Environment.NewLine);
            const string k_Separator = "|=========|=======|";
            const string k_LimitWallSeparator = "|         |       |";
            StringBuilder visualBoard = new StringBuilder();
            List<Guess> currentBoard = s_GamePlay.GameBoard;

            visualBoard.AppendLine("|Pins:    |Result:|");
            visualBoard.AppendLine(k_Separator);
            if(s_TurnNumber == s_MaxAmountOfTurns)
            {
                char[] computerChoice = s_GamePlay.ChosenLetters.ToArray(); 

                visualBoard.AppendFormat("| {0} {1} {2} {3} |       |{4}", 
                    computerChoice[0],
                    computerChoice[1],
                    computerChoice[2], 
                    computerChoice[3],
                    Environment.NewLine);
            }
            else
            {
                visualBoard.AppendLine("| # # # # |       |");
            }

            visualBoard.AppendLine(k_Separator);

            for (int i = 0; i < s_TurnNumber; i++)
            {
                Guess currentGuess = currentBoard[i];
                string boardPinsRow = currentGuess.GuessPins;
                int numOfSuccess = currentGuess.NumOfRightPositionLetters;
                int numOfTries = currentGuess.NumOfIncorrectPositionLetters;

                visualBoard.AppendFormat("| {0} {1} {2} {3} |", boardPinsRow[0], boardPinsRow[1], boardPinsRow[2], boardPinsRow[3]);
                visualBoard.Insert(visualBoard.Length, "V ", numOfSuccess);
                visualBoard.Insert(visualBoard.Length, "X ", numOfTries);
                if(numOfSuccess + numOfTries == 4)
                {
                    visualBoard.Remove(visualBoard.Length - 1, 1);
                }
                else
                {
                    visualBoard.Append(' ', 7 - (2 * (numOfSuccess + numOfTries)));
                }

                visualBoard.AppendFormat("|" + Environment.NewLine);
                visualBoard.AppendLine(k_Separator);
            }

            for (int i = s_TurnNumber; i < s_MaxAmountOfTurns; i++)
            {
                visualBoard.AppendLine(k_LimitWallSeparator);
                visualBoard.AppendLine(k_Separator);
            }

            Screen.Clear();
            Console.WriteLine(headLine);
            Console.WriteLine(visualBoard);
        }

        private static bool isWonAndNextMove(string io_UserGuess)
        {
            return s_GamePlay.CheckWinningAndUpdateBoard(io_UserGuess);
        }
    }
}
