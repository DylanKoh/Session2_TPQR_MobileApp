using System;
using System.Collections.Generic;
using System.Text;

namespace Session2_TPQR_MobileApp
{
    public class GlobalClass
    {
        public class User
        {
            public string userId { get; set; }
            public string name { get; set; }
            public string passwd { get; set; }
            public int userTypeIdFK { get; set; }
        }
        public class User_Type
        {
            public int userTypeId { get; set; }
            public string userTypeName { get; set; }
        }

        public class CustomView
        {
            public int PackageID { get; set; }
            public string PackageName { get; set; }
            public int AvailableQuantity { get; set; }
            public string PackageTier { get; set; }
            public int PackageValue { get; set; }
            public string Benefits { get; set; }
        }

        public class Booking
        {
            public int bookingId { get; set; }
            public string userIdFK { get; set; }
            public int packageIdFK { get; set; }
            public int quantityBooked { get; set; }
            public string status { get; set; }
        }

        public class GetCustomBookings
        {

            public int BookingID { get; set; }
            public string PackageTier { get; set; }
            public string PackageName { get; set; }
            public int PackageValue { get; set; }
            public int QuantityBooked { get; set; }
            public int SubTotal { get; set; }

        }
        public class Benefit
        {
            public int benefitId { get; set; }
            public int packageIdFK { get; set; }
            public string benefitName { get; set; }

        }
        public class Package
        {
            public int packageId { get; set; }
            public string packageTier { get; set; }
            public string packageName { get; set; }
            public long packageValue { get; set; }
            public int packageQuantity { get; set; }

        }

        public class BookingManagerView
        {

            public int BookingID { get; set; }
            public string CompanyName { get; set; }
            public string PackageName { get; set; }
            public string Status { get; set; }


        }

    }
}


