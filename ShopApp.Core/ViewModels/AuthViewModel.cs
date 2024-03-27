namespace ShopApp.Core.ViewModels
{
    public class AuthViewModel
    {
        public string Token { get; set; } = string.Empty;   

        public bool Result { get; set; }

        public List<string> Errors{ get; set; }=new List<string>();
    }
}
