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

namespace Pancréatite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            splashpic.Source = new BitmapImage(new Uri(@"pack://application:,,,/Pancréatite;component/Resources/pantoneofy_2019_dribbble_edition1_2x.jpg"));

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            prog.Value += 1;
            if(prog.Value == 100)
            {                
                this.Close();
            }
        }
    }
    }
