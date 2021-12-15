using System;
using System.Text;
using System.Collections.Generic;
using Ex02.ConsoleUtils;
using Logic;

namespace PlayerInterface
{
    public class PlayerInterface
    {
        private static Board m_Board;
        private static GamePlay m_GamePlay;
        private static int m_TurnNumber;
        private static int m_MaxAmountOfTurns;

        public static void Main()
        {
            RunGame();
        }

        public static void RunGame()
        {
            StringBuilder userPickedLetters;
            int foundNotInPlace;
            int foundInPlace;
            string keepPlaying = "Y";
            bool won;

            while (keepPlaying == "Y")
            {
                Screen.Clear();
                won = false;
                m_MaxAmountOfTurns = pickNumOfTurns();
                if (m_MaxAmountOfTurns == -1)  ///user pressed Q.  
                {
                    userQuits();
                    break;
                }

                m_GamePlay = new GamePlay();
                m_Board = new Board(m_MaxAmountOfTurns);
                m_TurnNumber = 0;
                updateVisualBoard();
                m_TurnNumber++;
                while (m_TurnNumber <= m_MaxAmountOfTurns && !won)
                {
                    userPickedLetters = getUserGuesses();
                    if(userPickedLetters == null) //// user pressed Q.
                    {
                        userQuits();
                        keepPlaying = "N";
                        break;
                    }

                    foundNotInPlace = m_GamePlay.LettersNotInRightPlace(userPickedLetters.ToString());
                    foundInPlace = m_GamePlay.LettersInRightPlace(userPickedLetters.ToString());
                    won = m_GamePlay.HasWon(userPickedLetters.ToString());
                    updateBoard(userPickedLetters, foundInPlace, foundNotInPlace);
                    updateVisualBoard();
                    m_TurnNumber++;
                }

                if (keepPlaying != "N" || m_MaxAmountOfTurns == -1) // player didnt press 'Q' while picking maxTurns or letters.
                {
                    if (won)
                    {
                        string WinningMsg = string.Format("You guessed after {0} steps!", m_TurnNumber);
                        Console.WriteLine(WinningMsg);
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
            string userInput;
            bool validInput = false;
            const string turnsPhrase = "Please enter how many turns would you like to play(4 - 10) or 'Q' to quit: ";

            Console.WriteLine(turnsPhrase);
            while(!validInput)
            {
                userInput = Console.ReadLine();
                validInput = int.TryParse(userInput, out numOfTurns);
                if(validInput)
                {
                    if(numOfTurns > 10 || numOfTurns < 4)
                    {
                        Console.WriteLine("Invalid Input! the given number is out of bounds");
                        Console.WriteLine(turnsPhrase);
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
                    Console.WriteLine(turnsPhrase);
                }
            }

            return numOfTurns;
        }

        private static StringBuilder getUserGuesses()
        {
            StringBuilder userPickedLetters = new StringBuilder();
            string userInput = string.Empty;
            bool invalidInput;
            List<char> availableOption = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            //List<char> lettersPicked = new List<char>(4);

            Console.WriteLine("Please type your next guess <A-H> or 'Q' to quit:");
            while (userPickedLetters.Length != 4)
            {
                userInput = Console.ReadLine();
                invalidInput = false;
                userInput = userInput.Replace(" ", string.Empty);
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
                            //lettersPicked.Add(letter);
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
                        //lettersPicked.Clear();
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

            return userPickedLetters;
        }

        private static void updateVisualBoard()
        {
            string headLine = string.Format("Current board status:{0}", Environment.NewLine);
            const string seperator = "|=========|=======|";
            const string limitWallSeperator = "|         |       |";
            StringBuilder visualBoard = new StringBuilder();
            string[] currentBoard = m_Board.GameBoard;
            char[] boardPinsRow;
            string boardResultRow;
            int numOfSuccess;
            int numOfTries;

            visualBoard.AppendLine("|Pins:    |Result:|");
            visualBoard.AppendLine(seperator);
            if(m_TurnNumber == m_MaxAmountOfTurns)
            {
                char[] computerChoice = m_GamePlay.chosenLetters.ToArray(); 
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

            visualBoard.AppendLine(seperator);

            for (int i = 0; i < m_TurnNumber; i++)
            {
                boardPinsRow = currentBoard[i].ToCharArray(0, 4);
                visualBoard.AppendFormat("| {0} {1} {2} {3} |", boardPinsRow[0], boardPinsRow[1], boardPinsRow[2], boardPinsRow[3]);
                boardResultRow = currentBoard[i].Substring(4);
                numOfSuccess = int.Parse(boardResultRow[0].ToString());
                visualBoard.Insert(visualBoard.Length, "V ", numOfSuccess);
                numOfTries = int.Parse(boardResultRow[1].ToString());
                visualBoard.Insert(visualBoard.Length, "X ", numOfTries);
                if(numOfTries + numOfSuccess == 4)
                {
                    visualBoard.Remove(visualBoard.Length - 1, 1);
                }
                else
                {
                    visualBoard.Append(' ', 7 - (numOfSuccess + numOfTries) * 2);
                }

                visualBoard.AppendFormat("|" + Environment.NewLine);
                visualBoard.AppendLine(seperator);
            }

            for (int i = m_TurnNumber; i < m_MaxAmountOfTurns; i++)
            {
                visualBoard.AppendLine(limitWallSeperator);
                visualBoard.AppendLine(seperator);
            }

            Screen.Clear();
            Console.WriteLine(headLine);
            Console.WriteLine(visualBoard);
        }

        private static void updateBoard(StringBuilder userPickedLetters, int foundInPlace, int foundNotInPlace)
        {
            m_Board.UpdateRow(userPickedLetters.ToString(), foundInPlace, foundNotInPlace);
        }
    }
}
