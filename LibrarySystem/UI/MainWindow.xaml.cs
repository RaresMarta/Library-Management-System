using System;
using System.Linq;
using System.Windows;
using LibrarySystem.Domain;
using LibrarySystem.Service.Implemented;

namespace LibrarySystem.UI
{
    public partial class MainWindow : Window
    {
        private readonly BookService   _bookSvc;
        private readonly MemberService _memberSvc;
        private readonly LoanService   _loanSvc;

        public MainWindow(BookService bs, MemberService ms, LoanService ls)
        {
            InitializeComponent();

            _bookSvc   = bs;
            _memberSvc = ms;
            _loanSvc   = ls;

            AddBookBtn.Click           += (_,_) => OnAddBook();
            DeleteBookBtn.Click        += (_,_) => OnDeleteBook();
            AddMemberBtn.Click         += (_,_) => OnAddMember();
            LoanBookBtn.Click          += (_,_) => OnAddLoan();
            ReturnBookBtn.Click        += (_,_) => OnReturnBook();
            RenewLoanBtn.Click         += (_,_) => OnRenewLoan();
            ShowMembersBtn.Click       += (_,_) => OnShowMembers();
            SearchBtn.Click            += (_,_) => ApplyFilters();
            BooksGrid.SelectionChanged += (_,_) => LoadLoanHistory();
            AuthorFilterCombo.SelectionChanged += (_,_) => ApplyFilters();
            ShowAnalyticsBtn.Click     += (_, _) => OnShowAnalytics();

            ls.UpdateOverdueLoans(); // update overdue loans on startup
            
            // populate the author filter dropdown
            InitCombobox();

            // initial population
            ApplyFilters();
        }

        private void InitCombobox()
        {
            AuthorFilterCombo.Items.Add("All");
            foreach (var author in _bookSvc.GetAllBooks().Select(b => b.Author).Distinct())
                AuthorFilterCombo.Items.Add(author);
            AuthorFilterCombo.SelectedIndex = 0;
        }
        
        private void ApplyFilters()
        {
            _loanSvc.UpdateOverdueLoans(); // update overdue loans with each filter change
            var title = TitleFilterBox.Text.Trim();
            var author = AuthorFilterCombo.SelectedItem as string;
    
            var isTitleEmpty = string.IsNullOrEmpty(title);
            var isAuthorEmpty = author == "All" || author == null;

            BooksGrid.ItemsSource = (isTitleEmpty, isAuthorEmpty) switch
            {
                (true, true) => _bookSvc.GetAllBooks(),
                (false, true) => _bookSvc.GetBooksByTitle(title),
                (true, false) => _bookSvc.GetBooksByAuthor(author!),
                (false, false) => _bookSvc.GetBooksByTitle(title)
                    .Where(b => b.Author == author).ToList()
            };

            // clear loan history when books list changes
            LoanHistoryGrid.ItemsSource = null;
        }

        private void LoadLoanHistory()
        {
            if (BooksGrid.SelectedItem is Book book)
                LoanHistoryGrid.ItemsSource = _loanSvc.GetLoansByBook(book.Id);
            else
                LoanHistoryGrid.ItemsSource = null;
        }

        private void OnAddBook()
        {
            var dlg = new AddBookWindow { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _bookSvc.AddBook(dlg.BookTitle, dlg.BookAuthor, dlg.BookQuantity, dlg.BookGenre);
                    RefreshAuthorFilter();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed to Add Book", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void OnDeleteBook()
        {
            if (BooksGrid.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book first", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to permanently delete “{book.Title}”?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _bookSvc.DeleteBook(book.Id);
                // refresh UI
                RefreshAuthorFilter();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void OnAddMember()
        {
            var dlg = new AddMemberWindow { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _memberSvc.CreateMember(dlg.NameText, dlg.EmailText, dlg.PhoneText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed to Add Member", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        
        private void OnAddLoan()
        {
            var dlg = new LoanBookWindow { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _loanSvc.LendBook(dlg.BookId, dlg.MemberId, dlg.DueDate);
                    RefreshAuthorFilter();
                    ApplyFilters();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Loan Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void OnReturnBook()
        {
            var dlg = new ReturnBookWindow { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var loan = _loanSvc.GetLoanByBookAndMember(dlg.BookId, dlg.MemberId);
                    _loanSvc.ReportReturned(loan.Id);
                    RefreshAuthorFilter();
                    ApplyFilters();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Return Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void OnRenewLoan()
        {
            if (LoanHistoryGrid.SelectedItem is not Loan loan)
            {
                MessageBox.Show("Select a loan in the history list first", "Renew Loan", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new RenewLoanWindow { Owner = this };
            dlg.NewDueDatePicker.SelectedDate = loan.DueDate;
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _loanSvc.RenewLoan(loan.Id, dlg.NewDueDate);
                    MessageBox.Show($"Loan #{loan.Id} renewed to {dlg.NewDueDate:d}", "Renewed", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshAuthorFilter();
                    ApplyFilters();
                    LoadLoanHistory();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Renew Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void OnShowMembers()
        {
            try
            {
                var dlg = new ShowMembersWindow(_memberSvc, _loanSvc) { Owner = this };
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to Show Members", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void OnShowAnalytics()
        {
            try
            {
                var analyticsService = new AnalyticsService(_loanSvc, _bookSvc);
                var dlg = new AnalyticsDashboardWindow(analyticsService) { Owner = this };
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to Show Analytics", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void RefreshAuthorFilter()
        {
            var current = AuthorFilterCombo.SelectedItem as string;
            AuthorFilterCombo.Items.Clear();
            AuthorFilterCombo.Items.Add("All");
            foreach (var author in _bookSvc.GetAllBooks().Select(b => b.Author).Distinct())
                AuthorFilterCombo.Items.Add(author);
            AuthorFilterCombo.SelectedItem = 
                AuthorFilterCombo.Items.Contains(current) ? current : "All";
        }
    }
}
