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
    public partial class BookPackages : ContentPage
    {
        User _user;
        List<CustomView> _originalSource = new List<CustomView>();
        List<string> _tier = new List<string>();
        List<string> _cbFilter = new List<string>();
        public BookPackages(User user)
        {
            InitializeComponent();
            _user = user;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _tier.Add("No Filter");
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Packages/GetCustomView", "POST", Encoding.UTF8.GetBytes(""));
                _originalSource = JsonConvert.DeserializeObject<List<CustomView>>(Encoding.Default.GetString(response));
                lvPackages.ItemsSource = _originalSource;
            }
            foreach (var item in _originalSource.Select(x => x.PackageTier).Distinct())
            {
                _tier.Add(item);
            }
            pTier.ItemsSource = _tier;

        }

        private void FilterList()
        {
            if (pTier.SelectedItem != null && pTier.SelectedItem.ToString() != "No Filter")
            {
                if (entryBudget.Text != null && entryBudget.Text != "" && entryBudget.Text != "0")
                {
                    var budget = Int32.Parse(entryBudget.Text);
                    var modifiedSource = (from x in _originalSource
                                          where x.PackageTier == pTier.SelectedItem.ToString() && x.PackageValue <= budget
                                          where x.Benefits.Contains(string.Join(", ", _cbFilter))
                                          select x).ToList();
                    lvPackages.ItemsSource = modifiedSource;
                }
                else
                {
                    var modifiedSource = (from x in _originalSource
                                          where x.PackageTier == pTier.SelectedItem.ToString()
                                          where x.Benefits.Contains(string.Join(", ", _cbFilter))
                                          select x).ToList();
                    lvPackages.ItemsSource = modifiedSource;
                }

            }
            else
            {
                if (entryBudget.Text != null && entryBudget.Text != "" && entryBudget.Text != "0")
                {
                    var budget = Int32.Parse(entryBudget.Text);
                    var modifiedSource = (from x in _originalSource
                                          where x.PackageValue <= budget
                                          where x.Benefits.Contains(string.Join(", ", _cbFilter))
                                          select x).ToList();
                    lvPackages.ItemsSource = modifiedSource;
                }
                else
                {
                    var modifiedSource = (from x in _originalSource
                                          where x.Benefits.Contains(string.Join(", ", _cbFilter))
                                          select x).ToList();
                    lvPackages.ItemsSource = modifiedSource;
                }
            }

        }

        private void pTier_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterList();
        }

        private void entryBudget_Completed(object sender, EventArgs e)
        {

            FilterList();

        }

        private void cbOnline_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbOnline.IsChecked)
            {
                _cbFilter.Add("Online");
            }
            else
            {
                _cbFilter.Remove("Online");
            }
            FilterList();
        }

        private void cbFlyer_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbFlyer.IsChecked)
            {
                _cbFilter.Add("Flyer");
            }
            else
            {
                _cbFilter.Remove("Flyer");
            }
            FilterList();
        }

        private void cbBanner_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbBanner.IsChecked)
            {
                _cbFilter.Add("Banner");
            }
            else
            {
                _cbFilter.Remove("Banner");
            }
            FilterList();
        }

        private async void btnBook_Clicked(object sender, EventArgs e)
        {
            if (lvPackages.SelectedItem == null)
            {
                await DisplayAlert("Book", "Please select a sponsorship package to book!", "Ok");
            }
            else if (entryDesiredQuantity.Text == null)
            {
                await DisplayAlert("Book", "Please input desired quantity!", "Ok");
            }
            else if (entryDesiredQuantity.Text == "" || entryDesiredQuantity.Text == "0")
            {
                await DisplayAlert("Book", "Please input desired quantity!", "Ok");
            }
            else
            {
                var toBook = (CustomView)lvPackages.SelectedItem;
                var getQuantity = Int32.Parse(entryDesiredQuantity.Text);
                if (toBook.AvailableQuantity - getQuantity < 0)
                {
                    await DisplayAlert("Book", "Unable to book more than current available quantity!", "Ok");
                }
                else
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.Headers.Add("Content-Type", "application/json");
                        var newBooking = new Booking() { packageIdFK = toBook.PackageID, userIdFK = _user.userId, quantityBooked = getQuantity, status = "Pending" };
                        var JsonData = JsonConvert.SerializeObject(newBooking);
                        var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Bookings/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                        var responseString = Encoding.Default.GetString(response);
                        if (responseString == "\"Unable to book package! Please try again later!\"")
                        {
                            await DisplayAlert("Book", "Unable to book package! Please try again later!", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Book", "Booking successful!", "Ok");
                            entryDesiredQuantity.Text = "";
                        }
                    }
                }
            }
            
        }
    }
}