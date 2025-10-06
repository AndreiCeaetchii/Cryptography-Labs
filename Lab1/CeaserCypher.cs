using System.Text;
using System.Text.RegularExpressions;

namespace Lab1;

public partial class CeaserCypher
{
    private readonly Dictionary<int, char> _alphabet;

    public CeaserCypher()
    {
        _alphabet = new Dictionary<int, char>();
        for (var i = 0; i < 26; i++)
        {
            _alphabet[i] = (char)('A' + i);
        }
    }

    public CeaserCypher(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Length < 7)
        {
            throw new ArgumentException("Key must be at least 7 letters long.");
        }

        key = key.ToUpper();
        if (!MyRegex().IsMatch(key))
        {
            throw new ArgumentException("Key must contain only letters A-Z.");
        }

        var keyChars = key.Distinct().ToList();
        var fullAlphabet = keyChars
            .Concat(Enumerable.Range('A', 26).Select(x => (char)x).Where(c => !keyChars.Contains(c))).ToList();
        _alphabet = new Dictionary<int, char>();
        for (var i = 0; i < fullAlphabet.Count; i++)
        {
            _alphabet[i] = fullAlphabet[i];
        }
    }

    private static string PrepareText(string text)
    {
        text = text.ToUpper().Replace(" ", "");
        return !MyRegex().IsMatch(text) ? throw new ArgumentException("Invalid input. Only letters A-Z are allowed.") : text;
    }

    public string Encrypt(int key, string plaintext)
    {
        plaintext = PrepareText(plaintext);
        var result = new StringBuilder();
        foreach (var newIndex in plaintext.Select(c => _alphabet.First(x => x.Value == c).Key).Select(originalIndex => (originalIndex + key) % 26))
        {
            result.Append(_alphabet[newIndex]);
        }

        return result.ToString();
    }

    public string Decrypt(int key, string ciphertext)
    {
        ciphertext = PrepareText(ciphertext);
        var result = new StringBuilder();
        foreach (var newIndex in ciphertext.Select(c => _alphabet.First(x => x.Value == c).Key).Select(originalIndex => (originalIndex - key + 26) % 26))
        {
            result.Append(_alphabet[newIndex]);
        }

        return result.ToString();
    }

    [GeneratedRegex(@"^[A-Z]+$")]
    private static partial Regex MyRegex();
}