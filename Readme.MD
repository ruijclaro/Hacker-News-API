Hi All.

For starters this one was a tricky one, due to the fact that some underlying questions arose to which I had no answers.
The job was simple though, I had to map an existing service and return a tranformed version of it's data.

I had to follow the following assumptions:

  1.  I did not know how and when information from the HackerNews API was subject to change;
      What I did for the users to get full control of this Service was to provide them with parameters on the AppSettings to manipulate caching as they would see fit.
  
  2.  I used LazyCache. Its a simple yet powerfull implementation of Microsoft Caching given by .NET Core
      This implementation has one drawback, the service has to be on just one server, as it's rely on memory caching.
      If further caching solutions are needed, ie: a farm of servers I would use Redis on Azure to implement caching.
      However that change is pretty simple.
      
  3.  ASP.Net Core 2.2 is out of production. I would rather use .Net 5 to build this project if I had no specific guidelines to work with.
      Some things would be made simpler if I could use .Net 5 or even .Net Core 3.1
      
  4. Since this is a API RestFull Service I made it asynchronous.
  
 Hope you will understand all things assumed and enjoy the code
 
 To run this just have .Net Core 2.2 binaries installed.
