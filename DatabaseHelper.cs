using Microsoft.Data.SqlClient;
using System;

namespace QLBanHang
{
    internal class DatabaseHelper
    {
        // Chuỗi kết nối
        private readonly string connectionString = @"Data Source=DESKTOP-K35NNJG\SQLEXPRESS01;Initial Catalog=QLBanH;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private SqlConnection conn;

        // Constructor khởi tạo đối tượng kết nối
        public DatabaseHelper()
        {
            conn = new SqlConnection(connectionString);
        }

        // Mở kết nối
        public void OpenConnection()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kết nối đến cơ sở dữ liệu: " + ex.Message);
                throw new Exception("Không thể kết nối tới cơ sở dữ liệu.", ex);
            }
        }

        // Đóng kết nối
        public void CloseConnection()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đóng kết nối: " + ex.Message);
            }
        }

        // Phương thức trả về đối tượng SqlConnection hiện tại
        public SqlConnection GetConnection()
        {
            return conn;  // Trả về đối tượng SqlConnection đã được khởi tạo trong constructor
        }

        // Phương thức trả về SqlDataReader cho truy vấn SELECT
        public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                OpenConnection();  // Mở kết nối tại đây

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteReader();  // Thực thi câu lệnh và trả về SqlDataReader
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thực thi truy vấn: " + ex.Message);
                return null;
            }
        }
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                OpenConnection();  // Mở kết nối tại đây

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                // Thực thi câu lệnh và trả về số dòng bị ảnh hưởng
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thực thi câu lệnh SQL: " + ex.Message);
                return -1;  // Trả về -1 nếu có lỗi
            }
            finally
            {
                CloseConnection();  // Đảm bảo đóng kết nối sau khi thực thi xong
            }
        }
        }
}
