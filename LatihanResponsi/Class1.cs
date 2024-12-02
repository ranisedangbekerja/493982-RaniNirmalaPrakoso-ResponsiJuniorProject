namespace junpro9
{
    public class Akun
    {
        private int saldo;  // Atribut saldo (private)

        // Property untuk mengakses saldo
        public int Saldo
        {
            get { return saldo; }
        }

        // Constructor untuk inisialisasi saldo awal
        public Akun(int saldoAwal)
        {
            saldo = saldoAwal;
        }

        // Method untuk setor uang (virtual agar bisa di-override)
        public virtual void Setor(int jumlah)
        {
            if (jumlah > 0)
                saldo += jumlah;
        }

        // Method untuk tarik uang dengan validasi saldo
        public bool Tarik(int jumlah)
        {
            if (jumlah > 0 && jumlah <= saldo)
            {
                saldo -= jumlah;
                return true;
            }
            return false;
        }
    }
}
