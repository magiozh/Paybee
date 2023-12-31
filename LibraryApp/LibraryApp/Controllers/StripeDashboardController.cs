﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using LibraryApp.Data.ViewModels;

namespace LibraryApp.Controllers
{
	public class StripeDashboardController : Controller
	{
		public IActionResult Index()
		{
			var response = new StripeDashboardVM();
			var balanceService = new BalanceService();
			var balanceResult = balanceService.Get();
			response.Balance = balanceResult;

			var transactionService = new BalanceTransactionService();
			var transactionResult = transactionService.List().ToList();
			response.Transactions = transactionResult;

			return View(response);
		}
	}
}

