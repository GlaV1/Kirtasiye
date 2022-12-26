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
    public partial class KullaniciGirisi : Form
    {
        public KullaniciGirisi()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=SERHAT\\SQLEXPRESS;Initial Catalog=Kirtasiye;Integrated Security=True");
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Select KullaniciAdi,Sifre From KullaniciGirisi where KullaniciAdi=@kulad AND Sifre=@sif", baglanti);
                komut.Parameters.AddWithValue("@kulad", txtKullaniciAdi.Text);
                komut.Parameters.AddWithValue("@sif", txtSifre.Text);
                SqlDataReader oku;
                oku = komut.ExecuteReader();
                if (oku.Read())
                {
                    MessageBox.Show("Giriş başarılı");
                    anasayfa a = new anasayfa();
                    a.Show();
                    this.Visible = false;

                }
                else
                {
                    MessageBox.Show("Yanlış Kullanıcı Adı veya Şifre");
                }

            }

            catch
            {
                MessageBox.Show("Yanlış Kullanıcı Adı veya Şifre");
            }

            finally
            {
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
            }
        }
    }
}
