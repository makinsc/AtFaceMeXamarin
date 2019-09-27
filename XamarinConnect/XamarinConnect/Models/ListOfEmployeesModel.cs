using ATFaceME.Xamarin.Core.Utils;
using System.Collections.ObjectModel;

namespace ATFaceME.Xamarin.Core.Models
{
    public class ListOfEmployeesModel
    {
        public ObservableRangeCollection<UserDetailModel> Employees { get; set; }
        public string NextLink { get; set; }
    }
}
