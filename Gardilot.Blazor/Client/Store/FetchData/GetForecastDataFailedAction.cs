namespace Gardilot.Client.Store.FetchData
{
    public class GetForecastDataFailedAction
    {
        public string ErrorMessage { get; }

        public GetForecastDataFailedAction(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
