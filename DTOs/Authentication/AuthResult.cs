namespace JobWellTest.DTOs.Authentication
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public AuthResult()
        {
            Errors = new List<string>();
        }
    }
}
