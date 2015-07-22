//------------------------------------------------------------------------------
// <copyright file="CreateGist.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Extensions.Gists.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.ComponentModel.Design;
using System.IO;

namespace Microsoft.VisualStudio.Extensions.Gists
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CreateGist
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("d4d5b28e-8f6a-4637-b652-a1b45cf60c7f");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGist"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CreateGist(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CreateGist Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this.package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new CreateGist(package);
        }

        private string GetTextFromEditor(bool selectedTextOnly = true)
        {
            var textManager = this.ServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
            IVsTextView textView = null;
            int mustHaveFocus = 1;
            textManager.GetActiveView(mustHaveFocus, null, out textView);

            var userData = textView as IVsUserData;
            if (userData == null)
            {
                // no text view is currently open
                return String.Empty;
            }
            else
            {
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                object holder;
                userData.GetData(ref guidViewHost, out holder);
                IWpfTextViewHost viewHost = (IWpfTextViewHost)holder;
                if (selectedTextOnly)
                {
                    return viewHost.TextView.Selection.SelectedSpans[0].GetText();
                }
                else
                {
                    return viewHost.TextView.TextSnapshot.GetText();
                }
            }
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void MenuItemCallback(object sender, EventArgs e)
        {
            var currentFilename = GetCurrentFilenameFromEditor();

            var publishDialog = new PublishGistDialog();
            publishDialog.Filename = Path.GetFileName(currentFilename);

            var result = publishDialog.ShowDialog();
            if (result == true)
            {
                var codeToPublish = GetTextFromEditor(publishDialog.PublishOnlySelection);

                var service = new GistsService();
                var uri = await service.PostNewGistAsync(codeToPublish, publishDialog.Description, publishDialog.Filename, publishDialog.IsPublic);

                var successDialog = new SuccessDialog();
                successDialog.Hyperlink = uri;

                successDialog.ShowDialog();
            }
        }

        private string GetCurrentFilenameFromEditor()
        {
            var textManager = this.ServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
            IVsTextView textView = null;
            int mustHaveFocus = 1;
            textManager.GetActiveView(mustHaveFocus, null, out textView);

            var userData = textView as IVsUserData;
            if (userData == null)
            {
                // no text view is currently open
                return String.Empty;
            }
            else
            {
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                object holder;
                userData.GetData(ref guidViewHost, out holder);
                IWpfTextViewHost viewHost = (IWpfTextViewHost)holder;

                ITextDocument doc;
                viewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out doc);
                return doc.FilePath;
            }
        }
    }
}
