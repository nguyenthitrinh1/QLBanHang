using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLBanHang
{
    public partial class FrmLoaiSP : Form
    {
        private DatabaseHelper dbHelper;

        public FrmLoaiSP()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadLoaiSP();
        }

        // Phương thức tải dữ liệu Loại Sản Phẩm
        private void LoadLoaiSP()
        {
            try
            {
                string query = "SELECT * FROM LoaiSP";
                SqlDataReader reader = dbHelper.ExecuteReader(query);

                dgvLoaiSP.Rows.Clear(); // Xóa các dòng cũ trong DataGridView
                while (reader.Read())
                {
                    dgvLoaiSP.Rows.Add(
                        reader["Id"].ToString(),
                        reader["TenLoaiSP"].ToString(),
                        reader["MoTa"].ToString()
                    );
                }

                reader.Close();
                dbHelper.CloseConnection(); // Đóng kết nối sau khi đọc xong
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // Phương thức thêm mới Loại Sản Phẩm
        private void InsertLoaiSP()
        {
            try
            {
                string query = "INSERT INTO LoaiSP (Id,TenLoaiSP, MoTa) VALUES (@Id,@TenLoaiSP, @MoTa)";
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", txtId.Text),
                    new SqlParameter("@TenLoaiSP", txtTenLoaiSP.Text),
                    new SqlParameter("@MoTa", txtMoTa.Text)
                };

                SqlDataReader reader = dbHelper.ExecuteReader(query, parameters);
                reader.Close();
                dbHelper.CloseConnection();

                MessageBox.Show("Thêm mới thành công!");
                ClearText();
                LoadLoaiSP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm mới: " + ex.Message);
            }
        }

        // Phương thức cập nhật Loại Sản Phẩm
        private void UpdateLoaiSP()
        {
            try
            {
                string query = "UPDATE LoaiSP SET TenLoaiSP = @TenLoaiSP, MoTa = @MoTa WHERE Id = @Id";
                SqlParameter[] parameters = {
                    new SqlParameter("@TenLoaiSP", txtTenLoaiSP.Text),
                    new SqlParameter("@MoTa", txtMoTa.Text),
                    new SqlParameter("@Id", int.Parse(txtId.Text))
                };

                SqlDataReader reader = dbHelper.ExecuteReader(query, parameters);
                reader.Close();
                dbHelper.CloseConnection();

                MessageBox.Show("Cập nhật thành công!");
                ClearText();
                LoadLoaiSP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        // Phương thức xóa Loại Sản Phẩm
        private void DeleteLoaiSP()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Mã loại sản phẩm không hợp lệ. Vui lòng kiểm tra lại!");
                    return;
                }

                // Truy vấn xóa toàn bộ thông tin của ID từ bảng LoaiSP
                string query = "DELETE FROM LoaiSP WHERE Id = @Id";

                // Tạo tham số để truyền vào truy vấn
                SqlParameter[] parameters = {
            new SqlParameter("@Id", id)  // Sử dụng giá trị id đã được lấy từ txtId.Text
        };

                // Thực thi truy vấn xóa
                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);  // Dùng ExecuteNonQuery cho lệnh xóa

                // Kiểm tra nếu có dòng bị xóa
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xóa thông tin loại sản phẩm thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy loại sản phẩm với mã Id này.");
                }

                // Xóa dữ liệu cũ trong các TextBox
                ClearText();

                // Tải lại danh sách sau khi xóa
                LoadLoaiSP();
            }
            catch (Exception ex)
            {
                // Thông báo lỗi nếu có lỗi khi xóa
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }



        // Xử lý sự kiện nhấp chuột vào dòng trong DataGridView

        // Xóa các ô nhập liệu
        private void ClearText()
        {
            txtId.Clear();
            txtTenLoaiSP.Clear();
            txtMoTa.Clear();
            
        }



        // Sự kiện nhấn nút "Sửa"

        private void BtnAdd_Click_1(object sender, EventArgs e)
        {
            InsertLoaiSP();
        }

        private void BtnEdit_Click_1(object sender, EventArgs e)
        {
            UpdateLoaiSP();

        }

        private void BtnDelete_Click_1(object sender, EventArgs e)
        {
            DeleteLoaiSP();

        }

        private void DgvLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng đã nhấp vào một hàng hợp lệ
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLoaiSP.Rows[e.RowIndex];

                // Sử dụng chỉ số cột thay vì tên cột để lấy giá trị
                txtId.Text = row.Cells[0].Value.ToString(); // Chỉ số 0 là cột "Id"
                txtTenLoaiSP.Text = row.Cells[1].Value.ToString(); // Chỉ số 1 là cột "TenLoaiSP"
                txtMoTa.Text = row.Cells[2].Value.ToString(); // Chỉ số 2 là cột "MoTa"
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            LoadLoaiSP();
        }
    }
}
