using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
//using Windows.Media
using System.Reflection;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
//using Windows.ApplicationModel.DataTransfer;

using Windows.ApplicationModel.Activation;

using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Shapes;
using SnapEdit.Common;
using Windows.ApplicationModel;
//using System.Windows.Media.Imaging;
//using System.Windows.Media;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SnapEdit
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class main1 : LayoutAwarePage
    {
        private StorageFile _photo; // Photo file to share
        BitmapImage bitmap = new BitmapImage();
        //WriteableBitmap wbitmap;
        ImageBrush ib = new ImageBrush();
       // Image im;


        InkManager MyInkManager = new InkManager();
        string tool;
        double X1, X2, Y1, Y2, StrokeThickness = 10;
        Line NewLine;
        Ellipse NewEllipse;
        Point StartPoint, PreviousContactPoint, CurrentContactPoint;
        Polyline Pencil;
        Rectangle NewRectangle;
        Color BorderColor, FillColor;
        uint PenID, TouchID;
       // Image io=

        public main1()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            canvas.PointerMoved += canvas_pointer_moved;
            canvas.PointerPressed += canvas_pointer_pressed;
            canvas.PointerExited += canvas_pointer_exited;
            canvas.PointerReleased += canvas_pointer_released;
            tool = "pencil";
            FillColor = Colors.Blue;
            BorderColor = Colors.Black;
            var colors = typeof(Colors).GetTypeInfo().DeclaredProperties;
            
            foreach (var item in colors)
            {
                //ImageBrush i= i
                Rectangle r = new Rectangle();
                //r.Fill=I
             
                //fill.Items.Add(item);
                
            }


            for (int i = 1; i < 20; i++)
            {
                ComboBoxItem Items = new ComboBoxItem();
                Items.Content = i;
                thick.Items.Add(Items);
            }
            thick.SelectedIndex = 0;

            

        }

       /* private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareImageHandler);
        }

        private async void ShareImageHandler(DataTransferManager sender,
    DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share Image Example";
            request.Data.Properties.Description = "Demonstrates how to share an image.";

            // Because we are making async calls in the DataRequested event handler,
            //  we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure we always call Complete on the deferral.
            try
            {
                StorageFile thumbnailFile =
                    await Package.Current.InstalledLocation.GetFileAsync("Assets\\SmallLogo.png");
                request.Data.Properties.Thumbnail =
                    RandomAccessStreamReference.CreateFromFile(thumbnailFile);
                StorageFile imageFile =
                    await Package.Current.InstalledLocation.GetFileAsync("Assets\\Logo.png");
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
            }
            finally
            {
                deferral.Complete();
            }
        }   */

       /* protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            // Code to handle activation goes here.

            ShareOperation shareOperation = args.ShareOperation;
            if (shareOperation.Data.Contains(StandardDataFormats.Text))
            {
                string text = await shareOperation.Data.GetTextAsync();

                // To output the text from this example, you need a TextBlock control
                // with a name of "sharedContent".
               // sharedContent.Text = "Text: " + text;
            }
        }  


        async void ReportCompleted(ShareOperation shareOperation, string quickLinkId, string quickLinkTitle)
        {
            QuickLink quickLinkInfo = new QuickLink
            {
                Id = quickLinkId,
                Title = quickLinkTitle,

                // For quicklinks, the supported FileTypes and DataFormats are set 
                // independently from the manifest
                SupportedFileTypes = { "*" },
                SupportedDataFormats = { StandardDataFormats.Text, StandardDataFormats.Uri, 
                StandardDataFormats.Bitmap, StandardDataFormats.StorageItems }
            };

            StorageFile iconFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(
                    "assets\\Logo.png", CreationCollisionOption.OpenIfExists);
            quickLinkInfo.Thumbnail = RandomAccessStreamReference.CreateFromFile(iconFile);
            shareOperation.ReportCompleted(quickLinkInfo);
        }  */


        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTextHandler);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share Text Example";
            request.Data.Properties.Description = "A demonstration that shows how to share text.";
            request.Data.SetText("Hello World!");
        }


      /*  protected override bool GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            string dataPackageText = "Having fun sketching using SnapEdit !!";
            if (!String.IsNullOrEmpty(dataPackageText))
            {
                DataPackage requestData = request.Data;
                requestData.Properties.Title = "SnapEdit";
                requestData.Properties.Description = "SnapEdit is an application for Sketching in Windows 8"; // The description is optional. 
                requestData.SetText(dataPackageText);
                succeeded = true;
            }
            else
            {
                request.FailWithDisplayText("Oops!! Unable to share :(");
            }
            return succeeded;
        }  */

        public struct PixelColor
         {
             public byte Blue;
             public byte Green;
             public byte Red;
             public byte Alpha;
         }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

       /* private async void OnCapturePhoto(object sender, TappedRoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                _photo = file;
                DataTransferManager.ShowShareUI();
            }
        } */

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker SavePicker = new Windows.Storage.Pickers.FileSavePicker();
            SavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            SavePicker.DefaultFileExtension = ".txt";
            SavePicker.FileTypeChoices.Add("TXT", new string[] { ".txt" });
            SavePicker.SuggestedFileName = "untitled";
            StorageFile File = await SavePicker.PickSaveFileAsync();
            await FileIO.WriteTextAsync(File, txtRecognizedText.Text);
        }


        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (EnsureUnsnapped())
            {

                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    BitmapImage b = new BitmapImage();
                    b.SetSource(await file.OpenAsync(FileAccessMode.Read));
                    ImageBrush ib = new ImageBrush();
                    ib.ImageSource = b;
                    
                    canvas.Background = ib;
                }
                else
                {
                  //  ss.Text = "Operation cancelled.";
                }

                
            }
        }

        internal bool EnsureUnsnapped()
        {
            // FilePicker APIs will not work if the application is in a snapped state.
            // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            /*if (!unsnapped)
            {
                NotifyUser("Cannot unsnap the sample.", NotifyType.StatusMessage);
            }*/

            return unsnapped;
        }








        private void line_Click(object sender, RoutedEventArgs e)
        {
            tool = "line";
        }

        private void pencil_Click(object sender, RoutedEventArgs e)
        {
            tool = "pencil";
        }

        private void ellipse_Click(object sender, RoutedEventArgs e)
        {
            tool = "ellipse";
        }

        private void rect_Click(object sender, RoutedEventArgs e)
        {
            tool = "rect";
        }

        private void eraser_Click(object sender, RoutedEventArgs e)
        {
            tool = "eraser";
        }

        void canvas_pointer_moved(object sender, PointerRoutedEventArgs e)
        {
            if (tool != "eraser")
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Cross, 1);
            else
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.UniversalNo, 1);

            switch (tool)
            {

                case "line":
                    {
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed == true)
                        {
                            NewLine.X2 = e.GetCurrentPoint(canvas).Position.X;
                            NewLine.Y2 = e.GetCurrentPoint(canvas).Position.Y;
                        }
                    }
                    break;

                case "pencil":
                    {
                        /* Old Code
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed == true)
                        {
                            if (StartPoint != e.GetCurrentPoint(canvas).Position)
                            {
                                Pencil.Points.Add(e.GetCurrentPoint(canvas).Position);
                            }
                        }
                        */

                        if (e.Pointer.PointerId == PenID || e.Pointer.PointerId == TouchID)
                        {
                            // Distance() is an application-defined function that tests
                            // whether the pointer has moved far enough to justify 
                            // drawing a new line.
                            CurrentContactPoint = e.GetCurrentPoint(canvas).Position;
                            X1 = PreviousContactPoint.X;
                            Y1 = PreviousContactPoint.Y;
                            X2 = CurrentContactPoint.X;
                            Y2 = CurrentContactPoint.Y;

                            if (Distance(X1, Y1, X2, Y2) > 2.0)
                            {
                                Line line = new Line()
                                {
                                    X1 = X1,
                                    Y1 = Y1,
                                    X2 = X2,
                                    Y2 = Y2,
                                    StrokeThickness = StrokeThickness,
                                    Stroke = new SolidColorBrush(BorderColor)
                                };

                                PreviousContactPoint = CurrentContactPoint;
                                canvas.Children.Add(line);
                                MyInkManager.ProcessPointerUpdate(e.GetCurrentPoint(canvas));
                            }
                        }
                    }
                    break;


                case "rect":
                    {
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed == true)
                        {
                            X2 = e.GetCurrentPoint(canvas).Position.X;
                            Y2 = e.GetCurrentPoint(canvas).Position.Y;
                            if ((X2 - X1) > 0 && (Y2 - Y1) > 0)
                                NewRectangle.Margin = new Thickness(X1, Y1, X2, Y2);
                            else if ((X2 - X1) < 0 && (Y2 - Y1) > 0)
                                NewRectangle.Margin = new Thickness(X2, Y1, X1, Y2);
                            else if ((Y2 - Y1) < 0 && (X2 - X1) > 0)
                                NewRectangle.Margin = new Thickness(X1, Y2, X2, Y1);
                            else if ((X2 - X1) < 0 && (Y2 - Y1) < 0)
                                NewRectangle.Margin = new Thickness(X2, Y2, X1, Y1);
                            NewRectangle.Width = Math.Abs(X2 - X1);
                            NewRectangle.Height = Math.Abs(Y2 - Y1);
                        }
                    }
                    break;

                case "ellipse":
                    {
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed == true)
                        {
                            X2 = e.GetCurrentPoint(canvas).Position.X;
                            Y2 = e.GetCurrentPoint(canvas).Position.Y;
                            if ((X2 - X1) > 0 && (Y2 - Y1) > 0)
                                NewEllipse.Margin = new Thickness(X1, Y1, X2, Y2);
                            else if ((X2 - X1) < 0 && (Y2 - Y1) > 0)
                                NewEllipse.Margin = new Thickness(X2, Y1, X1, Y2);
                            else if ((Y2 - Y1) < 0 && (X2 - X1) > 0)
                                NewEllipse.Margin = new Thickness(X1, Y2, X2, Y1);
                            else if ((X2 - X1) < 0 && (Y2 - Y1) < 0)
                                NewEllipse.Margin = new Thickness(X2, Y2, X1, Y1);
                            NewEllipse.Width = Math.Abs(X2 - X1);
                            NewEllipse.Height = Math.Abs(Y2 - Y1);
                        }
                    }
                    break;

                case "eraser":
                    {
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed == true)
                        {
                            if (StartPoint != e.GetCurrentPoint(canvas).Position)
                            {
                                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.UniversalNo, 1);
                                Pencil.Points.Add(e.GetCurrentPoint(canvas).Position);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }


        void canvas_pointer_pressed(object sender, PointerRoutedEventArgs e)
        {

            if (tool != "eraser")
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Cross, 1);
            else
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.UniversalNo, 1);


            switch (tool)
            {

                case "line":
                    {
                        NewLine = new Line();
                        NewLine.X1 = e.GetCurrentPoint(canvas).Position.X;
                        NewLine.Y1 = e.GetCurrentPoint(canvas).Position.Y;
                        NewLine.X2 = NewLine.X1 + 1;
                        NewLine.Y2 = NewLine.Y1 + 1;
                        NewLine.StrokeThickness = StrokeThickness;
                        NewLine.Stroke = new SolidColorBrush(BorderColor);
                        canvas.Children.Add(NewLine);
                    }
                    break;

                case "pencil":
                    {

                        /* old code
                         StartPoint = e.GetCurrentPoint(canvas).Position;
                         Pencil = new Polyline();
                         Pencil.Stroke = new SolidColorBrush(BorderColor);
                         Pencil.StrokeThickness = StrokeThickness;
                         canvas.Children.Add(Pencil);
                         * */


                        var MyDrawingAttributes = new InkDrawingAttributes();
                        MyDrawingAttributes.Size = new Size(StrokeThickness, StrokeThickness);
                        MyDrawingAttributes.Color = BorderColor;
                        MyDrawingAttributes.FitToCurve = true;
                        MyInkManager.SetDefaultDrawingAttributes(MyDrawingAttributes);

                        PreviousContactPoint = e.GetCurrentPoint(canvas).Position;
                        //PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;  to identify the pointer device
                        if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed)
                        {
                            // Pass the pointer information to the InkManager.
                            MyInkManager.ProcessPointerDown(e.GetCurrentPoint(canvas));
                            PenID = e.GetCurrentPoint(canvas).PointerId;
                            e.Handled = true;
                        }
                    }
                    break;

                case "rect":
                    {
                        NewRectangle = new Rectangle();
                        X1 = e.GetCurrentPoint(canvas).Position.X;
                        Y1 = e.GetCurrentPoint(canvas).Position.Y;
                        X2 = X1;
                        Y2 = Y1;
                        NewRectangle.Width = Math.Abs(X2 - X1);
                        NewRectangle.Height = Math.Abs(Y2 - Y1);
                        NewRectangle.StrokeThickness = StrokeThickness;
                        NewRectangle.Stroke = new SolidColorBrush(BorderColor);
                        NewRectangle.Fill = new SolidColorBrush(FillColor);
                        canvas.Children.Add(NewRectangle);
                    }
                    break;

                case "ellipse":
                    {
                        NewEllipse = new Ellipse();
                        X1 = e.GetCurrentPoint(canvas).Position.X;
                        Y1 = e.GetCurrentPoint(canvas).Position.Y;
                        X2 = X1;
                        Y2 = Y1;
                        NewEllipse.Width = Math.Abs(X2 - X1);
                        NewEllipse.Height = Math.Abs(Y2 - Y1);
                        NewEllipse.StrokeThickness = StrokeThickness;
                        NewEllipse.Stroke = new SolidColorBrush(BorderColor);
                        NewEllipse.Fill = new SolidColorBrush(FillColor);
                        canvas.Children.Add(NewEllipse);
                    }
                    break;

                case "eraser":
                    {
                        Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.UniversalNo, 1);
                        StartPoint = e.GetCurrentPoint(canvas).Position;
                        Pencil = new Polyline();
                        Pencil.Stroke = new SolidColorBrush(Colors.White);
                        Pencil.StrokeThickness = 10;
                        canvas.Children.Add(Pencil);
                    }
                    break;

                default:
                    break;
            }
        }

        void canvas_pointer_exited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        void canvas_pointer_released(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId == PenID || e.Pointer.PointerId == TouchID)
                MyInkManager.ProcessPointerUp(e.GetCurrentPoint(canvas));

            TouchID = 0;
            PenID = 0;
            e.Handled = true;
            Pencil = null;
            NewLine = null;
            NewRectangle = null;
            NewEllipse = null;
        }



        public void thick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StrokeThickness = Convert.ToInt32(thick.SelectedIndex + 1);
            
        }


        #region Border Color Click Events

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Red;
        }

        private void btnGreen_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Green;
        }

        private void btnBlue_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Blue;
        }

        private void btnBlack_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Black;
        }

        private void btnYellow_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Yellow;
        }

        private void btnMagenta_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Magenta;
        }

        private void btnCyan_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Cyan;
        }

        private void btnWhite_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.White;
        }

        private void btnPink_Click(object sender, RoutedEventArgs e)
        {
            BorderColor = Colors.Pink;
        }

        #endregion

        #region Fill Colors Click Events

        private void btnFillRed_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Red;
        }

        private void btnFillGreen_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Green;
        }

        private void btnFillBlue_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Blue;
        }

        private void btnFillBlack_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Black;
        }

        private void btnFillYellow_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Yellow;
        }

        private void btnFillMagenta_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Magenta;
        }

        private void btnFillCyan_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Cyan;
        }

        private void btnFillWhite_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.White;
        }

        private void btnFillPink_Click(object sender, RoutedEventArgs e)
        {
            FillColor = Colors.Pink;
        }

        #endregion




        private void border1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void fill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        

        

        private async void saveasimg_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker SavePicker = new Windows.Storage.Pickers.FileSavePicker();
            SavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            SavePicker.DefaultFileExtension = ".txt";
            SavePicker.FileTypeChoices.Add("TXT", new string[] { ".txt" });
            SavePicker.SuggestedFileName = "untitled";
            StorageFile File = await SavePicker.PickSaveFileAsync();
            try
            {
                await FileIO.WriteTextAsync(File, txtRecognizedText.Text);
            }
            catch
            {
               
            }
        }


        private void cls_Click(object sender, RoutedEventArgs e)
        {
            //MyInkManager.Mode = InkManipulationMode.Erasing;
            //for (int i = 0; i < MyInkManager.GetStrokes().Count; i++)
            //    MyInkManager.GetStrokes().ElementAt(i).Selected = true;
            //MyInkManager.DeleteSelected();
            //text.Text = string.Empty;
            FinalRecognizedText = string.Empty;
            canvas.Children.Clear();
            txtRecognizedText.Text = "";
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        IReadOnlyList<String> RecognizedText;
        int j = 0;
        string FinalRecognizedText = "";
        IReadOnlyList<InkRecognitionResult> x;
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                
                txtRecognizedText.Visibility = Windows.UI.Xaml.Visibility.Visible;
               // btnSaveRecognizedText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                canvas.SetValue(Grid.RowProperty, 3);
                canvas.SetValue(Grid.RowSpanProperty, 1);
                MyInkManager.Mode = InkManipulationMode.Inking;
                x = await MyInkManager.RecognizeAsync(InkRecognitionTarget.Recent);
                MyInkManager.UpdateRecognitionResults(x);
                txtRecognizedText.Text = "";
                foreach (InkRecognitionResult i in x)
                {
                    if (j == 0)
                    {
                        RecognizedText = i.GetTextCandidates();
                        FinalRecognizedText += " " + RecognizedText[0];
                        txtRecognizedText.Text += FinalRecognizedText;
                        
                    }
                    else
                    {
                       // txtRecognizedText.Text += FinalRecognizedText;
                        j = 0;
                    }
                }
                
                
                canvas.Children.Clear();
            }
            catch (Exception)
            {
                if (canvas.Children.Count == 0)
                {
                    var MsgDlg = new MessageDialog("Your screen has no handwriting. Please write something with pencil tool then click recognize.",
        "Error while recognizing");
                    MsgDlg.ShowAsync();
                }
                else
                {
                    var MsgDlg = new MessageDialog("Please clear the screen then write something with pencil tool", "Error while recognizing");
                    MsgDlg.ShowAsync();
                }
            }
        }

        private void erase_Click(object sender, RoutedEventArgs e)
        {
            j = 1;
            canvas.Children.Clear();
            
        }
        

        
       





    }
}