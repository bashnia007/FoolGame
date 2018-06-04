namespace CommonLibrary
{
    public static class SuitExtension
    {
        public static string Print(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Clubs:
                    return UnicodeToChar("2663");
                case Suit.Diamonds:
                    return "♦";
                case Suit.Hearts:
                    return "♥";
                case Suit.Spades:
                    return UnicodeToChar("2660");
                default: return string.Empty;
            }
        }

        private static string UnicodeToChar(string hex)
        {
            int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            string unicodeString = char.ConvertFromUtf32(code);
            return unicodeString;
        }
    }
}
