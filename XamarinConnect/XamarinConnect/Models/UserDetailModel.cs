using Apibackend.Trasversal.DTOs;
using ATFaceME.Xamarin.Core.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Models
{
    public class UserDetailModel : INotifyPropertyChanged
    {

        public string id { get; set; }
        public string displayName { get; set; }
        public string jobTitle { get; set; }
        public string officeLocation { get; set; }
        public string mail { get; set; }
        public List<string> businessPhones { get; set; }
        public bool hasPhoto { get; set; }

        public UserDetailModel(UserDetail user)
        {
            id = user.id;
            displayName = user.displayName;
            jobTitle = user.jobTitle;
            officeLocation = user.officeLocation;
            mail = user.mail;
            businessPhones = user.businessPhones;
            hasPhoto = user.hasPhoto;
        }

        private PhotoSourceType photoSource;
        private ImageSource photo;
        public PhotoSourceType PhotoSource { get
            {
                return photoSource;
            }
            set
            {
                photoSource = value;
                OnPropertyChanged("PhotoSource");
                switch (photoSource)
                {
                    case PhotoSourceType.URL:
                        Photo =  "https://atoffice365.blob.core.windows.net/profilephotoatsistemas/" + id + ".jpg";
                        break;
                    case PhotoSourceType.DUMMY_LIST:
                        Photo = ImageUtils.DummyUserImageList;
                        break;
                    case PhotoSourceType.DUMMY_DETAILS:
                        Photo = ImageUtils.DummyUserImageProfile;
                        break;
                    default:
                        Photo = null;
                        break;
                }
            }
        }

        public ImageSource Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
                OnPropertyChanged("Photo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public enum PhotoSourceType
    {
        NO_PHOTO, DUMMY_LIST, DUMMY_DETAILS, URL
    }
}
