using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
       

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    if(user.Role==Model.Role.OWNER)
                    {
                        OwnerOverview ownerOverview = new OwnerOverview();
                        ownerOverview.Show();
                        Close();
                    }else if(user.Role == Model.Role.GUIDE)
                    {
                        GuidesOverview guideOverview = new GuidesOverview();
                        guideOverview.Show();
                        Close();
                    }
                    else if (user.Role == Model.Role.GUEST1)
                    {
                        Guest1Overview guest1Overview = new Guest1Overview();
                        guest1Overview.Show(); 
                        Close();
                    }
                    else if (user.Role == Model.Role.GUEST2)
                    {
                        Guest2Overview guest2Overview = new Guest2Overview();
                        guest2Overview.Show();
                    Close();
                } 

                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
