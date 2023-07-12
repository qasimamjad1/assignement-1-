
using System;

class Program
{
    static void Main()
    {
        int[] numbers = { 2, 5, 7, 10, 12, 15, 18, 20, 21, 24 };
        int largestOdd = FindLargestOdd(numbers);
        int largestEven = FindLargestEven(numbers);

        int sum = largestOdd + largestEven;
        Console.WriteLine("Sum of largest odd and largest even: " + sum);
    }

    static int FindLargestOdd(int[] numbers)
    {
        int largestOdd = int.MinValue;
        foreach (int number in numbers)
        {
            if (number % 2 != 0 && number > largestOdd)
            {
                largestOdd = number;
            }
        }
        return largestOdd;
    }

    static int FindLargestEven(int[] numbers)
    {
        int largestEven = int.MinValue;
        foreach (int number in numbers)
        {
            if (number % 2 == 0 && number > largestEven)
            {
                largestEven = number;
            }
        }
        return largestEven;
    }
}
