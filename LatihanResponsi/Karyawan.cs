using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatihanResponsi
{
    internal class Karyawan
    {
        private string id_karyawan;
        private string nama;
        private string departemen;
        private int id_departemen;
        private string id_jabatan;

        // Encapsulation of Karyawan class
        public string Nama { get => nama; set => nama = value; }
        public int Id_departemen { get => id_departemen; set => id_departemen = value; }

        public Karyawan(string id_karyawan, string nama_karyawan, int id_dep)
        {
            id_karyawan = id_karyawan;
            Nama = nama_karyawan;
            Id_departemen = id_dep;
        }

    }
}
