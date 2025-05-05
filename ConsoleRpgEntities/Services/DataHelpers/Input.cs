using System.Diagnostics;

namespace ConsoleRpgEntities.Services.DataHelpers;

// The DataHelper.Input class contains a few methods and overrides that assist in gaining input from the user that includes different levels of input validation.
class Input
{
    /// <summary>
    /// Asks the user a question and returns an int.
    /// </summary>
    /// <param name="question">The question asked to the user.</param>
    /// <returns><strong>Int</strong> provided by the user.</returns>
    public static int GetInt(string question)
    {
        int response;
        do
        {
            Console.Write(question);
            try
            {
                response = Convert.ToInt32(Console.ReadLine());
                return response;
            }
            catch (FormatException)
            {
                Console.WriteLine("That number is not valid. Please try again. (Please enter a valid integer)");
                continue;
            }
            catch (OverflowException)
            {
                Console.WriteLine("That number is not valid. Please try again. (That number is either too big or too small)");
                continue;
            }
        } while (true);
    }

    /// <summary>
    /// Asks the user a question and returns an int with a minumum value.
    /// </summary>
    /// <param name="question">The question asked to the user.</param>
    /// <param name="minValue">Minimum value allowed to be returned by the user.</param>
    /// <param name="minValueErrorMessage">Error message to be returned to the user if the value is less than the minimum threshold.</param>
    /// <returns><strong>Int</strong> provided by the user that falls in the parameters.</returns>
    public static int GetInt(string question, int minValue, string minValueErrorMessage)
    {
        int response;
        do
        {
            response = GetInt(question);
            if (response < minValue)
            {
                Console.WriteLine($"That number is not valid. Please try again. ({minValueErrorMessage})");
                continue;
            }
            else
            {
                return response;
            }
        } while (true);
    }

    /// <summary>
    /// Asks the user a question and returns an int with a minumum and maximum value.
    /// </summary>
    /// <param name="question">The question asked to the user.</param>
    /// <param name="minValue">Minimum value allowed to be returned by the user.</param>
    /// <param name="maxValue">Maximum value allowed to be returned by the user.</param>
    /// <param name="errorMessage">Error message to be returned to the user if the value is less than the minimum threshold or greater than the maximum threshold.</param>
    /// <returns><strong>Int</strong> provided by the user that falls in the parameters.</returns>
    public static int GetInt(string question, int minValue, int maxValue, string errorMessage)
    {
        int response;
        do
        {
            response = GetInt(question, minValue, errorMessage);
            if (response > maxValue)
            {
                Console.WriteLine($"That number is not valid. Please try again. ({errorMessage})");
                continue;
            }
            else
            {
                return response;
            }
        } while (true);
    }

    /// <summary>
    /// Asks the user a question and returns an int with a minumum and maximum value.
    /// </summary>
    /// <param name="question">The question asked to the user.</param>
    /// <param name="minValue">Minimum value allowed to be returned by the user.</param>
    /// <param name="maxValue">Maximum value allowed to be returned by the user.</param>
    /// <param name="minValueErrorMessage">Error message to be returned to the user if the value is less than the minimum threshold.</param>
    /// <param name="maxValueErrorMessage">Error message to be returned to the user if the value is greater than the maximum threshold.</param>
    /// <returns><strong>Int</strong> provided by the user that falls in the parameters.</returns>
    public static int GetInt(string question, int minValue, int maxValue, string minValueErrorMessage, string maxValueErrorMessage)
    {
        int response;
        do
        {
            response = GetInt(question, minValue, minValueErrorMessage);
            if (response > maxValue)
            {
                Console.WriteLine($"That number is not valid. Please try again. ({maxValueErrorMessage})");
                continue;
            }
            else
            {
                return response;
            }
        } while (true);
    }

    /// <summary>
    /// Prompts the user without a question and returns an int with a minumum and maximum value.
    /// </summary>
    /// <param name="minValue">Minimum value allowed to be returned by the user.</param>
    /// <param name="maxValue">Maximum value allowed to be returned by the user.</param>
    /// <param name="minValueErrorMessage">Error message to be returned to the user if the value is less than the minimum threshold.</param>
    /// <param name="maxValueErrorMessage">Error message to be returned to the user if the value is greater than the maximum threshold.</param>
    /// <returns><strong>Int</strong> provided by the user that falls in the parameters.</returns>
    public static int GetInt(int minValue, int maxValue, string minValueErrorMessage, string maxValueErrorMessage)
    {
        int response;
        do
        {
            response = GetInt("", minValue, minValueErrorMessage);
            if (response > maxValue)
            {
                Console.WriteLine($"That number is not valid. Please try again. ({maxValueErrorMessage})");
                continue;
            }
            else
            {
                return response;
            }
        } while (true);
    }

    /// <summary>
    /// Prompts the user without a question and returns an int with a minumum and maximum value.
    /// </summary>
    /// <param name="minValue">Minimum value allowed to be returned by the user.</param>
    /// <param name="maxValue">Maximum value allowed to be returned by the user.</param>
    /// <param name="valueErrorMessage">Error message to be returned to the user if the value is less than the minimum threshold or greater than the maximum threshold.</param>
    /// <returns><strong>Int</strong> provided by the user that falls in the parameters.</returns>
    public static int GetInt(int minValue, int maxValue, string valueErrorMessage)
    {
        int response;
        do
        {
            response = GetInt("", minValue, valueErrorMessage);
            if (response > maxValue)
            {
                Console.WriteLine($"That number is not valid. Please try again. ({valueErrorMessage})");
                continue;
            }
            else
            {
                return response;
            }
        } while (true);
    }

    /// <summary>
    /// Prompts the user with a question and returns a string.
    /// </summary>
    /// <param name="prompt">Question asked to the user.</param>
    /// <returns><strong>nullable string</strong> provided by the user.</returns>
    public static string GetString(string prompt)
    {
        string? response = GetString(prompt, true);
        return response;
    }

    /// <summary>
    /// Prompts the user with a question and returns a string with an option to set if an input is required.
    /// </summary>
    /// <param name="prompt">Question asked to the user.</param>
    /// <param name="entryRequired">If true, response cannot be null or blank.</param>
    /// <returns><strong>nullable string</strong> provided by the user that falls within the parameters.</returns>
    public static string GetString(string prompt, bool entryRequired)
    {
        Console.Write(prompt);
        string? response = Console.ReadLine();

        while (response == null || response == "" && entryRequired)
        {
            Console.WriteLine("Please enter an allowed value. (Value cannot be blank)");
            Console.Write(prompt + " ");
            response = Console.ReadLine();
        }

        return response;
    }

    /// <summary>
    /// Prompts the user with a question but restricts the answers to a set list.
    /// </summary>
    /// <param name="prompt">Question asked to the user.</param>
    /// <param name="allowedResponsesList">A list of allowed responses.</param>
    /// <returns><strong>nullable string</strong> provided by the user that falls within the parameters.</returns>
    public static string GetString(string prompt, List<string> allowedResponsesList)
    {
        string? response;

        do
        {
            Console.Write(prompt);
            response = Console.ReadLine();

            if (IsNull(response, out string errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            if (IsEmpty(response, out errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            if (!IsResponseAllowed(response, allowedResponsesList, out errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            break;
        } while (true);

        return response!.ToUpper();
    }

    /// <summary>
    /// Prompts the user with a question but restricts the answers to "y" or "n"
    /// </summary>
    /// <param name="prompt">Question asked to the user.</param>
    /// <returns>returns "Y" or "N" dpending on user input.</returns>
    public static bool GetYN(string prompt)
    {
        List<string> allowedResponsesList = ["y", "n"];
        string? response;

        do
        {
            Console.Write(prompt);
            response = Console.ReadLine();

            if (IsNull(response, out string errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            if (IsEmpty(response, out errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            if (!IsResponseAllowed(response, allowedResponsesList, out errorMessage))
            {
                InvalidValue(errorMessage);
                continue;
            }

            break;
        } while (true);

        return response!.ToUpper() switch
        {
            "Y" => true,
            "N" => false,
            _ => throw new UnreachableException("Unreachable response triggered. (\"y\" or \"n\" allowed.")
        };
    }

    private static bool IsResponseAllowed(string? response, List<string> allowedResponsesList, out string errorMessage)
    {
        foreach (string allowedResponse in allowedResponsesList)
        {
            if (string.Equals(response, allowedResponse, StringComparison.InvariantCultureIgnoreCase))
            { 
                errorMessage = " Please enter an allowed value.";
                return true;
            }
        }
        errorMessage = " Please enter an allowed value.";
        return false;
    }

    private static bool IsEmpty(string? value)
    {
        return value == "";
    }

    private static bool IsEmpty(string? value, out string errorMessage)
    {
        errorMessage = IsEmpty(value) switch
        {
            true => " The value you entered was empty.",
            false => " The value you entered was not empty."
        };

        return IsEmpty(value);
    }

    private static bool IsNull(string? value)
    {
        return value == null;
    }

    private static bool IsNull(string? value, out string errorMessage)
    {
        errorMessage = IsNull(value) switch
        {
            true => " The value you entered was null.",
            false => " The value you entered was not null."
        };
        
        return IsNull(value);
    }

    private static void InvalidValue(string errorMessage)
    {
        Console.WriteLine($"Invalid value." + errorMessage);
    }

}
