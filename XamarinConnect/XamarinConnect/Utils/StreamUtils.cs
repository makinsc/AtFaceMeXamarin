using System.IO;

namespace ATFaceME.Xamarin.Core.Utils
{
    public class StreamUtils
    {
        public static Stream PrepareStream(Stream stream)
        {
            while (stream.Length == 0)
            {

            }
            return stream;
        }
    }
}
