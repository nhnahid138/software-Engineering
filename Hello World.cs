using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class oldPatient : Form
    {
        int c_id;
        int d_id;
        Panel parentPanel;
        int pid;
        String disease;
        void getoldpatient(int did)
        {
            using (SqlConnection con = new SqlConnection(Global.constring))
            {
                con.Open();

                string query = @"
        SELECT DISTINCT
               a.checkup_id,
               p.patient_id,
               p.name,
               p.age,
               a.date,
               a.disease
        FROM checkup a
        JOIN Patient p ON a.patient_id = p.patient_id
        WHERE a.doctor_id = @doctorId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@doctorId", did);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvoldpatient.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
               
                dgvoldpatient.EnableHeadersVisualStyles = false;

                dgvoldpatient.AllowUserToAddRows = false;
                dgvoldpatient.DataSource = dt;
            }
        }
        public oldPatient()
        {
            InitializeComponent();
        }
        public oldPatient(int did,Panel p)
        {
            InitializeComponent();
            d_id = did;
            parentPanel = p;
            getoldpatient(did);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void oldPatient_Load(object sender, EventArgs e)
        {

        }

        private void dgvoldpatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            pid = Convert.ToInt32(
       dgvoldpatient.Rows[e.RowIndex]
                       .Cells["patient_id"]
                       .Value);
             disease = 
      dgvoldpatient.Rows[e.RowIndex]
                      .Cells["disease"]
                      .Value.ToString();
            c_id = Convert.ToInt32(
       dgvoldpatient.Rows[e.RowIndex]
                       .Cells["checkup_id"]
                       .Value);

        }

        private void btnviewpres_Click(object sender, EventArgs e)
        {
            parentPanel.Controls.Clear(); // remove previous page

            Treatment t = new Treatment(pid,d_id,disease); // child form
            t.TopLevel = false;               // IMPORTANT
            t.FormBorderStyle = FormBorderStyle.None;
            t.Dock = DockStyle.Fill;

            parentPanel.Controls.Add(t);
            t.Show();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            Global.delete("Checkup","checkup_id",c_id);
            getoldpatient(d_id);
        }
    }
}
