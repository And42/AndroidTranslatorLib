using System.Text;

namespace AndroidTranslator.Utils
{
    internal static class StringUtils
    {
        public static string FormatLineEndings(string input)
        {
            int rIndex = input.IndexOf('\r');

            if (rIndex == -1)
                return input;

            var builder = new StringBuilder(input.Length);

            int previous = 0;

            do
            {
                builder.Append(input, previous, rIndex - previous);

                int nextIndex = rIndex + 1;

                if (nextIndex == input.Length || input[nextIndex] != '\n')
                    builder.Append('\n');

                previous = nextIndex;
                rIndex = input.IndexOf('\r', nextIndex);
            } while (rIndex != -1);

            if (previous != input.Length)
                builder.Append(input, previous, input.Length - previous);

            return builder.ToString();
        }
    }
}
