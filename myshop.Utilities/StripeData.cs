using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Utilities
{
	public class StripeData
	{
		public string SecretKey { get; set; }
		public string PublishableKey { get; set; }
	}
}

//steps to configuer api payment stripe
//1- add secretkey , publishablekey in appsetting.jason
//2-create class have two property to configure stripe setting
//3-register this in program.cs 
//builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));
//4-setup stripe in nuget
//5- Add Middleware stripe
//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
