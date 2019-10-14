using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Web.Http.SelfHost;
using System.Web.Http;

namespace FixOrderConsole
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9010/";

           // baseAddress = "http://localhost:9010/";

            OrderHelper.Initialize();

           // WebApp.Start<Startup>(url: baseAddress);


            var config = new HttpSelfHostConfiguration("http://localhost:9010");

            config.Routes.MapHttpRoute(name: "API", routeTemplate:
                                  "api/{controller}/{action}/{id}",
                                 defaults: new { controller = "Home", id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Web API Server has started at http://localhost:9010");
                Console.ReadLine();
            }

            //   Task.Factory.StartNew(OrderHelper.RandomOrderReceiver);

            //while(true)
            //{
            //    if(OrderHelper.OrderProcessors.Any())
            //    {
            //        OrderHelper.OrderProcessors[0].orderStatus = OrderStatus.ToBeRemove;
            //        OrderHelper.OrderProcessors[0].manualResetEvent.Set();
            //    }
            //    System.Threading.Thread.Sleep(60000);
            //}



            Console.ReadLine();

            // Start OWIN host 
            //using (WebApp.Start<Startup>(url: baseAddress))
            //{
            //    // Create HttpCient and make a request to api/values 
            //    //HttpClient client = new HttpClient();

            ////    var response = client.GetAsync(baseAddress + "api/market").Result;


            //    //Console.WriteLine(response);
            //    //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            //    Console.ReadLine();
            //}
        }
    }
}
