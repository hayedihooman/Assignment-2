using FarmersMarketWPFAPIAss2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FarmersMarketWPFAPIAss2
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        HttpClient httpClient = new HttpClient();
        private List<Products> cartProducts;
        private decimal totalAmount;

        public Sales()
        {
            httpClient.BaseAddress = new Uri("https://localhost:7224/api/Products/");

            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
            LoadProducts();
            cartProducts = new List<Products>();
        }

        private async void LoadProducts()
        {
            try
            {
                var response = await httpClient.GetStringAsync("getProducts");

                Response res = JsonConvert.DeserializeObject<Response>(response);

                if (res.StatusCode == 200 && res.Products != null)
                {
                    DataGrid_db.ItemsSource = res.Products;
                    cmbProducts.ItemsSource = res.Products.Where(x => x.amount > 0).Select(x => x.name).ToList();
                }
                else
                {
                    MessageBox.Show("No data found or an error occurred while fetching the data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void DataGrid_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_db.SelectedItem != null)
            {
                Products selectedProduct = (Products)DataGrid_db.SelectedItem;

                txtBox_prdName.Text = selectedProduct.name;
                txtBox_amount.Text = selectedProduct.amount.ToString();
                txtBox_price.Text = selectedProduct.price.ToString();
            }
        }

        private async void btn_SearchById_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.GetStringAsync("getProductByName/" + txtBox_searchName.Text); 

            Response res = JsonConvert.DeserializeObject<Response>(response);

            txtBox_prdName.Text = res.Product.name;
            txtBox_amount.Text = res.Product.amount.ToString();
            txtBox_price.Text = res.Product.price.ToString();

            MessageBox.Show(res.Message);
        }

        private async void btn_shop_Click(object sender, RoutedEventArgs e)
        {
            string selectedProduct = cmbProducts.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedProduct))
            {
                var quantityStr = txtBox_prdctkg.Text;
                decimal quantity;
                if (decimal.TryParse(quantityStr, out quantity))
                {
                    var response = await httpClient.GetStringAsync("getProductByName/" + selectedProduct);
                    Response res = JsonConvert.DeserializeObject<Response>(response);
                    var product = res.Product;
                    if (product != null)
                    {
                        if (product.amount >= quantity)
                        {
                            product.amount -= quantity;
                            await httpClient.PutAsJsonAsync("updateProduct/" + product.id, product);
                            product.amount = quantity;
                            cartProducts.Add(product);
                            dataGrid_Cart.ItemsSource = null;
                            dataGrid_Cart.ItemsSource = cartProducts;
                        }
                        else
                        {
                            MessageBox.Show("Not enough stock for this product.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product.");
            }

            cmbProducts.SelectedIndex = -1;
            txtBox_prdctkg.Text = string.Empty;

            LoadProducts();
        }

        private void btn_chekout_Click(object sender, RoutedEventArgs e)
        {
            totalAmount = 0;

            foreach (var product in cartProducts)
            {
                totalAmount += product.amount * product.price;
            }

            txtBox_TotalAmount.Text = totalAmount.ToString();
            cartProducts.Clear();
            dataGrid_Cart.ItemsSource = null;
            LoadProducts();
        }

        private void btn_Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
