﻿using Newtonsoft.Json;
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
            
        }

        private async Task UpdateData()
        {
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/GetSpecificBookings?userID={_user.userId}", "POST", Encoding.UTF8.GetBytes(""));
                _originalSource = JsonConvert.DeserializeObject<List<GetCustomBookings>>(Encoding.Default.GetString(response));
                lvUpdate.ItemsSource = _originalSource;
            }
            var total = 0;
            foreach (var item in _originalSource)
            {
                total += item.SubTotal;
            }
            lblTotal.Text = total.ToString();
        }
        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (lvUpdate.SelectedItem == null)
            {
                await DisplayAlert("Update", "Please select a booking to update!", "Ok");
            }
            else if (entryQuantity.Text == null)
            {
                await DisplayAlert("Update", "Please enter a valid quantity!", "Ok");
            }
            else if (entryQuantity.Text == "" || entryQuantity.Text == "0")
            {
                await DisplayAlert("Update", "Please enter a valid quantity!", "Ok");
            }
            else
            {
                var toEdit = (GetCustomBookings)lvUpdate.SelectedItem;
                var newQuantity = Int32.Parse(entryQuantity.Text);
                using (var webClient = new WebClient())
                {
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Bookings/Edit/{toEdit.BookingID}?quantity={newQuantity}", "POST", Encoding.UTF8.GetBytes(""));
                    var responseString = Encoding.Default.GetString(response);
                    if (responseString == "\"Booking edited successfully!\"")
                    {
                        await DisplayAlert("Update", "Booking updated successfully! Please wait while it is reapporved!", "Ok");
                        await UpdateData();
                    }
                    else if (responseString == "\"Package do not have enough quantity for new amount!\"")
                    {
                        await DisplayAlert("Update", "Package do not have enough quantity for new amount!", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Update", "Unable to edit bookings! Please contact our administrator", "Ok");

                    }
                }
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