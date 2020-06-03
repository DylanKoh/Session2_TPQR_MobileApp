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
    public partial class UpdateBookings : ContentPage
    {
        User _user;
        public UpdateBookings(User user)
        {
            InitializeComponent();
            _user = user;
        }
    }
}