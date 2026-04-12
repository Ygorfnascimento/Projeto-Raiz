namespace Raiz.ViewModels
{
    public class UsuarioViewModel
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required IEnumerable<string> Roles { get; set; }
    }
}