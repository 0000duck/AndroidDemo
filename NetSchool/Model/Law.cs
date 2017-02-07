using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Model
{
    public partial class Law
    {
        public Law() {
            lawID = Guid.Empty;
            title = string.Empty;
            content = string.Empty;
            author = string.Empty;
            editor = string.Empty;
            attachment = string.Empty;
        }
        private Guid lawID;
        private string title;
        private string content;
        private int type;
        private DateTime createTime;
        private string author;
        private int viewNum;
        private string editor;
        private DateTime editTime;
        private string attachment;
        public Guid LawID
        {
            get { return lawID; }
            set { lawID = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public int ViewNum
        {
            get { return viewNum; }
            set { viewNum = value; }
        }
        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }
        public DateTime EditTime
        {
            get { return editTime; }
            set { editTime = value; }
        }
        public string Attachment
        {
            get { return attachment; }
            set { attachment = value; }
        }

    }
}
