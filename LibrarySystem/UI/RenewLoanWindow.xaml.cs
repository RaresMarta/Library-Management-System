using System;
using System.Windows;

namespace LibrarySystem.UI
{
    public partial class RenewLoanWindow : Window
    {
        public DateTime NewDueDate => NewDueDatePicker.SelectedDate ?? DateTime.Today;

        public RenewLoanWindow()
        {
            InitializeComponent();
            Title = "Renew Loan";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (NewDueDate < DateTime.Today)
            {
                MessageBox.Show("New due date must be today or later", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}