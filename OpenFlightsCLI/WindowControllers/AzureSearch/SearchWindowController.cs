using System;

using NStack;
using Terminal.Gui;

using OpenFlightsCLI.Views;
using OpenFlightsCLI.Helpers;
using OpenFlightsCLI.Models;
using OpenFlightsCLI.Parsers;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenFlightsCLI.WindowControllers
{
    public class SearchWindowController
    {
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

            container.Add(
                credentialsFrame
            );
        }

        void AddIndexViews(View container)
        {
            var searchService = new Helpers.AzureSearchServiceBase<SearchBaseModel>(credentialsFrame.SearchName, credentialsFrame.ApiKey);
            var airlineIndexExists = searchService.IndexExistsForType<Airline>();
            var airlineCount = searchService.Count<Type>();
            var airportIndexExists = searchService.IndexExistsForType<Airport>();


            airlinesIndex = new SearchIndexFrameView(new Rect(3, 1, 34, 9), "Airlines");
            airlinesIndex.Exists = airlineIndexExists;
            airlinesIndex.PopulateClicked += PopulateAirlines;

            airportsIndex = new SearchIndexFrameView(new Rect(3, 10, 34, 9), "Airports");
            airportsIndex.Exists = airportIndexExists;
            airportsIndex.PopulateClicked += PopulateAirports;

            container.Remove(credentialsFrame);

            container.Add(
               airlinesIndex,
               airportsIndex
           );
        }

        void ValidateCredentials()
        {
            if(!string.IsNullOrEmpty(credentialsFrame.SearchName) && !string.IsNullOrEmpty(credentialsFrame.ApiKey))
            {
                var searchService = new Helpers.AzureSearchServiceBase<Models.Airline>(credentialsFrame.SearchName, credentialsFrame.ApiKey);
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

                        AddIndexViews(window);

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




        void PopulateAirports()
        {
            var searchService = new Helpers.AzureSearchServiceBase<Models.Airport>(credentialsFrame.SearchName, credentialsFrame.ApiKey);
            var indexExists = searchService.IndexExistsForType<Airport>();
            var airportParser = new AirportsParser();


            if(indexExists == true && airportsIndex.ReplaceExisting == false)
            {
                var dialog = new Dialog(
                    "Index already exists. Select Replace", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }

            if (indexExists == false)
            {
                searchService.CreateIndexOfType<Airport>();
            }

            var airports = airportParser.Parse();
            var airportCount = airports.Count;
            var uploaded = searchService.UploadAirports(airports);

            if(uploaded == true)
            {
                var dialog = new Dialog(
                    $"Complete. Uploaded: {airportCount} Airports", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }
            else
            {
                var dialog = new Dialog(
                    "Failed to upload airports", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }
        }

        void PopulateAirlines()
        {
            var searchService = new Helpers.AzureSearchServiceBase<Models.Airline>(credentialsFrame.SearchName, credentialsFrame.ApiKey);
            var indexExists = searchService.IndexExistsForType<Airline>();
            var AirlineParser = new AirlinesParser();


            if (indexExists == true && airportsIndex.ReplaceExisting == false)
            {
                var dialog = new Dialog(
                    "Index already exists. Select Replace", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }

            if (indexExists == false)
            {
                searchService.CreateIndexOfType<Airport>();
            }

            var airlines = AirlineParser.Parse();
            var airlinesCount = airlines.Count;
            var uploaded = searchService.UploadAirlines(airlines);

            if (uploaded == true)
            {
                var dialog = new Dialog(
                    $"Complete. Uploaded: {airlinesCount} Airlines", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }
            else
            {
                var dialog = new Dialog(
                    "Failed to upload airlines", 50, 6,
                      new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } });
                Application.Run(dialog);
            }
            
        }


        //Private Fields
        SearchIndexFrameView airportsIndex;
        SearchIndexFrameView airlinesIndex;
        SearchCredentialsFrameView credentialsFrame;
        Window window;

    }


}
