using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session2_TPQR_MobileApp.GlobalClass;

namespace Session2_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateBookings : ContentPage
    {
        User _user;
        List<GetCustomBookings> _originalSource = new List<GetCustomBookings>();
        public UpdateBookings(User user)
        {
            InitializeComponent();
            _user = user;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await UpdateData();
            var total = 0;
            foreach (var item in _originalSource)
            {
                total += item.SubTotal;
            }
            lblTotal.Text = total.ToString();
        }

        private async Task UpdateData()
        {
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/GetSpecificBookings?userID={_user.userId}", "POST", Encoding.UTF8.GetBytes(""));
                _originalSource = JsonConvert.DeserializeObject<List<GetCustomBookings>>(Encoding.Default.GetString(response));
                lvUpdate.ItemsSource = _originalSource;
            }
        }
        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (lvUpdate.SelectedItem == null)
            {
                await DisplayAlert("Update", "Please select a booking to update!", "Ok");
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (lvUpdate.SelectedItem == null)
            {
                await DisplayAlert("Delete", "Please select a booking to update!", "Ok");
            }
            else
            {
                var userResponse = await DisplayAlert("Delete", "Please select a booking to update!", "Yes", "Cancel");
                if (userResponse)
                {
                    var toDelete = (GetCustomBookings)lvUpdate.SelectedItem;
                    using (var webClient = new WebClient())
                    {
                        var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/Delete/{toDelete.BookingID}", "POST", Encoding.UTF8.GetBytes(""));
                        var responseString = Encoding.Default.GetString(response);
                        if (responseString != "\"Booking removed successfully!\"")
                        {
                            await DisplayAlert("Delete", "Unable to delete booking! PLease contact administrator!", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Delete", "Booking removed successfully!", "Ok");
                            
                        }
                    }
                    await UpdateData();
                }
                
            }
        }
    }
}