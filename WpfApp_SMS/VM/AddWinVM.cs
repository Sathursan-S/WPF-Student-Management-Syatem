using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    public partial class AddWinVM : ObservableObject
    {
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

        private string _imageFilePath;

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

        /// <summary>
        /// Saves a new student to the database.
        /// </summary>
        [RelayCommand]
        public void Save()
        {
            // Ensure the required fields are filled out.
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ensure the GPA is valid.
            if (GPA < 0 || GPA > 4)
            {
                MessageBox.Show("GPA value must be between 0 and 4.", "Error");
                return;
            }

            // Convert the date of birth to DateOnly format.
            DOB = DateOnly.FromDateTime(DOBDate);

            // Create a new student and add it to the database.
            Student student = new Student(FirstName, LastName, DOB, GPA, ConvertBitmapImageToByteArray(ImagePreview));
            using (var _context = new SMSDbContext())
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }

            // Show a success message and close the window.
            MessageBox.Show($"Student added successfully Student ID : {student.StudentID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Windows.OfType<AddWin>().FirstOrDefault()?.Close();
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

        [RelayCommand]
        public void Cancel()
        {
            Application.Current.Windows.OfType<AddWin>().FirstOrDefault()?.Close();
        }

        public AddWinVM()
        {
            DOBDate = DateTime.Now;
        }
    }
}
