using System.Collections.Generic;

namespace FarmersMarketWPFAPIAss2.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Products Product { get; set; }
        public List<Products> Products { get; set; }
    }
}
