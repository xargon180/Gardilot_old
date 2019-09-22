﻿using Blazor.Fluxor;

namespace Gardilot.Client.Store.FetchData
{
    public class FetchDataFeature : Feature<FetchDataState>
    {
        public override string GetName() => "FetchData";
        protected override FetchDataState GetInitialState() => new FetchDataState(
            isLoading: false,
            errorMessage: null,
            forecasts: null);
    }
}
