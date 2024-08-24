using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
//using System.Windows.Forms;

namespace NslookupApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Вызываем обработчик кнопки
                LookupButton_Click(sender, e);
            }
        }

        // icons by https://www.flaticon.com/free-icons/dns_206632

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SwitchToEnglish();
        }

        private void SwitchToEnglish()
        {
            // Получаем список доступных языковых раскладок
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo("en-US"));
        }

        

        private void LookupButton_Click(object sender, RoutedEventArgs e)
        {
            string domain = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(domain))
            {
                ResultTextBlock.Text = "Please enter a domain.";
                return;
            }

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("nslookup", domain)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    ResultTextBlock.Text = result;
                }
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void PingButton_Click(object sender, RoutedEventArgs e)
        {
            string domain = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(domain))
            {
                ResultTextBlock.Text = "Please enter a domain.";
                return;
            }

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("ping", domain)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    string result = process.StandardOutput.ReadToEnd();

                    byte[] bytes = Encoding.Default.GetBytes(result);
                    result = Encoding.GetEncoding(866).GetString(bytes);

                    process.WaitForExit();
                    ResultTextBlock.Text = result;
                }
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Error: {ex.Message}";
            }
        }
    }
}
