using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace MarketConsolePOC
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9010/";

            
            OrderHelper.Initialize();

            WebApp.Start<Startup>(url: baseAddress);

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
