﻿using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLocalVet.Helpers;
using TheLocalVet.Interfaces;
using TheLocalVet.Models;
using Xamarin.Forms;
using TheLocalVet.Languages;
using System.Collections.ObjectModel;
using TheLocalVet.Pages;

namespace TheLocalVet.ViewModels
{
    public class NearestVetSearchResultViewModel : BaseViewModel
    {
        private IParseHelper _parseHelper;

		private ObservableCollection<VetModel> _vets = new ObservableCollection<VetModel>();
		private ObservableCollection<VetModel> _fullVetList = new ObservableCollection<VetModel> ();
        private string _place;
        private double _latitude;
        private double _longitude;
        private double _distance;
		private INavigation _nav;

        public event EventHandler<EventArgs> OnSearchForVetFailed;

        public ObservableCollection<VetModel> Vets
        {
            get { return _vets; }
            set { _vets = value;  OnPropertyChanged("Vets"); }
        }

		private int _sortOn;

		public int SortOn
		{
			get { return _sortOn; }
			set { _sortOn = value;  OnPropertyChanged("SortOn"); Resort (); }
		}

		private void Resort()
		{
			ObservableCollection<VetModel> tempList = Vets;
			IOrderedEnumerable<VetModel> temp;

			switch(SortOn)
			{
			case 0:
				temp = tempList.OrderBy (vet => vet.Name);
				break;
			case 1:
				temp =tempList.OrderBy(vet => vet.MainCompetency);
				break;
			default:
				temp =tempList.OrderBy (vet => vet.Name);
				break;
			}

			Vets.Clear ();
			foreach (var tmp in temp)
				Vets.Add (tmp);

			OnPropertyChanged ("Vets");
		}

		public RelayCommand OpenFilterCommand { get; private set; }

		public NearestVetSearchResultViewModel(string place, INavigation nav, double latitude = 0.0, double longitude = 0.0, double distance = 0.0)
        {
            _parseHelper = DependencyService.Get<IParseHelper>();
            _place = place;
            _latitude = latitude;
            _longitude = longitude;
            _distance = distance;
			_nav = nav;
			OpenFilterCommand = new RelayCommand (OpenFilter);
			OpenFilterCommand.IsEnabled = true;
        }

		public async void OpenFilter()
		{
			FilterPageViewModel filterVm = new FilterPageViewModel (_nav);
			filterVm.OnFilterUpdated += OnFilterUpdated;

			FilterPage filterPage = new FilterPage () { BindingContext = filterVm };
			await _nav.PushAsync (filterPage);
		}

		private void OnFilterUpdated (object sender, FilterEventArgs e)
		{
			string filterType = e.FilterType;
			string filterValue = e.FilterValue;
			var updateVetList = new ObservableCollection<VetModel> ();

			foreach (var vet in _fullVetList) 
			{
				switch(filterType)
				{
				case "MainCompetency":
					if (vet.MainCompetency.ToString ().Equals (filterValue))
						updateVetList.Add (vet);
					break;
				case "HomeVisit":
					if (vet.HomeVisit.ToString ().Equals (filterValue))
						updateVetList.Add (vet);
					break;
				case "reset":
					updateVetList.Add (vet);
					break;
				default:
					updateVetList.Add(vet);
					break;
				}
			}

			Vets.Clear ();
			Vets = updateVetList;
		}

        public async Task GetNearestVet()
        {
            using (UserDialogs.Instance.Loading(AppResources.Loading))
            { 
                if (!string.IsNullOrEmpty(_place))
					_fullVetList = await _parseHelper.SearchByPlace(MiscFunctions.UpperCaseFirst(_place));
                else
					_fullVetList = await _parseHelper.SearchByGeoLocation(_latitude, _longitude, _distance);
				
				var temp = _fullVetList.OrderBy (vet => vet.Name);
				foreach (var entry in temp)
					Vets.Add (entry);
            }

            if (Vets.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.FailedToFindVet, AppResources.Error, "OK");

                if (OnSearchForVetFailed != null)
                    OnSearchForVetFailed(this, new EventArgs());
            }
        }
    }
}
