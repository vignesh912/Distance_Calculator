//VIGNESHWARAN KUMARAGURUBARAN
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SpeedConverter
{
    class Calculation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //Declaring the constants and variables
        const double FACTOR = 1.8;
        const int DEFAULT_ANGLE = -150;
        const int MAX_ANGLE = 140;
        const string OUTPUT_FILE = "Result.txt";
        private string result;      
        private string inputTime;
        private string inputSpeed;
        private string listBoxString;
        private string updateDistance = "0000000";
        //Object to rotate the Needle
        RotateTransform rotateTrans = new RotateTransform();
        public RotateTransform RotateTrans
        {
            get { return rotateTrans; }
            set { rotateTrans = value; NotifyPropertyChanged(); }
        }
        public string UpdateDistance
        {
            get { return updateDistance; }
            set { updateDistance = value; NotifyPropertyChanged(); }
        }
        public string Time
        {
            get { return inputTime; }
            set { inputTime = value; NotifyPropertyChanged(); }
        }
        public string Speed
        {
            get { return inputSpeed; }
            set { inputSpeed = value; NotifyPropertyChanged(); }
        }
        public BindingList<string> ListBoxPopulate { get; set; } = new BindingList<string>();
        //Calculating the Distance and modifying the meter in UI
        public async void CalculateDistance()
        {
            double speed = double.Parse(Speed);
            double time = double.Parse(Time);
            double distance = speed * time;
            double angleTravel = FACTOR * speed;
            if (speed > MAX_ANGLE) angleTravel = FACTOR * MAX_ANGLE;
            double error = 0.0;
            int del = 1;
            ListBoxPopulate.Clear();
            for (int i = 0, j = 0; ;)
            {
                await Task.Delay(del);
                if (i > distance && j > angleTravel) break;
                if (i <= distance)
                {
                    UpdateDistance = i.ToString().PadLeft(7, '0');
                    i++;
                }
                if (j <= angleTravel)
                {
                    error += 0.1;
                    RotateTrans.Angle = j + error + DEFAULT_ANGLE;
                    j++;
                }
            }
            LogFeed(speed, time);
        }
        //Creating logs for each hours and minutes 
        public void LogFeed(double speed, double time)
        {
            double roundTime = Math.Round((Math.Floor(time)));
            double remainingTime = time - roundTime;
            double instantaneousDistance;
            result = $"\nSpeed: {speed}  Time: {time}\n";
            for (int k = 1; k < roundTime + 1; k++)
            {
                instantaneousDistance = speed * k;
                listBoxString = $"Distance covered in {k} hours is \n {instantaneousDistance} kms";
                result += listBoxString + "\n";
                ListBoxPopulate.Add(listBoxString);
            }
            if (remainingTime != 0)
            {
                instantaneousDistance = Math.Round((remainingTime * speed), 2);
                remainingTime = Math.Round((remainingTime * 60), 2);
                listBoxString = $"Distance covered in {remainingTime} minutes is \n {instantaneousDistance} kms";
                result += listBoxString + "\n";
                ListBoxPopulate.Add(listBoxString);
            }
            result += $"\n Total distance covered: {speed * time} kms \n";
        }
        //Saving the logs into a Text file 
        public void SaveFile()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string file = Path.Combine(path, OUTPUT_FILE);
            File.AppendAllText(file, result);
        }
        //Clearing the UI
        public void Clear()
        {
            Time = string.Empty;
            Speed = string.Empty;
            ListBoxPopulate.Clear();
            RotateTrans.Angle = 0 + DEFAULT_ANGLE;
            UpdateDistance = "0000000";
        }
    }
}


