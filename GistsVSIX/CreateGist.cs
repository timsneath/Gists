﻿//------------------------------------------------------------------------------
// <copyright file="CreateGist.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.IO;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

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
            this.package = package ?? throw new ArgumentNullException(nameof(package));

            if (this.ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
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


        private IWpfTextViewHost GetCurrentViewHost()
        {
            var textManager = this.ServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;

            int mustHaveFocus = 1;
            textManager.GetActiveView(mustHaveFocus, null, out IVsTextView textView);

            var userData = textView as IVsUserData;
            if (userData == null)
            {
                return null;
            }
            else
            {
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out object holder);
                var viewHost = (IWpfTextViewHost)holder;

                return viewHost;

            }
        }

        private string GetSelectedTextFromEditor(IWpfTextViewHost viewHost) =>
            viewHost.TextView.Selection.SelectedSpans[0].GetText();

        private string GetAllTextFromEditor(IWpfTextViewHost viewHost) =>
            viewHost.TextView.TextSnapshot.GetText();

        private string GetCurrentFilename(IWpfTextViewHost viewHost)
        {
            viewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument doc);

            return doc.FilePath;
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
            var gistClient = new GistsApi.GistClient(Properties.APIKeys.GistClientID,
                                                     Properties.APIKeys.GistClientSecret,
                                                     "GistsForVisualStudio/1.0");

            var authDialog = new AuthDialog();
            authDialog.webBrowser.Navigate(gistClient.AuthorizeUrl);
            authDialog.ShowDialog();
            if (authDialog.DialogResult == true)
            {
                await gistClient.Authorize(authDialog.authCode);

                var viewHost = GetCurrentViewHost();
                var filename = Path.GetFileName(GetCurrentFilename(viewHost));

                var publishDialog = new PublishGistDialog()
                {
                    Filename = Path.GetFileName(filename)
                };
                if (publishDialog.ShowDialog() == true)
                {
                    string codeToPublish;
                    if (publishDialog.PublishOnlySelection)
                    {
                        codeToPublish = GetSelectedTextFromEditor(viewHost);
                    }
                    else
                    {
                        codeToPublish = GetAllTextFromEditor(viewHost);
                    }

                    filename = publishDialog.Filename;

                    var gistFiles = new[] { Tuple.Create(filename, codeToPublish) };

                    var result = await gistClient.CreateAGist(publishDialog.Description, publishDialog.IsPublic, gistFiles);

                    var successDialog = new SuccessDialog()
                    {
                        Hyperlink = new Uri(result.html_url)
                    };
                    successDialog.ShowDialog();
                }
            }
        }

        private string GetCurrentFilenameFromEditor()
        {
            var textManager = this.ServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
            int mustHaveFocus = 1;
            textManager.GetActiveView(mustHaveFocus, null, out IVsTextView textView);

            var userData = textView as IVsUserData;
            if (userData == null)
            {
                // no text view is currently open
                return String.Empty;
            }
            else
            {
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out object holder);
                IWpfTextViewHost viewHost = (IWpfTextViewHost)holder;

                viewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument doc);
                return doc.FilePath;
            }
        }
    }
}
