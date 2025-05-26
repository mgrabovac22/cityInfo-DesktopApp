using BusinessLogicLayer;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Varaždinski_Gradski_Info.Properties;
using static GMap.NET.Entity.OpenStreetMapRouteEntity;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcCommunalServices : UserControl
    {
        private GMapMarker currentMarker;
        private CommunalService communalService;
        private string userRole;
        private DateTime lastApiCallTime = DateTime.MinValue;
        private const int ApiCooldownMilliseconds = 1000;

        private string currentAddress;
        private string currentArea;

        private string selectedAddress;

        public UcCommunalServices()
        {
            try
            {

            InitializeComponent();
            InitializeMap();
            communalService = new CommunalService();

            userRole = new UsersService().ReturnDecryptedRole(Settings.Default.role);
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void InitializeMap()
        {
            gmapControl.MapProvider = GMapProviders.OpenStreetMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gmapControl.Position = new PointLatLng(46.3059, 16.3366);
            gmapControl.Zoom = 13;
            gmapControl.ShowCenter = false;
            gmapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

            gmapControl.MouseLeftButtonDown += GmapControl_MouseLeftButtonDown;
        }

        private async void GmapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentTime = DateTime.Now;

            if ((currentTime - lastApiCallTime).TotalMilliseconds < ApiCooldownMilliseconds)
            {
                return; 
            }

            lastApiCallTime = currentTime;

            var point = e.GetPosition(gmapControl);
            var latLng = gmapControl.FromLocalToLatLng((int)point.X, (int)point.Y);

            if (currentMarker != null)
            {
                currentMarker.Position = latLng; 
            }
            else
            {
                AddMarker(latLng.Lat, latLng.Lng);
            }

            await GetAddressFromCoordinates(latLng.Lat, latLng.Lng); 
        }

        private async Task GetAddressFromCoordinates(double lat, double lng)
        {
            var result = await communalService.GetAddressFromApiAsync(lat, lng);

            if (result != null)
            {
                if (result?.address?.city_district == "Varaždin")
                {
                    currentAddress = result?.address?.road ?? "Adresa nije dostupna";
                    currentArea = result?.address?.suburb ?? "Područje nije dostupno";
                    selectedAddress = currentAddress;

                    CheckLocationInDatabase(selectedAddress);
                }
                else
                {
                    lblInfo.Text = "Vratite se u područje Varaždina!";
                }
            }
            else
            {
                lblInfo.Text = "Greška u pozivanju API-ja!";
            }
        }

        private void CheckLocationInDatabase(string address)
        {
            var locationInfo = communalService.GetLocationInfoByAddress(address);

            if (locationInfo != null)
            {
                lblInfo.Text = $"Lokacija: {locationInfo.Location}, {locationInfo.Location_area}\nOdvoz smeća: {locationInfo.Period}";
                panelInputs.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblInfo.Text = "Lokacija nije pronađena u bazi. Unesite podatke za odvoz! Ako nemate ovlasti kontaktirajte podršku.";
                if (userRole == "Admin" || userRole == "Employee")
                    panelInputs.Visibility = Visibility.Visible;
            }
        }

        private void AddMarker(double lat, double lng)
        {
            var markerIcon = new BitmapImage(new Uri("Assets/poi.png", UriKind.Relative));

            var markerImage = new Image
            {
                Source = markerIcon,
                Width = 32,
                Height = 32
            };

            currentMarker = new GMapMarker(new PointLatLng(lat, lng))
            {
                Shape = markerImage,
                Offset = new Point(-markerImage.Width / 2, -markerImage.Height) 
            };

            gmapControl.Markers.Add(currentMarker);
        }


        private void BtnSpremi_Click(object sender, RoutedEventArgs e)
        {
            var timePeriod = txtVrijeme.Text;
            var day = (cmbDan.SelectedItem as ComboBoxItem)?.Content.ToString();

            var selectedWasteTypes = lstVrstaOtpada.SelectedItems;
            var wasteTypesList = new List<string>();

            foreach (ListBoxItem item in selectedWasteTypes)
            {
                wasteTypesList.Add(item.Content.ToString());
            }

            string wasteTypes = string.Join(", ", wasteTypesList);

            string timePattern = @"^\(\d{2}:\d{2} - \d{2}:\d{2}\)$";
            Regex timeRegex = new Regex(timePattern);

            if (!string.IsNullOrEmpty(timePeriod) && !string.IsNullOrEmpty(day) && wasteTypesList.Count > 0)
            {
                if (timeRegex.IsMatch(timePeriod)) 
                {
                    if (!string.IsNullOrEmpty(selectedAddress))
                    {
                        var message = communalService.AddPeriodLocation(timePeriod, selectedAddress, currentArea, day, wasteTypes);
                        lblInfo.Text = message;
                        panelInputs.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        lblInfo.Text = "Molimo odaberite lokaciju na karti!";
                    }
                }
                else
                {
                    lblError.Text = "Molimo unesite ispravan format vremena (hh:mm - hh:mm)!";
                }
            }
            else
            {
                lblError.Text = "Molimo unesite sve podatke!";
            }
        }

    }



}
