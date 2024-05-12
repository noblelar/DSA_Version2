using System;
namespace DAS_Coursework.utils
{
	public static class Parser
	{
        public static int AcceptIntegerInformation()
        {
            bool isValid = false;
            int answer = 0;

            while (isValid != true)
            {
                string response = Console.ReadLine();
                isValid = int.TryParse(response, out answer);

                if (isValid != true)
                {
                    Console.WriteLine("Please provide a valid answer (integer)");
                }
                else
                {
                    break;
                }
            }
            return answer;
        }

        public static double AcceptDoubleInformation()
        {
            bool isValid = false;
            double answer = 0;

            while (isValid != true)
            {
                string response = Console.ReadLine();
                isValid = double.TryParse(response, out answer);

                if (isValid != true)
                {
                    Console.WriteLine("Please provide a valid answer (decimal)");
                }
                else
                {
                    break;
                }
            }
            return answer;
        }

        public static bool AcceptBooleanInformation()
        {
            bool isValid = false;
            bool answer = false;

            while (isValid != true)
            {
                string response = Console.ReadLine();
                isValid = bool.TryParse(response, out answer);

                if (isValid != true)
                {
                    Console.WriteLine("Please provide a valid answer (TRUE / FALSE)");
                }
                else
                {
                    break;
                }
            }
            return answer;
        }
    }
}

