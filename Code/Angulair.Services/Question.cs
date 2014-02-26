using System;
using System.Collections.Generic;

namespace Services
{
    public class Question
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }

        public bool Archived { get; set; }
    }

    public interface IQuestionRepository
    {
        Question GetById(string id);
        void Save(Question question);
        IEnumerable<Question> GetAll();

        int VoteUp(string id);
        int VoteDown(string id);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private static Dictionary<string, Question> _store = new Dictionary<string, Question>()
        {
           {"default", new Question() {Id="default",Content = "Are you going to drink tonight?"}}
        };
        public Question GetById(string id)
        {
            if (_store.ContainsKey(id)) return _store[id];
            throw new KeyNotFoundException(id);
        }

        public void Save(Question question)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (_store.ContainsKey(question.Id))
                _store[question.Id] = question;
            else
                _store.Add(question.Id, question);
        }

        public IEnumerable<Question> GetAll()
        {
            return _store.Values;
        }

        public int VoteUp(string id)
        {
            var question = GetById(id);
            question.VoteUp++;
            Save(question);
            return question.VoteUp;
        }

        public int VoteDown(string id)
        {
            var question = GetById(id);
            question.VoteDown++;
            Save(question);
            return question.VoteDown;
        }
    }
    
}