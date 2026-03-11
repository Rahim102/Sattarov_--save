using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Sattarov_Глазки_save
{
    /// <summary>
    /// Логика взаимодействия для Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        private Agent _currentAgent; // добавьте это поле

        public Sales(Agent agent)
        {
            InitializeComponent();
            _currentAgent = agent;
            this.Title = $"Продажи - {agent.Title}";
            LoadData();
        }
        private void LoadData()
        {
            try
            {
          
                ProductBox.ItemsSource = GlazkiSattarovEntities.GetContext().Product.ToList();

          
                var sales = GlazkiSattarovEntities.GetContext().ProductSale
                    .Where(ps => ps.AgentID == _currentAgent.ID)
                    .ToList();

                SalesList.ItemsSource = sales;

                DateBox.SelectedDate = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (ProductBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите продукт");
                    return;
                }

                if (!int.TryParse(CountBox.Text, out int count) || count <= 0)
                {
                    MessageBox.Show("Введите корректное количество");
                    return;
                }

                if (DateBox.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату");
                    return;
                }

                var selectedProduct = ProductBox.SelectedItem as Product;

               
                var newSale = new ProductSale
                {
                    ProductID = selectedProduct.ID,
                    AgentID = _currentAgent.ID,
                    ProductCount = count,
                    SaleDate = DateBox.SelectedDate.Value
                };

                var context = GlazkiSattarovEntities.GetContext();
                context.ProductSale.Add(newSale);
                context.SaveChanges();

                LoadData();

                // Очищаем поля
                CountBox.Text = "1";
                ProductBox.SelectedItem = null;

                MessageBox.Show("Продажа добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSale = SalesList.SelectedItem as ProductSale;
                if (selectedSale == null)
                {
                    MessageBox.Show("Выберите продажу для удаления");
                    return;
                }

                var result = MessageBox.Show("Удалить запись о продаже?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var context = GlazkiSattarovEntities.GetContext();
                    var saleToDelete = context.ProductSale.Find(selectedSale.ID);
                    if (saleToDelete != null)
                    {
                        context.ProductSale.Remove(saleToDelete);
                        context.SaveChanges();
                    }

                    LoadData();
                    DeleteBtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }

        }

        private void SalesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteBtn.IsEnabled = SalesList.SelectedItem != null;

        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}