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
    public partial class CreateAccount : ContentPage
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            if (entryCompanyName.Text == null || entryUserID.Text == null || entryPassword.Text == null || entryRePassword.Text == null || entryCompanyName.Text.Trim() == string.Empty || entryUserID.Text.Trim() == string.Empty || entryPassword.Text.Trim() == string.Empty || entryRePassword.Text.Trim() == string.Empty)
            {
                await DisplayAlert("Create", "Please check your field(s)! One or more field(s) are empty!", "Ok");
            }
            else if (entryPassword.Text != entryRePassword.Text)
            {
                await DisplayAlert("Create", "Passwords do not match! Please try again!", "Ok");
            }
            else if (entryUserID.Text.Length < 8)
            {
                await DisplayAlert("Create", "User ID must be 8 characters long or more!", "Ok");
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Content-Type", "application/json");
                    var createUser = new User() { name = entryCompanyName.Text, passwd = entryRePassword.Text, userId = entryUserID.Text, userTypeIdFK = 2 };
                    var JsonData = JsonConvert.SerializeObject(createUser);
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Users/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                    var responseString = Encoding.Default.GetString(response);
                    if (responseString == "\"Unable to create account! If problem persist, please contact administrator!\"")
                    {
                        await DisplayAlert("Create", "Unable to create account! If problem persist, please contact administrator!", "Ok");
                    }
                    else if (responseString == "\"Company already has a sponsor account!\"")
                    {
                        await DisplayAlert("Create", "Company already has a sponsor account!", "Ok");
                    }
                    else if (responseString == "\"User ID has been used!\"")
                    {
                        await DisplayAlert("Create", "User ID has been used!", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Create", "Account created successfully!", "Ok");
                        await Navigation.PopAsync();
                    }
                }
            }
        }
    }
}