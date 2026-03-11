using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sattarov_Глазки_save
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private Agent _currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
                _currentAgent = SelectedAgent;

            DataContext = _currentAgent;

            if (_currentAgent.ID == 0)
            {
                ComboType.SelectedIndex = 0; // новый агент
            }
            else
            {
                foreach (ComboBoxItem item in ComboType.Items)
                {
                    if (!string.IsNullOrEmpty(_currentAgent.TypeAgent) &&
                        item.Content.ToString() == _currentAgent.TypeAgent)
                    {
                        ComboType.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        //var _currentClient = GlazkiSattarovEntities.GetContext().Agent.ToList();

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                {
                    string fileName = System.IO.Path.GetFileName(myOpenFileDialog.FileName);
                    _currentAgent.Logo = "/agents/" + fileName;
                    LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
                
            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentAgent.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(" ", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("Укажите правильно телефон агента");
                if (ph.Length < 11)
                {
                    errors.AppendLine("Укажите правильно телефон агента");
                }
            }


            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Выберите тип агента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var selectedType = ComboType.SelectedItem as ComboBoxItem;
            var type = GlazkiSattarovEntities.GetContext().AgentType
                .FirstOrDefault(x => x.Title == selectedType.Content.ToString());
            _currentAgent.AgentTypeID = type.ID;

            if (_currentAgent.ID == 0)
                GlazkiSattarovEntities.GetContext().Agent.Add(_currentAgent);
            try
            {

                GlazkiSattarovEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.Navigate(new AgentPage());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var _context = GlazkiSattarovEntities.GetContext();
                if (_currentAgent.ProductSale.Count > 0)
                {
                    MessageBox.Show("Невозможно удалить агента, так как у него есть информация о реализации продукции");
                    return;
                }
                var result = MessageBox.Show("Вы действительно хотите удалить агента?", "Подтверждение", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (_currentAgent.AgentPriorityHistory != null && _currentAgent.AgentPriorityHistory.Count > 0)
                        _context.AgentPriorityHistory.RemoveRange(_currentAgent.AgentPriorityHistory);

                    if (_currentAgent.Shop != null && _currentAgent.Shop.Count > 0)
                        _context.Shop.RemoveRange(_currentAgent.Shop);
                    _context.Agent.Remove(_currentAgent);
                    _context.SaveChanges();
                    MessageBox.Show("Агент удален");
                    Manager.MainFrame.Navigate(new AgentPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

        private void SalesHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentAgent.ID == 0)
                {
                    MessageBox.Show("Сначала сохраните агента", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Sales salesWindow = new Sales(_currentAgent);
                salesWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
