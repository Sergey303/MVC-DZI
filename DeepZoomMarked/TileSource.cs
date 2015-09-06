using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DeepZoomMarked
{
    public class TileSource :  MultiScaleTileSource 
    {
        public TileSource()
            : base(1000 * 256, 1000 * 256, 256, 256, 0)
        {
            
        }
        protected override void GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY, IList<object> tileImageLayerSources)
        {
            tileImageLayerSources.Add(new Uri(string.Format("/home/dzi/{0}/{1}/{2}",tileLevel, tilePositionX, tilePositionY),UriKind.Relative));
        }
    }
}
