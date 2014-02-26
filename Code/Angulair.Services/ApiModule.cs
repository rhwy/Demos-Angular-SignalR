using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Provider;
using Nancy;
using Nancy.ModelBinding;

namespace Services
{
    
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