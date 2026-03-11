using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
           
            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TboxSearch.Text.ToLower()) ||
                p.Phone.Replace("+7", "8").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Contains(TboxSearch.Text.Replace("+7", "8").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""))
               || p.Email.ToLower().Contains(TboxSearch.Text.ToLower())).ToList();
     
         
            AgentListView.ItemsSource = currentAgent.ToList();
            TableList = currentAgent;
            
            ChangePage(0, 0);
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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CountRecords = TableList.Count;
            int pageSize = 10;

            CountPage = CountRecords / pageSize;
            if (CountRecords % pageSize > 0)
                CountPage++;

            if (selectedPage.HasValue)
                CurrentPage = selectedPage.Value;
            else if (direction == 1 && CurrentPage > 0)
                CurrentPage--;
            else if (direction == 2 && CurrentPage < CountPage - 1)
                CurrentPage++;
            else
                return;

            CurrentPageList = TableList.Skip(CurrentPage * pageSize).Take(pageSize).ToList();

            PageListBox.ItemsSource = Enumerable.Range(1, CountPage);
            PageListBox.SelectedIndex = CurrentPage;

            int shown = CurrentPage * pageSize + CurrentPageList.Count;
            TBCount.Text = shown.ToString();
            TBAllRecords.Text = " из " + CountRecords;

            AgentListView.ItemsSource = CurrentPageList;
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count >0)
            {
                ChangePriorityBtn.Visibility = Visibility.Visible;
            }
            else
            {
                ChangePriorityBtn.Visibility = Visibility.Hidden;
            }
        }

        private void ChangePriorityBtn_Click_1(object sender, RoutedEventArgs e)
        {
            int maxPriority = 0;
            foreach (Agent selectedAgent in AgentListView.SelectedItems)
            {
                if (selectedAgent.Priority > maxPriority)
                {
                    maxPriority = selectedAgent.Priority;
                }
            }
            PriorChange prior = new PriorChange(maxPriority);
            prior.ShowDialog();
            int newPriority = Convert.ToInt32(prior.TBPriority.Text);
            foreach (Agent agent in AgentListView.SelectedItems) {
                agent.Priority = newPriority; }
            try
            {
                GlazkiSattarovEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                AgentListView.SelectedItems.Clear();
                UpdateAgent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addAgentBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent ));
        }

    }
}
