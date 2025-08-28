using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL.Services;

namespace Presentation.Pages.Customer
{
    public partial class CustomerListPage : Form
    {
        private readonly IUserService _userService;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private DataGridView gridCustomers;
        private Button buttonViewDetail;

        public CustomerListPage()
        {
            _userService = new UserService();
            InitializeComponent();
            this.Load += CustomerListPage_Load;
        }

        private void InitializeComponent()
        {
            this.textBoxSearch = new TextBox();
            this.buttonSearch = new Button();
            this.gridCustomers = new DataGridView();
            this.buttonViewDetail = new Button();

            this.SuspendLayout();

            // Search
            this.textBoxSearch.Left = 20;
            this.textBoxSearch.Top = 20;
            this.textBoxSearch.Width = 300;

            this.buttonSearch.Left = 330;
            this.buttonSearch.Top = 18;
            this.buttonSearch.Width = 100;
            this.buttonSearch.Text = "Tìm";
            this.buttonSearch.Click += async (_, __) => await LoadCustomersAsync();

            // Grid
            this.gridCustomers.Left = 20;
            this.gridCustomers.Top = 60;
            this.gridCustomers.Width = 820;
            this.gridCustomers.Height = 440;
            this.gridCustomers.ReadOnly = true;
            this.gridCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridCustomers.MultiSelect = false;
            this.gridCustomers.AutoGenerateColumns = false;
            this.gridCustomers.AllowUserToAddRows = false;
            this.gridCustomers.AllowUserToDeleteRows = false;

            this.gridCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "UserId", Width = 60 });
            this.gridCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên đăng nhập", DataPropertyName = "Username", Width = 150 });
            this.gridCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ tên", DataPropertyName = "Fullname", Width = 200 });
            this.gridCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = "Email", Width = 200 });
            this.gridCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Điện thoại", DataPropertyName = "Phone", Width = 120 });

            // View detail
            this.buttonViewDetail.Left = 20;
            this.buttonViewDetail.Top = 510;
            this.buttonViewDetail.Width = 160;
            this.buttonViewDetail.Text = "Xem chi tiết";
            this.buttonViewDetail.Click += ButtonViewDetail_Click;

            // Form
            this.Text = "Quản lý khách hàng";
            this.Width = 900;
            this.Height = 600;
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.gridCustomers);
            this.Controls.Add(this.buttonViewDetail);

            this.ResumeLayout(false);
        }

        private async void CustomerListPage_Load(object? sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            var keyword = textBoxSearch.Text?.Trim();
            var customers = await _userService.GetCustomersAsync(keyword);
            var binding = customers.Select(u => new
            {
                u.UserId,
                u.Username,
                u.Fullname,
                u.Email,
                u.Phone
            }).ToList();
            gridCustomers.DataSource = binding;
        }

        private void ButtonViewDetail_Click(object? sender, EventArgs e)
        {
            if (gridCustomers.CurrentRow == null) return;
            var idObj = gridCustomers.CurrentRow.Cells["UserId"]?.Value;
            if (idObj == null) return;
            int userId = Convert.ToInt32(idObj);
            var detail = new CustomerDetailPage(userId);
            detail.ShowDialog(this);
        }
    }
}

