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
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
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
            polylineOptions.InvokeWidth(20);

            foreach (var position in ((CustomMap)this.Element).RouteCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            polyline = map.AddPolyline(polylineOptions);
        }

        private void UpdateDistance()
        {
            List<Position> coordinates = ((CustomMap)this.Element).RouteCoordinates;
            int size = ((CustomMap)this.Element).RouteCoordinates.Count;
            if (size > 1)
            {
                ((CustomMap)this.Element).totalDistance += Utils.Utils.GetDistanceInMeters(coordinates[size - 2], coordinates[size - 1]);
            }
        }

        private void UpdatePins()
        {
            map.Clear();
            // map clear deletes polyline
            UpdatePolyLine();

            foreach (var position in ((CustomMap)this.Element).PinsPoop)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(position.Latitude, position.Longitude));
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.poop));
                map.AddMarker(marker);
            }

            foreach (var position in ((CustomMap)this.Element).PinsPee)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(position.Latitude, position.Longitude));
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pee));
                map.AddMarker(marker);
            }
            if(((CustomMap)this.Element).FlagStartCoordinate != null)
            {
                AddStartFlag();
            }
            if (((CustomMap)this.Element).FlagFinishCoordinate != null)
            {
                AddFinishFlag();
            }
            isDrawn = true;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;
      
            if ((e.PropertyName.Equals("VisibleRegion") && !isDrawn) || (e.PropertyName.Equals(CustomMap.PinsPeeProperty.PropertyName)) || 
                (e.PropertyName.Equals(CustomMap.PinsPoopProperty.PropertyName)))
            {
                UpdatePins();
            }

            if (e.PropertyName.Equals(CustomMap.RouteCoordinatesProperty.PropertyName))
            {
                UpdatePolyLine();
                UpdateDistance();
            }

           if (e.PropertyName.Equals(CustomMap.FlagStartCoordinateProperty.PropertyName))
            {
                UpdatePins();
                AddStartFlag();
            }
           else if (e.PropertyName.Equals(CustomMap.FlagFinishCoordinateProperty.PropertyName))
            {
                UpdatePins();
                AddFinishFlag();
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            UpdatePolyLine();
        }

        public void AddStartFlag()
        {
            var markerS = new MarkerOptions();
            Position p = ((CustomMap)this.Element).FlagStartCoordinate;
            markerS.SetPosition(new LatLng(p.Latitude, p.Longitude));
            markerS.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.flagStart));
            map.AddMarker(markerS);
        }

        public void AddFinishFlag()
        {
            var markerF = new MarkerOptions();
            Position p = ((CustomMap)this.Element).FlagFinishCoordinate;
            markerF.SetPosition(new LatLng(p.Latitude, p.Longitude));
            markerF.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.flagFinish));
            map.AddMarker(markerF);
        }
    }
}
