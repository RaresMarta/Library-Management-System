using System.Windows;
using System.Windows.Controls;
using LibrarySystem.Domain;
using LibrarySystem.Service.Implemented;

namespace LibrarySystem.UI
{
    public partial class ShowMembersWindow : Window
    {
        private readonly LoanService _loanSvc;

        public ShowMembersWindow(MemberService memberSvc, LoanService loanSvc)
        {
            InitializeComponent();
            Title = "Members & Loans";
            _loanSvc = loanSvc;
            MembersGrid.ItemsSource = memberSvc.GetAllMembers();
        }

        /// Load loans for the selected member
        private void MembersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MembersGrid.SelectedItem is Member m)
                MemberLoansGrid.ItemsSource = _loanSvc.GetLoansByMember(m.Id);
            else
                MemberLoansGrid.ItemsSource = null;
        }
    }
}