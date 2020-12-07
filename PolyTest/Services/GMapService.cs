using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolyTest.Services
{
    public class GMapService
    {
        public GMapControl _gMap;
        public GMapService(GMapControl gMap, GMapProvider gMapProvider)
        {
            _gMap = gMap;
            _gMap.MapProvider = gMapProvider;
            GetDefaultSettings();
        }

        public void DrawPolygon(List<PointLatLng> points)
        {
            GMapPolygon polygon = new GMapPolygon(points, "mypolygon");
            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polyOverlay.Polygons.Add(polygon);          
            _gMap.Overlays.Add(polyOverlay);           
            _gMap.Position = new PointLatLng(points[0].Lat, points[0].Lng);

        }

        public void ClearOverlay()
        {

            _gMap.Overlays.Clear();
        }
        private void GetDefaultSettings()
        {
            // < ----- SETTINGS ----->

            // Rotate to horizontal state //
            _gMap.Bearing = 0;

            // Drag and Drop//
            _gMap.CanDragMap = true;
            _gMap.DragButton = MouseButtons.Left;

            // ZOOM //
            _gMap.MaxZoom = 18;
            _gMap.MinZoom = 2;

            // Mouse Position without re-center //
            _gMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

            // Off Negative Mode
            _gMap.NegativeMode = false;
            
            // Enable Polygon for drawing on the map
            _gMap.PolygonsEnabled = true;

            // Disable Grid
            _gMap.ShowTileGridLines = false;
            
            _gMap.Zoom = 10;
            _gMap.ShowCenter = false;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
        }
    }
}
