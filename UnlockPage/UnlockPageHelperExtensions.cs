namespace SitefinityWebApp.UnlockPage
{
    /// <summary>
    /// Uses the UnlockPageHelper methods. 
    /// Provides easy and direct install of the page unlocking.
    /// If you need to modify the install, inherit and override the methods in the UnlockPageHelper.
    /// </summary>
    public class UnlockPageHelperExtensions
    {
        public static UnlockPageHelper Get()
        {
            return new UnlockPageHelper();
        }

        /// <summary>
        /// Gets UnlockPageHelper and installs command widgets,
        /// extension scripts and the service
        /// </summary>
        public static void Install()
        {
            var helper = UnlockPageHelperExtensions.Get();
            helper.InstallCommandWidgets();
            helper.InstallExtensionScript();
            helper.RegisterService<UnlockPageService>();
        }
    }
}