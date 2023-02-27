using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp5.SettingForm;

namespace WindowsFormsApp5
{
    public partial class WeatherData : Form
    {
        //WeatherInfo weatherInfo;
        private int panelIndex = 0;
        private int totalCities = 0;
        private HttpClient httpClient = new HttpClient();
        private string apiKey = MyStrings.APIKEY;
        // SettingForm settingForm;
        private WeatherSettings settings;
        public WeatherData(WeatherSettings settings)
        {

            InitializeComponent();
            this.settings = settings;
            totalCities = settings.Cities.Count;
            // weatherInfo = new WeatherInfo();
            UpdateLabels();
            timer1.Interval = Int32.Parse(settings.RefreshTime) * 1000;
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            panelIndex++;

            if (panelIndex >= totalCities)
            {
                panelIndex = 0;
            }
            Console.WriteLine(MyStrings.Tick + panelIndex);
            UpdateLabels();

        }
        private async Task<string> GetWeatherData(string city)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"An error occurred while getting the weather data for {city}: {ex.Message}");
                return null;
            }
        }
        private async void UpdateLabels()
        {

            try
            {
                var city = settings.Cities.ElementAt(panelIndex);
                Console.WriteLine(panelIndex.ToString());
                var weatherData = await GetWeatherData(city);
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(weatherData);
                double temperature = json.main.temp;
                double humidity = json.main.humidity;
                double pressure = json.main.pressure;
                label1.Text = city;
                lblHumidity.Text = $"Humidity: {humidity}%";
                lblAtmosphere.Text = $"Atmosphere: {pressure} Pa";
                lblTemperature.Text = $"Temperature: {temperature}°C";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = WindowsFormsApp5.Properties.Settings.Default.Left;
            this.Width = WindowsFormsApp5.Properties.Settings.Default.Width;
            this.Height = WindowsFormsApp5.Properties.Settings.Default.Height;
            this.Top = WindowsFormsApp5.Properties.Settings.Default.Top;
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            var newSettingsForm = new SettingForm();
            newSettingsForm.ShowDialog();

            if (newSettingsForm.Settings != null)
            {
                settings = newSettingsForm.Settings;
                totalCities = settings.Cities.Count;
                UpdateLabels();
                timer1.Interval = Int32.Parse(settings.RefreshTime) * 1000;
            }
            
            this.Close();
        }      
        private void WeatherData_FormClosed(object sender, FormClosedEventArgs e)
        {
            WindowsFormsApp5.Properties.Settings.Default.Left = this.Left;
            WindowsFormsApp5.Properties.Settings.Default.Width = this.Width;
            WindowsFormsApp5.Properties.Settings.Default.Height = this.Height;
            WindowsFormsApp5.Properties.Settings.Default.Top = this.Top;
            WindowsFormsApp5.Properties.Settings.Default.Save();
        }
    }
}



      

