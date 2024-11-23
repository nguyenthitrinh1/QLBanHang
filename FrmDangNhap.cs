using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using Interface;

namespace QLBanHang
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private bool Login(string username, string password)
        {
            try
            {
                DatabaseHelper cn = new DatabaseHelper();
                SqlConnection connection = cn.GetConnection();  // Lấy kết nối

                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@username", SqlDbType.VarChar) { Value = username },
            new SqlParameter("@password", SqlDbType.VarChar) { Value = password }
                };

                SqlCommand cmd = new SqlCommand("SELECT * FROM Login WHERE username = @username AND password = @password", connection);
                cmd.Parameters.AddRange(parameters);

                // Mở kết nối trước khi thực thi
                cn.OpenConnection();  // Mở kết nối thủ công, nếu bạn chưa mở trong ExecuteReader

                SqlDataReader resultSet = cmd.ExecuteReader();  // Thực thi câu lệnh SQL

                List<string> userInfo = new List<string>();
                while (resultSet.Read())
                {
                    userInfo.Add(resultSet["id"].ToString()); // id
                    userInfo.Add(resultSet["username"].ToString()); // username
                    userInfo.Add(resultSet["role"].ToString()); // role
                }

                resultSet.Close();
                cn.CloseConnection();  // Đảm bảo đóng kết nối sau khi thực hiện truy vấn

                return userInfo.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
        }

        private void Btnexit_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the form when the exit button is clicked
        }

        private void BtnDN_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;
            bool status = Login(username, password);

            if (status)
            {
                MessageBox.Show("Login successful!");
                this.Close(); // Close the login form on success
                FrmMenu frm= new FrmMenu();
                frm.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FrmDangky frm= new FrmDangky();
            frm.ShowDialog();
        }
    }
}
