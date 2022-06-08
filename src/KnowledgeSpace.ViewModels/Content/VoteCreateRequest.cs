using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels.Content
{
    public class VoteCreateRequest
    {
        public int KnowledgeBaseId { get; set; }
        public string UserId { get; set; }
    }
}
