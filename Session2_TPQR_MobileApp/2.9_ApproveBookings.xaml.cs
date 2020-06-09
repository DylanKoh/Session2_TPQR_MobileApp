using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session2_TPQR_MobileApp.GlobalClass;

namespace Session2_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApproveBookings : ContentPage
    {
        List<BookingManagerView> _originalSource = new List<BookingManagerView>();
        public ApproveBookings()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }

        private async Task LoadData()
        {
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Bookings/GetManagerView", "POST", Encoding.UTF8.GetBytes(""));
                _originalSource = JsonConvert.DeserializeObject<List<BookingManagerView>>(Encoding.Default.GetString(response));
                lvBookings.ItemsSource = _originalSource;
            }
        }

        private async void btnApprove_Clicked(object sender, EventArgs e)
        {
            if (lvBookings.SelectedItem == null)
            {
                await DisplayAlert("Approve", "Please select a booking to approve!", "Ok");
            }
            else
            {
                var approveValue = "Approved";
                var toApprove = (BookingManagerView)lvBookings.SelectedItem;
                using (var webClient = new WebClient())
                {
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/ApproveBookings/{toApprove.BookingID}?value={approveValue}",
                        "POST", Encoding.UTF8.GetBytes(""));
                    var responseString = Encoding.Default.GetString(response);
                    if (responseString == "\"Unable to approve booking! Package quantity is not enough!\"")
                    {
                        await DisplayAlert("Approve", "Unable to approve booking! Package quantity is not enough!", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Approve", "Booking approved successfully!", "Ok");
                    }
                }
                await LoadData();
            }
        }

        private async void btnReject_Clicked(object sender, EventArgs e)
        {
            if (lvBookings.SelectedItem == null)
            {
                await DisplayAlert("Reject", "Please select a booking to reject!", "Ok");
            }
            else
            {
                var rejectValue = "Rejected";
                var toReject = (BookingManagerView)lvBookings.SelectedItem;
                using (var webClient = new WebClient())
                {
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/ApproveBookings/{toReject.BookingID}?value={rejectValue}",
                        "POST", Encoding.UTF8.GetBytes(""));
                    var responseString = Encoding.Default.GetString(response);
                    if (responseString == "\"Booking rejected successfully!\"")
                    {
                        await DisplayAlert("Reject", "Booking rejected successfully!", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Reject", "Unable to reject booking!", "Ok");
                    }
                }
                await LoadData();
            }
        }
    }
}