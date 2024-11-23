using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace QLBanHang
{
    public partial class FrmNhanVien : Form
    {
        private DatabaseHelper dbHelper;
        private Label selectLabel;

        public FrmNhanVien()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            selectLabel = new Label();
            selectLabel.Text = " select";
            LoadNhanVien();
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;
        }

        // Phương thức tải danh sách nhân viên
        private void LoadNhanVien()
        {
            try
            {
                string query = "SELECT * FROM NhanVien";
                SqlDataReader reader = dbHelper.ExecuteReader(query);

                dgvNhanVien.Rows.Clear(); // Xóa các dòng cũ trong DataGridView
                while (reader.Read())
                {
                    dgvNhanVien.Rows.Add(
                        reader["MaNhanVien"].ToString(),
                        reader["TenNhanVien"].ToString(),
                        reader["GioiTinh"].ToString(),
                        reader["NgaySinh"].ToString(),
                        reader["Luong"].ToString()
                    );
                }

                reader.Close();
                dbHelper.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message);
            }
        }


            public void GetNhanVienByName(string tenNhanVien)
            {
                try
                {
                    // Assuming tbNhanVien is a DataGridView
                    DataGridView dgvNhanVien = new DataGridView();
                    dgvNhanVien.Rows.Clear(); // Clear existing rows
                    DatabaseHelper cn = new DatabaseHelper();

                    Console.WriteLine("Connected SQL success");

                    string query = "SELECT TOP 1000 * FROM NhanVien " +
                                   "WHERE TenNhanVien LIKE N'%' + @TenNhanVien + '%' " +
                                   "ORDER BY create_time DESC";

                    using (SqlCommand cmd = new SqlCommand(query, cn.GetConnection()))
                    {
                        cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien); // Set parameter value

                        using (SqlDataReader resultSet = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Connected and fetched data.");

                            while (resultSet.Read())
                            {
                                // Create a new row
                                DataGridViewRow row = new DataGridViewRow();
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = resultSet["MaNhanVien"].ToString() });
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = resultSet["TenNhanVien"].ToString() });
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = resultSet["GioiTinh"].ToString() });
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = resultSet["NgaySinh"].ToString() });
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = resultSet["Luong"].ToString() });

                                dgvNhanVien.Rows.Add(row); // Add the row to the DataGridView
                            }

                            cn.CloseConnection(); // Ensure connection is closed after operation
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
        }

        // Thêm mới nhân viên
        private void InsertNhanVien()
        {
            try
            {
                string query = "INSERT INTO NhanVien (MaNhanVien, TenNhanVien, GioiTinh, NgaySinh, Luong) " +
                               "VALUES (@MaNhanVien, @TenNhanVien, @GioiTinh, @NgaySinh, @Luong)";

                SqlParameter[] parameters = {
            new SqlParameter("@MaNhanVien", txtID.Text),
            new SqlParameter("@TenNhanVien", txtName.Text),
            new SqlParameter("@GioiTinh", selectLabel.Text),  // Kiểm tra giá trị của GioiTinh
            new SqlParameter("@NgaySinh", txtNgaySinh.Text),
            new SqlParameter("@Luong", txtLuong.Text)
        };

                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm mới thành công!");
                    ClearText();
                    LoadNhanVien();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu được thêm vào.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm mới: " + ex.Message);
            }
        }


        // Cập nhật nhân viên
        private void UpdateNhanVien()
        {
            try
            {
                string query = "UPDATE NhanVien SET TenNhanVien = @TenNhanVien, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, Luong = @Luong WHERE MaNhanVien = @MaNhanVien" + 
                               " WHERE MaNhanVien = @MaNhanVien";
                SqlParameter[] parameters = {
                    new SqlParameter("@TenNhanVien", txtName.Text),  // Use txtName instead of txtID for the name field
                    new SqlParameter("@GioiTinh", selectLabel.Text),
                    new SqlParameter("@NgaySinh", txtNgaySinh.Text),
                    new SqlParameter("@Luong", txtLuong.Text),
                    new SqlParameter("@MaNhanVien", int.Parse(txtID.Text))
                };

                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    ClearText();
                    LoadNhanVien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        // Xóa nhân viên
        private void DeleteNhanVien()
        {
            try
            {
                string query = "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                SqlParameter[] parameters = {
                    new SqlParameter("@MaNhanVien", int.Parse(txtID.Text))
                };

                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    ClearText();
                    LoadNhanVien();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên với mã này.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }

        // Xóa dữ liệu trong TextBox
        private void ClearText()
        {
            txtID.Clear();
            txtName.Clear(); // Correct the second clear for txtName instead of txtID again
            selectLabel.Text = "";
            txtNgaySinh.Clear();
            txtLuong.Clear();
        }
        private void RbnGT_CheckedChanged(object sender, EventArgs e)
        {
            if (RbnGT.Checked)
            {
                selectLabel.Text = "Nam"; // Cập nhật giới tính là "Nam" nếu RbnGT được chọn
            }
            else if (RbnGT2.Checked)
            {
                selectLabel.Text = "Nu"; // Cập nhật giới tính là "Nu" nếu RbnGT2 được chọn
            }
        }

        private void RbnGT2_CheckedChanged(object sender, EventArgs e)
        {
            if (RbnGT2.Checked)
            {
                selectLabel.Text = "Nu"; // Cập nhật giới tính là "Nu" nếu RbnGT2 được chọn
            }
            else if (RbnGT.Checked)
            {
                selectLabel.Text = "Nam"; // Cập nhật giới tính là "Nam" nếu RbnGT được chọn
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            LoadNhanVien();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            InsertNhanVien();
            LoadNhanVien();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            UpdateNhanVien();
            LoadNhanVien();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteNhanVien();
                LoadNhanVien(); // Tải lại danh sách sau khi xóa
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo người dùng chọn dòng hợp lệ
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                // Lấy dữ liệu từ dòng được chọn
                txtID.Text = row.Cells["MaNhanVien"].Value.ToString();
                txtName.Text = row.Cells["TenNhanVien"].Value.ToString();
                selectLabel.Text = row.Cells["GioiTinh"].Value.ToString();
                txtNgaySinh.Text = row.Cells["NgaySinh"].Value.ToString();
                txtLuong.Text = row.Cells["Luong"].Value.ToString();

                // Chọn RadioButton tương ứng
                if (selectLabel.Text == "Nam")
                {
                    RbnGT.Checked = true;
                    RbnGT2.Checked = false;
                }
                else
                {
                    RbnGT.Checked = false;
                    RbnGT2.Checked = true;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string tenNhanVien=txtSearch.Text;
            GetNhanVienByName(tenNhanVien);
        }
    }
}
