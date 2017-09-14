using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XXX1
{
	public class TakePicturePage : ContentPage
	{
        private Image PhotoImage;
        private Button CameraButton;

        public TakePicturePage ()
		{
            
            Grid mainGrid = new Grid()
            {
                RowDefinitions =
                {
                    //new RowDefinition { Height = new GridLength(10,GridUnitType.Star)},
                    //new RowDefinition { Height = GridLength.Auto}
                    new RowDefinition { Height = 500},
                    new RowDefinition { Height = GridLength.Auto}
                }
            };

            //<Image x:Name="PhotoImage" />
            PhotoImage = new Image() { };
            //<Button x:Name="CameraButton" Text="Take Photo" Grid.Row="1" />
            CameraButton = new Button() { Text = "Take Photo" };

            mainGrid.Children.Add(PhotoImage, 0, 0);
            mainGrid.Children.Add(CameraButton, 1, 0);
            Content = mainGrid;

            CameraButton.Clicked += CameraButton_Clicked;
        }
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
    }
}