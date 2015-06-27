﻿using System.Windows.Controls;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGISRuntime.Samples.Desktop
{
    /// <summary>
	/// Shows how to set the initial extent of the map (Map.InitialViewpoint).
    /// </summary>
	/// <title>Set Initial Map Extent</title>
	/// <category>Mapping</category>
	public partial class SetInitialMapExtent : UserControl
    {
        public SetInitialMapExtent()
        {
            InitializeComponent();

			// Note: uncomment the following to set the initial extent of the map in code.
			//MyMapView.Map.InitialViewpoint = new Envelope(-117.182686,32.695853,-117.133872,32.718530, SpatialReferences.Wgs84);


			//MySceneView.SetView(new Viewpoint(new Envelope(-117.182686, 32.695853, -117.133872, 32.718530, SpatialReferences.Wgs84)));

			MySceneView.SetView(MyMapView.Map.InitialViewpoint);
        }
    }
}
