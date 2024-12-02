using System.Windows.Forms;

namespace junpro9
{
    public class AkunPremium : Akun
    {
        private double bonusRate;  // Tingkat bonus (contoh 5% = 0.05)

        // Constructor AkunPremium, menggunakan constructor dari Akun (base)
        public AkunPremium(int saldoAwal, double bonusRate)
            : base(saldoAwal)  // Memanggil constructor kelas induk
        {
            this.bonusRate = bonusRate;
        }

        // Override metode Setor untuk menambahkan bonus
        public override void Setor(int jumlah)
        {
            if (jumlah > 0)
            {
                // Tambahkan jumlah ke saldo dan berikan bonus
                base.Setor(jumlah);  // Panggil metode Setor dari kelas induk
                int bonus = (int)(jumlah * bonusRate);
                base.Setor(bonus);  // Tambahkan bonus ke saldo

                // Tampilkan pesan bonus
                MessageBox.Show($"Setor berhasil dengan bonus {bonus}!");
            }
        }
    }
}
