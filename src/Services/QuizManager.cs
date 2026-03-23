using System;
using LabiryntMatematyczny.Models;

namespace LabiryntMatematyczny.Services
{
    public class QuizManager
    {
        private Random random = new Random();

        public MathQuestion GenerateQuestion(int levelIndex)
        {
            int num1, num2, num3, answer;
            string questionText;

            //poziom 1 - dodawanie
            if (levelIndex == 0)
            {
                num1 = random.Next(1, 11);
                num2 = random.Next(1, 11);
                answer = num1 + num2;
                questionText = $"Ile to jest {num1} + {num2}?";
            }
            // poziom 2 - dodawanie, odejmowanie
            else if (levelIndex == 1)
            {
                num1 = random.Next(1, 30);
                num2 = random.Next(1, 20);

                // 0 to dodawanie, 1 to odejmowanie
                if (random.Next(0, 2) == 0)
                {
                    answer = num1 + num2;
                    questionText = $"Ile to jest {num1} + {num2}?";
                }
                else
                {
                    if (num2 > num1)
                    {
                        int temp = num1;
                        num1 = num2;
                        num2 = temp;
                    }
                    answer = num1 - num2;
                    questionText = $"Ile to jest {num1} - {num2}?";
                }
            }
            //poziom 3 - mnozenie 
            else if (levelIndex == 2)
            {
                num1 = random.Next(2, 10);
                num2 = random.Next(2, 10);
                answer = num1 * num2;
                questionText = $"Ile to jest {num1} * {num2}?";
            }
            //poziom 4 - mnozenie i dodawanie
            else if (levelIndex == 3)
            {
                num1 = random.Next(2, 9);
                num2 = random.Next(2, 9);
                num3 = random.Next(1, 50);

                answer = (num1 * num2) + num3;
                questionText = $"Ile to jest {num1} * {num2} + {num3}?";
            }
            //poziom 5 - wszystko 
            else
            {
                if (random.Next(0, 2) == 0)
                {
                    num1 = random.Next(3, 10);
                    num2 = random.Next(3, 10);
                    num3 = random.Next(1, 20);

                    answer = (num1 * num2) - num3;
                    questionText = $"Ile to jest {num1} * {num2} - {num3}?";
                }
                else
                {
                    // Przygotowanie dzielenia bez reszty
                    num2 = random.Next(2, 10); 
                    int wynikDzielenia = random.Next(2, 10);
                    num1 = num2 * wynikDzielenia; 

                    num3 = random.Next(1, 100); 

                    answer = wynikDzielenia + num3;
                    questionText = $"Ile to jest {num1} / {num2} + {num3}?";
                }
            }
            return new MathQuestion(questionText, answer);
        }
    }
}