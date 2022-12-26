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
    public partial class MarkaEkle : Form
    {
        public MarkaEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=SERHAT\\SQLEXPRESS;Initial Catalog=Kirtasiye;Integrated Security=True");
        bool durum;
        private void MarkaKontrol()
        {
           
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from MarkaBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text == read["Kategori"].ToString() && textBox1.Text == read["Marka"].ToString() || comboBox1.Text == "" || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void KategoriGetir()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from KategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["Kategori"].ToString());
            }
            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            MarkaKontrol();
            if (durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into MarkaBilgileri(Kategori,Marka)values('" + comboBox1.Text + "','" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Marka Eklendi");
            }
            else
            {
                MessageBox.Show("böyle bir kategori ve marka  var", "uyarı");
            }
            textBox1.Text = "";
            comboBox1.Text = "";
        }
        private void MarkaEkle_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }

       
    }
}
