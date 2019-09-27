using Apibackend.Trasversal.DTOs;
using Apibackend.Trasversal.DTOs.RequestDTO;
using ApiBackend.Transversal.DTOs.PLC;
using ApiBackend.Transversal.DTOs.PLC.RequestDTO;
using ATFaceME.Xamarin.Core.Agents;
using ATFaceME.Xamarin.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.Utils;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.ViewModels
{
    public class ListOfEmployeesViewModel : BaseViewModel
    {
        #region Private properties

        private ListOfEmployeesModel paginatedUserDetails;
        private bool isListFiltered;
        private bool disposed;

        private ListOfEmployeesFilterModel filter;
        private ListOfEmployeesFilterModel oldFilter;
        private string[] officeLocation;

        #endregion

        #region Public properties

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ObservableRangeCollection<UserDetailModel> ListOfEmployees
        {
            get { return paginatedUserDetails.Employees; }
            set
            {
                paginatedUserDetails.Employees = value;
                OnPropertyChanged("ListOfEmployees");
            }
        }

        public string LinkToNext
        {
            get { return paginatedUserDetails.NextLink; }
            set
            {
                paginatedUserDetails.NextLink = value;
                OnPropertyChanged("LinkToNext");
            }
        }

        public bool IsListFiltered
        {
            get { return isListFiltered; }
            set
            {
                isListFiltered = value;
                OnPropertyChanged("IsFiltered");
                OnPropertyChanged("CanLoadMore");
            }
        }

        public bool CanLoadMore
        {
            get { return !isListFiltered; }
        }

        public string Name
        {
            get { return filter.Name; }
            set
            {
                filter.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Surname
        {
            get { return filter.Surname; }
            set
            {
                filter.Surname = value;
                OnPropertyChanged("Surname");
            }
        }

        public string[] OfficeLocation
        {
            get { return officeLocation; }
            set
            {
                officeLocation = value;
                OnPropertyChanged("OfficeLocation");
            }
        }

        public int SelectedIndexOfficeLocation
        {
            get { return filter.SelectedIndexOfficeLocation; }
            set
            {
                filter.SelectedIndexOfficeLocation = value;
                OnPropertyChanged("SelectedIndexOfficeLocation");
                OnPropertyChanged("OfficeLocation");
            }
        }

        public string SelectedOffice
        {
            get
            {
                string res = "";
                if (filter.SelectedIndexOfficeLocation > 0 &&
                    filter.SelectedIndexOfficeLocation < officeLocation.Length)
                {
                    res = officeLocation[filter.SelectedIndexOfficeLocation];
                }
                return res;
            }
        }

        #endregion

        #region Contructor & Destructor

        public ListOfEmployeesViewModel() : base()
        {
            paginatedUserDetails = new ListOfEmployeesModel();
            paginatedUserDetails.Employees = new ObservableRangeCollection<UserDetailModel>();
            paginatedUserDetails.NextLink = "";

            officeLocation = OfficeLocationUtils.OfficeLocations;
            filter = new ListOfEmployeesFilterModel();
            oldFilter = new ListOfEmployeesFilterModel();

            filter.Name = "";
            filter.Surname = "";

            oldFilter.Name = "";
            oldFilter.Surname = "";
        }

        ~ListOfEmployeesViewModel()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        public async Task<string> LoadViewModel(bool isInitialGetting = true)
        {
            string response = "OK";
            IsListFiltered = (HasToApplyFilter() && !isFilterEmpty());

            try
            {
                 await (isListFiltered 
                        ? GetAllFilteredUsers() 
                        : GetAllUsers(isInitialGetting));
                updateAsApplied();

            }
            catch (Exception e)
            {
                response = "ERROR";
            }           

            return response;
        }
        
        public bool IsTheLastItem(object item)
        {
            bool result = false;
            UserDetailModel userItem = item as UserDetailModel;

            if (userItem != null)
            {
                result = ListOfEmployees.IndexOf(userItem) == ListOfEmployees.Count - 1;
            }

            return result;
        }

        public bool HasResult()
        {
            return ListOfEmployees.Count != 0;
        }

        public void Restore()
        {
            Name = oldFilter.Name;
            Surname = oldFilter.Surname;
            SelectedIndexOfficeLocation = oldFilter.SelectedIndexOfficeLocation;
        }

        public bool HasToApplyFilter()
        {
            var case1 = !IsListFiltered && !isFilterEmpty();
            var case2 = IsListFiltered && !isFilterEmpty() && filterConditionsHasChanged();
            var case3 = IsListFiltered && !isFilterEmpty() && !filterConditionsHasChanged() && 
                        (!String.IsNullOrEmpty(LinkToNext) && LinkToNext.Contains("filter"));

            return case1 || case2 || case3;
        }

        public void CheckIfHasToResetEmployees()
        {
            if (filterConditionsHasChanged() || !HasToApplyFilter())
            {
                ListOfEmployees.Clear();
                LinkToNext = null;
            }
        }

        #endregion

        #region Protected methods

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                paginatedUserDetails = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        private async Task GetAllUsers(bool isInitialGetting)
        {
            var response = await BackendClient.CallHttp<GetAllUserByFilterRequest, PaginatedUserDetail>(
                                                                                     Endpoints.GetAllUser,
                                                                                     HttpVerbs.GET,
                                                                                     AuthenticationHelper.TokenForUser,
                                                                                     null,
                                                                                     LinkToNext);
            LinkToNext = response.ODataNextLink;

            ListOfEmployees.AddRange(ListToObservable(response.value));
        }

        private async Task GetAllFilteredUsers()
        {
            GetAllUserByFilterRequest content = new GetAllUserByFilterRequest()
                {
                    UserName = Name,
                    Surname = Surname,
                    OfficeLocation = SelectedOffice
                };
            
            var response = await BackendClient
                            .CallHttp<GetAllUserByFilterRequest, PaginatedUserDetail>(
                                    Endpoints.GetAllByFilter,
                                    HttpVerbs.POST,
                                    AuthenticationHelper.TokenForUser,
                                    content,
                                    LinkToNext);

            LinkToNext = response.ODataNextLink;

            ListOfEmployees.AddRange(ListToObservable(response.value));
        }

        private bool isFilterEmpty()
        {
            return (string.IsNullOrEmpty(Name) &&
                    string.IsNullOrEmpty(Surname) &&
                    string.IsNullOrEmpty(SelectedOffice));
        }

        private void updateAsApplied()
        {
            oldFilter.Name = filter.Name;
            oldFilter.Surname = filter.Surname;
            oldFilter.SelectedIndexOfficeLocation = filter.SelectedIndexOfficeLocation;
        }
        
        private ObservableCollection<UserDetailModel> ListToObservable(List<UserDetail> list)
        {
            var result = new ObservableCollection<UserDetailModel>();

            foreach (var user in list)
            {
                UserDetailModel userModel = new UserDetailModel(user);
                result.Add(userModel);
            }

            return result;
        }

        private bool filterConditionsHasChanged()
        {
            return !filter.Name.Equals(oldFilter.Name) ||
                   !filter.Surname.Equals(oldFilter.Surname) ||
                   !filter.SelectedIndexOfficeLocation.Equals(oldFilter.SelectedIndexOfficeLocation);
        }

        #endregion

    }
}
