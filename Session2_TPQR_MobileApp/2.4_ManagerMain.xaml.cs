using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session2_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerMain : ContentPage
    {
        public ManagerMain()
        {
            InitializeComponent();
        }

        private async void btnViewPackages_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewPackages());
        }

        private async void btnAddPackages_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPackages());
        }

        private async void btnApproveBookings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ApproveBookings());
        }

        private async void btnViewSummary_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewSummary());
        }
    }
}