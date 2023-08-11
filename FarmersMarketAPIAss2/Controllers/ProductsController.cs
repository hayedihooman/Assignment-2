using FarmersMarketAPIAss2.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace FarmersMarketAPIAss2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("addProduct")]
        public Response AddProduct(Products product)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.AddProduct(con, product);

            return response;
        }

        [HttpPut]
        [Route("updateProduct/{id}")]
        public Response UpdateProduct(int id, Products product)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.UpdateProduct(con, id, product);

            return response;
        }


        [HttpDelete]
        [Route("deleteProduct/{id}")]
        public Response DeleteProduct(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.DeleteProduct(con, id);

            return response;
        }

        [HttpGet]
        [Route("getProduct/{id}")]
        public Response GetProduct(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.GetProduct(con, id);

            return response;
        }

        [HttpGet]
        [Route("getProductByName/{name}")]
        public Response GetProductByName(string name)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.GetProductByName(con, name);

            return response;
        }


        [HttpGet]
        [Route("getProducts")]
        public Response GetProducts()
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.GetProducts(con);

            return response;
        }
    }

}
