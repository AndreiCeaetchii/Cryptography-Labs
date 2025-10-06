namespace Lab1;

public class Menu
{
    public static void Start()
    {
        Console.WriteLine("=== Caesar Cipher ===");

        string modeChoice;
        do
        {
            Console.WriteLine("\nChoose cipher mode:");
            Console.WriteLine("1. Normal Caesar (A-Z)");
            Console.WriteLine("2. Custom Caesar (requires key string)");
            Console.Write("Your choice: ");
            modeChoice = Console.ReadLine()?.Trim();

            if (modeChoice != "1" && modeChoice != "2")
            {
                Console.WriteLine("❌ Invalid option. Please choose 1 or 2.");
            }
        } while (modeChoice != "1" && modeChoice != "2");

        CeaserCypher cypher;

        if (modeChoice == "1")
        {
            cypher = new CeaserCypher();
        }
        else
        {
            string customKey;
            do
            {
                Console.Write("Enter custom key string (min 7 letters): ");
                customKey = Console.ReadLine() ?? "";

                if (customKey.Length >= 7) continue;
                Console.WriteLine("❌ Key must be at least 7 letters long. Try again.");
                customKey = "";
            } while (string.IsNullOrEmpty(customKey));

            try
            {
                cypher = new CeaserCypher(customKey);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        string choice;
        do
        {
            Console.WriteLine("\n=== Operation ===");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.Write("Choose option: ");
            choice = Console.ReadLine()?.Trim();

            if (choice != "1" && choice != "2")
            {
                Console.WriteLine("❌ Invalid option. Please choose 1 or 2.");
            }
        } while (choice != "1" && choice != "2");

        int key;
        while (true)
        {
            Console.Write("Enter key (1-25): ");
            var keyInput = Console.ReadLine();

            if (!int.TryParse(keyInput, out key))
            {
                Console.WriteLine("❌ Please enter a valid number between 1 and 25.");
                continue;
            }

            if (key is < 1 or > 25)
            {
                Console.WriteLine("❌ Key must be between 1 and 25.");
                continue;
            }

            break;
        }

        string text;
        do
        {
            Console.Write("Enter text: ");
            text = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("❌ Text cannot be empty. Please enter some text.");
            }
        } while (string.IsNullOrWhiteSpace(text));

        try
        {
            if (choice == "1")
            {
                var encrypted = cypher.Encrypt(key, text);
                Console.WriteLine($"\n🔒 Encrypted: {encrypted}");
            }
            else
            {
                var decrypted = cypher.Decrypt(key, text);
                Console.WriteLine($"\n🔓 Decrypted: {decrypted}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}