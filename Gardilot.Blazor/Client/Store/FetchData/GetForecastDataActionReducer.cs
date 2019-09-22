﻿using Blazor.Fluxor;

namespace Gardilot.Client.Store.FetchData
{
    public class GetForecastDataActionReducer : Reducer<FetchDataState, GetForecastDataAction>
    {
        public override FetchDataState Reduce(FetchDataState state, GetForecastDataAction action)
        {
            return new FetchDataState(
                isLoading: true,
                errorMessage: null,
                forecasts: null);
        }
    }
}
