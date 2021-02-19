using ImTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Keyboard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variasbles
        private DispatcherTimer _chronometr = new DispatcherTimer();
        private DispatcherTimer _progtimer = new DispatcherTimer();
        private string[] _inputstr = new string[40];
        private int _position = 0;
        private int _chronCounter = 0;
        private int _failCounter = 0;
        private int _speedCounter = 0;
        private bool _progIsActive = false;

        public MainWindow()
        {
            InitializeComponent();
            MyStamina.KeyDown += keyImput;
            
        }

        private void keyImput(object sender, KeyEventArgs e)
        {
            string chr = e.Key.ToString();
            int codeascii = KeyInterop.VirtualKeyFromKey(e.Key);
            if (_position == 40)
            {
                finishProgram();
            }

            if(codeascii >= 65 && codeascii <= 90)
            {
                if (chr == _inputstr[_position] && _progIsActive && _position < 40)
                {
                    TextBlock block = new TextBlock();
                    block.Background = Brushes.Green;
                    block.Text = chr;
                    
                    textGrid.Children.Add(block);
                    Grid.SetRow(block, 1);
                    Grid.SetColumn(block, _position);
                    _position++;
                    _speedCounter++;
                }
                else
                {
                    _failCounter++;
                    labelFails.Content = _failCounter.ToString();
                }
            }
        }

        private void finishProgram()
        {
            _chronometr.Stop();
            labelSpeed.Visibility = Visibility.Visible;
            labelSpeed.Content = $"{_speedCounter} / {_chronCounter} ps";
            _progIsActive = false;
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            _progtimer.Stop();
            clearTextGrid();
        }

        private void startButtonClick(object sender, RoutedEventArgs e)
        {
            _chronCounter = 0;
            _failCounter = 0;
            _speedCounter = 0;
            labelSpeed.Visibility = Visibility.Hidden;
            labelSpeed.Content = "";
            Random rand = new Random();
            int code;

            for (int i = 0; i < 40; i++)
            {
                code = rand.Next(65, 90);
                TextBlock block = new TextBlock();
                _inputstr[i] += Convert.ToChar(code);
                block.Text += Convert.ToChar(code);
                block.Background = Brushes.Green;
                textGrid.Children.Add(block);
                Grid.SetRow(block, 0);
                Grid.SetColumn(block, i);
            }

            _progIsActive = true;
            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
            chronometrTimerStart();
            programTimerStart();
        }

        private void programTimerStart()
        {
            _progtimer.Tick += new EventHandler(progTimerTick);
            _progtimer.Interval = TimeSpan.FromSeconds(60);
            _progtimer.Start();
        }

        private void progTimerTick(object sender, EventArgs e)
        {
            finishProgram();
        }

        private void chronometrTimerStart()
        {
            _chronometr.Tick += new EventHandler(chonometrTick);
            _chronometr.Interval = TimeSpan.FromSeconds(1);
            _chronometr.Start();
        }

        private void chonometrTick(object sender, EventArgs e)
        {
            _chronCounter++;
        }

        private void clearTextGrid()
        {
            int clr = textGrid.Children.Count;
            for (int i = 0; i < clr; i++)
            {
                textGrid.Children.RemoveAt(0);
            }
            for (int i = 0; i < 40; i++)
            {
                //textGrid.ColumnDefinitions.RemoveAt(0);
                _inputstr[i] = "";
            }
            _position = 0;
        }

        private void stopButtonClick(object sender, RoutedEventArgs e)
        {
            finishProgram();
        }
    }
}
