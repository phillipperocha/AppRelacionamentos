namespace AppRelacionamentos.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        // 1- Para guardar nossa senha no banco a guardaremos como um PasswordHash
        public byte[] PasswordHash { get; set; }
        // Nosso passwordSalt vai ser como uma chave, para ser possível recriar o Hash e comparar com a senha que o usuário digitar
        public byte[] PasswordSalt { get; set; }
        
    }
}