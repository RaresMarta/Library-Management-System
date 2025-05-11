using System.Windows;

namespace LibrarySystem.UI
{
    public partial class AddMemberWindow : Window
    {
        public string NameText  => NameBox.Text.Trim();
        public string EmailText => EmailBox.Text.Trim();
        public string PhoneText => PhoneBox.Text.Trim();

        public AddMemberWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText) ||
                string.IsNullOrWhiteSpace(EmailText) ||
                string.IsNullOrWhiteSpace(PhoneText))
            {
                MessageBox.Show("Name and email are required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
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