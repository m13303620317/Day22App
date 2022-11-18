using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Day22App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var text = Message.Text;
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);
            mediaElement.SetSource(stream, ".mp3");
            mediaElement.Play();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile;
            if (await storageFolder.TryGetItemAsync("test.mp3") == null)
            {
                storageFile = await storageFolder.CreateFileAsync("test.mp3");
            }
            else
            {
                storageFile = await storageFolder.GetFileAsync("test.mp3");
            }
            var storageFileStream = await storageFile.OpenStreamForWriteAsync();
            await stream.AsStreamForRead().CopyToAsync(storageFileStream);
            new ToastContentBuilder()
                .AddText(text, hintMaxLines: 1)
                .AddAudio(new Uri("ms-appx:///" + storageFile))
                .Show();
        }
    }
}
