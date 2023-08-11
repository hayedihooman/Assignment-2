using FarmersMarketWPFAPIAss2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {

        HttpClient httpClient = new HttpClient();

        public Admin()
        {
            httpClient.BaseAddress = new Uri("https://localhost:7224/api/Products/");

            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
            LoadProducts();
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

        private void clearTextField()
        {
            txtBox_id.Text = string.Empty;
            txtBox_prdName.Text = string.Empty;
            txtBox_amount.Text = string.Empty;
            txtBox_price.Text = string.Empty;
        }

        private async void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Products product = new Products();
            product.id = int.Parse(txtBox_id.Text);
            product.name = txtBox_prdName.Text;
            product.amount = decimal.Parse(txtBox_amount.Text);
            product.price = decimal.Parse(txtBox_price.Text);

            var response = await httpClient.PostAsJsonAsync("addProduct", product);

            MessageBox.Show(response.StatusCode.ToString());

            LoadProducts();
            clearTextField();
        }

        private async void btn_SearchById_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.GetStringAsync("getProduct/" + int.Parse(txtBox_searchID.Text));

            Response res = JsonConvert.DeserializeObject<Response>(response);

            txtBox_id.Text = res.Product.id.ToString();
            txtBox_prdName.Text = res.Product.name;
            txtBox_amount.Text = res.Product.amount.ToString();
            txtBox_price.Text = res.Product.price.ToString();

            MessageBox.Show(res.Message);
        }

        private async void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Products product = new Products();

            product.id = int.Parse(txtBox_id.Text);
            product.name = txtBox_prdName.Text;
            product.amount = decimal.Parse(txtBox_amount.Text);
            product.price = decimal.Parse(txtBox_price.Text);

            var response = await httpClient.PutAsJsonAsync("updateProduct/" + product.id, product);

            MessageBox.Show(response.StatusCode + " - " + response.ReasonPhrase);
            LoadProducts();
            clearTextField();

        }

        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.DeleteAsync("deleteProduct/" + int.Parse(txtBox_id.Text));

            MessageBox.Show(response.StatusCode.ToString());

            LoadProducts();
            clearTextField();
        }

        private void DataGrid_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_db.SelectedItem != null)
            {
                Products selectedProduct = (Products)DataGrid_db.SelectedItem;

                int id = selectedProduct.id;
                string name = selectedProduct.name;
                decimal amount = selectedProduct.amount;
                decimal price = selectedProduct.price;

                txtBox_id.Text = id.ToString();
                txtBox_prdName.Text = name;
                txtBox_amount.Text = amount.ToString();
                txtBox_price.Text = price.ToString();
            }

            

        }

        private void btn_Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
