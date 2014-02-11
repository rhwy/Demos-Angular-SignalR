using System.Collections.Generic;

namespace Angulair.Services
{
    public interface IQuestionRepository
    {
        Question GetById(string id);
        void Save(Question question);
        IEnumerable<Question> GetAll();

        int VoteUp(string id);
        int VoteDown(string id);
    }
}