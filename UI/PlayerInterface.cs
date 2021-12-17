using System;
using System.Text;
using System.Collections.Generic;
using Ex02.ConsoleUtils;
using Logic;

namespace UI
{
    public class PlayerInterface
    {
        private const string k_SeparatorUi = "|=========|=======|";
        private const string k_LimitWallSeparatorUi = "|         |       |";
        private static GamePlay s_GamePlay;
        private static int s_TurnNumber;
        private static int s_MaxAmountOfTurns;

        public static void RunGame()
        {
            string keepPlaying = "Y";
            s_GamePlay = new GamePlay();
            bool isWon;

            while (keepPlaying == "Y")
            {
                isWon = false;

                initiateGameAndUi();
                while (s_TurnNumber < s_MaxAmountOfTurns && !isWon)
                {
                    string userPickedLetters = getUserGuesses();

                    isWon = isWonAndNextMove(userPickedLetters);
                    s_TurnNumber++;
                    updateVisualBoard();
                }

                printWinOrLossMessage(isWon);

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
            Environment.Exit(0);
        }

        private static void initiateGameAndUi()
        {
            Screen.Clear();
            s_MaxAmountOfTurns = pickNumOfTurns();
            s_GamePlay.SetNewGame(s_MaxAmountOfTurns);
            s_TurnNumber = 0;
            updateVisualBoard();
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
                    if(userInput == "Q")
                    {
                        userQuits();
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
                    userQuits();
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
            StringBuilder visualBoard = new StringBuilder();

            visualBoard.AppendLine("|Pins:    |Result:|");
            visualBoard.AppendLine(k_SeparatorUi);
            appendHiddenOrShownAnswerLine(visualBoard);
            appendUsedPinsAndResultLines(visualBoard);
            appendBlankPinsAndResultLines(visualBoard);
            Screen.Clear();
            Console.WriteLine(headLine);
            Console.WriteLine(visualBoard);
        }

        private static void appendHiddenOrShownAnswerLine(StringBuilder io_VisualBoard)
        {
            if (s_TurnNumber == s_MaxAmountOfTurns)
            {
                char[] computerChoice = s_GamePlay.ChosenLetters.ToArray();

                io_VisualBoard.AppendFormat("| {0} {1} {2} {3} |       |{4}",
                    computerChoice[0],
                    computerChoice[1],
                    computerChoice[2],
                    computerChoice[3],
                    Environment.NewLine);
            }
            else
            {
                io_VisualBoard.AppendLine("| # # # # |       |");
            }

            io_VisualBoard.AppendLine(k_SeparatorUi);
        }

        private static void appendUsedPinsAndResultLines(StringBuilder io_VisualBoard)
        {
            List<Guess> currentBoard = s_GamePlay.GameBoard;

            for (int i = 0; i < s_TurnNumber; i++)
            {
                Guess currentGuess = currentBoard[i];
                string boardPinsRow = currentGuess.GuessPins;
                int numOfSuccess = currentGuess.NumOfRightPositionLetters;
                int numOfTries = currentGuess.NumOfIncorrectPositionLetters;

                io_VisualBoard.AppendFormat("| {0} {1} {2} {3} |", boardPinsRow[0], boardPinsRow[1], boardPinsRow[2], boardPinsRow[3]);
                io_VisualBoard.Insert(io_VisualBoard.Length, "V ", numOfSuccess);
                io_VisualBoard.Insert(io_VisualBoard.Length, "X ", numOfTries);
                if (numOfSuccess + numOfTries == 4)
                {
                    io_VisualBoard.Remove(io_VisualBoard.Length - 1, 1);
                }
                else
                {
                    io_VisualBoard.Append(' ', 7 - (2 * (numOfSuccess + numOfTries)));
                }

                io_VisualBoard.AppendFormat("|" + Environment.NewLine);
                io_VisualBoard.AppendLine(k_SeparatorUi);
            }
        }

        private static void appendBlankPinsAndResultLines(StringBuilder io_VisualBoard)
        {
            for (int i = s_TurnNumber; i < s_MaxAmountOfTurns; i++)
            {
                io_VisualBoard.AppendLine(k_LimitWallSeparatorUi);
                io_VisualBoard.AppendLine(k_SeparatorUi);
            }
        }

        private static void printWinOrLossMessage(bool i_isWon)
        {
            if(i_isWon)
            {
                string winningMsg = string.Format("You guessed after {0} steps!", s_TurnNumber);

                Console.WriteLine(winningMsg);
            }
            else
            {
                Console.WriteLine("No more guesses allowed. You Lost.");
            }
        }

        private static bool isWonAndNextMove(string io_UserGuess)
        {
            return s_GamePlay.CheckWinningAndUpdateBoard(io_UserGuess);
        }
    }
}
