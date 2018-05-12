using Lands.Models;
using Lands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    class LandsViewModels : BaseViewModel
    {
        #region Services
        private ApiService apiServices;

        #endregion
        #region Attributes
        private ObservableCollection<Land> lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }
        #endregion

        #region Constructors
        public LandsViewModels()
        {
            this.apiServices = new ApiService();
            this.LoadLands();
        }
        #endregion

        #region MyRegion
        private async void LoadLands()
        {
            var connection = await this.apiServices.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                   connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;

            }

            var response = await this.apiServices.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                   response.Message,
                    "Accept");
                return;
            }

            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);
        }
        #endregion
    }
}
