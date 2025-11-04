namespace Lab4;

class Program
{
    // Expansion Permutation (E-box) - expands 32-bit Ri-1 to 48 bits
    static readonly int[] E =
    {
        32, 1, 2, 3, 4, 5,
        4, 5, 6, 7, 8, 9,
        8, 9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32, 1
    };

    public static void Main(string[] args)
    {
        Start();
    }

    public static void Start()
    {
        Console.WriteLine("=== DES B1B2...B8 Calculator ===");
        Console.WriteLine("This program calculates B1B2B3B4B5B6B7B8 for round i given Ki and Ri-1\n");

        string ri1Binary;
        string kiBinary;

        Console.WriteLine("Choose input method:");
        Console.WriteLine("1. Random generation");
        Console.WriteLine("2. Manual input");
        Console.Write("Enter choice (1 or 2): ");
        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            // Random generation
            ri1Binary = GenerateRandomBinary(32);
            kiBinary = GenerateRandomBinary(48);

            Console.WriteLine("\n--- Randomly Generated Values ---");
            Console.WriteLine($"\nRi-1 (32 bits, hex): {BinaryToHex(ri1Binary)}");
            DisplayBinaryAs2DArray(ri1Binary, "Ri-1 (32 bits)", 8);
            
            Console.WriteLine($"\nKi (48 bits, hex): {BinaryToHex(kiBinary)}");
            DisplayBinaryAs2DArray(kiBinary, "Ki (48 bits)", 8);
        }
        else
        {
            // Manual input
            Console.WriteLine("\n--- Manual Input ---");
            Console.Write("Enter Ri-1 (32 bits binary or 8 hex chars): ");
            string ri1Input = Console.ReadLine().Trim();
            ri1Binary = ParseInput(ri1Input, 32);

            Console.Write("Enter Ki (48 bits binary or 12 hex chars): ");
            string kiInput = Console.ReadLine().Trim();
            kiBinary = ParseInput(kiInput, 48);
        }

        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("STEP-BY-STEP CALCULATION OF B1B2B3B4B5B6B7B8");
        Console.WriteLine(new string('=', 70));

        // Step 1: Apply E-box expansion to Ri-1
        Console.WriteLine("\nSTEP 1: Apply E-box expansion to Ri-1");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("The E-box expands 32 bits of Ri-1 to 48 bits by duplication");
        Console.WriteLine($"\nInput to E-box (32 bits, hex): {BinaryToHex(ri1Binary)}");
        DisplayBinaryAs2DArray(ri1Binary, "Ri-1 (32 bits)", 8);

        string expandedRi1 = Permute(ri1Binary, E, 48);

        Console.WriteLine($"\nOutput from E-box (48 bits, hex): {BinaryToHex(expandedRi1)}");
        DisplayBinaryAs2DArray(expandedRi1, "E(Ri-1) (48 bits)", 8);

        // Step 2: XOR with Ki
        Console.WriteLine("\n\nSTEP 2: XOR E(Ri-1) with Ki");
        Console.WriteLine("----------------------------");
        Console.WriteLine("XOR (exclusive OR) operation: 0⊕0=0, 0⊕1=1, 1⊕0=1, 1⊕1=0\n");
        
        Console.WriteLine($"E(Ri-1) (48 bits, hex): {BinaryToHex(expandedRi1)}");
        DisplayBinaryAs2DArray(expandedRi1, "E(Ri-1)", 8);
        
        Console.WriteLine($"\nKi (48 bits, hex): {BinaryToHex(kiBinary)}");
        DisplayBinaryAs2DArray(kiBinary, "Ki", 8);

        string bResult = XOR(expandedRi1, kiBinary);

        Console.WriteLine($"\nB = E(Ri-1) ⊕ Ki (48 bits, hex): {BinaryToHex(bResult)}");
        DisplayBinaryAs2DArray(bResult, "B1B2B3B4B5B6B7B8 (Result)", 8);
        
        // Step 3: Split into B1, B2, ..., B8 (each 6 bits)
        Console.WriteLine("\n\nSTEP 3: Split result into B1, B2, ..., B8");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Each Bi contains 6 bits (input to S-box i)\n");
        
        for (int i = 0; i < 8; i++)
        {
            string bi = bResult.Substring(i * 6, 6);
            Console.WriteLine($"B{i + 1} = {bi} (bits {i * 6 + 1}-{i * 6 + 6})");
        }

        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }

    static void DisplayBinaryAs2DArray(string binary, string label, int cols)
    {
        Console.WriteLine($"\n{label}:");
        
        // Calculate rows needed
        int rows = (int)Math.Ceiling((double)binary.Length / cols);
        
        // Top border
        Console.Write("┌");
        for (int i = 0; i < cols; i++)
        {
            Console.Write("────────");
            if (i < cols - 1) Console.Write("┬");
        }
        Console.WriteLine("┐");
        
        // Display rows
        for (int row = 0; row < rows; row++)
        {
            Console.Write("│");
            for (int col = 0; col < cols; col++)
            {
                int index = row * cols + col;
                if (index < binary.Length)
                {
                    Console.Write($"   {binary[index]}    │");
                }
                else
                {
                    Console.Write("        │");
                }
            }
            Console.WriteLine();
            
            if (row < rows - 1)
            {
                Console.Write("├");
                for (int i = 0; i < cols; i++)
                {
                    Console.Write("────────");
                    if (i < cols - 1) Console.Write("┼");
                }
                Console.WriteLine("┤");
            }
        }
        
        // Bottom border
        Console.Write("└");
        for (int i = 0; i < cols; i++)
        {
            Console.Write("────────");
            if (i < cols - 1) Console.Write("┴");
        }
        Console.WriteLine("┘");
    }

    static string GenerateRandomBinary(int length)
    {
        Random rand = new Random();
        char[] binary = new char[length];
        for (int i = 0; i < length; i++)
        {
            binary[i] = rand.Next(2) == 0 ? '0' : '1';
        }

        return new string(binary);
    }

    static string ParseInput(string input, int expectedBits)
    {
        input = input.Replace(" ", "").ToUpper();

        // Check if it's binary
        if (input.All(c => c == '0' || c == '1'))
        {
            if (input.Length != expectedBits)
            {
                throw new ArgumentException($"Binary input must be exactly {expectedBits} bits");
            }

            return input;
        }

        // Assume it's hex
        if (input.Length != expectedBits / 4)
        {
            throw new ArgumentException($"Hex input must be exactly {expectedBits / 4} characters");
        }

        return HexToBinary(input);
    }

    static string HexToBinary(string hex)
    {
        string binary = "";
        foreach (char c in hex)
        {
            binary += c switch
            {
                '0' => "0000", '1' => "0001", '2' => "0010", '3' => "0011",
                '4' => "0100", '5' => "0101", '6' => "0110", '7' => "0111",
                '8' => "1000", '9' => "1001", 'A' => "1010", 'B' => "1011",
                'C' => "1100", 'D' => "1101", 'E' => "1110", 'F' => "1111",
                _ => throw new ArgumentException($"Invalid hex character: {c}")
            };
        }

        return binary;
    }

    static string BinaryToHex(string binary)
    {
        // Pad if necessary
        while (binary.Length % 4 != 0)
        {
            binary = "0" + binary;
        }

        string hex = "";
        for (int i = 0; i < binary.Length; i += 4)
        {
            string chunk = binary.Substring(i, 4);
            hex += chunk switch
            {
                "0000" => "0", "0001" => "1", "0010" => "2", "0011" => "3",
                "0100" => "4", "0101" => "5", "0110" => "6", "0111" => "7",
                "1000" => "8", "1001" => "9", "1010" => "A", "1011" => "B",
                "1100" => "C", "1101" => "D", "1110" => "E", "1111" => "F",
                _ => "?"
            };
        }

        return hex;
    }

    static string Permute(string input, int[] table, int n)
    {
        char[] permutation = new char[n];
        for (int i = 0; i < n; i++)
        {
            permutation[i] = input[table[i] - 1];
        }

        return new string(permutation);
    }

    static string XOR(string a, string b)
    {
        char[] result = new char[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] == b[i] ? '0' : '1';
        }

        return new string(result);
    }
}