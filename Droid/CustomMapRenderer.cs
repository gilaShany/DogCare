﻿using System.Collections.Generic;
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

            foreach (var position in ((CustomMap)this.Element).RouteCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            polyline = map.AddPolyline(polylineOptions);
        }

        private void UpdatePins()
        {
            map.Clear();
            // map clear deletes polyline
            UpdatePolyLine();

            foreach (var pin in ((CustomMap)this.Element).PinsPoop)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.poop));
                map.AddMarker(marker);
            }

            foreach (var pin in ((CustomMap)this.Element).PinsPee)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pee));
                map.AddMarker(marker);
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

            else if (e.PropertyName.Equals(CustomMap.RouteCoordinatesProperty.PropertyName))
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