using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QLBanHang;

namespace Interface
{
    public partial class FrmDangky : Form
    {
        public FrmDangky()
        {
            InitializeComponent();
        }
        private bool Register(string username, string password, string confirm)
        {
            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu không khớp. Vui lòng nhập lại!");
                return false;
            }

            try
            {
                DatabaseHelper cn = new DatabaseHelper();
                SqlConnection connection = cn.GetConnection();  // Lấy kết nối

                string query = "INSERT INTO Login (username, password) VALUES (@username, @password)";
                using (SqlCommand cmd = new SqlCommand(query, connection))  // Sử dụng `connection` thay vì `cn`
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cn.OpenConnection();

                    int rowsInserted = cmd.ExecuteNonQuery();
                    cn.CloseConnection();  // Đảm bảo đóng kết nối sau khi thực thi
                    return rowsInserted > 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Đăng ký không thành công. Vui lòng thử lại.");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private void BtnDangKy_Click_1(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;
            string confirmPassword = txtConfirm.Text;

            bool status = Register(username, password, confirmPassword);
            if (status)
            {
                MessageBox.Show("Đăng ký thành công!");
                this.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

