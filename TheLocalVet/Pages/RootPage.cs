﻿using TheLocalVet.Pages;
using Xamarin.Forms;
using TheLocalVet;
using TheLocalVet.Languages;

namespace TheLocalVet.Pages
{
    public class RootPage : TabbedPage
    {
        public RootPage()
        {
			Title = "Din Lokale Dyrlege";
            Children.Add( new NearestVetSearchPage (this.Navigation) { Title = AppResources.NearestVetPageTitle, Icon = "search_75" });
            Children.Add( new InfoPage { Title = AppResources.InfoPageTitle, Icon = "info_75"});
            //this.Children.Add(new EmergencyContactPage { Title = "Emergency", Icon = "" });
        }
    }
}
