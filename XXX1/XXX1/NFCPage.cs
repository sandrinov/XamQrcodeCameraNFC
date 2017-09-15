using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;


namespace XXX1
{
    public class NFCPage : ContentPage
    {
#if __ANDROID__
        private readonly Poz1.NFCForms.Abstract.INfcForms device;
#endif
        private StackLayout welcomePanel;

        private Switch IsWriteable;
        private Switch IsConnected;
        private Switch IsNDEFSupported;

        private ListView TechList;
        private ListView NDEFMessage;

        public NFCPage()
        {
#if __ANDROID__
            device = DependencyService.Get<Poz1.NFCForms.Abstract.INfcForms>();
            device.NewTag += HandleNewTag;
            device.TagConnected += device_TagConnected;
            device.TagDisconnected += device_TagDisconnected;
#endif

            Grid mainGrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star},
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star}
                }
            };

            Grid boolInfo = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            IsWriteable = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsWriteableLabel = new Label() { Text = "Write Unlocked", HorizontalOptions = LayoutOptions.Center };

            IsConnected = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsConnectedLabel = new Label() { Text = "Tag Connected", HorizontalOptions = LayoutOptions.Center };

            IsNDEFSupported = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsNDEFSupportedLabel = new Label() { Text = "NDEF Support", HorizontalOptions = LayoutOptions.Center };

            boolInfo.Children.Add(IsWriteable, 0, 0);
            boolInfo.Children.Add(IsWriteableLabel, 0, 1);

            boolInfo.Children.Add(IsConnected, 1, 0);
            boolInfo.Children.Add(IsConnectedLabel, 1, 1);

            boolInfo.Children.Add(IsNDEFSupported, 2, 0);
            boolInfo.Children.Add(IsNDEFSupportedLabel, 2, 1);

            TechList = new ListView();

            NDEFMessage = new ListView();

            Button writeButton = new Button() { Text = "Write Tag" };
#if __ANDROID__
            writeButton.Clicked += HandleClicked;
#endif

            Label welcomeLabel = new Label
            {
                Text = "Hello, XForms!" + System.Environment.NewLine + "Scan a tag!",
                XAlign = TextAlignment.Center,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            welcomePanel = new StackLayout()
            {
                Children = { welcomeLabel },
                BackgroundColor = Color.White
            };

            mainGrid.Children.Add(boolInfo);
            mainGrid.Children.Add(TechList, 0, 1);
            mainGrid.Children.Add(NDEFMessage, 0, 2);
            mainGrid.Children.Add(writeButton, 0, 3);
            mainGrid.Children.Add(welcomePanel, 0, 4);
            Content = mainGrid;
        }

#if __ANDROID__
        void device_TagDisconnected(object sender, Poz1.NFCForms.Abstract.NfcFormsTag e)
        {
            IsConnected.IsToggled = false;
        }

        void device_TagConnected(object sender, Poz1.NFCForms.Abstract.NfcFormsTag e)
        {
            IsConnected.IsToggled = true;
        }

        void HandleClicked(object sender, EventArgs e)
        {

            var spRecord = new NdefLibrary.Ndef.NdefSpRecord
            {
                Uri = "www.poz1.com",
                NfcAction = NdefLibrary.Ndef.NdefSpActRecord.NfcActionType.DoAction,
            };
            spRecord.AddTitle(new NdefLibrary.Ndef.NdefTextRecord
            {
                Text = "NFCForms - XamarinForms - Poz1.com",
                LanguageCode = "en"
            });
            // Add record to NDEF message
            var msg = new NdefLibrary.Ndef.NdefMessage { spRecord };
            try
            {
                device.WriteTag(msg);
            }
            catch (Exception excp)
            {
                DisplayAlert("Error", excp.Message, "OK");
            }

        }

        private ObservableCollection<string> readNDEFMEssage(NdefLibrary.Ndef.NdefMessage message)
        {

            ObservableCollection<string> collection = new ObservableCollection<string>();
            foreach (NdefLibrary.Ndef.NdefRecord record in message)
            {
                // Go through each record, check if it's a Smart Poster
                if (record.CheckSpecializedType(false) == typeof(NdefLibrary.Ndef.NdefSpRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefLibrary.Ndef.NdefSpRecord(record);
                    collection.Add("URI: " + spRecord.Uri);
                    collection.Add("Titles: " + spRecord.TitleCount());
                    collection.Add("1. Title: " + spRecord.Titles[0].Text);
                    collection.Add("Action set: " + spRecord.ActionInUse());
                }

                if (record.CheckSpecializedType(false) == typeof(NdefLibrary.Ndef.NdefUriRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefLibrary.Ndef.NdefUriRecord(record);
                    collection.Add("Text: " + spRecord.Uri);
                }

                if (record.CheckSpecializedType(false) == typeof(NdefLibrary.Ndef.NdefTextRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefLibrary.Ndef.NdefTextRecord(record);
                    collection.Add("Text: " + spRecord.Text);
                }
            }
            return collection;
        }

        

        void HandleNewTag(object sender, Poz1.NFCForms.Abstract.NfcFormsTag e)
        {
            welcomePanel.IsVisible = false;

            IsWriteable.IsToggled = e.IsWriteable;
            IsNDEFSupported.IsToggled = e.IsNdefSupported;

            if (TechList != null)
                TechList.ItemsSource = e.TechList;

            if (e.IsNdefSupported)
                NDEFMessage.ItemsSource = readNDEFMEssage(e.NdefMessage);
        }
#endif

    }
}