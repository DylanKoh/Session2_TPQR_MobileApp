using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session2_TPQR_MobileApp.GlobalClass;

namespace Session2_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SponsorMain : ContentPage
    {
        User _user;
        public SponsorMain(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private async void btnBook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookPackages(_user));
        }

        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateBookings(_user));
        }
    }
}