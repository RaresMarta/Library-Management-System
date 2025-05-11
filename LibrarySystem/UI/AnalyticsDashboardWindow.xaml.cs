using System.Linq;
using System.Windows;
using LibrarySystem.Domain;
using LibrarySystem.Service.Interfaces;

namespace LibrarySystem.UI
{
    public partial class AnalyticsDashboardWindow : Window
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsDashboardWindow(IAnalyticsService analyticsService)
        {
            InitializeComponent();
            _analyticsService = analyticsService;
            LoadData();
        }

        private void LoadData()
        {
            // 1) Genre Distribution
            var genreData = _analyticsService.GetGenreDistribution()
                .Select(g => new { Genre = g.Key, Count = g.Value });
            GenreGrid.ItemsSource = genreData;

            // 2) Loan Status Totals
            var loanData = _analyticsService.GetLoanStatusDistribution();
            loanData.TryGetValue(LoanStatus.Active,   out var activeCount);
            loanData.TryGetValue(LoanStatus.Overdue,  out var overdueCount);
            ActiveLoansText.Text  = activeCount.ToString();
            OverdueLoansText.Text = overdueCount.ToString();

            // 3) Top 5 Borrowed Books
            TopBooksList.ItemsSource = _analyticsService
                .GetMostBorrowedBooks(5)
                .Select(b => new {
                    TitleAndCount = $"{b.Title} (×{_analyticsService.GetBorrowCount(b.Id)})"
                })
                .ToList();
        }
    }
}