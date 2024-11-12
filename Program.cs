using System.Xml.Serialization;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

// new option picker
var db = new DataContext();
var blogs = new List<Blog>();
var posts = new List<Post>();

while (true)
{

  Console.WriteLine("\nChoose an option:");
  Console.WriteLine("1. Display all blogs");
  Console.WriteLine("2. Add Blog");
  Console.WriteLine("3. Create Post");
  Console.WriteLine("4. Display Post");
  var choice = Console.ReadLine();

  
  if (choice == "1")
  {

    // Display all Blogs from the database
    var query = db.Blogs.OrderBy(b => b.Name);
    Console.WriteLine("All blogs in the database:");
    foreach (var item in query)
    {
      Console.WriteLine(item.Name);
    }
  }

  else if (choice == "2")
  {

    // Create and save a new Blog
    Console.Write("Enter a name for a new Blog: ");
    var name = Console.ReadLine();
    var blog = new Blog { Name = name };
    
    
    db.AddBlog(blog);
    logger.Info("Blog added - {name}", name);
    Console.WriteLine($"Blog '{name} added successfully.");
  }

  else if (choice == "3")
  {
    if (blogs.Count == 0)
        {
            Console.WriteLine("No blogs available. Add a blog first pretty please.");
            continue;
        }
        Console.WriteLine($"Select the blog ID to post to:");
        foreach (var blog in blogs)
        {
          Console.WriteLine($"{blog.BlogId}. {blog.Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int blogId) || blogs.Find(b => b.BlogId == blogId) == null)
        {
          Console.WriteLine("Invalid blog ID. Please try again.");
          continue;
        }

        Console.Write("Enter post title: ");
        var title = Console.ReadLine();
        Console.Write("Enter post content: ");
        var content = Console.ReadLine();
    
        var post = new Post { PostId = posts.Count + 1, Title = title, Content = content, BlogId = blogId};
        posts.Add(post);
        logger.Info("Post added to blog ID {blogId} - {title}", blogId, title);
        Console.WriteLine("Post added successfully.");
  }

  else if (choice == "4") 
  {

  }
}

logger.Info("Program ended");