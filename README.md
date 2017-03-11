# Gists
This extension allows you to select code and upload it to GitHub as a [Gist](https://gist.github.com/) for others to view. 

When installed, it will add a Create a New Gist command to the Tools menu. The first time you run it, GitHub will prompt you to approve the application for read/write access to your GitHub repo; once approved, you can use it without further authorization. 

The extension prompts you to either upload a selection or the entire file. Once uploaded, it will provide you with the URL for your Gist so that others can also use it.

Extension is live on the Visual Studio Marketplace here:
https://marketplace.visualstudio.com/items?itemName=TimSneath.GistsforVisualStudio

It's a little hacky in places, but there's some good samples in the code of how to grab a selection from the Visual Studio code editor, how to identify the current filename, and how to create a dialog from within a Visual Studio extension. Hopefully it's of some use to someone. Feel free to submit issue requests or (even better) a pull request to improve it!
