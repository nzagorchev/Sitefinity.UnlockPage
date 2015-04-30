using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Config;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Widgets;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;

namespace SitefinityWebApp.UnlockPage
{
    public class UnlockPageHelper
    {
        public virtual void InstallExtensionScript()
        {
            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var masterView = this.GetPagesMasterView(configManager, out config);
                var script = masterView.Scripts.Elements
                    .Where(e => e.ScriptLocation == "~/UnlockPage/UnlockPageExtensionScript.js")
                    .FirstOrDefault();

                if (script == null)
                {
                    var clientScript = new ClientScriptElement(masterView.Scripts);
                    clientScript.ScriptLocation = "~/UnlockPage/UnlockPageExtensionScript.js";
                    clientScript.LoadMethodName = "pagesMasterViewLoadedExtended";
                    clientScript.CollectionItemName = "script";
                    masterView.Scripts.Add(clientScript);

                    configManager.SaveSection(config);
                }
            }
        }

        public virtual void InstallCommandWidgets()
        {
            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var masterView = this.GetPagesMasterView(configManager, out config);

                // TreeTable
                var treeTableElement = masterView.ViewModesConfig.Elements.FirstOrDefault(vm => vm.Name == "TreeTable");
                var treeTableModelElement = treeTableElement as GridViewModeElement;
                var actionsLinkColumn = treeTableModelElement.ColumnsConfig.Elements.FirstOrDefault(c => c.Name == "ActionsLinkText");
                var actionsMenuElement = actionsLinkColumn as ActionMenuColumnElement;

                bool needSaveChanges = false;

                var command = actionsMenuElement.MenuItems.Elements
                    .Where(e => e.WidgetType == typeof(CommandWidget)
                        && ((CommandWidgetElement)e).CommandName == "unlockPageCustom")
                    .FirstOrDefault();

                if (command == null)
                {
                    var commandWidget = this.CreateUnlockPageCommandWidgetElement(actionsMenuElement);
                    actionsMenuElement.MenuItems.Add(commandWidget);
                    needSaveChanges = true;
                }

                // ListView
                var gridElement = masterView.ViewModesConfig.Elements.FirstOrDefault(vm => vm.Name == "Grid");
                var gridModelElement = gridElement as GridViewModeElement;
                var actionsLinkColumnList = gridModelElement.ColumnsConfig.Elements.FirstOrDefault(c => c.Name == "ActionsLinkText");
                var actionsMenuElementList = actionsLinkColumnList as ActionMenuColumnElement;

                var commandList = actionsMenuElementList.MenuItems.Elements
                    .Where(e => e.WidgetType == typeof(CommandWidget)
                        && ((CommandWidgetElement)e).CommandName == "unlockPageCustom")
                    .FirstOrDefault();

                if (commandList == null)
                {
                    var commandWidgetList = this.CreateUnlockPageCommandWidgetElement(actionsMenuElementList);
                    actionsMenuElementList.MenuItems.Add(commandWidgetList);
                    needSaveChanges = true;
                }

                if (needSaveChanges)
                {
                    configManager.SaveSection(config);
                }
            }
        }

        public virtual void RegisterService<T>() where T : IUnlockPageService
        {
            SystemManager.RegisterWebService(typeof(T),
"Sitefinity/Services/Pages/UnlockPageService.svc/");
        }

        internal protected virtual MasterGridViewElement GetPagesMasterView(ConfigManager configManager, out ContentViewConfig config)
        {
            config = configManager.GetSection<ContentViewConfig>();
            var contentBackend = config.ContentViewControls["FrontendPages"];
            var frontendPagesListView = contentBackend.ViewsConfig.Values.FirstOrDefault(v => v.ViewName == "FrontendPagesListView");
            var masterView = frontendPagesListView as MasterGridViewElement;

            return masterView;
        }

        protected virtual CommandWidgetElement CreateUnlockPageCommandWidgetElement(ActionMenuColumnElement parent)
        {
            var commandWidget = new CommandWidgetElement(parent.MenuItems);
            commandWidget.CommandArgument = "";
            commandWidget.CommandName = "unlockPageCustom";
            commandWidget.Name = "UnlockPage";
            commandWidget.Text = "Unlock Page";
            commandWidget.WrapperTagKey = HtmlTextWriterTag.Li;
            commandWidget.ButtonType = Telerik.Sitefinity.Web.UI.Backend.Elements.Enums.CommandButtonType.Standard;
            commandWidget.WidgetType = typeof(CommandWidget);

            return commandWidget;
        }
    }
}