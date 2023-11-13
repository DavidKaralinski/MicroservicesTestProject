namespace Core.Extensions;

public static class StringExtensions
{
    private const string SpaceCharacter = " ";

    public static string? RemoveSpacesToLower(this string? sourceString)
    {
        if(sourceString is null)
        {
            return null;
        }

        return sourceString.ToLower().Replace(SpaceCharacter, String.Empty);
    }

    public static bool CompareWith(this string? sourceString, string? comparableString,
         bool ignoreCase = true, bool ignoreWhitespaces = true)
    {
        if(sourceString is null && comparableString is null)
        {
            return true;
        }

        if(sourceString is null || comparableString is null)
        {
            return false;
        }

        return ignoreCase ?
                    ignoreWhitespaces ? sourceString.RemoveSpacesToLower() == comparableString.RemoveSpacesToLower()
                        : sourceString.ToLower() == comparableString.ToLower()
                    : ignoreWhitespaces ? sourceString.RemoveSpacesToLower() == comparableString.RemoveSpacesToLower()
                        : sourceString == comparableString;
    }
}