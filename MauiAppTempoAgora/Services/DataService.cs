using MauiAppTempoAgora.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Maui.Controls; 

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiKey = "cf530c4f09d2b29ccedfd005965a984f"; 

        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            if (!IsInternetConnected())
            {
                await Application.Current.MainPage.DisplayAlert("Sem Conexão", "Você não está conectado à internet. Verifique sua conexão e tente novamente.", "OK");
                return null;
            }

            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={apiKey}&units=metric&lang=pt_br";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro de Autenticação", "Erro de autenticação com a API. Verifique sua chave.", "OK");
                    return null;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    await Application.Current.MainPage.DisplayAlert("Cidade Não Encontrada", "Cidade não encontrada. Verifique o nome digitado.", "OK");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao acessar os dados. Tente novamente mais tarde. Código: {response.StatusCode}", "OK");
                    return null;
                }

                string conteudo = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(conteudo);

                Tempo t = new Tempo
                {
                    Lat = json.coord?.lat,
                    Lon = json.coord?.lon,
                    Sunrise = UnixTimeToString((long?)json.sys?.sunrise),
                    Sunset = UnixTimeToString((long?)json.sys?.sunset),
                    TempMin = json.main?.temp_min,
                    TempMax = json.main?.temp_max,
                    Description = json.weather?[0]?.description,
                    WindSpeed = json.wind?.speed,
                    Visibility = json?.visibility
                };

                return t;
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert("Erro de Conexão", "Sem conexão com a internet. Verifique sua rede.", "OK");
                return null;
            }
            catch (JsonSerializationException)
            {
                await Application.Current.MainPage.DisplayAlert("Erro de Dados", "Erro ao processar os dados da previsão do tempo.", "OK");
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro Inesperado", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
                return null;
            }
        }

        private static string UnixTimeToString(long? unixTime)
        {
            if (unixTime.HasValue)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime.Value).ToLocalTime().DateTime;
                return dateTime.ToString("HH:mm");
            }
            return string.Empty;
        }

        private static bool IsInternetConnected()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}