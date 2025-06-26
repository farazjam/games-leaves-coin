using System;

public static class EventBus
{
    public static class Loader
    {
        public static event Action OnLoadingComplete;
        public static void LoadingCompleted() => OnLoadingComplete?.Invoke();
    }


    public static class UI
    {
        public static event Action<CurrencyType> OnShopOpen;
        public static event Action<ActionTabType> OnActionTabPressed;

        public static void ShopOpen(CurrencyType currencyType) => OnShopOpen?.Invoke(currencyType);
        public static void ActionTabPressed(ActionTabType actionTabType) => OnActionTabPressed?.Invoke(actionTabType);
    }


    public static class Shop
    {
        public static event Action<CurrencyType, Action> OnDailyRewardRequested;
        public static event Action<CurrencyType, long> OnCurrencyUpdated;
        public static event Action<CurrencyType, long> OnCurrencySpendRequested;

        public static void RequestDailyReward(CurrencyType currencyType, Action callback) => OnDailyRewardRequested?.Invoke(currencyType, callback);
        public static void CurrencyUpdated(CurrencyType currencyType, long amount) => OnCurrencyUpdated?.Invoke(currencyType, amount);
        public static void CurrencySpendRequested(CurrencyType currencyType, long amount) => OnCurrencySpendRequested?.Invoke(currencyType, amount);
    }

}
