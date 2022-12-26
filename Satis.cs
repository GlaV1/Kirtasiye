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
    public partial class Satis : Form
    {
        public Satis()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=SERHAT\\SQLEXPRESS;Initial Catalog=Kirtasiye;Integrated Security=True");
        DataSet daset = new DataSet();
        bool durum;

        private void SepetListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from Sepet", baglanti);
            adtr.Fill(daset, "Sepet");
            dataGridView1.DataSource = daset.Tables["Sepet"];
            dataGridView1.Columns[0].Visible = true;
            baglanti.Close();
        }
        private void Temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtÜrünMiktarı)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        private void BarkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Sepet", baglanti);
            SqlDataReader read= komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["BarkodNo"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void Hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(ToplamFiyat) from Sepet", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + "TL";
                baglanti.Close();
            }
            catch (Exception)
            {

                
            }
        }
        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Urun where BarkodNo like '"+txtBarkodNo.Text+"' ", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtÜrünAdı.Text = read["UrunAdi"].ToString();
                txtSatışFiyati.Text = read["SatisFiyat"].ToString();
            }
            baglanti.Close();
        }
        private void Satis_Load(object sender, EventArgs e)
        {
            SepetListele();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtÜrünMiktarı_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (decimal.Parse(txtÜrünMiktarı.Text) * decimal.Parse(txtSatışFiyati.Text)).ToString();
            }
            catch (Exception)
            {
                ;

            }
        }

        private void txtSatışFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (decimal.Parse(txtÜrünMiktarı.Text) * decimal.Parse(txtSatışFiyati.Text)).ToString();
            }
            catch (Exception)
            {
                ;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BarkodKontrol();
            if (durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into Sepet(BarkodNo,UrunAdi,Miktari,SatisFiyat,ToplamFiyat,Tarih) values (@BarkodNo,@UrunAdi,@Miktari,@SatisFiyat,@ToplamFiyat,@Tarih)", baglanti);
                komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@UrunAdi", txtÜrünAdı.Text);
                komut.Parameters.AddWithValue("@Miktari", int.Parse(txtÜrünMiktarı.Text));
                komut.Parameters.AddWithValue("@SatisFiyat", decimal.Parse(txtSatışFiyati.Text));
                komut.Parameters.AddWithValue("@ToplamFiyat", decimal.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update Sepet set Miktari=Miktari+'" + int.Parse(txtÜrünMiktarı.Text) + "' where BarkodNo='" + txtBarkodNo.Text + "'", baglanti);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update Sepet set ToplamFiyat=Miktari*SatisFiyat where BarkodNo='" + txtBarkodNo.Text + "'", baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();
            }
            txtÜrünMiktarı.Text = "1";
            daset.Tables["Sepet"].Clear();
            SepetListele();
            Hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtÜrünMiktarı)
                    {
                        item.Text = "";
                    }
                }
            }
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into Satis(BarkodNo,UrunAdi,Miktari,SatisFiyat,toplamfiyati,Tarih) values (@BarkodNo,@UrunAdi,@Miktari,@SatisFiyat,@ToplamFiyat,@Tarih)", baglanti);
                komut.Parameters.AddWithValue("@BarkodNo", dataGridView1.Rows[i].Cells["BarkodNo"].Value.ToString());
                komut.Parameters.AddWithValue("@UrunAdi", dataGridView1.Rows[i].Cells["UrunAdi"].Value.ToString());
                komut.Parameters.AddWithValue("@Miktari", int.Parse(dataGridView1.Rows[i].Cells["Miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@SatisFiyat", decimal.Parse(dataGridView1.Rows[i].Cells["SatisFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@ToplamFiyat", decimal.Parse(dataGridView1.Rows[i].Cells["ToplamFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update Urun set Miktari=Miktari - '" + int.Parse(dataGridView1.Rows[i].Cells["Miktari"].Value.ToString())+"' where BarkodNo='" + dataGridView1.Rows[i].Cells["BarkodNo"].Value.ToString() +"' ", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from Sepet ", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Sepet"].Clear();
            SepetListele();
            Hesapla();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Sepet where BarkodNo='" + dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("ürünler sepetten çıkarıldı");
            daset.Tables["Sepet"].Clear();
            SepetListele();
            Hesapla();
        }

        private void btnSatisİptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Sepet ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Satış İptal Edildi");
            daset.Tables["Sepet"].Clear();
            SepetListele();
            Hesapla();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
