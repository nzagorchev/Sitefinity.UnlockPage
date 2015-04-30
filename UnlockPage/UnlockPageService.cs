using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Web.Services;

namespace SitefinityWebApp.UnlockPage
{
    [ServiceBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UnlockPageService : IUnlockPageService
    {
        public void UnlockPage(string pageTitle, string culture)
        {
            throw new NotImplementedException();
        }

        public virtual void UnlockPage(Guid pageId)
        {
            ServiceUtility.RequestBackendUserAuthentication();

            var manager = PageManager.GetManager();
            // check for user
            bool canUserUnlockPage = true;

            if (canUserUnlockPage)
            {
                using (new ElevatedModeRegion(manager))
                {
                    var pageNode = manager.GetPageNode(pageId);
                    if (pageNode != null)
                    {
                        var pageData = pageNode.GetPageData();

                        if (pageData != null)
                        {
                            pageData.LockedBy = Guid.Empty;
                            manager.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}