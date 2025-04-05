using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Net.Http;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text.Trim());

                    if (t != null)
                    {
                        string dados_previsao = $"Latitude: {t.Lat} \n" +
                                                $"Longitude: {t.Lon} \n" +
                                                $"Nascer do Sol: {t.Sunrise} \n" +
                                                $"Pôr do Sol: {t.Sunset} \n" +
                                                $"Temp Máx: {t.TempMax}°C \n" +
                                                $"Temp Min: {t.TempMin}°C \n" +
                                                $"Clima: {t.Description} \n" +
                                                $"Vento: {t.WindSpeed} m/s \n" +
                                                $"Visibilidade: {t.Visibility} metros";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão para essa cidade.";
                    }
                }
                else
                {
                    lbl_res.Text = "Por favor, digite o nome de uma cidade.";
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Erro de Conexão", "Sem conexão com a internet. Verifique sua rede e tente novamente.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
