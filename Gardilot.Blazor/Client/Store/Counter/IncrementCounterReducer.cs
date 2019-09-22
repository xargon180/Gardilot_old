﻿using Blazor.Fluxor;

namespace Gardilot.Client.Store.Counter
{
    public class IncrementCounterReducer : Reducer<CounterState, IncrementCounterAction>
    {
        public override CounterState Reduce(CounterState state, IncrementCounterAction action)
        {
            return new CounterState(state.ClickCount + 1);
        }
    }
}
