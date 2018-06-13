namespace TestMakerFreeWebApp.ViewModels
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptOut)]
    public class UserViewModel
    {
        public UserViewModel()
        {

        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}