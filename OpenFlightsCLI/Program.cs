using System;
using System.Linq;
using FileHelpers;
using Mono.Terminal;
using Terminal.Gui;

using OpenFlightsCLI.Models;
using OpenFlightsCLI.Views.Menus;
using OpenFlightsCLI.WindowControllers;

namespace OpenFlightsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupUI();
            Application.Run();
        }

        static void SetupUI()
        {
            Application.Init();
            var top = Application.Top;
            var terminalFrame = top.Frame;

            var window = new Window(new Rect(0, 1, terminalFrame.Width, terminalFrame.Height - 1), "Cloud Workshop Data Populator 5000");

            //Create MenuBar
            var menuBuilder = new MenuBarBuilder();
            var menu = menuBuilder.BuildMenuBar();

            //Create default viewdia
            var searchWindowController = new SearchWindowController();

            // Add views to window
            top.Add(searchWindowController.Window);
            top.Add(menu);
        }

    }
}
