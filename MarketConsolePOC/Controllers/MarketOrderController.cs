﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MarketConsolePOC.ViewModel;

namespace MarketConsolePOC.Controllers
{
    public class MarketOrderController : ApiController
    {
        [HttpPost]
        public async Task<CreateMarketOrderResponse> CreateMarketOrder(CreateMarketOrderRequest createMarketOrderRequest)
        {
            var createMarketOrderResponse = new CreateMarketOrderResponse();
            return createMarketOrderResponse;

        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
