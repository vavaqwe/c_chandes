using NAudio.CoreAudioApi;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.Collections.Generic;

namespace curvva
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private IPAddress Set_ip()
        {
            string hostName = Dns.GetHostName(); // Получить имя хоста
            IPAddress[] ipAddress = Dns.GetHostAddresses(hostName); // Получить все IP-адреса для этого хоста
            IPAddress ipAdd = null;
            foreach (IPAddress ipAddres in ipAddress)
            {
                if (ipAddres.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // Выбрать только IPv4 адреса
                {
                    string ip = ipAddres.ToString();
                    ipAdd = ipAddres;
                }
            }
            return ipAdd;
        }
        string Set_1p()
        {
            string hostName = Dns.GetHostName(); // Получить имя хоста
            IPAddress[] ipAddress = Dns.GetHostAddresses(hostName); // Получить все IP-адреса для этого хоста
            string ip = null;
            foreach (IPAddress ipAddres in ipAddress)
            {
                if (ipAddres.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // Выбрать только IPv4 адреса
                {
                    ip = ipAddres.ToString();
                    IPAddress ipAdd = ipAddres;
                }
            }
            return ip;
        }
        private async void Server()
        {
            try
            {
                IPAddress ipAdd = Set_ip();
                // Создание серверного сокета
                TcpListener listener = new TcpListener(ipAdd, 12345);
                listener.Start();
                string user = Environment.UserName.ToString();
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    try
                    {
                        NetworkStream stream = client.GetStream();
                        byte[] buffer = new byte[1024];

                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        int req = message[0] - '0';

                        if (req == 0)
                        {
                            Process cmd = new Process();
                            cmd.StartInfo.FileName = "cmd.exe";
                            cmd.StartInfo.RedirectStandardInput = true;
                            cmd.StartInfo.UseShellExecute = false;
                            cmd.Start();
                            cmd.StandardInput.WriteLine("shutdown /s /t 0");
                            cmd.WaitForExit();
                        }
                        if (req == 1)
                        {
                            string path = $@"C:\Users\{user}\AppData\Local\Discord\Update.exe";
                            string arguments = "--processStart Discord.exe";

                            Process process = new Process();
                            process.StartInfo.FileName = path;
                            process.StartInfo.Arguments = arguments;
                            process.Start();


                        }
                        if (req == 2)
                        {
                            Process.Start("https://google.com");
                        }
                        if (req == 3)
                        {
                            Process.Start("https://www.youtube.com/watch?v=0-C0lCPFTj8");
                        }
                        if (req == 4)
                        {
                            Process cmd = new Process();
                            cmd.StartInfo.FileName = "cmd.exe";
                            cmd.StartInfo.RedirectStandardInput = true;
                            cmd.StartInfo.UseShellExecute = false;
                            cmd.Start();
                            cmd.StandardInput.WriteLine("shutdown /r /t 0");
                            cmd.WaitForExit();
                        }
                        if (req == 5)
                        {
                            string newValue = ConfigurationManager.AppSettings["game1"];
                            Process.Start(newValue);
                        }
                        if (req == 6)
                        {
                            string cutURI = HttpUtility.UrlDecode(message, Encoding.UTF8);
                            cutURI = cutURI.Remove(0, 1);
                            if (cutURI[4].Equals("https") && cutURI[3].Equals("http"))
                            {
                                Process.Start(cutURI);
                            }
                            else
                            {
                                string encodedQuery = HttpUtility.UrlEncode(cutURI);
                                Process.Start("https://www.google.com/search?q=" + encodedQuery);
                            }

                        }
                        if (req == 7)
                        {
                            string cutURI = HttpUtility.UrlDecode(message, Encoding.UTF8);
                            cutURI = cutURI.Remove(0, 1);

                            string encodedQuery = HttpUtility.UrlEncode(cutURI);
                            Process.Start("https://www.youtube.com/results?search_query=" + encodedQuery);
                        }
                        if (req == 8)
                        {
                            string cutURI = message.Remove(0, 1);
                            float volume = float.Parse(cutURI, CultureInfo.InvariantCulture.NumberFormat);
                            volume /= 100;
                            MMDevice defaultPlaybackDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                            // устанавливаем громкость 
                            defaultPlaybackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
                        }
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "curva";
            notifyIcon1.BalloonTipText = "приложение закрыть а ты хуесос можешь кнопочки на телефоне нажимать";
            notifyIcon1.Text = "curva";

            panelContainer.Width = 0;
            panelContainer.Visible = false;
            btnTogglePanel.Text = ">>";

            string ip = Set_1p();
            string myVariable = ConfigurationManager.AppSettings["start_up"];
            if (myVariable.Equals('1'))
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "curva.exe");
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.SetValue("MyApp", filePath);

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["start_up"].Value = "0";
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            label1.Text = ip;
            QRCoder.QRCodeGenerator QR = new QRCoder.QRCodeGenerator();
            var MyData = QR.CreateQrCode(ip, QRCoder.QRCodeGenerator.ECCLevel.H);
            var code = new QRCoder.QRCode(MyData);
            pictureBox1.Image = code.GetGraphic(50);
            Server();
        }

        private void BtnTogglePanel_Click(object sender, EventArgs e)
        {
            if (panelContainer.Width == 0)
            {
                panelContainer.Visible = true;
                this.Width = 600;
                panelContainer.Width = 200; // Ширина панели.
                panelContainer.BringToFront();
                btnTogglePanel.Text = "<<"; // Текст кнопки для скрытия панели.
            }
            else
            {
                // Если ширина панели больше 0, значит панель открыта, и мы ее скрываем.
                panelContainer.Visible = false;
                this.Width = 350;
                panelContainer.Width = 0; // Ширина панели.
                btnTogglePanel.Text = ">>"; // Текст кнопки для открытия панели.
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Получаем текущее значение переменной из app.config
            string currentValue = ConfigurationManager.AppSettings["game1"];

            // Изменяем значение переменной на "new value"
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["game1"].Value = textBox1.Text;
            config.Save(ConfigurationSaveMode.Modified);

            // После сохранения изменений, обновляем значения в ConfigurationManager
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);

            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void ЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        int counter = 1;
        private List<Button> buttons = new List<Button>();
        private List<TextBox> textBoxes = new List<TextBox>();

        private void addButton_Click(object sender, EventArgs e)
        {
            TextBox textBox = new TextBox();
            textBox.Text = textBox1.Text;
            textBox.Size = textBox1.Size;
            textBox.Location = new Point(14, textBox1.Location.Y + counter * 50  );


            Button button = new Button();
            button.Text = button1.Text;
            button.Size = button1.Size;
            button.Location = new Point(14, button1.Location.Y + counter * 50  );


            buttons.Add(button);
            textBoxes.Add(textBox);

            counter++;

            panelContainer.Controls.Add(button);
            panelContainer.Controls.Add(textBox);

            if (counter >= 5)
            {
                buttonAdd.Visible = false;
            }
            else
            {
                buttonAdd.Visible = true;
            }
            button.Click += new EventHandler(button_Click);
        }
        private void button_Click(object sender, EventArgs e)
        {
            // Получаем кнопку, на которую нажали
            Button button = (Button)sender;

            // Получаем индекс кнопки в списке
            int index = buttons.IndexOf(button);

            // Получаем соответствующий текстбокс
            TextBox textBox = textBoxes[index];
            string game = ConfigurationManager.ConnectionStrings[];
            config.AppSettings.Settings["start_up"].Value = "0";
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (buttons.Count > 0) // если есть хотя бы одна кнопка
            {
                Button buttonToRemove = buttons[buttons.Count - 1]; // получаем последнюю кнопку из списка
                TextBox textBoxToRemove = textBoxes[textBoxes.Count - 1]; // получаем последний текстбокс из списка

                panelContainer.Controls.Remove(buttonToRemove); // удаляем кнопку из контейнера
                panelContainer.Controls.Remove(textBoxToRemove); // удаляем текстбокс из контейнера

                buttons.Remove(buttonToRemove); // удаляем кнопку из списка
                textBoxes.Remove(textBoxToRemove); // удаляем текстбокс из списка

                counter--; // уменьшаем счетчик
            }

            if (counter < 5)
            {
                buttonAdd.Visible = true; // делаем кнопку добавления видимой, если еще не достигнут лимит
            }
        }
    }
}



