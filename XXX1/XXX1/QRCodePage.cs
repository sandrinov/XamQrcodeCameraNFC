using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;


namespace XXX1
{
	public class QRCodePage : ContentPage
	{
		public QRCodePage ()
		{
#if __IOS__
            var scanPage = new ZXing.Net.Mobile.Forms.ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
            // Navigate to our scanner page
            Navigation.PushAsync(scanPage);
#endif
        }
	}
}