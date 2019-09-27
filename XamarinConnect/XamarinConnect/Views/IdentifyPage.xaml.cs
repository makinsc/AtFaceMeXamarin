using ApiBackend.Transversal.DTOs.PLC.ResultDTO;
using ATFaceME.Xamarin.Core.Utils;
using System.Threading.Tasks;
using ATFaceME.Xamarin.Core.ViewModels;
using ATFaceME.Xamarin.Core.Views.Common;
using System;
using ATFaceME.Xamarin.Core.Models;
using Plugin.Media;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Views
{
    public partial class IdentifyPage : BasePage
    {
        private bool disposed;

        public IdentifyPage()
        {
            InitalizeViewModel(new IdentifyViewModel());
            InitializeComponent();
        }

        ~IdentifyPage()
        {
            Dispose(false);
        }

        public IdentifyViewModel GetViewModel()
        {
            return (IdentifyViewModel)ViewModel;
        }

        /// <summary>
        /// Take a picture and try to identify the person
        /// </summary>
        public async void IdentifyUser()
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsCameraAvailable &&
               CrossMedia.Current.IsTakePhotoSupported)
            {
                await ChangePageState(PageState.INITIAL);

                // Prepare the state of the page to make the photo and wait the api response

                try
                {
                    var photo = await CrossMedia.Current.TakePhotoAsync(
                        new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                        {
                            CompressionQuality = 92
                        });

                    if (photo != null)
                    {
                        await ChangePageState(PageState.PROCESSING);

                        var photoStream = StreamUtils.PrepareStream(photo.GetStream());

                        var personData = await DoRequestWithResponse(() => GetViewModel().LoadViewModel(photoStream));

                        await ChangePageState(PageState.FINAL);

                        // Finally we show the result to the user
                        if (personData.ResultCode != IdentifyResultCode.PERSON_FOUND)
                        {
                            await ChangePageState(PageState.INITIAL, "Persona no reconocida o no entrenada");
                        }
                        else
                        {
                            GetViewModel().User = new UserDetailModel(personData.User);
                        }
                        
                    }
                    else
                    {
                        await ChangePageState(PageState.INITIAL, "Se ha cancelado la operación");
                    }
                    
                }
                catch (Exception e)
                {
                    await ChangePageState(PageState.INITIAL, "La foto no ha podido procesarse");
                }
            }
            else
            {
                await ChangePageState(PageState.INITIAL, "Ha habido un problema con la cámara");
            }

            if (GetViewModel().User != null)
            {
                await Navigation.PushAsync(new DetailOfEmployeePage((GetViewModel().User)));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(GetViewModel().User != null)
            {

                Navigation.PopAsync();
            }
            else
            {
                IdentifyUser();
            }

            GetViewModel().User = null;
            GetViewModel().InfoText = "";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        /// <summary>
        /// Change the state of the page 
        /// </summary>
        /// <param name="state">The state to change</param>
        /// <param name="message">Message to display in an alert</param>
        protected async override Task ChangePageState(PageState state, string message = null)
        {
            switch (state)
            {
                case PageState.INITIAL:
                    if (!string.IsNullOrEmpty(message))
                    {
                        await Navigation.PopAsync();
                    }
                    break;

                case PageState.PROCESSING:
                    GetViewModel().InfoText = "Procesando...";
                    break;
            }

            await base.ChangePageState(state, message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // In the future, you'll have to dispose the attributes
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}