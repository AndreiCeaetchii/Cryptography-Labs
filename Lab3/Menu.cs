using System.Text;
using System.Text.RegularExpressions;

namespace Lab3;

public abstract partial class Menu
{
    public static void Start()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Vigenere Cipher ===");

        while (true)
        {
            string choice;
            do
            {
                Console.WriteLine("\n=== Operation ===");
                Console.WriteLine("1. Encrypt");
                Console.WriteLine("2. Decrypt");
                Console.WriteLine("3. Exit");
                Console.Write("Your choice: ");
                choice = Console.ReadLine()?.Trim();

                if (choice != "1" && choice != "2" && choice != "3")
                {
                    Console.WriteLine("❌ Invalid option. Please choose 1, 2, or 3.");
                }
            } while (choice != "1" && choice != "2" && choice != "3");

            if (choice == "3")
            {
                Console.WriteLine("Exiting...");
                break;
            }

            string key;
            while (true)
            {
                Console.Write("\nEnter key (Romanian letters only, min 7 characters): ");
                key = Console.ReadLine() ?? "";

                if (key.Length < 7)
                {
                    Console.WriteLine("❌ Key must be at least 7 letters long. Try again.");
                    continue;
                }

                if (!MyRegex().IsMatch(key.ToUpper(new System.Globalization.CultureInfo("ro-RO"))))
                {
                    Console.WriteLine("❌ Key must contain only Romanian letters (A–Z, Ă, Â, Î, Ș, Ț).");
                    continue;
                }

                break;
            }

            string text;
            while (true)
            {
                Console.Write("\nEnter text: ");
                text = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("❌ Text cannot be empty. Please enter some text.");
                    continue;
                }

                break;
            }

            var cipher = new VigenereCipher();

            try
            {
                if (choice == "1")
                {
                    var encrypted = cipher.Encrypt(key, text);
                    Console.WriteLine($"\n🔒 Encrypted: {encrypted}");
                }
                else
                {
                    var decrypted = cipher.Decrypt(key, text);
                    Console.WriteLine($"\n🔓 Decrypted: {decrypted}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress ENTER to continue or type 'exit' to quit.");
            var exitInput = Console.ReadLine()?.Trim().ToLower();
            if (exitInput != "exit") continue;
            Console.WriteLine("Goodbye!");
            break;
        }
    }

    [GeneratedRegex(@"^[A-ZĂÂÎȘȚ]+$")]
    private static partial Regex MyRegex();
}