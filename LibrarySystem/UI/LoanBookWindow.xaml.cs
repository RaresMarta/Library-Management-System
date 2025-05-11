using System;
using System.Windows;

namespace LibrarySystem.UI
{
    public partial class LoanBookWindow : Window
    {
        public int BookId => int.TryParse(BookIdBox.Text, out var id) ? id : -1;
        public int MemberId => int.TryParse(MemberIdBox.Text, out var id) ? id : -1;
        public DateTime DueDate => DueDatePicker.SelectedDate ?? DateTime.Today;

        public LoanBookWindow()
        {
            InitializeComponent();
            Title = "Loan Book";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (BookId <= 0 || MemberId <= 0 || DueDate < DateTime.Today)
            {
                MessageBox.Show("Valid Book ID, Member ID and Due Date required", 
                                "Validation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
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