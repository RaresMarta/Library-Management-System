using System.Windows;

namespace LibrarySystem.UI
{
    public partial class AddBookWindow : Window
    {
        public string BookTitle => TitleBox.Text.Trim();
        public string BookAuthor => AuthorBox.Text.Trim();
        public int BookQuantity => int.TryParse(QtyBox.Text, out var q) ? q : 0;
        
        public string BookGenre => GenreBox.Text.Trim();

        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BookTitle)
                || string.IsNullOrWhiteSpace(BookAuthor)
                || BookQuantity <= 0
                || string.IsNullOrWhiteSpace(BookGenre))
            {
                MessageBox.Show("All fields are required and quantity must be > 0");
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