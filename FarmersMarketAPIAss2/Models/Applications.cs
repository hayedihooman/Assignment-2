using Npgsql;

namespace FarmersMarketAPIAss2.Models
{
    public class Applications
    {
        public Response AddProduct(NpgsqlConnection con, Products product)
        {
            Response response = new Response();
            try
            {
                string query = "INSERT INTO \"Products\" VALUES (@id, @name, @amount, @price)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", product.id);
                cmd.Parameters.AddWithValue("@name", product.name);
                cmd.Parameters.AddWithValue("@amount", product.amount);
                cmd.Parameters.AddWithValue("@price", product.price);
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Product added successfully";
                    response.Product = product;
                    response.Products = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.Message = "Nothing is added";
                    response.Product = null;
                    response.Products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while adding the product: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }

            return response;
        }

        public Response UpdateProduct(NpgsqlConnection con, int id, Products product)
        {
            Response response = new Response();
            try
            {
                string query = "UPDATE \"Products\" SET \"Product Name\" = @name, \"Amount(kg)\" = @amount, \"Price per kg\" = @price WHERE \"Product ID\" = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", product.name);
                cmd.Parameters.AddWithValue("@amount", product.amount);
                cmd.Parameters.AddWithValue("@price", product.price);
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Product updated successfully";
                    response.Product = product;
                    response.Products = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.Message = "Nothing is updated";
                    response.Product = null;
                    response.Products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while updating the product: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }

            return response;
        }


        public Response DeleteProduct(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            try
            {
                string query = "DELETE FROM \"Products\" WHERE \"Product ID\" = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Product deleted successfully";
                    response.Product = null;
                    response.Products = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.Message = "Nothing is deleted";
                    response.Product = null;
                    response.Products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while deleting the product: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }

            return response;
        }

        public Response GetProduct(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            try
            {
                string query = "SELECT * FROM \"Products\" WHERE \"Product ID\" = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Products product = new Products()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        amount = reader.GetDecimal(2),
                        price = reader.GetDecimal(3)
                    };

                    response.StatusCode = 200;
                    response.Message = "Product fetched successfully";
                    response.Product = product;
                    response.Products = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.Message = "Product not found";
                    response.Product = null;
                    response.Products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while fetching the product: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }

            return response;
        }

        public Response GetProductByName(NpgsqlConnection con, string name)
        {
            Response response = new Response();
            try
            {
                string query = "SELECT * FROM \"Products\" WHERE \"Product Name\" = @name";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                con.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Products product = new Products()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        amount = reader.GetDecimal(2),
                        price = reader.GetDecimal(3)
                    };

                    response.StatusCode = 200;
                    response.Message = "Product fetched successfully";
                    response.Product = product;
                    response.Products = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.Message = "Product not found";
                    response.Product = null;
                    response.Products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while fetching the product: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public Response GetProducts(NpgsqlConnection con)
        {
            Response response = new Response();
            try
            {
                string query = "SELECT * FROM \"Products\"";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Products> products = new List<Products>();

                while (reader.Read())
                {
                    Products product = new Products()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        amount = reader.GetDecimal(2),
                        price = reader.GetDecimal(3)
                    };
                    products.Add(product);
                }

                response.StatusCode = 200;
                response.Message = "Products fetched successfully";
                response.Product = null;
                response.Products = products;
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while fetching the products: " + ex.Message;
                response.Product = null;
                response.Products = null;
            }

            return response;
        }
    }
}
    

