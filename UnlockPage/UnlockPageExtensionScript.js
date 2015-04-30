function pagesMasterViewLoadedExtended(_masterGridView) {
    var currentPageUnlocker = pageUnlocker.get(_masterGridView);
    currentPageUnlocker.initialize();
}

var pageUnlocker = (function ($) {
    // --- public --- //

    function PageUnlocker(masterGridView) {
        if (!masterGridView) {
            console.error("MasterGridView parameter is obligatory and should be different than null or undefined!")
        }
        else {
            // set the masterGridView parameter
            this._masterGridView = masterGridView;
            // set reusable properties
            this._unlockPageServiceUrl = "/Sitefinity/Services/Pages/UnlockPageService.svc/UnlockPage";
            this._commandName = "unlockPageCustom";

            return this;
        }
    }

    // Initializes the PageUnlocker by attaching the listViews event handlers
    PageUnlocker.prototype.initialize = function() {
        var _itemsGrid, _itemsList, _itemsTreeTable;

        _itemsGrid = this._masterGridView.get_itemsGrid();
        _attachEventHandlers(this, _itemsGrid);

        _itemsList = this._masterGridView.get_itemsList();
        _attachEventHandlers(this, _itemsList);

        _itemsTreeTable = this._masterGridView.get_itemsTreeTable();
        _attachEventHandlers(this, _itemsTreeTable);
    }

    // Gets PageUnlocker instance
    function get(masterGridView) {
        var unlocker = new PageUnlocker(masterGridView);
        return unlocker;
    }

    // --- private --- //

    function _attachEventHandlers(sender, itemsElement) {
        var binder;
        binder = itemsElement.getBinder();
        var delegateDataBound = createDelegate(sender, _itemDataBoundExtended);
        binder.add_onItemDataBound(delegateDataBound);
        var delegateCommandHandler = createDelegate(sender, _itemCommandHandlerExtended);
        itemsElement.add_itemCommand(delegateCommandHandler);
    }

    function _itemDataBoundExtended(sender, args) {
        var dataItem, itemElement, masterGridView, commandName;

        dataItem = args.get_dataItem();
        itemElement = args.get_itemElement();
        masterGridView = this._masterGridView;
        commandName = this._commandName;

        _configureMoreActionsMenuExtended(masterGridView, dataItem, itemElement, commandName);
    }

    function _configureMoreActionsMenuExtended(masterGridView, dataItem, itemElement, commandName) {
        var _actionCommandPrefix, list;

        _actionCommandPrefix = "sf_binderCommand_";
        list = masterGridView.get_currentItemsList();

        if (list) {
            if (dataItem.PageLifecycleStatus && dataItem.PageLifecycleStatus.IsLocked == true && dataItem.PageLifecycleStatus.IsAdmin) {
                list.removeActionsMenuItems([_actionCommandPrefix + commandName], itemElement);
            }
            else if (dataItem.PageLifecycleStatus && dataItem.PageLifecycleStatus.IsLocked != true) {
                list.removeActionsMenuItems([_actionCommandPrefix + commandName], itemElement);
            }
        }
    }

    // Checks the command name and calls the unlock service with the data from the event arguments
    function _itemCommandHandlerExtended(sender, args) {
        var commandName, dataItem, id, url, self;

        commandName = args.get_commandName();
        if (commandName == this._commandName) {
            dataItem = args.get_commandArgument();
            id = dataItem.Id;
            url = this._unlockPageServiceUrl + '?pageId=' + id;
            self = this;
            jQuery.ajax({
                type: "PUT",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    self._masterGridView.get_currentItemsList().dataBind();
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    }

    // From Microsoft.Ajax - Function.createDelegate
    function createDelegate(a, b) {
        return function () { return b.apply(a, arguments) }
    }

    // --- revealed --- //
    return {
        get: get,
    }

}(jQuery));