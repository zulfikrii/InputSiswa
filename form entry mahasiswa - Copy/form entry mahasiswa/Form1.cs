using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace form_entry_mahasiswa
{
    public partial class Form1 : Form
    {
        public delegate void SaveUpdateEventHandler(Mahasiswa obj);
        public event SaveUpdateEventHandler OnSave;
        public event SaveUpdateEventHandler OnUpdate;
        private Form1 frm;
        private bool isNewData = true;
        private Mahasiswa mhs = null;
        private IList<Mahasiswa> listOfMahasiswa = new List<Mahasiswa>();
        
        public Form1()
        {
            InitializeComponent();
            InisialisasiListView();
            InisialisasiDataDummy();
           
        }
    
        public void Form12 (Mahasiswa obj)
        {

            this.isNewData = false;
            this.mhs = obj;

            txtNim.Text = this.mhs.Nim;
            txtName.Text = this.mhs.Name;

            if (this.mhs.Gender == "Laki-laki")
                rdoLakilaki.Checked = true;
            else
                rdoPerempuan.Checked = true;

            txtAlamat.Text = this.mhs.Alamat;
            txtBorn.Text = this.mhs.Born;
            dtpDate.Value = DateTime.Parse(this.mhs.Date);
        }
      
        private void InisialisasiListView()
        {
            lvwMahasiswa.View = System.Windows.Forms.View.Details;
            lvwMahasiswa.FullRowSelect = true;
            lvwMahasiswa.GridLines = true;

            lvwMahasiswa.Columns.Add("No.", 30, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Nim", 91, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Name", 200, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Jenis Kelamin", 90, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Tempat lahir", 200, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Tanggal lahir", 91, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Alamat", 200, HorizontalAlignment.Left);
        }

        private void InisialisasiDataDummy()
        {
            listOfMahasiswa.Add(new Mahasiswa { Nim = "17.11.0001", Name = "BUDI", Gender = "Laki-laki", Born="Jakarta",Date="10/10/1990",Alamat="Jakarta" });
            listOfMahasiswa.Add(new Mahasiswa { Nim = "17.11.0002", Name = "SANTI", Gender = "Perempuan",Born="Jakarta",Date="10/10/1990",Alamat="Bogor" });
           

            foreach (var obj in listOfMahasiswa)
            {
                FillToListView(true, obj);
            }
        }

      
        private void FillToListView(bool isNewData, Mahasiswa mhs)
        {
            if (isNewData) 
            {
                int noUrut = lvwMahasiswa.Items.Count + 1;

                ListViewItem item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(mhs.Nim);
                item.SubItems.Add(mhs.Name);
                item.SubItems.Add(mhs.Gender);
                item.SubItems.Add(mhs.Born);
                item.SubItems.Add(mhs.Date);
                item.SubItems.Add(mhs.Alamat);

                lvwMahasiswa.Items.Add(item);
            }
            else 
            {
                int row = lvwMahasiswa.SelectedIndices[0];

                ListViewItem itemRow = lvwMahasiswa.Items[row];
                itemRow.SubItems[1].Text = mhs.Nim;
                itemRow.SubItems[2].Text = mhs.Name;
                itemRow.SubItems[3].Text = mhs.Gender;
                itemRow.SubItems[4].Text = mhs.Born;
                itemRow.SubItems[5].Text = mhs.Date;
                itemRow.SubItems[6].Text = mhs.Alamat;
            }
        }
       
        
        private void Form1_OnSave(Mahasiswa obj)
        {
            listOfMahasiswa.Add(obj);
            FillToListView(true, obj);
        }

        
        private void reset()
        {
            txtNim.Clear();
            txtName.Clear();
            rdoLakilaki.Checked = true;
            txtAlamat.Clear();
            dtpDate.Value = DateTime.Today;
            txtBorn.Clear();


            txtNim.Focus();
        }
        private void Form1_OnUpdate(Mahasiswa obj)
        {
            listOfMahasiswa.Add(obj);
            FillToListView(false, obj);
        }
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                var mhs = listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];

                var msg = string.Format("Apakah data mahasiswa '{0}' ingin dihapus ?", mhs.Name);

                if (MessageBox.Show(msg, "Konfirmasi", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    listOfMahasiswa.Remove(mhs); 

                    lvwMahasiswa.Items.Clear();
                    foreach (var obj in listOfMahasiswa)
                    {
                        FillToListView(true, obj);
                    }
                }
            }
            else 
            {
                MessageBox.Show("Data belum dipilih", "Peringatan", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvwMahasiswa_DoubleClick(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                var mhs = listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];
                Form12(mhs);

            }
            
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {

            if (!txtNim.MaskFull)
            {
                MessageBox.Show("Nim Harus diisi", "Konfirmasi", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return;
            }
            if (!(txtName.Text.Length > 0))
            {
                MessageBox.Show("Nama Harus diisi", "Konfirmasi", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return;
            }
            var msg = string.Format("Apakah data mahasiswa '{0}' ingin disimpan ?", txtName.Text);

            if (MessageBox.Show(msg, "Konfirmasi", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (isNewData)
                    mhs = new Mahasiswa();

                mhs.Nim = txtNim.Text;
                mhs.Name = txtName.Text;
                mhs.Gender = rdoLakilaki.Checked ? "Laki-laki" : "Perempuan";
                mhs.Alamat = txtAlamat.Text;
                mhs.Born = txtBorn.Text;
                mhs.Date = dtpDate.Value.ToString("dd/MM/yyyy");
                if (isNewData) // data baru
                {
                    Form1_OnSave(mhs);
                    reset();
                }
                else
                {
                    Form1_OnUpdate(mhs);// panggil event OnUpdate
                    reset();
                    isNewData = true;
                }
            }
            
        }

        private void btnSimpan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!txtNim.MaskFull)
            {
                MessageBox.Show("Nim Harus diisi", "Konfirmasi", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return;
            }
            if (!(txtName.Text.Length > 0))
            {
                MessageBox.Show("Nama Harus diisi", "Konfirmasi", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return;
            }
            if (isNewData)
                mhs = new Mahasiswa();

            mhs.Nim = txtNim.Text;
            mhs.Name = txtName.Text;
            mhs.Gender = rdoLakilaki.Checked ? "Laki-laki" : "Perempuan";
            mhs.Alamat = txtAlamat.Text;
            mhs.Born = txtBorn.Text;
            mhs.Date = dtpDate.Value.ToString("dd/MM/yyyy");
            if (isNewData) // data baru
            {
                Form1_OnSave(mhs);
                reset();
            }
            else
            {
                Form1_OnUpdate(mhs);// panggil event OnUpdate
                reset();
                isNewData = true;
            }
        }
    }
}
