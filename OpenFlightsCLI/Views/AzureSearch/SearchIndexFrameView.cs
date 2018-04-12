using Terminal.Gui;

namespace OpenFlightsCLI.Views
{
    class SearchIndexFrameView : FrameView
    {
        bool exists;
        Label lblExists;
        int itemCount;
        Label lblItemCount;
        bool replaceExisting;
        CheckBox chkReplaceExisting;
        Button btnPopulate;

        public SearchIndexFrameView(Rect frame, NStack.ustring title) : base(frame, title)
        {
            lblExists = new Label(1, 0, "Index Already Exists: Uknown");
            lblItemCount = new Label(1, 1, $"{title}: Uknown");
            chkReplaceExisting = new CheckBox(1, 4, "Replace Existing");
            btnPopulate = new Button(8, 6, "Populate");
            btnPopulate.Clicked += () => { PopulateClicked(); };

            Add(lblExists, lblItemCount, chkReplaceExisting, btnPopulate);
        }

        public bool Exists
        {
            get
            {
                return exists;
            }
            set
            {
                exists = value;
                lblExists.Text = value == true ? (NStack.ustring)"Index Already Exists: Yes" : (NStack.ustring)"Index Already Exists: No";
            }
        }

        public int ItemCount
        {
            get
            {
                return itemCount;
            }
            set
            {
                itemCount = value;
                lblItemCount.Text = $"Count {itemCount}";
            }
        }

        public bool ReplaceExisting
        {
            get
            {
                return chkReplaceExisting.Checked;
            }
        }

        //Events
        public delegate void PopulateClickedHandler();
        public event PopulateClickedHandler PopulateClicked;

    }
}
