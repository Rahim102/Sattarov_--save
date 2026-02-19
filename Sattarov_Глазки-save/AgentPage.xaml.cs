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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sattarov_Глазки_save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = GlazkiSattarovEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;
            ComboType.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            UpdateAgent();
        }
        private void UpdateAgent()
        {
            var currentAgent = GlazkiSattarovEntities.GetContext().Agent.ToList();



            if (ComboType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "МФО").ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "ЗАО").ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "МКК").ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "ОАО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(a => a.TypeAgent == "ПАО").ToList();
            }



            if (ComboSort.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 3)
            {
                currentAgent = currentAgent.OrderBy(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 4)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 5)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }
            if (TboxSearch.Text.Length > 0)
            {
                currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TboxSearch.Text.ToLower()) ||
                p.Phone.ToLower().Contains(TboxSearch.Text.ToLower())||
                p.Email.ToLower().Contains(TboxSearch.Text.ToLower())).ToList();
            }
            AgentListView.ItemsSource = currentAgent.ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void ChangePriorityBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void addAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ComboType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();

        }
    }
}
