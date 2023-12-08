using System;
using System.Collections.Generic;
using Stripe;

namespace LibraryApp.Data.ViewModels
{
    public class StripeDashboardVM
    {
        public Balance Balance { get; set; }
        
        public List<BalanceTransaction> Transactions { get; set; }
        
    }
}

