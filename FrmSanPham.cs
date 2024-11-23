using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QLBanHang
{
    public partial class FrmSanPham : Form
    {
        private DatabaseHelper dbHelper;

        public FrmSanPham()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            GetSanPham();
            LoadLoaiSP();
            dgvSP.CellClick += DgvSanPham_CellClick;
        }
        // Load all SanPham data into DataGridView
        private void GetSanPham()
        {
            try
            {
                string query = "SELECT TOP 1000 SanPham.*, LoaiSP.TenLoaiSP " +
                               "FROM SanPham " +
                               "INNER JOIN LoaiSP ON SanPham.LoaiSP = LoaiSP.id " +
                               "ORDER BY SanPham.create_time DESC;";
                SqlDataReader reader = dbHelper.ExecuteReader(query);

                dgvSP.Rows.Clear();
                while (reader.Read())
                {
                    dgvSP.Rows.Add(
                        reader["MaSP"].ToString(),
                        reader["TenSP"].ToString(),
                        reader["Mota"].ToString(),
                        reader["LoaiSP"].ToString(),
                        reader["Gia"].ToString(),
                        reader["SoLuong"].ToString(),
                        reader["DonVi"].ToString()
                        

                        
                    );
                }

                reader.Close();
                dbHelper.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading SanPham: " + ex.Message);
            }
        }

        // Load LoaiSanPham data into ComboBox (tkLoaiSP, itemLoaiSP)
        private void LoadLoaiSP()
        {
            try
            {
                string query = "SELECT id, TenLoaiSP FROM LoaiSP";
                SqlDataReader reader = dbHelper.ExecuteReader(query);

                tkLoaiSP.Items.Clear();
                itemLoaiSP.Items.Clear();
                tkLoaiSP.Items.Add("Select Loai San Pham");

                while (reader.Read())
                {
                    string value = reader["id"].ToString();
                    string label = reader["TenLoaiSP"].ToString();
                    string comboItem = $"{value}:{label}";

                    tkLoaiSP.Items.Add(comboItem);
                    itemLoaiSP.Items.Add(comboItem);
                }

                reader.Close();
                dbHelper.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading LoaiSanPham: " + ex.Message);
            }
        }

        // Filter SanPham by selected LoaiSP
        private void LoadSanPhamByLoaiSP(int Id)
        {
            try
            {
                string query = "SELECT TOP 1000 SanPham.*, LoaiSP.TenLoaiSP " +
                               "FROM SanPham " +
                               "INNER JOIN LoaiSanPham ON SanPham.LoaiSP = LoaiSanPham.id " +
                               "WHERE LoaiSP = @LoaiSP " +
                               "ORDER BY SanPham.create_time DESC;";
                SqlParameter[] parameters = { new SqlParameter("@LoaiSP", Id) };

                SqlDataReader reader = dbHelper.ExecuteReader(query, parameters);
                dgvSP.Rows.Clear();

                while (reader.Read())
                {
                    dgvSP.Rows.Add(
                        reader["MaSP"].ToString(),
                        reader["TenSP"].ToString(),
                        reader["Mota"].ToString(),
                        reader["LoaiSP"].ToString(),
                        reader["Gia"].ToString(),
                        reader["SoLuong"].ToString(),
                        reader["DonVi"].ToString()

                       
                    );
                }

                reader.Close();
                dbHelper.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading SanPham by LoaiSP: " + ex.Message);
            }
        }

        // Insert new SanPham
        private void InsertSanPham()
        {
            try
            {
                string query = "INSERT INTO SanPham (MaSP, TenSP, Mota,LoaiSP,Gia, SoLuong, DonVi) " +
                               "VALUES (@MaSP, @TenSP, @Mota,@LoaiSP,@Gia, @SoLuong, @DonVi)";
                SqlParameter[] parameters = {
                    new SqlParameter("@MaSP", txtIdSp.Text),
                    new SqlParameter("@TenSP", txtTenSP.Text),
                    new SqlParameter("@Mota", txtMoTa.Text),
                    new SqlParameter("@LoaiSP", ((string)itemLoaiSP.SelectedItem).Split(':')[0]),
                    new SqlParameter("@Gia", txtGia.Text),
                    new SqlParameter("@SoLuong", txtSoLuong.Text),
                    new SqlParameter("@DonVi", txtDonVi.Text)
                    
                    
                    
                };

                int result = dbHelper.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Inserted new SanPham successfully!");
                    ClearText();
                    GetSanPham();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting SanPham: " + ex.Message);
            }
        }

        // Update SanPham
        private void UpdateSanPham()
        {
            try
            {
                string query = "UPDATE SanPham SET TenSP = @TenSP, Mota = @Mota, LoaiSP = @LoaiSP, Gia = @Gia, SoLuong = @SoLuong ,DonVi=@DonVi" +
                               " WHERE MaSP = @MaSP";
                SqlParameter[] parameters = {
                    new SqlParameter("@TenSP", txtTenSP.Text),
                    new SqlParameter("@Mota", txtMoTa.Text),
                    new SqlParameter("@LoaiSP", ((string)itemLoaiSP.SelectedItem).Split(':')[0]),
                    new SqlParameter("@Gia", txtGia.Text),
                    new SqlParameter("@SoLuong", txtSoLuong.Text),
                    new SqlParameter("@DonVi", txtDonVi.Text),          
                    new SqlParameter("@MaSP", txtIdSp.Text)
                };

                int result = dbHelper.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Updated SanPham successfully!");
                    ClearText();
                    GetSanPham();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating SanPham: " + ex.Message);
            }
        }

        // Delete SanPham
        private void DeleteSanPham()
        {
            try
            {
                string query = "DELETE FROM SanPham WHERE MaSP = @MaSP";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", txtIdSp.Text) };

                int result = dbHelper.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Deleted SanPham successfully!");
                    ClearText();
                   GetSanPham();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting SanPham: " + ex.Message);
            }
        }

        // Clear input fields
        private void ClearText()
        {
            txtIdSp.Clear();
            txtTenSP.Clear();
            txtMoTa.Clear();
            txtDonVi.Clear();
            txtSoLuong.Clear();
            txtGia.Clear();
            itemLoaiSP.SelectedIndex = 0;
        }

        // Button Click Events for Insert, Update, Delete
        private void Button1_Click(object sender, EventArgs e)
        {
            GetSanPham();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            InsertSanPham();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            UpdateSanPham();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DeleteSanPham();
        }
        private void DgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = dgvSP.Rows[e.RowIndex];

                // Retrieve the values from the selected row
                string id = row.Cells["MaSP"].Value.ToString();
                string name = row.Cells["TenSP"].Value.ToString();
                string mota = row.Cells["Mota"].Value.ToString();
                string donVi = row.Cells["DonVi"].Value.ToString();
                string soLuong = row.Cells["SoLuong"].Value.ToString();
                string gia = row.Cells["Gia"].Value.ToString();
                string loaiSP = row.Cells["LoaiSP"].Value.ToString(); // Assuming this is the category name column

                // Set the values to the corresponding textboxes and combobox
                txtIdSp.Text = id;
                txtTenSP.Text = name;
                txtMoTa.Text = mota;
                txtDonVi.Text = donVi;
                txtSoLuong.Text = soLuong;
                txtGia.Text = gia;

                // Set the selected item in ComboBox, assuming the LoaiSP value is in the format "id:TenLoaiSP"
                foreach (var item in itemLoaiSP.Items)
                {
                    if (item.ToString().StartsWith(loaiSP.Split(':')[0]))
                    {
                        itemLoaiSP.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("No row selected. Please select a product.");
            }
        }
        private void TkLoaiSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected item is not null and contains the expected format "id:TenLoaiSP"
            if (tkLoaiSP.SelectedItem != null)
            {
                string selectedItem = tkLoaiSP.SelectedItem.ToString();

                if (selectedItem.Contains(":"))
                {
                    string idLoaiSP = selectedItem.Split(':')[0]; // Extract the ID part before the ":"

                    try
                    {
                        // Try parsing the ID to an integer
                        int idLoaiSPInt = int.Parse(idLoaiSP);
                        Console.WriteLine("Connected name: " + idLoaiSPInt);

                        // Call the method to get products by LoaiSP ID
                        LoadSanPhamByLoaiSP(idLoaiSPInt);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid IdLoaiSP format: " + idLoaiSP);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format or no item selected");
                }
            }
        }

    }
}
