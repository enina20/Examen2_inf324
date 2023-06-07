using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;



namespace Regiones
{
    public partial class Form1 : Form
    {
        int cR, cG, cB;
        int ultimoR, ultimoG, ultimoB;
        public Form1()
        {
            InitializeComponent();
            mostrar_list_View();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {


                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Color c = new Color();
                int mR, mG, mB;
                mR = 0; mG = 0; mB = 0;

                c = bmp.GetPixel(e.X, e.Y);
                ultimoR = mR + c.R;
                ultimoG = mG + c.G;
                ultimoB = mB + c.B;

                for (int i = e.X - 5; i < e.X + 5; i++)
                    for (int j = e.Y - 5; j < e.Y + 5; j++)
                    {
                        c = bmp.GetPixel(i, j);
                        mR = mR + c.R;
                        mG = mG + c.G;
                        mB = mB + c.B;

                    }
                mR = mR / 100;
                mG = mG / 100;
                mB = mB / 100;
                textBox1.Text = mR.ToString();
                textBox2.Text = mG.ToString();
                textBox3.Text = mB.ToString();
                cR = mR;
                cG = mG;
                cB = mB;

                Color red = Color.Red;
                textBox5.BackColor = Color.FromArgb(cR, cG, cB);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();

                con.ConnectionString = "Server=DESKTOP-A2SIL6H\\MSSQLSERVER01;Database=academico;Trusted_Connection=True;MultipleActiveResultSets = true; TrustServerCertificate=True";
                cmd.Connection = con;

                String hexColor = System.Drawing.ColorTranslator.ToHtml(Color.FromArgb(ultimoR, ultimoG, ultimoB, 255));  //returns the hex value

                cmd.CommandText = "insert into colores values('" +
                    textBox4.Text + "'," + cR.ToString() + "," + cG.ToString()
                    + "," + cB.ToString() + ",'" + hexColor + "')";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                mostrar_list_View();
            }
        }
       

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (listView1.SelectedIndices.Count <= 0)
                {
                    return;
                }
                int intselectedindex = listView1.SelectedIndices[0];
                if (intselectedindex >= 0)
                {
                    String nombre_region = listView1.Items[intselectedindex].Text;

                    Bitmap bmp = new Bitmap(pictureBox1.Image);
                    Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
                    int mmR, mmG, mmB;
                    Color c = new Color();
                    string cad = "";

                    SqlConnection cn = new SqlConnection();
                    SqlCommand cm = new SqlCommand();

                    cn.ConnectionString = "Server=DESKTOP-A2SIL6H\\MSSQLSERVER01;Database=academico;Trusted_Connection=True;MultipleActiveResultSets = true; TrustServerCertificate=True";
                    cm.Connection = cn;
                    cm.CommandText = "select*from colores where descripcion like '" + nombre_region.ToString() + "'";
                    cn.Open();
                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cR = dr.GetInt32(1);
                            cG = dr.GetInt32(2);
                            cB = dr.GetInt32(3);

                            cad = cad + " : " + dr.GetString(0);

                            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                            {
                                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                                {
                                    mmR = 0; mmG = 0; mmB = 0;
                                    for (int k = i; k < i + 10; k++)
                                        for (int l = j; l < j + 10; l++)
                                        {
                                            c = bmp.GetPixel(k, l);
                                            mmR = mmR + c.R;
                                            mmG = mmG + c.G;
                                            mmB = mmB + c.B;
                                        }
                                    mmR = mmR / 100;
                                    mmG = mmG / 100;
                                    mmB = mmB / 100;

                                    if (((cR - 20 < mmR) && (mmR < cR + 20)) && ((cG - 10 < mmG) && (mmG < cG + 20))
                                        && ((cB - 20 < mmB) && (mmB < cB + 20)))
                                    {
                                        for (int k = i; k < i + 10; k++)
                                            for (int l = j; l < j + 10; l++)
                                            {
                                                if (k == i || k == i + 19 || l == j || l == j + 19)
                                                {
                                                    bmp2.SetPixel(k, l, Color.Blue);
                                                }
                                                else
                                                {
                                                    bmp2.SetPixel(k, l, Color.FromArgb(255 - cR, 255 - cG, 255 - cB));
                                                }
                                            }
                                    }
                                    else
                                    {
                                        for (int k = i; k < i + 10; k++)
                                            for (int l = j; l < j + 10; l++)
                                            {
                                                c = bmp.GetPixel(k, l);
                                                bmp2.SetPixel(k, l, Color.FromArgb(c.R, c.G, c.B));
                                            }
                                    }
                                }
                            }
                            pictureBox2.Image = bmp2;
                            bmp = new Bitmap(pictureBox2.Image);
                        }

                    }
                    cn.Close();
                }
            }
        }

        private void mostrar_list_View()
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();

            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand();

            cn.ConnectionString = "Server=DESKTOP-A2SIL6H\\MSSQLSERVER01;Database=academico;Trusted_Connection=True;MultipleActiveResultSets = true; TrustServerCertificate=True";
            cm.Connection = cn;
            cm.CommandText = "select * from colores";
            cn.Open();
            SqlDataReader dr = cm.ExecuteReader();
            if (dr.HasRows)
            {
                
                while (dr.Read())
                {
                    cR = dr.GetInt32(1);
                    cG = dr.GetInt32(2);
                    cB = dr.GetInt32(3);
                    listView1.Items.Add((dr.GetString(0)));
                    listView2.Items.Add("aaaa");
                    listView2.Items[listView2.Items.Count - 1].ForeColor = Color.FromArgb(255 - cR, 255 - cG, 255 - cB);
                    listView2.Items[listView2.Items.Count - 1].BackColor = Color.FromArgb(255-cR,255- cG,255- cB);
                    listView3.Items.Add((dr.GetString(0)));

                    listView4.Items.Add("aaaa");
                    listView4.Items[listView4.Items.Count - 1].ForeColor = Color.FromArgb(cR, cG, cB);
                    listView4.Items[listView4.Items.Count - 1].BackColor = Color.FromArgb(cR, cG, cB);

                }
            }

            cn.Close();
        }       

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {


                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
                Color c = new Color();
                int mmR, mmG, mmB;
                con.ConnectionString = "Server=DESKTOP-A2SIL6H\\MSSQLSERVER01;Database=academico;Trusted_Connection=True;MultipleActiveResultSets = true; TrustServerCertificate=True";
                cmd.Connection = con;
                cmd.CommandText = "select DISTINCT descripcion from colores";
                con.Open();
                string cadd = "";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String nombre_region = dr.GetString(0);
                    SqlConnection cn = new SqlConnection();
                    SqlCommand cm = new SqlCommand();

                    cn.ConnectionString = "Server=DESKTOP-A2SIL6H\\MSSQLSERVER01;Database=academico;Trusted_Connection=True;MultipleActiveResultSets = true; TrustServerCertificate=True";
                    cm.Connection = cn;
                    cm.CommandText = "select*from colores where descripcion like '" + nombre_region.ToString() + "'";
                    cn.Open();
                    SqlDataReader drr = cm.ExecuteReader();
                    if (drr.HasRows)
                    {
                        while (drr.Read())
                        {
                            cR = drr.GetInt32(1);
                            cG = drr.GetInt32(2);
                            cB = drr.GetInt32(3);

                            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                            {
                                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                                {
                                    mmR = 0; mmG = 0; mmB = 0;
                                    for (int k = i; k < i + 10; k++)
                                        for (int l = j; l < j + 10; l++)
                                        {
                                            c = bmp.GetPixel(k, l);
                                            mmR = mmR + c.R;
                                            mmG = mmG + c.G;
                                            mmB = mmB + c.B;
                                        }
                                    mmR = mmR / 100;
                                    mmG = mmG / 100;
                                    mmB = mmB / 100;

                                    if (((cR - 10 < mmR) && (mmR < cR + 10)) && ((cG - 10 < mmG) && (mmG < cG + 10))
                                        && ((cB - 10 < mmB) && (mmB < cB + 10)))
                                    {
                                        for (int k = i; k < i + 10; k++)
                                            for (int l = j; l < j + 10; l++)
                                            {
                                                if (k == i || k == i + 19 || l == j || l == j + 19)
                                                {
                                                    bmp2.SetPixel(k, l, Color.Blue);
                                                }
                                                else
                                                {
                                                    bmp2.SetPixel(k, l, Color.FromArgb(255 - cR, 255 - cG, 255 - cB));                                                    
                                                }
                                            }
                                    }
                                    else
                                    {
                                        for (int k = i; k < i + 10; k++)
                                            for (int l = j; l < j + 10; l++)
                                            {
                                                c = bmp.GetPixel(k, l);
                                                bmp2.SetPixel(k, l, Color.FromArgb(c.R, c.G, c.B));
                                            }
                                    }
                                }
                            }
                            pictureBox2.Image = bmp2;
                            bmp = new Bitmap(pictureBox2.Image);

                        }

                    }
                    cadd = cadd + " : " + dr.GetString(0);
                    cn.Close();

                }

                pictureBox2.Image = bmp2;

                con.Close();
            }
        }
    }
}