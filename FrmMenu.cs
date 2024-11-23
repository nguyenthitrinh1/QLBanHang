using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanHang
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FrmLoaiSP frm=new FrmLoaiSP();
            frm.ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FrmSanPham frm=new FrmSanPham();
            frm.ShowDialog();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FrmNhanVien frm=new FrmNhanVien();
            frm.ShowDialog();

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
