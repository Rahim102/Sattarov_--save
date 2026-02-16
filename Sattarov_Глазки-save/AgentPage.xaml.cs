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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            foreach(Agent agent in AgentListView.SelectedItems)
            {
                agent.Priority = newPriority;
            }
            try
            {
                GlazkiSattarovEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                AgentListView.SelectedItems.Clear();
                Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 0)
            {
                ChangePriorityBtn.Visibility = Visibility.Visible;

            }
            else
            {
                ChangePriorityBtn.Visibility = Visibility.Hidden;
            }
        }
        
    }
}
