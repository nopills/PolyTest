using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using Newtonsoft.Json;
using PolyTest.Services;

namespace PolyTest
{
    public partial class Form1 : Form
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36";

        GMapService gMapService;

        List<GeoJson> geoJson;

        List<PointLatLng> points = new List<PointLatLng>();

        public Form1()
        {
            InitializeComponent();
        }


        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapService = new GMapService(gMap, GMapProviders.GoogleMap);  // Create GMap
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // < ---- Clear Overlay and Points List before start working ----> //
            gMapService.ClearOverlay();
            points.Clear();

            // < ---- How many points we need to use ----> //
            int countPoint = Convert.ToInt32(numericUpDown1.Value);

            // < ---- Check geo for MultiPoligon or Single ----> //
            if (geoJson[comboBox1.SelectedIndex].geoJson.ToString().Contains("\"type\": \"MultiPolygon\""))
            {
                // < ---- Just deserialize Json ----> //
                var geoCoord = JsonConvert.DeserializeObject<MultiPolygonCoordinates>(geoJson[comboBox1.SelectedIndex].geoJson.ToString());
                for (int i = 0; i < geoCoord.coordinates.Length; i++)
                {
                    // < ---- User our countPoint ----> //
                    points.AddRange(geoCoord.coordinates[i][0].Where((value, index) => index % countPoint == 0).Select(x => new PointLatLng(Convert.ToDouble(x[1]), Convert.ToDouble(x[0]))));
                    gMapService.DrawPolygon(points); // Drawing Polygons on the Map
                    points.Clear(); // Clear Points List
                }

            }
            else // Just Signle Polygon
            if (geoJson[comboBox1.SelectedIndex].geoJson.ToString().Contains("\"type\": \"Polygon\""))
            {
                var geoCoord = JsonConvert.DeserializeObject<Coordinates>(geoJson[comboBox1.SelectedIndex].geoJson.ToString());
                points.AddRange(geoCoord.coordinates[0].Where((value, index) => index % countPoint == 0).Select(x => new PointLatLng(Convert.ToDouble(x[1]), Convert.ToDouble(x[0]))));
                gMapService.DrawPolygon(points);
                points.Clear();
            }
        }

        // < ---- Send Request Button  ----> //
        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                HttpService httpService = new HttpService(); // Create Http Service for sending Requests
                string url = String.Format("https://nominatim.openstreetmap.org/search?q={0}&format=json&polygon_geojson=1", Uri.EscapeUriString(textBox1.Text)); ; ;

                Stream response = httpService.GET(url, userAgent); // Send Request and Get Response

                using (var streamReader = new StreamReader(response))
                {
                    JsonTextReader jsonReader = new JsonTextReader(streamReader); // Create JsonReader for Deserialize Http Response Data
                    jsonReader.SupportMultipleContent = true;

                    string jsonData = streamReader.ReadToEnd();

                    geoJson = JsonConvert.DeserializeObject<List<GeoJson>>(jsonData); // Some Deserialize for Coordinates

                    comboBox1.Items.Clear();
                    foreach (var g in geoJson)
                    {
                        // < ---- Check only Regions and City and add in ComboBox ----> //
                        if (g.type.Contains("administrative") || g.type.Contains("city"))
                        {
                            comboBox1.Items.Add(g.display_name + " (" + g.type + ")");
                        }
                    }
                    comboBox1.SelectedIndex = 0;

                    button2.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("TextBox is null or empty", "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        // < ---- Save Image Button ----> //
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PNG (*.png)|*.png";
                    dialog.FileName = "GMap.NET image";
                    Image image = gMap.ToImage();
                    if (image != null)
                    {
                        using (image)
                        {
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                string fileName = dialog.FileName;
                                if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                {
                                    fileName += ".png";
                                }
                                image.Save(fileName);
                                MessageBox.Show("Image saved: " + dialog.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Image save failed: " + exception.Message, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }
        }
    }
}

