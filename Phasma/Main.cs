using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Phasma
{
    public partial class Main : Form
    { 
        private const string CIPHER_KEY = "CHANGE ME TO YOUR OWN RANDOM STRING";
        private static SaveData saveData;
        private static string saveLoc;
        
        public Main()
        {
            InitializeComponent();
        }
        
        private string XOR(string text, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] data = Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= keyBytes[i % keyBytes.Length];
            }
            
            return Encoding.UTF8.GetString(data);
        }
        
        private void Main_Load(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Save file",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "appdata\\locallow\\Kinetic Games\\Phasmophobia")
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                saveLoc = dialog.FileName;
                string raw = File.ReadAllText(dialog.FileName);
                string text = XOR(raw, CIPHER_KEY);
                    
                try
                {
                    saveData = JsonConvert.DeserializeObject<SaveData>(text);

                    foreach (IntData data in saveData.IntData)
                    {
                        SaveDataGrid.Rows.Add(data.Key, data.Value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
            else
            {
                MessageBox.Show("Load saveData canceled.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(0);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveLoc == null)
            {
                MessageBox.Show("Something went horribly wrong.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            foreach (DataGridViewRow row in SaveDataGrid.Rows)
            {
                string key = row.Cells[0].Value.ToString();

                if (!int.TryParse(row.Cells[1].Value.ToString(), out int value))
                {
                    continue;
                }

                foreach (IntData data in saveData.IntData)
                {
                    if (data.Key == key)
                    {
                        data.Value = value;
                        break;
                    }
                }
            }
            
            File.WriteAllText(saveLoc, XOR(JsonConvert.SerializeObject(saveData), CIPHER_KEY));
            MessageBox.Show("Save data written.", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}