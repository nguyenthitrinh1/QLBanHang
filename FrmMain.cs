using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interface;

namespace QLBanHang
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FrmDangNhap frm=new FrmDangNhap();
            frm.ShowDialog();
            this.Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FrmDangky frm=new FrmDangky();
            frm.ShowDialog();
        }

        private void LOẠISẢNPHẦMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLoaiSP frm=new FrmLoaiSP();
            frm.ShowDialog();
        }
    }
}
