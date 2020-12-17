using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;



namespace HTTPMediaPlayerCore.Models
{
  public class DuwaysContext : DbContext
  {

    public DuwaysContext(DbContextOptions<DuwaysContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"server=18.156.26.156;Database=friendsmasters;user=duser;password=V$67sl,R8q23");
    }

    public DbSet<ActionLog> ActionLog { get; set; }
    public DbSet<ActionType> ActionType { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<CourseLesson> CourseLesson { get; set; }
    public DbSet<Course> Course { get; set; }
    public DbSet<File> File { get; set; }
    public DbSet<Lesson> Lesson { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<UserCourse> UserCourse { get; set; }
    public DbSet<UserLesson> UserLesson { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<AuthorCourse> AuthorCourse { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Author> Author { get; set; }
    public DbSet<Filter> Filter { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ActionType>()
      .ToTable("vd_ActionType"); 
      
      modelBuilder.Entity<ActionLog>()
      .ToTable("vd_ActionLog");

      modelBuilder.Entity<CourseLesson>()
      .ToTable("vd_CourseLessons");

      modelBuilder.Entity<File>()
      .ToTable("vd_Files");

      modelBuilder.Entity<Lesson>()
      .ToTable("vd_Lessons");

      modelBuilder.Entity<Log>()
      .ToTable("vd_Log");

      modelBuilder.Entity<Order>()
      .ToTable("vd_Orders");

      modelBuilder.Entity<UserLesson>()
      .ToTable("vd_UserLessons");

      modelBuilder.Entity<Course>()
      .ToTable("vd_Courses");

      modelBuilder.Entity<Country>()
      .ToTable("vd_Countries");

      modelBuilder.Entity<User>()
      .ToTable("vd_Users");

      modelBuilder.Entity<UserCourse>()
      .ToTable("vd_UserCourses");

      modelBuilder.Entity<AuthorCourse>()
      .ToTable("vd_AuthorCourses");

      modelBuilder.Entity<Category>()
      .ToTable("vd_Category");

      modelBuilder.Entity<Author>()
      .ToTable("vd_Authors"); 
      
      modelBuilder.Entity<Filter>()
      .ToTable("vd_Filters");

    }

  }

  public class ActionLog
  {
    [Key]
    public int Id { get; set; }
    public int? SessionId { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string Remarks { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int ActionTypeId { get; set; }
    public ActionType ActionType { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
  }

  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlName { get; set; }
    public string Surname { get; set; }
    public string LoginName { get; set; }
    public string Password { get; set; }
    public DateTime? BirthDate { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string Email { get; set; }
    public bool IsAuthor { get; set; }
    public bool IsAdmin { get; set; }
    public int? CountryId { get; set; }
    public  Country Country { get; set; }

    [Column(TypeName = "NVARCHAR(500)")]
    public string Address { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string City { get; set; }
    
    public bool IsActivated { get; set; }
    
    public Guid Token { get; set; }
    
    public DateTime? RegistrationDate { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string Phone { get; set; }
    public List<UserCourse> UserCourses { get; } 
    public List<UserLesson> UserLessons { get; } 
    public List<AuthorCourse> AuthorCourses { get; } 
    public List<Order> Orders { get; set; } 
    public List<Log> Logs { get; set; } 
    public List<ActionLog> ActionLogs { get; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string Title { get; set; }

    [Column(TypeName = "NVARCHAR(500)")]
    public string Description { get; set; }

  }

  public class Filter
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(20)")]
    public string Name { get; set; }
  }

  public class Author
  {
    private ILazyLoader LazyLoader { get; set; }

    public Author(ILazyLoader lazyLoader)
    {
      LazyLoader = lazyLoader;
    }

    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string AuthorUrl { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string WorkShopName { get; set; }

    private List<AuthorCourse> _authorCourses;

    public List<AuthorCourse> AuthorCourses
    {
      get => LazyLoader.Load(this, ref _authorCourses);
      set => _authorCourses = value;
    }

    public int UserId { get; set; }

    private User _user;
    public User User
    {
      get => LazyLoader.Load(this, ref _user);
      set => _user = value;
    }

    [Column(TypeName = "NVARCHAR(100)")]
    public string Title { get; set; }

    [Column(TypeName = "NVARCHAR(500)")]
    public string Description { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string PersonalPageTitle { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string PersonalPageSlogan { get; set; }

    [Column(TypeName = "NVARCHAR(20)")]
    public string PersonalPageHeaderPic { get; set; }
    
    [Column(TypeName = "NVARCHAR(MAX)")]
    public string PersonalPageHTML { get; set; }

    public int? FilterId { get; set; }

    private Filter _filter;

    public Filter Filter
    {
      get => LazyLoader.Load(this, ref _filter);
      set => _filter = value;
    }

    [Column(TypeName = "NVARCHAR(100)")]
    public string Instagram { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string Facebook { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string VK { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string YouTube { get; set; }

    public bool? HasOwnRobokassa { get; set; }
    
    public bool? IsPayingRobokassaFee { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string RobokassaShopId { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string RobokassaPassword1 { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string RobokassaPassword2 { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string RobokassaTestPassword1 { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string RobokassaTestPassword2 { get; set; }

    public bool? HasContactForm { get; set; }

    [Column(TypeName = "NVARCHAR(20)")]
    public string ContactFormHeaderText { get; set; }

    [Column(TypeName = "NVARCHAR(30)")]
    public string ContactFormButtonText { get; set; }
    
    [Column(TypeName = "NVARCHAR(50)")]
    public string ContactFormMessageBoxText { get; set; }

  }

  public class ActionType
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string ActionName { get; set; }
    public List<ActionLog> ActionLogs { get; } 
  }

  public class Country
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(200)")]
    [Required]
    public string RusName { get; set; }


    [Column(TypeName = "NVARCHAR(200)")]
    public string EngName { get; set; }

    [Column(TypeName = "NVARCHAR(10)")]
    public string Alpha2 { get; set; }

    [Column(TypeName = "NVARCHAR(10)")]
    public string Alpha3 { get; set; }

    [Column(TypeName = "NVARCHAR(10)")]
    public string ISO { get; set; }
    public List<User> Users { get; set; } 

  }

  public class CourseLesson
  {
    [Key]
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public bool? IsForTrial { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
  }

  public class AuthorCourse
  {
    public AuthorCourse()
    {
    }

    public  string GetCoursePageHtml()
    {
      string html = "";

      string path = @"c:\Duways\Pages\" + AuthorId + "\\" + CourseId + "\\" + "main.html";
      // Open the file to read from.
      if (System.IO.File.Exists(path))
      {
        html =  System.IO.File.ReadAllText(path);
      }

      return html;
    }

    public AuthorCourse(ILazyLoader lazyLoader)
    {
      LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; }

    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }


    [Column(TypeName = "NVARCHAR(50)")]
    public string CourseName { get; set; }

    private Course _course;

    public Course Course
    {
      get => LazyLoader.Load(this, ref _course);
      set => _course = value;
    }

    public int AuthorId { get; set; }

    private Author _author;

    public Author Author
    {
      get => LazyLoader.Load(this, ref _author);
      set => _author = value;
    }
  }

  public class Category
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string CategoryName { get; set; }
  }

  public class Course
  {
    public Course(ILazyLoader lazyLoader)
    {
      LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; }

    private Category _category;
    public Category Category
    {
      get => LazyLoader.Load(this, ref _category);
      set => _category = value;
    }

    [Key]
    public int Id { get; set; }
    public int? CategoryId { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string Name { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string UrlName { get; set; }

    [Column(TypeName = "NVARCHAR(4000)")]
    [Required]
    public string Description { get; set; }

    [Column(TypeName = "NVARCHAR(400)")]
    public string ShortDescription { get; set; }
    public int? OrderNumber { get; set; }
    public int Price { get; set; }
    public bool? IsForBeginners { get; set; }
    public bool? IsForIntermediate { get; set; }
    public bool? IsForAdvanced { get; set; }
    public bool? IsForKids { get; set; }
    public bool? IsForAdults { get; set; }
    public int? FullCourseId { get; set; }

    [Column(TypeName = "NVARCHAR(4000)")]
    public string ForWhom { get; set; }

    [Column(TypeName = "NVARCHAR(4000)")]
    public string Contents { get; set; }

    [Column(TypeName = "NVARCHAR(4000)")]
    public string Info { get; set; }
    public bool? HasTrial { get; set; }
    public bool? IsBook { get; set; }
    public bool? HasDownloadItem { get; set; }
    public int? PriceDownloadItem { get; set; }

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string PageText { get; set; }
    public bool? IsLanguageCourse { get; set; }
    public bool? IsWritingCourse { get; set; }
    public bool? IsPackage { get; set; }
    public int? ParentCourseId { get; set; }
    public bool? HasPackages { get; set; }
    public DateTime? ActivationDate { get; set; }
    public int? ActivationFreq { get; set; }
    public List<CourseLesson> CourseLessons { get; set; } = new List<CourseLesson>();
    public List<Order> Orders { get; set; } 
    public List<UserCourse> UserCourses { get; set; }
    public List<UserLesson> UserLessons { get; set; }
    public int AuthorId { get; set; }
    //public Author Author { get; set; }
    public int? LessonNumber { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string LessonName { get; set; }

    public bool? IsCustomized { get; set; }

    public int? FilterId { get; set; }

    private Filter _filter;
    public Filter Filter
    {
      get => LazyLoader.Load(this, ref _filter);
      set => _filter = value;
    }
  }

  public class File
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string FileName { get; set; }

    [Column(TypeName = "NVARCHAR(1000)")]
    public string Url { get; set; }

    [Column(TypeName = "NVARCHAR(4)")]
    public string Ext { get; set; }
    public List<Lesson> Lessons { get; set; } 
  }

  public class Lesson
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string Name { get; set; }

    [Column(TypeName = "NVARCHAR(1000)")]
    [Required]
    public string Description { get; set; }
    public int FileId { get; set; }
    public int? AuthorId { get; set; }

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string Text { get; set; }
    public List<CourseLesson> CourseLessons { get; set; } 
    public File File { get; set; }
    public List<UserLesson> UserLessons { get; set; } 

    public bool? IsCustomized { get; set; }
  }

  public class Log
  {
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginDT { get; set; }
    public DateTime? LogoutDT { get; set; }
    public User User { get; set; }
  }

  public class Order
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(200)")]
    public string Text { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreationDateTime { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaymentDateTime { get; set; }

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string ResultResponse { get; set; }
    public decimal Sum { get; set; }
    public decimal? IncSum { get; set; }
    public decimal? Fee { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string SignatureValue { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string PaymentMethod { get; set; }
    public bool? IsTest { get; set; }

    [Column(TypeName = "NVARCHAR(50)")]
    public string EMail { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
    public int? UserCourseId { get; set; }
    public int? AuthorId { get; set; }
    public UserCourse UserCourse { get; set; }
    public int? CourseId { get; set; }
    public Course Course { get; set; }
  }

  public class UserCourse
  {
    [Key]
    public int Id { get; set; }
    public bool IsActivated { get; set; }
    public bool? IsPaid { get; set; }
    public DateTime? SubscriptionDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Order> Orders { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int AuthorId { get; set; }

  }

  public class UserLesson
  {
    [Key]
    public int Id { get; set; }
    public Guid UniqueId { get; set; }
    public DateTime? ActivationDate { get; set; }
    public bool? IsReadByUser { get; set; }
    public bool? IsLinkSent { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
  }

  
}