using System;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Core.Scoping;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace ContentProtector.Controllers
{
    [PluginController(ContentProtectorSettings.PluginAreaName)]
    [Tree(Constants.Applications.Settings, treeAlias: ContentProtectorSettings.Alias, TreeTitle = ContentProtectorSettings.TreeTitle, TreeGroup = Constants.Trees.Groups.ThirdParty, SortOrder = 5)]
    public class ContentProtectorTreeController : TreeController
    {
        private readonly IScopeProvider _scopeProvider;
        public ContentProtectorTreeController(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            // check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {

                // create our node collection
                var nodes = new TreeNodeCollection();

                TreeNode save = CreateTreeNode("1", "-1", queryStrings, "Save", "icon-save", false);
                save.MenuUrl = null;

                TreeNode publish = CreateTreeNode("6", "-1", queryStrings, "Publish", "icon-umb-deploy", false);
                publish.MenuUrl = null;

                TreeNode unpublished = CreateTreeNode("7", "-1", queryStrings, "Unpublish", "icon-download-alt", false);
                unpublished.MenuUrl = null;

                TreeNode trash = CreateTreeNode("3", "-1", queryStrings, "Trash", "icon-trash", false);
                trash.MenuUrl = null;

                TreeNode delete = CreateTreeNode("4", "-1", queryStrings, "Delete", "icon-trash-alt", false);
                delete.MenuUrl = null;

                TreeNode rollback = CreateTreeNode("8", "-1", queryStrings, "RollBack", "icon-refresh", false);
                rollback.MenuUrl = null;

                nodes.Add(save);
                nodes.Add(publish);
                nodes.Add(unpublished);
                nodes.Add(trash);
                nodes.Add(delete);
                nodes.Add(rollback);
                return nodes;
            }

            // this tree doesn't support rendering more than 1 level
            throw new NotSupportedException();
        }

        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            TreeNode root = base.CreateRootNode(queryStrings);
            //optionally setting a routepath would allow you to load in a custom UI instead of the usual behaviour for a tree
            root.RoutePath = string.Format("{0}/{1}/{2}", Constants.Applications.Settings, ContentProtectorSettings.Alias, "content");
            // set the icon
            root.Icon = "icon-shield color-green";
            // set to false for a custom tree with a single node.
            root.HasChildren = true;
            //url for menu
            root.MenuUrl = null;

            return root;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            // create a Menu Item Collection to return so people can interact with the nodes in your tree
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                // root actions, perhaps users can create new items in this tree, or perhaps it's not a content tree, it might be a read only tree, or each node item might represent something entirely different...
                // add your menu item actions or custom ActionMenuItems
                menu.Items.Add(new CreateChildEntity(Services.TextService));
                // add refresh menu item (note no dialog)
                menu.Items.Add(new RefreshNode(Services.TextService, true));
                return menu;
            }
            return menu;
        }
    }
}