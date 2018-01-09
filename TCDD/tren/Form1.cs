using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tren
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TCDD TCDDOtomasyon = new TCDD();

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMKaydet_Click(object sender, EventArgs e)
        {
            try
            {
            Musteri m = new Musteri();
            m.Ad = txtAd.Text;
            m.Soyad = txtSoyad.Text;
            m.TcNo = txtTcNo.Text;
            m.Iletisim.Adres = txtAdres.Text;
            m.Iletisim.Email = txtEmail.Text;
            m.Iletisim.Telefon = txtTel.Text;

            TCDDOtomasyon.MusteriEkle(m);

            MusteriListele();
            }
            catch (Exception)
            {
                MessageBox.Show("Kayıt yapılamadı kontrol et ", "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                
            }
            
        }

        public void MusteriListele()
        {
          
            listBoxMusteri2.Items.Clear();

            foreach(var m in TCDDOtomasyon.MusteriListesi)
            {
               
                listBoxMusteri2.Items.Add(m.Ad + " " + m.Soyad);
            }
        }
        public void SeferListele()
        {
            
            listBoxSefer2.Items.Clear();

            foreach (var s in TCDDOtomasyon.SeferListesi)
            {
                
                listBoxSefer2.Items.Add(s.KalkısYeri + "-" + s.VarısYeri+" "+s.SeferTipi+" "+s.GidisTarihi.ToShortDateString()+" "+s.DonusTarihi.ToShortDateString());
            }
        }
        public void RezListele()
        {
            listBoxRez.Items.Clear();
           

            foreach (var r in TCDDOtomasyon.RezervasyonListesi)
            {
                listBoxRez.Items.Add(r.müsteri.TcNo +" " +r.müsteri.Ad + " " + r.müsteri.Soyad +" "+r.sefer.KalkısYeri+" "+r.sefer.VarısYeri+" "+r.sefer.GidisTarihi+" "+r.sefer.DonusTarihi);
            }
        }
        double indirimlitutar;


       
        public void BiletListele()
        {
            listBoxBilet.Items.Clear();

            foreach (var b in TCDDOtomasyon.BiletListesi)
            {
               
             listBoxBilet.Items.Add(b.rezervasyon.müsteri.TcNo + " " + b.rezervasyon.müsteri.Ad + " " + b.rezervasyon.müsteri.Soyad + " " + b.rezervasyon.müsteri.Iletisim.Telefon+" "+" " +b.rezervasyon.müsteri.Iletisim.Email +" "+b.rezervasyon.müsteri.Iletisim.Adres+Environment.NewLine+ b.rezervasyon.sefer.KalkısYeri + " " + b.rezervasyon.sefer.VarısYeri + " " + b.rezervasyon.sefer.GidisTarihi + " " + b.rezervasyon.sefer.DonusTarihi+" "+ b.Ucret + "₺");
            }
        }





        
        private void btnSKaydet_Click(object sender, EventArgs e)
        {
           
           int kalkisindisi = cmbKalkisNoktasi.SelectedIndex;
            int varisindisi = cmbVarisNoktasi.SelectedIndex;
            Hatlar hatlar = new Hatlar();
            if (rbTekYon.Checked)
            {
                Sefer s = new Sefer();
                s.SeferTipi = rbTekYon.Text;
                s.KalkısYeri = cmbKalkisNoktasi.SelectedItem.ToString();
                s.GidisTarihi = DateTime.Parse(dtGidis.Value.ToShortDateString());
                s.VarısYeri = cmbVarisNoktasi.SelectedItem.ToString();
                

                s.Ucret = s.SeferUcretiHesapla(hatlar.KMler[kalkisindisi], hatlar.KMler[varisindisi]);

                TCDDOtomasyon.SeferEkle(s);

                SeferListele();
            }
            else if (rbGidisDonus.Checked)
            {
                 
                Sefer s = new Sefer();
                s.SeferTipi = rbGidisDonus.Text;
                s.KalkısYeri = cmbKalkisNoktasi.SelectedItem.ToString();
                s.GidisTarihi = DateTime.Parse(dtGidis.Value.ToShortDateString());
                s.VarısYeri = cmbVarisNoktasi.SelectedItem.ToString();
                s.DonusTarihi = DateTime.Parse(dtDonus.Value.ToShortDateString());

                s.Ucret = 2*(s.SeferUcretiHesapla(hatlar.KMler[kalkisindisi], hatlar.KMler[varisindisi]));
               
                TCDDOtomasyon.SeferEkle(s);

                SeferListele();
            }
            
            
        }
      
        Musteri m;
        private void btnRezervasyon_Click(object sender, EventArgs e)
        {
             m = TCDDOtomasyon.MusteriListesi[listBoxMusteri2.SelectedIndex];
            Sefer s = TCDDOtomasyon.SeferListesi[listBoxSefer2.SelectedIndex];
            
            TCDDOtomasyon.REzervasyonEkle(new Rezervasyon(m,s));

        
         
            RezListele();
           txtToplamPuan.Text = m.MusteriPuaniGetir().ToString();
            


        }

        private void btnBiletKes_Click(object sender, EventArgs e)
        {
            
            Rezervasyon r = TCDDOtomasyon.RezervasyonListesi[listBoxRez.SelectedIndex];
            if (rbYasli.Checked)
            {
                YasliBilet b = new YasliBilet(r);
                TCDDOtomasyon.BiletEkle(b);
                indirimlitutar +=b.Ucret;
            }
            else if(rbOgrenci.Checked)
            {
                
                OgrenciBilet b = new OgrenciBilet(r);
                TCDDOtomasyon.BiletEkle(b);
                indirimlitutar += b.Ucret;
            }
            else
            {
                StandartBilet b= new StandartBilet(r);
                TCDDOtomasyon.BiletEkle(b);
                indirimlitutar += b.Ucret;
            }

            BiletListele();
            AnlikHesaplama();

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Hatlar hatlar = new Hatlar();
            dtDonus.Enabled = false;
            
            foreach (string item in hatlar.Sehirler)
            {
                cmbKalkisNoktasi.Items.Add(item);
                cmbVarisNoktasi.Items.Add(item);
            }
            txtToplamPuan.Enabled = false;
            txtKrediKartiOdeme.Enabled = false;
            txtNakitÖdeme.Enabled = false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void rbGidisDonus_CheckedChanged(object sender, EventArgs e)
        {
            dtDonus.Enabled = true;
        }

        private void rbTekYon_CheckedChanged(object sender, EventArgs e)
        {
            dtDonus.Enabled = false;
        }

        private void chcKrediKarti_CheckedChanged(object sender, EventArgs e)
        {
            frmKrediKarti frmkredikarti = new frmKrediKarti();
            frmkredikarti.Visible = true;
            txtKrediKartiOdeme.Enabled = true;
        }

        private void label13_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_TextChanged(object sender, EventArgs e)
        {

        }
        public void AnlikHesaplama()
        {
            if(txtNakitÖdeme.Text == "")
            {
                txtNakitÖdeme.Text = "0";
            }
            if(txtKrediKartiOdeme.Text=="")
            {
                txtKrediKartiOdeme.Text = "0";
            }
            
           
            double toplamPuan = double.Parse(txtToplamPuan.Text);
            double kredikarti = double.Parse(txtKrediKartiOdeme.Text);
            double nakit = double.Parse(txtNakitÖdeme.Text);
            if(chcPuan.Checked)
            lblKalanTutar.Text = (indirimlitutar - (toplamPuan + kredikarti + nakit)).ToString();
            else
            lblKalanTutar.Text = (indirimlitutar - (kredikarti + nakit)).ToString();
        }
        private void txtToplamPuan_TextChanged(object sender, EventArgs e)
        {
            AnlikHesaplama();
        }

        private void txtKrediKartiOdeme_TextChanged(object sender, EventArgs e)
        {
            AnlikHesaplama();
        }

        private void txtNakitÖdeme_TextChanged(object sender, EventArgs e)
        {
            AnlikHesaplama();
        }

        private void chcPuan_CheckedChanged(object sender, EventArgs e)
        {
            txtToplamPuan.Enabled = false;
            AnlikHesaplama();
        }

        private void chcNakit_CheckedChanged(object sender, EventArgs e)
        {
            txtNakitÖdeme.Enabled = true; 
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnRezervasyonTamamla_Click(object sender, EventArgs e)
        {
            if(double.Parse(lblKalanTutar.Text) > 0)
            {
                
                MessageBox.Show("Ödenmesi gereken tutarın tamamını ödemediğinizden dolayı rezervasyon işleminiz tamamlanamadı.","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if(double.Parse(lblKalanTutar.Text) <= 0)
            {
                MessageBox.Show("Rezervasyon işleminiz tamamlandı.", "İŞLEM BAŞARILI", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
               // this.Close();
               Form1 frm1 = new Form1();
               this.Hide();
 frm1.Show();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        { 
        }
    }
}
