namespace Angulair.Services
{
    public class Question
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }

        public bool Archived { get; set; }
    }
}