using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;

namespace HTTPMediaPlayerCore.Models
{
  public class PageContainerMain
  {

    public List<Category> Categories { get; private set; }

    public List<Author> RecommendedAuthors { get; private set; }

    public List<AuthorCourse> BestSellers { get; private set; }
    public List<AuthorCourse> RecommendedCourses{ get; private set; }

    public PageContainerMain(List<Category> categories, List<Author> recommendedAuthors, List<AuthorCourse> bestSellers, List<AuthorCourse> recommendedCourses)
    {
      Categories = categories;
      BestSellers = bestSellers;
      RecommendedAuthors = recommendedAuthors;
      RecommendedCourses = recommendedCourses;
    }
  }
}
