namespace Property_inventory.Services.View
{
    public static class GLOBAL
    {
        public delegate void delegateTheme();
        public static event delegateTheme Event;

        public static void UpdateTheme()
        {
            Event?.Invoke();
        }
    }
}
