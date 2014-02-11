using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Nancy;
using Nancy.ModelBinding;

namespace Angulair.Services
{
    [HubName("votes")]
    public class VoteHub : Hub
    {
        private IQuestionRepository repo;

        public VoteHub(IQuestionRepository repo)
        {
            this.repo = repo;
        }

        public void VoteUp(string id)
        {
            int current = repo.VoteUp(id);
            Clients.All.updateQuestionVoteUp(id, current);
        }

        public void VoteDown(string id)
        {
            int current = repo.VoteDown(id);
            Clients.All.updateQuestionVoteDown(id, current);
        }


    }


    public class ChatHub : Hub
    {

        public void Send(string name, string message)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new HubException("sender name can't not be empty");
            }
            Clients.Others.broadcastMessage(name, message);
            Clients.Caller.sent();
        }
    }



    public class ApiModule : NancyModule
    {
        private readonly IQuestionRepository questions;

        public ApiModule(IQuestionRepository questions)
        {
            this.questions = questions;

            Get["/api/question/{id}"] = p =>
            {
                Question question = questions.GetById(p.id);
                return Negotiate.WithModel(question).WithStatusCode(200);
            };

            Post["/api/question"] = p =>
            {
                Question question = this.Bind<Question>();
                if (question != null)
                {
                    questions.Save(question);
                    GlobalHost
                        .ConnectionManager.GetHubContext<VoteHub>()
                        .Clients.All.updateQuestionText(question.Content);

                    return Negotiate.WithModel(question).WithStatusCode(HttpStatusCode.Created);
                }
                return Negotiate.WithStatusCode(HttpStatusCode.BadRequest);
            };

            Get["/api/question"] = p => Negotiate.WithModel(questions.GetAll()).WithStatusCode(HttpStatusCode.OK);

            Post["/api/question/{id}/up"] = p =>
            {
                int value = questions.VoteUp(p.id);
                return Negotiate.WithModel(value).WithStatusCode(HttpStatusCode.OK);
            };

            Get["/api/question/{id}/up"] = p =>
            {
                Question question = questions.GetById(p.id);
                return Negotiate.WithModel(question.VoteUp).WithStatusCode(HttpStatusCode.OK);
            };
            Post["/api/question/{id}/down"] = p =>
            {
                int value = questions.VoteDown(p.id);
                return Negotiate.WithModel(value).WithStatusCode(HttpStatusCode.OK);
            };
            Get["/api/question/{id}/down"] = p =>
            {
                Question question = questions.GetById(p.id);
                return Negotiate.WithModel(question.VoteDown).WithStatusCode(HttpStatusCode.OK);
            };
        }
    }

}

/* test it:
GET /api/question
POST /api/question
setup content-type application/json + raw body:
	{
	    "id": "lovejs",
	    "content": "Aimez-vous le javascript?",
	    "voteUp": 0,
	    "voteDown": 0
	}
	//then:
	{
    "id": "html5",
    "content": "html5/Js c\u0027est mieux que silverlight?",
    "voteUp": 0,
    "voteDown": 0
	}

POST /api/question/default/up

*/