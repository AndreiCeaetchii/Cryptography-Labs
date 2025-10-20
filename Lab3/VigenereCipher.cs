using System.Text;
using System.Text.RegularExpressions;

namespace Lab3;

public partial class VigenereCipher
{
    private readonly Dictionary<char, int> _alphabet;

    public VigenereCipher()
    {
        _alphabet = new Dictionary<char, int>();
        const string letters = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";

        for (var i = 0; i < letters.Length; i++)
        {
            _alphabet[letters[i]] = i;
        }
    }

    private static string PrepareText(string text)
    {
        text = text.ToUpper(new System.Globalization.CultureInfo("ro-RO")).Replace(" ", "");

        return !MyRegex().IsMatch(text) ? throw new ArgumentException("Invalid input. Only Romanian letters are allowed.") : text;
    }

    public string Encrypt(string key, string plaintext)
    {
        plaintext = PrepareText(plaintext);
        key = PrepareText(key);

        var result = new StringBuilder();

        for (var i = 0; i < plaintext.Length; i++)
        {
            var currentLetter = key[i % key.Length];

            var indexToAdd = _alphabet[currentLetter];
            var originalIndex = _alphabet[plaintext[i]];

            var newIndex = (originalIndex + indexToAdd) % _alphabet.Count;
            var newLetter = _alphabet.First(x => x.Value == newIndex).Key;

            result.Append(newLetter);
        }

        return result.ToString();
    }

    public string Decrypt(string key, string ciphertext)
    {
        ciphertext = PrepareText(ciphertext);
        key = PrepareText(key);

        var result = new StringBuilder();

        for (var i = 0; i < ciphertext.Length; i++)
        {
            var currentLetter = key[i % key.Length];

            var indexToSubtract = _alphabet[currentLetter];
            var encryptedIndex = _alphabet[ciphertext[i]];

            var newIndex = (encryptedIndex - indexToSubtract + _alphabet.Count) % _alphabet.Count;
            var newLetter = _alphabet.First(x => x.Value == newIndex).Key;

            result.Append(newLetter);
        }

        return result.ToString();
    }

    [GeneratedRegex(@"^[A-ZĂÂÎȘȚ]*$")]
    private static partial Regex MyRegex();
}