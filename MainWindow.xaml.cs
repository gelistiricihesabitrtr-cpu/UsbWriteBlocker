using System;
using System.Windows;
using System.Windows.Media;
using UsbWriteBlocker.Services;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace UsbWriteBlocker
{
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon? _trayIcon;
        private System.Drawing.Icon? _greenIcon;
        private System.Drawing.Icon? _redIcon;
        private bool _isExiting;

        public MainWindow()
        {
            InitializeComponent();
            LoadTrayIcons();
            InitializeTrayIcon();
            RefreshStatus();

            StateChanged += (_, _) =>
            {
                if (WindowState == WindowState.Minimized)
                {
                    Hide();
                }
            };
            Closing += MainWindow_Closing;
        }

        private void LoadTrayIcons()
        {
            _greenIcon = new System.Drawing.Icon(
                System.Windows.Application.GetResourceStream(new Uri("Resources/icon-green.ico", UriKind.Relative)).Stream);
            _redIcon = new System.Drawing.Icon(
                System.Windows.Application.GetResourceStream(new Uri("Resources/icon-red.ico", UriKind.Relative)).Stream);
        }

        private void InitializeTrayIcon()
        {
            _trayIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = _greenIcon,
                Visible = true,
                Text = "USB Write Blocker: Korumasız"
            };

            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Göster", null, (_, _) => ShowWindow());
            menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            menu.Items.Add("Çıkış", null, (_, _) => ExitApplication());
            _trayIcon.ContextMenuStrip = menu;

            _trayIcon.DoubleClick += (_, _) => ShowWindow();
        }

        private void ShowWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void ExitApplication()
        {
            _isExiting = true;
            Close();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExiting)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
                return;
            }

            _trayIcon?.Dispose();
            _greenIcon?.Dispose();
            _redIcon?.Dispose();
        }

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteProtectService.Enable();
                RefreshStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Yazma koruması etkinleştirilemedi:\n{ex.Message}",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteProtectService.Disable();
                RefreshStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Yazma koruması devre dışı bırakılamadı:\n{ex.Message}",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshStatus()
        {
            bool active = WriteProtectService.IsWriteBlockActive();

            if (active)
            {
                StatusDot.Fill = new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));
                StatusTitle.Text = "WRITE BLOCKER AKTİF";
                StatusSubtitle.Text = "USB depolama aygıtlarına yazma işlemi engelleniyor.";
                EnableButton.IsEnabled = false;
                DisableButton.IsEnabled = true;

                if (_trayIcon != null)
                {
                    _trayIcon.Icon = _redIcon;
                    _trayIcon.Text = "USB Write Blocker: AKTİF";
                }
            }
            else
            {
                StatusDot.Fill = new SolidColorBrush(Color.FromRgb(0x22, 0xC5, 0x5E));
                StatusTitle.Text = "KORUMASIZ";
                StatusSubtitle.Text = "USB depolama aygıtlarına yazma işlemi engellenmiyor.";
                EnableButton.IsEnabled = true;
                DisableButton.IsEnabled = false;

                if (_trayIcon != null)
                {
                    _trayIcon.Icon = _greenIcon;
                    _trayIcon.Text = "USB Write Blocker: Korumasız";
                }
            }
        }
    }
}
