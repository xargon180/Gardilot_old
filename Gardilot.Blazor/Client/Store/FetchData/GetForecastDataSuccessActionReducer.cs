﻿using Blazor.Fluxor;

namespace Gardilot.Client.Store.FetchData
{
    public class GetForecastDataSuccessActionReducer : Reducer<FetchDataState, GetForecastDataSuccessAction>
    {
        public override FetchDataState Reduce(FetchDataState state, GetForecastDataSuccessAction action)
        {
            return new FetchDataState(
                isLoading: false,
                errorMessage: null,
                forecasts: action.WeatherForecasts);
        }
    }
}
