using Blazor.Fluxor;
using Gardilot.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gardilot.Client.Store.FetchData
{
    public class GetForecastDataEffect : Effect<GetForecastDataAction>
    {
        private readonly HttpClient HttpClient;

        public GetForecastDataEffect(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected async override Task HandleAsync(GetForecastDataAction action, IDispatcher dispatcher)
        {
            try
            {
                WeatherForecast[] forecasts = await HttpClient
                    .GetJsonAsync<WeatherForecast[]>("/WeatherForecast");

                dispatcher.Dispatch(new GetForecastDataSuccessAction(forecasts));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new GetForecastDataFailedAction(errorMessage: e.Message));
            }
        }
    }
}
