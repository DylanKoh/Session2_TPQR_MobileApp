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

    }
}


