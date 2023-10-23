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
using System.Speech.Synthesis;

namespace WpfAlytalo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        Heat lampo = new Heat();        //oliot
        Sauna sauna = new Sauna();
        Lights keittio = new Lights();
        Lights olohuone = new Lights();

        static SpeechSynthesizer Aani = new SpeechSynthesizer();    //Äänen luonti
        public DispatcherTimer SaunaTimer = new DispatcherTimer();  //Timerin luonti
        public int MaxHeat = 100;       //nostaa saunan lämpötilan 100 asteeseen timerilla

        public MainWindow()

        {
            InitializeComponent();
            sauna.SaunaTemp = lampo.CurrentHeat;    //saunan lämpötila alussa
            SaunaTimer.Tick += SaunaTimer_Tick;
            SaunaTimer.Interval = new TimeSpan(0, 0, 1);    //timer 1 sek

        }
        public void SaunaTimer_Tick(object sender, EventArgs e)     //saunan timerille luotu void
        {
            if (sauna.Power)
            {
                btnSaunaOn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            } else
            {
                if (sauna.SaunaTemp > lampo.CurrentHeat)
                {
                    btnSaunaOff.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else
                {
                    SaunaTimer.Stop();
                }
            }
        }

        #region valot
        private void sldrKitchen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtKitchenLights.Text = sldrKitchen.Value.ToString();   //sliderin arvo tekstikenttään
            if (sldrKitchen.Value == 0) //kun slideri vedetään nollaan, valot sammuvat
            {
                btnLightsOffKitchen_Click(sender, e);   //hakee tiedot lightsoff-buttonista
                Aani.Speak("Lights off");
            }
            else
            {
                btnLightsOnKitchen.Background = Brushes.LightGreen;
                btnLightsOffKitchen.Background = Brushes.White;
                keittio.LightsOn();

            }

        }
        private void btnLightsOnKitchen_Click(object sender, RoutedEventArgs e)
        {
            keittio.LightsOn();
            if (keittio.Switched == true)
            {
                btnLightsOnKitchen.Background = Brushes.LightGreen;
                btnLightsOffKitchen.Background = Brushes.White;
                sldrKitchen.Value = 50;
                txtKitchenLights.Text = sldrKitchen.Value.ToString();
                Aani.Speak("Lights on");
            }

        }

        private void btnLightsOffKitchen_Click(object sender, RoutedEventArgs e)
        {
            keittio.LightsOff();
            if (keittio.Switched != true)
            {
                btnLightsOffKitchen.Background = Brushes.LightSalmon;
                btnLightsOnKitchen.Background = Brushes.White;
                sldrKitchen.Value = 0;
                txtKitchenLights.Text = sldrKitchen.Value.ToString();
            }
        }
        private void sldrLRoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtLRoomLights.Text = sldrLRoom.Value.ToString();
            if (sldrLRoom.Value == 0)
            {
                btnLightsOffLRoom_Click(sender, e);
                Aani.Speak("Lights off");
            }
            else
            {
                btnLightsOnLRoom.Background = Brushes.LightGreen;
                btnLightsOffLRoom.Background = Brushes.White;
                olohuone.LightsOn();
                txtLRoomLights.Text = sldrLRoom.Value.ToString();
            }
        }
        private void btnLightsOnLRoom_Click(object sender, RoutedEventArgs e)
        {
            olohuone.LightsOn();
            if (olohuone.Switched == true)
            {
                btnLightsOnLRoom.Background = Brushes.LightGreen;
                btnLightsOffLRoom.Background = Brushes.White;
                sldrLRoom.Value = 50;
                txtLRoomLights.Text = sldrLRoom.Value.ToString();
                Aani.Speak("Lights on");
            }
        }

        private void btnLightsOffLRoom_Click(object sender, RoutedEventArgs e)
        {
            olohuone.LightsOff();
            if (olohuone.Switched != true)
            {
                btnLightsOffLRoom.Background = Brushes.LightSalmon;
                btnLightsOnLRoom.Background = Brushes.White;
                sldrLRoom.Value = 0;
                txtLRoomLights.Text = sldrLRoom.Value.ToString();
            }
        }
        #endregion

        #region lämmitys  
        private void btnSetHeat_Click(object sender, RoutedEventArgs e)
        {  try { 
            lampo.CurrentHeat = double.Parse(txtWantedHeat.Text);
            sauna.SaunaTemp = lampo.CurrentHeat;    //saunan lämpötila lähtee asunnon lämpötilasta nousemaan           
            
                if (double.Parse(txtWantedHeat.Text) < 35 && double.Parse(txtWantedHeat.Text) > 15) //lämpötilan oltava väliltä 15-35
                {

                    txtCurrentHeat.Text = txtWantedHeat.Text + " °C";
                    txtWantedHeat.Text = "";
                    Aani.Speak("New temperature is now set to " + lampo.CurrentHeat + "°");

                }
                else
                {
                    MessageBox.Show("Anna lämpötila väliltä 15-35 °C.");    //ilmoittaa kun liian suuri/pieni arvo
                    return;
                }
            } catch
            {
                MessageBox.Show("Syötä vain numeroita!");
            }
        }
        #endregion

        #region sauna
        private void btnSaunaOn_Click(object sender, RoutedEventArgs e)
        {
            sauna.Power = true;
            btnSaunaOn.Background = Brushes.LightGreen;
            btnSaunaOff.Background = Brushes.White;

            if (!SaunaTimer.IsEnabled)         
            {
                SaunaTimer.Start();     //timer starttaa ja lämpötila nousee 0,5°C  sek
            }
            sauna.SaunaOn();    //hakee oliolta tiedon miten toimia
            txtSaunaLampo.Text = sauna.SaunaTemp + " °C";
            if (sauna.SaunaTemp == MaxHeat)
            {                                 //timer lopettaa kun sauna noussut 100 asteeseen
             SaunaTimer.Stop();
                Aani.Speak("Sauna is now ready");
            }
        }
        private void btnSaunaOff_Click(object sender, RoutedEventArgs e)
        {
            sauna.Power = false;
            btnSaunaOff.Background = Brushes.LightSalmon;
            btnSaunaOn.Background = Brushes.White;
            if (!SaunaTimer.IsEnabled)
            {
                SaunaTimer.Start();     //saunan timer lähtee tiputtamaan lämpöä alaspäin 1 asteen sek
            }
            sauna.SaunaOff();       //oliolta tieto kuinka toimia
            txtSaunaLampo.Text = sauna.SaunaTemp + " °C";                
        }

        private void btnStopSauna_Click_1(object sender, RoutedEventArgs e)
        {
            SaunaTimer.Stop();  //pysäyttää timerin
        }
        #endregion

        private void btnEnergyInfo_Click(object sender, RoutedEventArgs e)
        {
            string message = "Keittiön valot: " + keittio.Switched + "\n" + 
                "Olohuoneen valot: " + olohuone.Switched + "\n" +
         "Keittiön valon voimakkuus: " + txtKitchenLights.Text + "\n" +
         "Olohuoneen valon voimakkuus: " + txtLRoomLights.Text + "\n" +
         "Asunnon lämpötila: " + txtCurrentHeat.Text.ToString() + "\n" +
         "Saunan tila: " + sauna.Power.ToString() + "\n" +
         "Saunan lämpötila: " + txtSaunaLampo.Text  + "\n";

            MessageBox.Show(message);
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            sldrKitchen.Value = 0;
            sldrLRoom.Value = 0;
            btnLightsOffKitchen.Background = Brushes.White;
            btnLightsOnKitchen.Background = Brushes.White;
            btnLightsOnLRoom.Background = Brushes.White;
            btnLightsOffLRoom.Background = Brushes.White;
            txtKitchenLights.Text = "0";
            txtLRoomLights.Text = "0";
            txtCurrentHeat.Text = "20";
            txtSaunaLampo.Text = "";
            SaunaTimer.Stop();
            btnSaunaOn.Background = Brushes.White;
            btnSaunaOff.Background = Brushes.White;
        }
    }
}