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
    public partial class AddPackages : ContentPage
    {
        List<string> _benefits = new List<string>();
        public AddPackages()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadPicker();
        }

        private async Task LoadPicker()
        {
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Packages/GetTiers", "POST", Encoding.UTF8.GetBytes(""));
                pTier.ItemsSource = JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(response));
            }
        }

        private void cbOnline_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbOnline.IsChecked)
            {
                _benefits.Add("Online");
            }
            else
            {
                _benefits.Remove("Online");
            }
        }

        private void cbFlyer_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbFlyer.IsChecked)
            {
                _benefits.Add("Flyer");
            }
            else
            {
                _benefits.Remove("Flyer");
            }
        }

        private void cbBanner_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbBanner.IsChecked)
            {
                _benefits.Add("Banner");
            }
            else
            {
                _benefits.Remove("Banner");
            }
        }

        private async void btnClear_Clicked(object sender, EventArgs e)
        {
            await LoadPicker();
            entryPackageName.Text = string.Empty;
            entryQuantity.Text = string.Empty;
            entryValue.Text = string.Empty;
            cbOnline.IsChecked = false;
            cbFlyer.IsChecked = false;
            cbBanner.IsChecked = false;
        }

        private async void btnAddPackage_Clicked(object sender, EventArgs e)
        {
            if (pTier.SelectedItem == null)
            {
                await DisplayAlert("Add Package", "Please select Tier!", "Ok");
            }
            else if (entryPackageName.Text == null || entryPackageName.Text.Trim() == string.Empty)
            {
                await DisplayAlert("Add Package", "Please enter pacakge name!", "Ok");
            }
            else if (entryValue.Text == null || entryValue.Text == "" || entryValue.Text == "0")
            {
                await DisplayAlert("Add Package", "Please enter pacakge value!", "Ok");
            }
            else if (entryQuantity.Text == null || entryQuantity.Text == "" || entryQuantity.Text == "0")
            {
                await DisplayAlert("Add Package", "Please enter pacakge's available quantity!", "Ok");
            }
            else
            {
                if (pTier.SelectedItem.ToString() == "Bronze" && _benefits.Count != 1)
                {
                    await DisplayAlert("Add Package", "Bronze Tier can only have 1 benefit!", "Ok");
                }
                else if (pTier.SelectedItem.ToString() == "Silver" && _benefits.Count != 2)
                {
                    await DisplayAlert("Add Package", "Silver Tier can only have 2 benefit!", "Ok");
                }
                else if (pTier.SelectedItem.ToString() == "Gold" && _benefits.Count != 3)
                {
                    await DisplayAlert("Add Package", "Gold Tier must have all 3 benefit!", "Ok");
                }
                else
                {
                    string responseString = string.Empty;
                    using (var webClient = new WebClient())
                    {
                        webClient.Headers.Add("Content-Type", "application/json");
                        var newPackage = new Package()
                        {
                            packageName = entryPackageName.Text,
                            packageTier = pTier.SelectedItem.ToString(),
                            packageQuantity = Int32.Parse(entryQuantity.Text),
                            packageValue = Int32.Parse(entryValue.Text)
                        };
                        var JsonData = JsonConvert.SerializeObject(newPackage);
                        var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Packages/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                        responseString = Encoding.Default.GetString(response);
                    }
                    if (responseString == "\"Package Name has been taken!\"")
                    {
                        await DisplayAlert("Add Package", "Package Name has been taken!", "Ok");
                    }
                    else if (responseString == "\"Package value for Gold Tier is invalid!\"")
                    {
                        await DisplayAlert("Add Package", "Gold Tier value must be at least $50,000!", "Ok");
                    }
                    else if (responseString == "\"Package value for Silver Tier is invalid!\"")
                    {
                        await DisplayAlert("Add Package", "Silver Tier value must be more than $10,000 and less than $50,000!", "Ok");
                    }
                    else if (responseString == "\"Package value for Bronze Tier is invalid!\"")
                    {
                        await DisplayAlert("Add Package", "Bronze Tier value must be more than $0 and at most $10,000!", "Ok");
                    }
                    else
                    {
                        var checkServerResponse = new List<string>();
                        var newID = Int32.Parse(responseString);
                        foreach (var item in _benefits)
                        {
                            var newBenefit = new Benefit() { packageIdFK = newID, benefitName = item };
                            var JsonData = JsonConvert.SerializeObject(newBenefit);
                            using (var webClient = new WebClient())
                            {
                                webClient.Headers.Add("Content-Type", "application/json");
                                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Benefits/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                                checkServerResponse.Add(Encoding.Default.GetString(response));
                            }
                        }
                        if (checkServerResponse.Contains("\"Unable to add Benefits!\""))
                        {
                            await DisplayAlert("Add Package", "Unable to add Benefits properly! Please contact our Administrator!", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Add Package", "Package added successfully!", "Ok");
                            btnClear_Clicked(null, null);
                        }
                    }
                }
            }
        }
    }
}