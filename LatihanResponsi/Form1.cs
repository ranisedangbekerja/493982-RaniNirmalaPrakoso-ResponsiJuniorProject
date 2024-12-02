using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace LatihanResponsi

{
    
    public partial class Form1 : Form
    {
        public Form1() //inheritance
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            Form1_Load(null, null);
        }
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private string sql;
        public DataTable dt;
        private DataGridViewRow selectedRow;
        private string host = "localhost";
        private string port = "5432";
        private string user = "postgres";
        private string password = "informatika";
        private string database = "finalresponsi";
        public string connectionString() //abstraction
        {
            return "Host=" + host + "; Port=" + port + "; Username=" + user + "; Password=" + password + "; Database=" + database;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text; // Display this in the ComboBox
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                MessageBox.Show("Koneksi Berhasil");
                conn.Close();
                LoadComboJab();
                LoadComboDep();

                btnLoad_Click(sender, e);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private int GetSelectedJabatanId()
        {
            if (comboJab.SelectedItem != null)
            {
                // Cast the selected item back to ComboBoxItem
                ComboBoxItem selectedItem = (ComboBoxItem)comboJab.SelectedItem;
                return selectedItem.Value; // Retrieve the id_jabatan
            }
            return -1; // Return -1 if no item is selected
        }
        private void LoadComboJab()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString()))
                {
                    conn.Open();
                    string sql = "SELECT id_jabatan, nama_jabatan FROM public.jabatan";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing items
                            comboJab.Items.Clear();

                            while (reader.Read())
                            {
                                // Add items to the ComboBox
                                comboJab.Items.Add(new ComboBoxItem
                                {
                                    Text = reader["nama_jabatan"].ToString(),
                                    Value = Convert.ToInt32(reader["id_jabatan"])
                                });
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private int GetSelectedDepartemenId()
        {
            if (comboDep.SelectedItem != null)
            {
                // Cast the selected item back to ComboBoxItem
                ComboBoxItem selectedItem = (ComboBoxItem)comboDep.SelectedItem;
                return selectedItem.Value; // Retrieve the id_jabatan
            }
            return -1; // Return -1 if no item is selected
        }
        private void LoadComboDep()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString()))
                {
                    conn.Open();
                    string sql = "SELECT id_dep, nama_departemen FROM public.departemen";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing items
                            comboDep.Items.Clear();

                            while (reader.Read())
                            {
                                // Add items to the ComboBox
                                comboDep.Items.Add(new ComboBoxItem
                                {
                                    Text = reader["nama_departemen"].ToString(),
                                    Value = Convert.ToInt32(reader["id_dep"])
                                });
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)  //encapsulation
        {
            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                sql = "SELECT * FROM public.view_karyawan";
                cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void txtNama_TextChanged(object sender, EventArgs e)
        {
            // check if the text box is empty
            if (txtNama.Text == "")
            {
                btnInsert.Enabled = false;
            }
            else
            {
                btnInsert.Enabled = true;
            }
        }

        private void comboDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(comboDep.Text + " Selected!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Example SQL QUERY
            // DELETE FROM public.Karyawan WHERE id_karyawan = 'RN1'

            try
            {
                selectedRow = dataGridView1.SelectedRows[0];
                MessageBox.Show(selectedRow.Cells[0].Value.ToString() + " Selected!");

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Pilih data yang ingin dihapus");
                return;
            }

            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM public.Karyawan WHERE \"id_karyawan\" = '" + selectedRow.Cells[0].Value.ToString() + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Dihapus");
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            btnLoad_Click(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Membuka koneksi ke database
            conn = new NpgsqlConnection(connectionString());
            try
            {
                // Validasi data yang dipilih di DataGridView
                try
                {
                    selectedRow = dataGridView1.SelectedRows[0];
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Pilih data yang ingin diubah!");
                    return;
                }

                // Validasi input
                if (string.IsNullOrWhiteSpace(txtNama.Text) || comboDep.SelectedIndex == -1)
                {
                    MessageBox.Show("Nama karyawan dan departemen harus diisi!");
                    return;
                }

                // Ambil input dari form
                string namaKaryawan = txtNama.Text.Trim();
                int idDepartemen = GetSelectedDepartemenId();
                int idJabatan = GetSelectedJabatanId();
          


                // Pastikan idDepartemen valid
                if (idDepartemen == -1)
                {
                    MessageBox.Show("Departemen tidak valid!");
                    return;
                }

                // Pastikan idJabatan valid
                if (idJabatan == -1)
                {
                    MessageBox.Show("Jabatan tidak valid!");
                    return;
                }

                // Ambil id_karyawan dari baris yang dipilih
                string idKaryawan = selectedRow.Cells[0].Value.ToString();

                // Query dengan parameter
                string query = "UPDATE public.karyawan SET nama_karyawan=@nama, id_dep=@id_dep, id_jabatan=@id_jabatan WHERE id_karyawan=@id_karyawan;";

                // Buka koneksi
                conn.Open();

                // Gunakan parameterized query untuk keamanan
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", namaKaryawan);
                    cmd.Parameters.AddWithValue("@id_dep", idDepartemen);
                    cmd.Parameters.AddWithValue("@id_jabatan", idJabatan);
                    cmd.Parameters.AddWithValue("@id_karyawan", idKaryawan.Substring(0, 6));


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data Berhasil Diubah");
                    }
                    else
                    {
                        MessageBox.Show("Tidak ada data yang diubah. Periksa ID Karyawan.");
                    }
                }

                // Beri notifikasi kepada pengguna
                MessageBox.Show("Data Berhasil Diubah");

                // Tutup koneksi
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                // Tampilkan pesan error jika ada
                MessageBox.Show($"Error: {ex.Message}");
                conn.Close();
            }

            // Refresh tabel
            btnLoad_Click(sender, e);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Membuka koneksi ke database
            conn = new NpgsqlConnection(connectionString());
            try
            {
                // Validasi input
                if (string.IsNullOrWhiteSpace(txtNama.Text) || comboDep.SelectedIndex == -1)
                {
                    MessageBox.Show("Nama karyawan dan departemen harus diisi!");
                    return;
                }

                // Ambil input pengguna
                string namaKaryawan = txtNama.Text.Trim();
                int idDepartemen = GetSelectedDepartemenId();
                int idJabatan = GetSelectedJabatanId();
                MessageBox.Show(namaKaryawan);
                MessageBox.Show(idDepartemen.ToString());
                MessageBox.Show(idJabatan.ToString());

                // Pastikan idDepartemen valid
                if (idDepartemen == -1)
                {
                    MessageBox.Show("Departemen tidak valid!");
                    return;
                }

                // Query menggunakan parameter
                string query = "INSERT INTO public.karyawan (nama_karyawan, id_dep, id_jabatan) VALUES (@nama, @id_dep, @id_jabatan)";

                // Buka koneksi
                conn.Open();

                // Menggunakan parameterized query
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", namaKaryawan);
                    cmd.Parameters.AddWithValue("@id_dep", idDepartemen);
                    cmd.Parameters.AddWithValue("@id_jabatan", idJabatan);
                    // Eksekusi query
                    cmd.ExecuteNonQuery();
                }

                // Beri notifikasi kepada pengguna
                MessageBox.Show("Data Berhasil Ditambahkan");

                // Tutup koneksi
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                // Tampilkan pesan error jika ada
                MessageBox.Show($"Error: {ex.Message}");
                conn.Close();
            }

            // Refresh tabel
            btnLoad_Click(sender, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedRow = dataGridView1.SelectedRows[0];
                MessageBox.Show(selectedRow.Cells[0].Value.ToString() + " Selected!");
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
            txtNama.Text = selectedRow.Cells[1].Value.ToString();
            comboDep.Text = selectedRow.Cells[2].Value.ToString();
            comboJab.Text = selectedRow.Cells[3].Value.ToString();

        }


        private void comboJab_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(comboJab.Text + " Selected!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 sd = new Form2();
            sd.Show();
        }
    }
    
}
