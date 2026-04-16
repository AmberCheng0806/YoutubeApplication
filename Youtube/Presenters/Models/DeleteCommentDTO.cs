using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    public class DeleteCommentDTO
    {
        public string CommentId { get; set; }
        public int ReplyCount { get; set; }
        public string ParentId { get; set; }

        public DeleteCommentDTO(string commentId, int replyCount, string parentId)
        {
            CommentId = commentId;
            ReplyCount = replyCount;
            ParentId = parentId;
        }
    }
}
