//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTTPMediaPlayer.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class vd_CourseLessons
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public int OrderNumber { get; set; }
        public Nullable<bool> IsForTrial { get; set; }
    
        public virtual vd_Lessons vd_Lessons { get; set; }
        public virtual vd_Courses vd_Courses { get; set; }
    }
}
