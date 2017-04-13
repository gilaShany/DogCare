using System.Collections.Generic;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using DogCare.Droid;
using DogCare;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace DogCare.Droid
{
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        GoogleMap map;
        Polyline polyline;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                ((MapView)Control).GetMapAsync(this);
            }
        }

        private void UpdatePolyLine()
        {
            if (polyline != null)
            {
                polyline.Remove();
                polyline.Dispose();
            }

            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66FF0000);

            foreach (var position in ((CustomMap)this.Element).RouteCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            polyline = map.AddPolyline(polylineOptions);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.RouteCoordinatesProperty.PropertyName)
            {
                UpdatePolyLine();
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            UpdatePolyLine();
        }
    }
}
