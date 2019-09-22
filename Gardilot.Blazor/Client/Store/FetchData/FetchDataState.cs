using Gardilot.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Gardilot.Client.Store.FetchData
{
    public class FetchDataState
    {
        public FetchDataState(bool isLoading, string errorMessage, IEnumerable<WeatherForecast> forecasts)
        {
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            Forecasts = forecasts == null ? null : forecasts.ToArray();
        }

        public bool IsLoading { get; }
        public string ErrorMessage { get; }
        public WeatherForecast[] Forecasts { get; }
    }
}
