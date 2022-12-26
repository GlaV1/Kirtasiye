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
    public partial class Stok : Form
    {
        public Stok()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=SERHAT\\SQLEXPRESS;Initial Catalog=Kirtasiye;Integrated Security=True");
        DataSet daset = new DataSet();

        private void UrunListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from Urun", baglanti);
            adtr.Fill(daset, "Urun");
            dataGridView1.DataSource = daset.Tables["Urun"];
            baglanti.Close();
        }
        private void KategoriGetir()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from KategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["Kategori"].ToString());
            }
            baglanti.Close();
        }
        private void Stok_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UrunListele();
            KategoriGetir();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BarkodNotxt.Text = dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString();
            Kategoritxt.Text = dataGridView1.CurrentRow.Cells["Kategori"].Value.ToString();
            txtMarka.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
            ÜrünAdıtxt.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            Miktarıtxt.Text = dataGridView1.CurrentRow.Cells["Miktari"].Value.ToString();
            AlışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["AlisFiyat"].Value.ToString();
            SatışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["SatisFiyat"].Value.ToString();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select Kategori,Marka from MarkaBilgileri where Kategori='" + comboKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["Marka"].ToString());
            }
            baglanti.Close();
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from Urun where BarkoNo like '%" + txtBarkodNoAra.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Urun set UrunAdi=@UrunAdi ,Miktari=@Miktari, AlisFiyat=@AlisFiyat,SatisFiyat=@SatisFiyat,Tarih=@Tarih where BarkodNo=@BarkodNo", baglanti);
            komut.Parameters.AddWithValue("@BarkodNo", BarkodNotxt.Text);
            komut.Parameters.AddWithValue("@UrunAdi", ÜrünAdıtxt.Text);
            komut.Parameters.AddWithValue("@Miktari", int.Parse(Miktarıtxt.Text));
            komut.Parameters.AddWithValue("@AlisFiyat", double.Parse(AlışFiyatıtxt.Text));
            komut.Parameters.AddWithValue("@SatisFiyat", double.Parse(SatışFiyatıtxt.Text));
            komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Urun"].Clear();
            UrunListele();
            MessageBox.Show("güncelleme yapıldı");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Urun where BarkodNo= '" + dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString() + "'  ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Urun"].Clear();
            UrunListele();
            MessageBox.Show("Kayıt Silindi");
        }

        private void MarkaGuncelle_Click(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text != "")
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update Urun set Kategori=@Kategori,Marka=@Marka where BarkodNo=@BarkodNo", baglanti);
                komut.Parameters.AddWithValue("@BarkodNo", BarkodNotxt.Text);
                komut.Parameters.AddWithValue("@Kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@Marka", comboMarka.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("güncelleme yapıldı");
                daset.Tables["Urun"].Clear();
                UrunListele();

            }
            else
            {
                MessageBox.Show("Barkod No yazılı değildir", "uyarı");
            }
            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }
    }
}
