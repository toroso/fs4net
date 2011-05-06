namespace fs4net.Framework
{
    public static class PathWashers
    {
        public static string NullWasher(this string path)
        {
            return path;
        }

        public static void SampleHowToUse()
        {
            string kalle = "hiopa";

            // Extension methodish
            string olle = kalle.NullWasher().NullWasher();

            // Funtional
            System.Func<string, string> w = path => path.NullWasher().NullWasher();
            string nisse = w(kalle);
        }

        // Other washers:
        //  * Remove duplicate '\' (unless network share)
        //  * Convert '/' into '\'
        //  * Convert illegal characters to '.' (or specified character)
        //  * Remove illegal characters
        //  * Remove illegal spaces
        //  * Trim leading and/or ending spaces
    }
}