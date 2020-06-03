using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Session2_TPQR_MobileApp.GlobalClass;

namespace Session2_TPQR_MobileApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (entryUserID.Text == null || entryPassword.Text == null || entryUserID.Text.Trim() == string.Empty || entryPassword.Text.Trim() == string.Empty)
            {
                await DisplayAlert("Login", "Please check your fields. One or both field(s) are empty!", "Ok");
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:60022/Users/Login?userID={entryUserID.Text}&password={entryPassword.Text}",
                        "POST", Encoding.UTF8.GetBytes(""));
                    var responseString = Encoding.Default.GetString(response);
                    if (responseString == "\"User not found!\"")
                    {
                        await DisplayAlert("Login", "User not found!", "Ok");
                    }
                    else if (responseString == "\"Password is incorrect!\"")
                    {
                        await DisplayAlert("Login", "Password is incorrect!", "Ok");
                    }
                    else
                    {
                        var user = JsonConvert.DeserializeObject<User>(responseString);
                        if (user.userTypeIdFK == 1)
                        {
                            await DisplayAlert("Login", $"Welcome {user.name}!", "Ok");
                            await Navigation.PushAsync(new ManagerMain());
                        }
                        else
                        {
                            await DisplayAlert("Login", $"Welcome {user.name}!", "Ok");
                            await Navigation.PushAsync(new SponsorMain(user));
                        }
                    }
                }
            }
        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccount());
        }
    }
}
