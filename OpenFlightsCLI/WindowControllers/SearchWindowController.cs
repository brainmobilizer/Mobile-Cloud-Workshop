using System;

using NStack;
using Terminal.Gui;

using OpenFlightsCLI.Views;

namespace OpenFlightsCLI.ViewControllers
{
    public class SearchWindowController
    {
        SearchIndexFrameView airportsIndex;
        SearchIndexFrameView airlinesIndex;
        SearchCredentialsFrameView credentialsFrame;
        Window window;

        public SearchWindowController()
        {
            var top = Application.Top;
            var terminalFrame = top.Frame;
            window = new Window(new Rect(0, 1, terminalFrame.Width, terminalFrame.Height - 1), "Cloud Workshop Data Populator 5000");
            AddElements(window);
        }

        public Window Window
        {
            get
            {
                return window;
            }
        }

        void AddElements(View container)
        {
            credentialsFrame = new SearchCredentialsFrameView(new Rect(3, 1, 70, 9), "Azure Search Credentials");
            credentialsFrame.ValidateClicked += ValidateCredentials;


            airlinesIndex = new SearchIndexFrameView(new Rect(3, 10, 34, 9), "Airlines");
            airportsIndex = new SearchIndexFrameView(new Rect(39, 10, 34, 9), "Airports");

            container.Add(
                credentialsFrame,
                airlinesIndex,
                airportsIndex
            );
        }

        void ValidateCredentials()
        {
            if(!string.IsNullOrEmpty(credentialsFrame.SearchName) && !string.IsNullOrEmpty(credentialsFrame.ApiKey))
            {
                var searchService = new Helpers.AzureSearchService<Models.Airline>(credentialsFrame.SearchName, credentialsFrame.ApiKey);
                try
                {
                    var isWorking = searchService.WorkingConnection();
                    if (isWorking == false)
                    {
                        var dialog = new Dialog(
                            "Error: Connection failed", 40, 6,
                           new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                        Application.Run(dialog);

                    }
                    else
                    {
                        var dialog = new Dialog(
                            "Valid Connection!", 40, 6,
                           new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                        Application.Run(dialog);
                    }
                }
                catch(Exception ex)
                {
                    var dialog = new Dialog(
                            ex.Message, 40, 6,
                           new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                    Application.Run(dialog);
                }
            }
            else
            {
                var dialog = new Dialog(
                       "No credentials founds", 40, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }
        }

    }


}
