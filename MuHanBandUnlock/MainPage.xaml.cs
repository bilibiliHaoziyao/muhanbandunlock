using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MuHanBandUnlock.Services;

namespace MuHanBandUnlock
{
    public sealed partial class MainPage : Page
    {
        private string _lastCode = string.Empty;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCard.Visibility = Visibility.Collapsed;
            GenerateButton.IsEnabled = MacInput.Text.Trim().Length > 0 || SnInput.Text.Trim().Length > 0;
        }

        private void Algorithm_Toggled(object sender, RoutedEventArgs e)
        {
            ErrorCard.Visibility = Visibility.Collapsed;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorCard.Visibility = Visibility.Collapsed;

            var mac = MacInput.Text;
            var sn = SnInput.Text;

            if (UnlockCodeService.NormalizeMac(mac).Length == 0 &&
                UnlockCodeService.NormalizeSn(sn).Length == 0)
            {
                ShowError("MAC 和 SN 都不能为空");
                return;
            }

            try
            {
                _lastCode = UnlockCodeService.CalculateUnlockCode(mac, sn, NewAlgorithmToggle.IsOn);
                ResultText.Text = _lastCode;
                ResultCard.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_lastCode)) return;

            var dataPackage = new DataPackage();
            dataPackage.SetText(_lastCode);
            Clipboard.SetContent(dataPackage);

            ResultHint.Text = "已复制到剪贴板 · 在手环密码输入界面依次输入这 10 位数字即可解锁";
        }

        private void ShowError(string message)
        {
            ResultCard.Visibility = Visibility.Collapsed;
            ErrorText.Text = message;
            ErrorCard.Visibility = Visibility.Visible;
        }
    }
}
