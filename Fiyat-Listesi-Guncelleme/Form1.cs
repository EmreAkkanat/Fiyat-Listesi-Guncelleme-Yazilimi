using System.IO;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace empmakros2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            anaData(dataGridView1);
            olmayanData(dataGridView2);
            yinelenenData(dataGridView3);
        }

        // VERİ TABANI BAĞLANTISINI YAPTIKTAN SONRA ÇALIŞIR

        SqlConnection baglanti = new SqlConnection("veri tabanı bağlantısı");
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView3.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView1.AllowUserToAddRows = false;
            //---------------------------------------------------Veri Tabanında olmayan ürünler ayırma
            List<int> list1 = new List<int>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                SqlCommand cmd = new SqlCommand("select stok_kodu from tablo_ismi", baglanti);
                baglanti.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                string dss = "";
                string deger = "";
                while (dr.Read())
                {
                    deger = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    if (deger.Equals(dr["stok_kodu"].ToString()))
                    {
                        dss = "var";
                        break;
                    }
                    else
                    {
                        dss = "yok";
                    }
                }
                if (dss == "yok")
                {
                    list1.Add(i);
                    dataGridView2.Rows.Add(deger);
                }
                baglanti.Close();
            }
            list1.Reverse();
            foreach (int item in list1)
            {
                dataGridView1.Rows.RemoveAt(item);
            }
            list1.Clear();
            //-------------------------------------------------------------------------------------son
            //---------------------------------------------------------------Yinelenen ürünleri ayırma
            List<string> list2 = new List<string>();
            List<string> list6 = new List<string>();
            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                string ass = dataGridView1.Rows[j].Cells[0].Value.ToString();
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    list2.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                list2.RemoveAt(j);
                for (int k = 0; k < list2.Count; k++)
                {
                    string asss = list2.ElementAt(k).ToString();
                    if (ass == asss)
                    {
                        dataGridView3.Rows.Add(ass);
                        list6.Add(ass);
                        break;
                    }
                }
                list2.Clear();
            }
            for (int h = 0; h < dataGridView1.RowCount; h++)
            {
                string hgf = dataGridView1.Rows[h].Cells[0].Value.ToString();
                for (int p = 0; p < list6.Count; p++)
                {
                    string fgh = list6.ElementAt(p);
                    if (hgf==fgh)
                    {
                        dataGridView1.Rows.RemoveAt(h);
                        h--;
                        break;
                    }
                }
            }
            list6.Clear();
            List<string> list25 = new List<string>();
            for (int j = 0; j < dataGridView3.RowCount; j++)
            {
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    list25.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                }
                string ass = dataGridView3.Rows[j].Cells[0].Value.ToString();
                list25.RemoveAt(j);
                for (int k = 0; k < list25.Count; k++)
                {
                    string asss = list25.ElementAt(k).ToString();
                    if (ass == asss)
                    {
                        dataGridView3.Rows.RemoveAt(j);
                        j--;
                        break;
                    }
                }
                list25.Clear();
            }
            //-------------------------------------------------------------------------------------son
            dataGridView3.Refresh();
            dataGridView2.Refresh();
            dataGridView1.Refresh();
        }

        private void yinelenenData(DataGridView dataGridView3)
        {
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToDeleteRows = true;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.AllowUserToAddRows = false;

            dataGridView3.ColumnCount = 1;
            dataGridView3.Columns[0].Name = "STOK_KODU";

            dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void olmayanData(DataGridView dataGridView2)
        {
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToDeleteRows = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;

            dataGridView2.ColumnCount = 1;
            dataGridView2.Columns[0].Name = "STOK_KODU";

            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void anaData(DataGridView dataGridView1)
        {
            dataGridView1.ReadOnly = false; // sadece okunabilir olması yani veri düzenleme kapalı
            dataGridView1.AllowUserToDeleteRows = false; // satırların silinmesi engelleniyor
            dataGridView1.RowHeadersVisible = false; //Gizlenmesini sağlar
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.ColumnCount = 4; //Kaç kolon olacağı belirleniyor…
            dataGridView1.Columns[0].Name = "STOK_KODU";//Kolonların adı belirleniyor
            dataGridView1.Columns[1].Name = "TL";
            dataGridView1.Columns[2].Name = "DOLAR";
            dataGridView1.Columns[3].Name = "EURO";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            if (dataGridView1.RowCount < 1)
            {
                dataGridView1.Rows.Add(1);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Replace("\n", "").Split('\r');

                List<string> list = new List<string>(lines);
                list.RemoveAt(lines.Length - 1);
                lines = list.ToArray();
                list.Clear();

            int row = dataGridView1.CurrentCell.RowIndex;
                int col = dataGridView1.CurrentCell.ColumnIndex;
                if (lines.Length > 1)
                {
                    dataGridView1.Rows.Add(lines.Length-1);
                }

                string[] fields;
                foreach (string item in lines)
                {
                    //dataGridView1.Rows.Add(1);
                    fields = item.Split('\t');
                    foreach (string f in fields)
                    {
                        dataGridView1[col, row].Value = f;
                        col++;
                    }
                    row++;
                    col = dataGridView1.CurrentCell.ColumnIndex;
                }
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                dataGridView1.Rows.Add(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (dataGridView1.RowCount < 1)
            {
                dataGridView1.Rows.Add(1);
            }
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                String insertData = "INSERT INTO tablo_ismi(stok_kodu, tl, dolar, euro) VALUES (@stok_kodu, @tl, @dolar, @euro)";
                SqlCommand cmd = new SqlCommand(insertData, baglanti);
                cmd.Parameters.AddWithValue("@stok_kodu", dataGridView1.Rows[i].Cells[0].Value);
                cmd.Parameters.AddWithValue("@tl", dataGridView1.Rows[i].Cells[1].Value);
                cmd.Parameters.AddWithValue("@dolar", dataGridView1.Rows[i].Cells[2].Value);
                cmd.Parameters.AddWithValue("@euro", dataGridView1.Rows[i].Cells[3].Value);
                da.InsertCommand = cmd;
                cmd.ExecuteNonQuery();
            }
            baglanti.Close();
            MessageBox.Show("İşleminiz başarıyla gerçekleştirilmiştir");
        }
    }
}
