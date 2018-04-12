using System;
using Terminal.Gui;

namespace OpenFlightsCLI.Views.Menus
{
    public class MenuBarBuilder
    {
        public MenuBar BuildMenuBar()
        {
            return new MenuBar(new MenuBarItem[] {

            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", NewFile),
                new MenuItem ("_Open", "", null),
                new MenuItem ("_Close", "", () => Close ()),
                new MenuItem ("_Quit", "", () => { if (Quit ()) Application.Top.Running = false; })
            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
                }),
            });
        }

        //Menu Item Implementations
        void NewFile()
        {
            var d = new Dialog(
                "New File", 50, 20,
                new Button("Ok", is_default: true) { Clicked = () => { Application.RequestStop(); } },
                new Button("Cancel") { Clicked = () => { Application.RequestStop(); } });

            Application.Run(d);
        }

        bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Populator 5000", "Are you sure you want to quit this app?", "Yes", "No");
            return n == 0;
        }

        void Close()
        {
            MessageBox.ErrorQuery(50, 5, "Error", "There is nothing to close", "Ok");
        }

    }
}
