using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace demo10t01_dispatcher_timer1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // butterfly
        private Butterfly butterfly;

        // flower
        private Flower flower;

        // randomizer
        private Random rand = new Random();

        // game loop timer
        private DispatcherTimer timer;

        // canvas width and height (used to randomize a new flower)
        private double CanvasWidth;
        private double CanvasHeight;

        // which keys are pressed 
        private bool UpPressed;
        private bool LeftPressed;
        private bool RightPressed;

        // audio
        //private MediaElement mediaElement;

        public MainPage()
        {
            this.InitializeComponent();

            // change the default startup mode
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // specify the size width:800, height:600 window size
            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);

            // key listeners
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // get canvas width and height
            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;

            // add butterfly
            butterfly = new Butterfly
            {
                LocationX = CanvasWidth / 2,
                LocationY = CanvasHeight / 2
            };
            MyCanvas.Children.Add(butterfly);

            // add flower
            AddFlower();

            // init audio
            //InitAudio();

            // game loop 
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Start();
        }

        /// <summary>
        /// Load audio file, used when butterfly collides with flower.
        /// </summary>
        /*private async void InitAudio()
        {
            // audios
            mediaElement = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("tada.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.AutoPlay = false;
            mediaElement.SetSource(stream, file.ContentType);
        }*/

        /// <summary>
        /// Add a new flower to Canvas.
        /// </summary>
        private void AddFlower()
        {

            flower = new Flower
            {
                LocationX = rand.Next(1, (int)CanvasWidth - 50),
                LocationY = rand.Next(1, (int)CanvasHeight - 50)
            };
            MyCanvas.Children.Add(flower);
            flower.UpdateLocation();
        }

        /// <summary>
        /// Game loop.
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            // move 
            if (UpPressed) butterfly.Move();

            // rotate
            if (LeftPressed) butterfly.Rotate(-1);
            if (RightPressed) butterfly.Rotate(1);

            // update
            butterfly.UpdateLocation();

            // collision
            CheckCollision();
        }

        /// <summary>
        /// Check collision with butterfly and flower. Add a new flower if collision has happend.
        /// </summary>
        private void CheckCollision()
        {
            // get butterfly and flower rects
            Rect r1 = new Rect(butterfly.LocationX, butterfly.LocationY, butterfly.ActualWidth, butterfly.ActualHeight);
            Rect r2 = new Rect(flower.LocationX, flower.LocationY, flower.ActualWidth, flower.ActualHeight);
            // does thoes intersects
            r1.Intersect(r2);
            // yes if not empty
            if (!r1.IsEmpty)
            {
                // play audio
                //mediaElement.Play();
                // remove flower
                MyCanvas.Children.Remove(flower);
                // add flower
                AddFlower();
            }
        }

        /// <summary>
        /// Check if some keys are pressed.
        /// </summary>
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = true;
                    break;
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if some keys are released.
        /// </summary>
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = false;
                    break;
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
                default:
                    break;
            }
        }

    }
}