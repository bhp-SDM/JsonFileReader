using System;

namespace JsonFileReader
{
    public class BEReview
    {
        public int Reviewer { get; set; }
        public int Movie { get; set; }
        public int Grade { get; set; }
        public DateTime ReviewDate { get; set; }

        public override string ToString()
        {
            return string.Format($"Reviewer: {Reviewer}\nMovie: {Movie}\nGrade: {Grade}\nReviewDate: {ReviewDate}\n");
        }
    }
}
