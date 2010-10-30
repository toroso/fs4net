using System.IO;

namespace fs4net.Framework.Utils
{
    public static class RootedFileUtilities
    {
        public static string ReadText(this RootedFile me)
        {
            using (var stream = me.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static void WriteText(this RootedFile me, string text)
        {
            using (var stream = me.CreateWriteStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }

        public static void AppendText(this RootedFile me, string text)
        {
            using (var stream = me.CreateAppendStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }
    }
}