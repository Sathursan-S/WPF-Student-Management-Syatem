using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp_SMS.Models;
using WpfApp_SMS.Views;

namespace WpfApp_SMS.VM
{
    public partial class EditWinVM : ObservableObject
    {
        [ObservableProperty]
        public string stdId;
        [ObservableProperty]
        public string firstName;
        [ObservableProperty]
        public string lastName;
        [ObservableProperty]
        public DateOnly dOB;
        [ObservableProperty]
        public DateTime dOBDate;
        [ObservableProperty]
        public float gPA;
        [ObservableProperty]
        private BitmapImage imagePreview;
        [ObservableProperty]
        private byte[] img;
        [ObservableProperty]
        private Student std;

        private string _imageFilePath;

        [RelayCommand]
        public void Edit()
        {
            if (IsEdited)
            {
                DOB = DateOnly.FromDateTime(DOBDate);

                using var db = new SMSDbContext();
                Std = db.Students.Find(StdId);
                Std.FirstName = FirstName;
                Std.LastName = LastName;
                Std.DOB = DOB;
                Std.GPA = GPA;
                if (ImagePreview != null)
                {
                    Std.Img = ConvertBitmapImageToByteArray(ImagePreview);
                }

                db.SaveChanges();
                MessageBox.Show("Saved", "Done", MessageBoxButton.OK);
                Application.Current.Windows.OfType<EditWin>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("No changes made", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
        }

        public bool IsEdited
        {
            get
            {
                return Std.LastName != LastName || Std.LastName != LastName || Std.DOB.ToDateTime(TimeOnly.Parse("10:00 PM")) != DOBDate || Std.GPA!= GPA|| !string.IsNullOrEmpty(_imageFilePath);
            }
        }

        public Action CloseAction { get; internal set; }


        [RelayCommand]
        private void UploadImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                _imageFilePath = dialog.FileName;
                BitmapImage image = new BitmapImage(new Uri(_imageFilePath));
                ImagePreview = image;
            }
        }

        public EditWinVM(Student std)
        {
            
            StdId = std.StudentID;
            FirstName = std.FirstName;
            LastName = std.LastName;
            DOB = std.DOB;
            GPA = std.GPA;
            Img = std.Img;
            ImagePreview = ConvertByteArrayToBitmapImage(Img);
            DOBDate = DOB.ToDateTime(TimeOnly.Parse("10:00 PM"));

            using var db = new SMSDbContext();
            Std = db.Students.Find(StdId);
        }


        public byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            BitmapEncoder encoder = new PngBitmapEncoder(); // or JpegBitmapEncoder or any other encoder you prefer
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        public BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                memoryStream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze(); // Optional, but recommended to improve performance and allow cross-thread access
            return bitmapImage;
        }

        [RelayCommand]
        public void Cancel()
        {
            Application.Current.Windows.OfType<EditWin>().FirstOrDefault()?.Close();
        }
    }
}
