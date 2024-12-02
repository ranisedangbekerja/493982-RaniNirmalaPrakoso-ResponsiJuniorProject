using junpro9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatihanResponsi
{
    public partial class Form2 : Form
    {
        private Akun akun;  // Instance kelas Akun
        private string correctPassword = "mama";  // Password untuk validasi transaksi
        public Form2()
        {
            InitializeComponent();
            akun = new AkunPremium(143132, 0.05); // pake class 2
            TampilkanSaldo();  // Tampilkan saldo awal di Label
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            TampilkanSaldo();  // Tampilkan saldo saat form pertama kali dibuka
            txtPassword.PasswordChar = '*';  // Sembunyikan input password dengan '*'
        }
        private void TampilkanSaldo()
        {
            lblSaldo.Text = "Saldo: " + akun.Saldo.ToString();  // lblSaldo: Label untuk saldo
        }

        private void btnLanjut_Click(object sender, EventArgs e)
        {
            // Cek apakah password benar
            if (txtPassword.Text != "mama")
            {
                MessageBox.Show("Password salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cek apakah jumlah uang yang dimasukkan valid
            int jumlahUang;
            bool validJumlah = int.TryParse(textJumlah.Text, out jumlahUang);

            if (!validJumlah || jumlahUang <= 0)
            {
                MessageBox.Show("Masukkan jumlah uang yang valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cek apakah opsi Setor atau Tarik dipilih
            if (rdbSetor.Checked)
            {
                akun.Setor(jumlahUang);
                MessageBox.Show("Setor berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rdbTarik.Checked)
            {
                if (akun.Tarik(jumlahUang))
                    MessageBox.Show("Tarik berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Saldo tidak cukup.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Pilih jenis transaksi (Setor atau Tarik).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update saldo setelah transaksi
            TampilkanSaldo();
            // Reset input
            textJumlah.Clear();
            txtPassword.Clear();
        }
    }
}
