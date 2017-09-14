using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XXX1
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
#if __ANDROID__
            this.btn_Read_nfc.IsEnabled = true;
            this.btn_Read_qrcode.IsEnabled = false;
#else
            this.btn_Read_nfc.IsEnabled = false;
            this.btn_Read_qrcode.IsEnabled = true;
#endif
        }
        public void btn_Read_clicked(Object sender, EventArgs e)
        {
#if __ANDROID__

				Navigation.PushAsync(new NFCPage());
#else
                Navigation.PushAsync(new QRCodePage());
#endif
        }
        public void btn_take_picture_clicked(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new TakePicturePage());
        }
    }
}
