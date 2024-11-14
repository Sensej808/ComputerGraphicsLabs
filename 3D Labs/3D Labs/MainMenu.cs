namespace _3D_Labs
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void OpenLab6(object sender, EventArgs e)
        {
            FormLab6 formLab6 = new FormLab6();
            formLab6.Show();
        }
    }
}