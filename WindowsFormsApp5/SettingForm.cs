using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class SettingForm : Form
    {
        public WeatherSettings Settings { get; set; }

        public SettingForm()
        {
            InitializeComponent();
            Settings = new WeatherSettings();
        }
        public class WeatherSettings
        {
            public List<string> Cities { get; set; }
            public string RefreshTime { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBoxCities.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one city.");
                return;
            }
            Settings.Cities = new List<string>();
            foreach (var item in checkedListBoxCities.CheckedItems)
            {
                Settings.Cities.Add(item.ToString());
            }
            Settings.RefreshTime = numericUpDownTime.Value.ToString();
            this.Hide();
            WeatherData form1 = new WeatherData(Settings);
            form1.Show();
        }

        private void SettingForm_Load_1(object sender, EventArgs e)
        {
            checkedListBoxCities.Items.Add(MyStrings.City1);
            checkedListBoxCities.Items.Add(MyStrings.City2);
            checkedListBoxCities.Items.Add(MyStrings.City3);
            checkedListBoxCities.Items.Add(MyStrings.City4);
            checkedListBoxCities.Items.Add(MyStrings.City5);
            checkedListBoxCities.CheckOnClick = true;

            numericUpDownTime.Minimum = 5;
            numericUpDownTime.Maximum = 15;
            numericUpDownTime.Value = 5;
            numericUpDownTime.Increment = 5;
        }
    }

}

