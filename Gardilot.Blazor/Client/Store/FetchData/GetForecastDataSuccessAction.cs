using Gardilot.Shared;

namespace Gardilot.Client.Store.FetchData
{
    public class GetForecastDataSuccessAction
    {
        public WeatherForecast[] WeatherForecasts { get; private set; }

        public GetForecastDataSuccessAction(WeatherForecast[] weatherForecasts)
        {
            WeatherForecasts = weatherForecasts;
        }
    }
}
