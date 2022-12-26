using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Kirtasiye
{
    public partial class UrunEkle : Form
    {
        public UrunEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=SERHAT\\SQLEXPRESS;Initial Catalog=Kirtasiye;Integrated Security=True");
        bool durum;
        private void KategoriGetir()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from KategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["Kategori"].ToString());
            }
            baglanti.Close();
        }
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["BarkodNo"].ToString() || txtBarkodNo.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void UrunEkle_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboMarka.Items.Clear();
            ComboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select Kategori,Marka from MarkaBilgileri where kategori='" + comboKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                ComboMarka.Items.Add(read["Marka"].ToString());
            }
            baglanti.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text == "")
            {
                lblmiktari.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Urun where BarkodNo like '" + BarkodNotxt.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["Kategori"].ToString();
                txtMarka.Text = read["Marka"].ToString();
                ÜrünAdıtxt.Text = read["UrunAdi"].ToString();
                lblmiktari.Text = read["Miktari"].ToString();
                AlışFiyatıtxt.Text = read["AlisFiyat"].ToString();
                SatışFiyatıtxt.Text = read["SatisFiyat"].ToString();

            }
            baglanti.Close();
        }

        private void YeniUrunEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into Urun(BarkodNo,Kategori,Marka,UrunAdi,Miktari,AlisFiyat,SatisFiyat,Tarih)values(@BarkodNo,@Kategori,@Marka,@UrunAdi,@Miktari,@AlisFiyat,@SatisFiyat,@Tarih)", baglanti);
                komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@Kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@Marka", ComboMarka.Text);
                komut.Parameters.AddWithValue("@UrunAdi", txtÜrünAdı.Text);
                komut.Parameters.AddWithValue("@Miktari", int.Parse(txtMiktarı.Text));
                komut.Parameters.AddWithValue("@AlisFiyat", decimal.Parse(txtAlışFiyatı.Text));
                komut.Parameters.AddWithValue("@SatisFiyat", decimal.Parse(txtSatışFiyatı.Text));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("ürün eklendi");

            }
            else
            {
                MessageBox.Show("Böyle Bir Barkod No bulunmaktadır", "uyarı");
            }
            ComboMarka.Items.Clear();
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void VarOlanUrun_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Urun set  Miktari=Miktari+'" + int.Parse(Miktarıtxt.Text) + "' where BarkodNo='" + BarkodNotxt.Text + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var olan Ürüne Ekleme Yapıldı");
        }
    }
}
