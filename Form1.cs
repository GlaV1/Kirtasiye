using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kirtasiye
{
    public partial class anasayfa : Form
    {
        public anasayfa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Satis a=new Satis();
            a.ShowDialog();
        }

        private void UrunEkle_Click(object sender, EventArgs e)
        {
            UrunEkle a = new UrunEkle();
            a.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stok a = new Stok();
            a.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KategoriEkle a = new KategoriEkle();
            a.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MarkaEkle a = new MarkaEkle();
            a.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RaporAl a = new RaporAl();
            a.ShowDialog();
        }

        private void Cıkıs_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Çıkmak İstediğinize Emin misiniz?", "çıkış Mesajı", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (x == DialogResult.No)
            {

            }
        }
    }
}
