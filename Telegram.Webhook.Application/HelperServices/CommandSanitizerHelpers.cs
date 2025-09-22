using System.Text.RegularExpressions;

namespace Telegram.Webhook.Application.HelperServices;

public class CommandSanitizerHelpers
{
    public static bool TryNormalizePhoneNumber(string raw, out string result)
    {
        result = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
            return false;

        // Strip formatting characters
        string clean = Regex.Replace(raw, @"[\s\-\(\)\+]+", "");

        if (clean.StartsWith("00962") || clean.StartsWith("962") || raw.StartsWith("+962"))
            result = NormalizeJordan(clean);
        else if (clean.StartsWith("0020") || clean.StartsWith("20") || raw.StartsWith("+20"))
            result = NormalizeEgypt(clean);
        else if (clean.StartsWith("00968") || clean.StartsWith("968") || raw.StartsWith("+968"))
            result = NormalizeOman(clean);
        else
            result = DetectAndNormalizeLocal(clean);

        return !string.IsNullOrEmpty(result);
    }

    private static string NormalizeJordan(string n)
    {
        // strip any prefix
        if (n.StartsWith("00962")) n = n[5..];
        else if (n.StartsWith("962")) n = n[3..];

        if (n.StartsWith("0")) n = n[1..];

        // must be 9 digits (starts with 7, + country code yields 11 total)
        return Regex.IsMatch(n, @"^7\d{8}$")
           ? "962" + n
           : string.Empty;
    }

    private static string NormalizeEgypt(string n)
    {
        if (n.StartsWith("0020")) n = n[4..];
        else if (n.StartsWith("20")) n = n[2..];

        if (n.StartsWith("0")) n = n[1..];

        return Regex.IsMatch(n, @"^1\d{9}$")
           ? "20" + n
           : string.Empty;
    }

    private static string NormalizeOman(string n)
    {
        if (n.StartsWith("00968")) n = n[5..];
        else if (n.StartsWith("968")) n = n[3..];

        if (n.StartsWith("0")) n = n[1..];

        return Regex.IsMatch(n, @"^[79]\d{7}$")
           ? "968" + n
           : string.Empty;
    }

    private static string DetectAndNormalizeLocal(string n)
    {
        // strip leading zero if any
        if (n.StartsWith("0"))
            n = n[1..];

        // Jordan local
        if (Regex.IsMatch(n, @"^7\d{8}$")) return "962" + n;
        // Egypt local
        if (Regex.IsMatch(n, @"^1\d{9}$")) return "20" + n;
        // Oman local
        if (Regex.IsMatch(n, @"^[79]\d{7}$")) return "968" + n;

        // nothing matched
        return string.Empty;
    }
}
