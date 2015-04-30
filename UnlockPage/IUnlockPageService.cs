using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SitefinityWebApp.UnlockPage
{
    [ServiceContract]
    public interface IUnlockPageService
    {
        [WebInvoke(Method = "PUT", UriTemplate = "/UnlockPage/?pageId={pageId}")]
        [OperationContract]
        void UnlockPage(Guid pageId);

        [WebInvoke(Method = "PUT", UriTemplate = "/UnlockPageByTitle/?pageTitle={pageTitle}&culture={culture}")]
        [OperationContract(Name = "UnlockPageByTitle")]
        void UnlockPage(string pageTitle, string culture);
    }
}
