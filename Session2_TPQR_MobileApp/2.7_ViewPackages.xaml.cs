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
    public partial class ViewPackages : ContentPage
    {

        List<CustomView> _originalSource = new List<CustomView>();

        public ViewPackages()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:60022/Packages/GetManagerView", "POST", Encoding.UTF8.GetBytes(""));
                _originalSource = JsonConvert.DeserializeObject<List<CustomView>>(Encoding.Default.GetString(response));
                lvPackages.ItemsSource = _originalSource;
            }
            LoadSorter();
        }

        private void LoadSorter()
        {
            List<string> sorterFields = new List<string>() { "Tier", "Name", "Value (Ascending)", "Quantity (Descending)" };
            pSort.ItemsSource = sorterFields;
        }

        private void pSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pSort.SelectedItem.ToString() == "Tier")
            {
                var modifiedList = _originalSource.OrderByDescending(x => x.PackageTier == "Gold").ThenByDescending(x => x.PackageTier == "Silver").ThenByDescending(x => x.PackageTier == "Bronze");
                lvPackages.ItemsSource = modifiedList;
            }
            else if (pSort.SelectedItem.ToString() == "Name")
            {
                var modifiedList = _originalSource.OrderBy(x => x.PackageName);
                lvPackages.ItemsSource = modifiedList;
            }
            else if (pSort.SelectedItem.ToString() == "Value (Ascending)")
            {
                var modifiedList = _originalSource.OrderBy(x => x.PackageValue);
                lvPackages.ItemsSource = modifiedList;
            }
            else if (pSort.SelectedItem.ToString() == "Quantity (Descending)")
            {
                var modifiedList = _originalSource.OrderByDescending(x => x.AvailableQuantity);
                lvPackages.ItemsSource = modifiedList;
            }
        }
    }
}