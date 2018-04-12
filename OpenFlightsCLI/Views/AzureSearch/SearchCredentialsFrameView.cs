using Terminal.Gui;

namespace OpenFlightsCLI.Views
{
    public class SearchCredentialsFrameView : FrameView
    {
        string azureSearchName;
        string azureSearchApiKey;

        // User Entery
        Label lblSearchNameTitle;
        TextField txbSearchName;

        Label lblApiKey;
        TextField tbxApiKey;
        FrameView credentialsView;

        public SearchCredentialsFrameView(Rect frame, NStack.ustring title) : base(frame, title)
        {
            lblSearchNameTitle = new Label(3, 1, "Azure Service Name: ");
            txbSearchName = new TextField(23, 1, 40, "");

            lblApiKey = new Label(12, 3, "Admin Key: ");
            tbxApiKey = new TextField(23, 3, 40, "") { Secret = true };

            txbSearchName.Changed += (sender, e) => { azureSearchName = txbSearchName.Text.ToString(); };
            tbxApiKey.Changed += (sender, e) => { azureSearchApiKey = tbxApiKey.Text.ToString(); };

            var validateButton = new Button(51, 5, "Validate");
            validateButton.Clicked += () => { ValidateClicked(); }; 

            Add(lblSearchNameTitle, txbSearchName, lblApiKey, tbxApiKey, validateButton);

        }

        public string SearchName 
        {
            get
            {
                return azureSearchName;
            }
            set
            {
                azureSearchName = value;
            }
        }

        public string ApiKey
        {
            get
            {
                return azureSearchApiKey;
            }
            set
            {
                azureSearchApiKey = value;
            }
        }


        //Events
        public delegate void ValidateClickedHandler();
        public event ValidateClickedHandler ValidateClicked;


    }
}
