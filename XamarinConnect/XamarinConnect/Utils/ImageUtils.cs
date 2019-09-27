using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Utils
{
    public class ImageUtils
    {
        private const string IMAGE_FOLDER = "ATFaceME.Xamarin.Core.Resources.Images.";

        public static ImageSource DummyUserImageList;
        public static ImageSource DummyUserImageProfile;

        public static void LoadDummies()
        {
            if(DummyUserImageList == null)
            {
                DummyUserImageList = GetSourceImage("photo_list.png");
            }
            if(DummyUserImageProfile == null)
            {
                DummyUserImageProfile = GetSourceImage("photo_profile.png");
            }
        }

        public static ImageSource GetSourceImage(string imageName)
        {
            return ImageSource.FromResource(IMAGE_FOLDER + imageName);
        }
    }
}
