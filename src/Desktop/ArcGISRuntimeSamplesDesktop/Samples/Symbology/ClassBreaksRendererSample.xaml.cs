﻿using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGISRuntime.Samples.Desktop
{
	/// <summary>
	/// Sample shows how to create a ClassBreaksRenderer for a graphics layer. USA cities data points are pulled from an online source and rendered using the GraphicsLayer ClassBreaksRenderer.
	/// </summary>
	/// <title>Class Breaks Renderer</title>
	/// <category>Symbology</category>
	public partial class ClassBreaksRendererSample : UserControl
	{
		private Random _random = new Random();
		private GraphicsOverlay _cities;

		/// <summary>Construct Class Breaks Renderer sample control</summary>
		public ClassBreaksRendererSample()
		{
			InitializeComponent();
			MyMapView.SetView(new Viewpoint(MapExtent));
			_cities = MyMapView.GraphicsOverlays["cities"];

			MyMapView.CameraChanged += MyMapView_ExtentChanged;
		}

		private Envelope MapExtent = new Envelope(-15053000, 2749000, -6540000, 6590000, SpatialReferences.WebMercator);
		// Load earthquake data
		private async void MyMapView_ExtentChanged(object sender, EventArgs e)
		{
			try
			{
				MyMapView.CameraChanged -= MyMapView_ExtentChanged;
				await LoadUSACitiesAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading data: " + ex.Message, "Class Breaks Renderer Sample");
			}
		}

		// Change the graphics layer renderer to a new ClassBreaksRenderer
		private void ChangeRendererButton_Click(object sender, RoutedEventArgs e)
		{
			SimpleMarkerStyle style = (SimpleMarkerStyle)_random.Next(0, 6);

			_cities.Renderer = new ClassBreaksRenderer()
			{
				Field = "pop2000",
				Infos = new ClassBreakInfoCollection() 
				{ 
					new ClassBreakInfo() { Minimum = 0, Maximum = 50000, Symbol = GetRandomSymbol(style) },
					new ClassBreakInfo() { Minimum = 50000, Maximum = 100000, Symbol = GetRandomSymbol(style) },
					new ClassBreakInfo() { Minimum = 100000, Maximum = 250000, Symbol = GetRandomSymbol(style) },
					new ClassBreakInfo() { Minimum = 250000, Maximum = 500000, Symbol = GetRandomSymbol(style) },
					new ClassBreakInfo() { Minimum = 500000, Maximum = 1000000, Symbol = GetRandomSymbol(style) },
					new ClassBreakInfo() { Minimum = 1000000, Maximum = 5000000, Symbol = GetRandomSymbol(style) },
				}
			};
		}

		// Load earthquakes from map service
		private async Task LoadUSACitiesAsync()
		{
			var queryTask = new QueryTask(
				new Uri("http://sampleserver6.arcgisonline.com/ArcGIS/rest/services/USA/MapServer/0"));
			var query = new Query(MapExtent)
			{
				ReturnGeometry = true,
				OutSpatialReference = MyMapView.SpatialReference,
				Where = "pop2000 > 50000",
				OutFields = new OutFields(new List<string> { "pop2000" })
			};
			var result = await queryTask.ExecuteAsync(query);

			_cities.Graphics.Clear();
			_cities.Graphics.AddRange(result.FeatureSet.Features.OfType<Graphic>());
		}

		// Utility: Generate a random simple marker symbol
		private SimpleMarkerSymbol GetRandomSymbol(SimpleMarkerStyle style)
		{
			return new SimpleMarkerSymbol()
			{
				Size = 12,
				Color = GetRandomColor(),
				Style = style
			};
		}

		// Utility function: Generate a random System.Windows.Media.Color
		private Color GetRandomColor()
		{
			var colorBytes = new byte[3];
			_random.NextBytes(colorBytes);
			return Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);
		}
	}
}
