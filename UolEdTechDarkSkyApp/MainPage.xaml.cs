using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.Essentials;
using Xamarin.Forms;
using DarkSkyApi;
using DarkSkyApi.Models;
using UOLEdTechDarkSky.Exceptions;
using UOLEdTechDarkSky.Utilitys;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator;



namespace UOLEdTechDarkSky
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        // These coordinates came from the Forecast API documentation,
        // and should return forecasts with all blocks.
        private const string _constApiKey      = "2119bac3f1b696cb92120af2a08c7161";
        private const double BarueriLatitude   = 37.8267;
        private const double BarueriLongitude  = -122.423;
        private static double LocalLatitude    = -2.5368808;
        private static double LocalLongitude   = -46.645944;
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get => _IsBusy;
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy { get => !_IsBusy; }

        #region variaveis MVVM
        //-- Dados dos Proximos 7 dias -----------------------------------------------------------------------
        public List<Weather> _Weathers     ; public List<Weather> Weathers     { get => _Weathers           ; set { _Weathers            = value; OnPropertyChanged(); } }
        //-- Dados do Dia ------------------------------------------------------------------------------------
        private string _DarkSky_Temperatura; public string DarkSky_Temperatura { get => _DarkSky_Temperatura; set { _DarkSky_Temperatura = value; OnPropertyChanged(); } }
        private string _DarkSky_Cidade     ; public string DarkSky_Cidade      { get => _DarkSky_Cidade     ; set { _DarkSky_Cidade      = value; OnPropertyChanged(); } }
        private string _DarkSky_UF         ; public string DarkSky_UF          { get => _DarkSky_UF         ; set { _DarkSky_UF          = value; OnPropertyChanged(); } }
        private string _DarkSky_Descritivo ; public string DarkSky_Descritivo  { get => _DarkSky_Descritivo ; set { _DarkSky_Descritivo  = value; OnPropertyChanged(); } }
        private string _DarkSky_DataHora   ; public string DarkSky_DataHora    { get => _DarkSky_DataHora   ; set { _DarkSky_DataHora    = value; OnPropertyChanged(); } }
        private string _DarkSky_Humidade   ; public string DarkSky_Humidade    { get => _DarkSky_Humidade   ; set { _DarkSky_Humidade    = value; OnPropertyChanged(); } }
        private string _DarkSky_Vento      ; public string DarkSky_Vento       { get => _DarkSky_Vento      ; set { _DarkSky_Vento       = value; OnPropertyChanged(); } }
        private string _DarkSky_Pressao    ; public string DarkSky_Pressao     { get => _DarkSky_Pressao    ; set { _DarkSky_Pressao     = value; OnPropertyChanged(); } }
        private string _DarkSky_Nebulosa   ; public string DarkSky_Nebulosa    { get => _DarkSky_Nebulosa   ; set { _DarkSky_Nebulosa    = value; OnPropertyChanged(); } }
        //-- Proximo 7 dias -----------------------------------------------------------------------
        private string _DarkSky_Temp1DiaMin; public string DarkSky_Temp1DiaMin { get => _DarkSky_Temp1DiaMin; set { _DarkSky_Temp1DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp2DiaMin; public string DarkSky_Temp2DiaMin { get => _DarkSky_Temp2DiaMin; set { _DarkSky_Temp2DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp3DiaMin; public string DarkSky_Temp3DiaMin { get => _DarkSky_Temp3DiaMin; set { _DarkSky_Temp3DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp4DiaMin; public string DarkSky_Temp4DiaMin { get => _DarkSky_Temp4DiaMin; set { _DarkSky_Temp4DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp5DiaMin; public string DarkSky_Temp5DiaMin { get => _DarkSky_Temp5DiaMin; set { _DarkSky_Temp5DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp6DiaMin; public string DarkSky_Temp6DiaMin { get => _DarkSky_Temp6DiaMin; set { _DarkSky_Temp6DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp7DiaMin; public string DarkSky_Temp7DiaMin { get => _DarkSky_Temp7DiaMin; set { _DarkSky_Temp7DiaMin = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp1DiaMax; public string DarkSky_Temp1DiaMax { get => _DarkSky_Temp1DiaMax; set { _DarkSky_Temp1DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp2DiaMax; public string DarkSky_Temp2DiaMax { get => _DarkSky_Temp2DiaMax; set { _DarkSky_Temp2DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp3DiaMax; public string DarkSky_Temp3DiaMax { get => _DarkSky_Temp3DiaMax; set { _DarkSky_Temp3DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp4DiaMax; public string DarkSky_Temp4DiaMax { get => _DarkSky_Temp4DiaMax; set { _DarkSky_Temp4DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp5DiaMax; public string DarkSky_Temp5DiaMax { get => _DarkSky_Temp5DiaMax; set { _DarkSky_Temp5DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp6DiaMax; public string DarkSky_Temp6DiaMax { get => _DarkSky_Temp6DiaMax; set { _DarkSky_Temp6DiaMax = value; OnPropertyChanged(); } }
        private string _DarkSky_Temp7DiaMax; public string DarkSky_Temp7DiaMax { get => _DarkSky_Temp7DiaMax; set { _DarkSky_Temp7DiaMax = value; OnPropertyChanged(); } }

        //-----------------------------------------------------------------------------------------
        //-- Proximo 7 dias -----------------------------------------------------------------------
        #endregion

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            Weathers            = WeatherData();
            DarkSky_Temperatura = "";
            DarkSky_UF          = "SP";
            DarkSky_Cidade      = "SÃO PAULO, " + DarkSky_UF;
            DarkSky_Descritivo  = "Dia nublado";
            DarkSky_DataHora    = DateTime.Now.Day.ToString("D2") + " de Março - " + DateTime.Now.ToString("HH:mm") + "Hs";

            //-- Valores Padrões ----------------------------------------------------------------------- 
            DarkSky_Descritivo  = "Dia nublado";
            DarkSky_Temperatura = "26";
            DarkSky_Humidade    = "25%";
            DarkSky_Vento       = "6.21 m/s";
            DarkSky_Pressao     = "1013.03 hpa";
            DarkSky_Nebulosa    = "20%";
            LocalLatitude       = -2.5368808;
            LocalLongitude      = -46.645944;

            //------------- Chama a API depois de 2 segundos, pede autorização para acessar GPS ----------------
            Device.StartTimer(TimeSpan.FromSeconds( 1), () => { _GetWeatherDataAsync(); return false; });     //Só executa uma vez ao carregar o app
            Device.StartTimer(TimeSpan.FromSeconds(15), () => { _GetWeatherDataAsync(); return true ; });     //Só executa a cada X segundos, atualizando a tela
        }



        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            _GetWeatherDataAsync();
        }



        /* ===========================================================================================
        *  Edinaldo Silva - UOL EdTech
        *  Mar/2020
        *  Obtem a geolocalizacao e mostra no celular
        ===========================================================================================*/
        private async void _GetWeatherDataAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine($"Sem acesso a internet...");
                throw new ConnectivityException();
            }
            try
            {
                var hasPermission = await Utilitys.Permissions.CheckPermissions(Permission.Location);
                if (hasPermission)
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 25;
                    var pPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(60), null, false);
                    if (pPosition != null)
                    {
                        LocalLatitude  = pPosition.Latitude;
                        LocalLongitude = pPosition.Longitude;
                    }
                } //Se não tiver permissão, pega um valor predefinido, para não complicar
                IsBusy              = true;
                var client          = new DarkSkyService(_constApiKey);
                Forecast sResult    = await client.GetWeatherDataAsync(LocalLatitude, LocalLongitude, Unit.CA);
                DarkSky_Descritivo  = sResult.Currently.Summary.ToString();                       //Tem que traduzir
                DarkSky_Temperatura = sResult.Currently.Temperature.ToString("0");
                DarkSky_Humidade    = sResult.Currently.Humidity.ToString("0")+"%";
                DarkSky_Vento       = sResult.Currently.WindSpeed.ToString()+" m/s";
                DarkSky_Pressao     = sResult.Currently.Pressure.ToString()+" hpa";
                DarkSky_Nebulosa    = sResult.Currently.Visibility.ToString("0")+"%";
                //-- Proximo 6 dias ----------------------------------------------------------------------- Peguei temperatura mais alta, o certo seria informar min e max na tela
                DarkSky_Temp1DiaMin = sResult.Daily.Days[0].LowTemperature.ToString("0");
                DarkSky_Temp2DiaMin = sResult.Daily.Days[1].LowTemperature.ToString("0");
                DarkSky_Temp3DiaMin = sResult.Daily.Days[2].LowTemperature.ToString("0");
                DarkSky_Temp4DiaMin = sResult.Daily.Days[3].LowTemperature.ToString("0");
                DarkSky_Temp5DiaMin = sResult.Daily.Days[4].LowTemperature.ToString("0");
                DarkSky_Temp6DiaMin = sResult.Daily.Days[5].LowTemperature.ToString("0");
                DarkSky_Temp1DiaMax = sResult.Daily.Days[0].HighTemperature.ToString("0");
                DarkSky_Temp2DiaMax = sResult.Daily.Days[1].HighTemperature.ToString("0");
                DarkSky_Temp3DiaMax = sResult.Daily.Days[2].HighTemperature.ToString("0");
                DarkSky_Temp4DiaMax = sResult.Daily.Days[3].HighTemperature.ToString("0");
                DarkSky_Temp5DiaMax = sResult.Daily.Days[4].HighTemperature.ToString("0");
                DarkSky_Temp6DiaMax = sResult.Daily.Days[5].HighTemperature.ToString("0");
                //-----------------------------------------------------------------------------------------
                Weathers = WeatherDataPreenchido();
                await Task.Delay(475);
            }
            catch (Exception ex)
                {
                System.Diagnostics.Debug.WriteLine($"_GetWeatherDataAsync Error: {ex.Message + " " + ex.InnerException }");
            }
            IsBusy = false;
        }


        //7 dias vazio
        private List<Weather> WeatherData()
        {
            var tempList = new List<Weather>();
            tempList.Add(new Weather { TempMin = "20", TempMax = "29", Date = "Quarta ", Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = "23", TempMax = "27", Date = "Quinta" , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = "25", TempMax = "26", Date = "Sexta"  , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = "22", TempMax = "25", Date = "Sábado" , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = "24", TempMax = "24", Date = "Domingo", Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = "26", TempMax = "27", Date = "Segunda", Icon = "weather.png" });
            return tempList;
        }
        private List<Weather> WeatherDataPreenchido()
        {
            var tempList = new List<Weather>();
            tempList.Add(new Weather { TempMin = DarkSky_Temp1DiaMin, TempMax = DarkSky_Temp1DiaMax, Date = "Quarta ", Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = DarkSky_Temp2DiaMin, TempMax = DarkSky_Temp2DiaMax, Date = "Quinta" , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = DarkSky_Temp3DiaMin, TempMax = DarkSky_Temp3DiaMax, Date = "Sexta"  , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = DarkSky_Temp4DiaMin, TempMax = DarkSky_Temp4DiaMax, Date = "Sábado" , Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = DarkSky_Temp5DiaMin, TempMax = DarkSky_Temp5DiaMax, Date = "Domingo", Icon = "weather.png" });
            tempList.Add(new Weather { TempMin = DarkSky_Temp6DiaMin, TempMax = DarkSky_Temp6DiaMax, Date = "Segunda", Icon = "weather.png" });
            return tempList;
        }

    }


    public class Weather
    {
        public string Date    { get; set; }
        public string Icon    { get; set; }
        public string TempMin { get; set; }
        public string TempMax { get; set; }
    }
}
